//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System;
//using System.IO;
//
//public class ArtResEditor {
//
//	public static bool BeShowLog = true;
//
//	/// <summary>
//	/// 收集ResInfo
//	/// 1 resid(bundle资源相对路径)
//	/// 2 卸载方式(默认使用引用计数方式)
//	/// 3 bundle打包类型0单独打包，1多资源，2包含依赖 
//	/// 4 依赖信息
//	/// </summary>
//	static List<ResInfo> ResInfoList = new List<ResInfo>(); 
//
//	static int num = 0;
//
//    [MenuItem("NEditor/生成PC bundle资源")]
//    public static void BuildPCBudle()
//    {
//        BuildBudle();
//
//        EditorHelper7.CreateFolder(EditorHelper7.OUTPUT_PC_PATH);
//        BuildPipeline.BuildAssetBundles(EditorHelper7.OUTPUT_PC_PATH, BuildAssetBundleOptions.CollectDependencies, EditorUserBuildSettings.selectedStandaloneTarget);
//        AssetDatabase.Refresh();
//    }
//
//    [MenuItem("NEditor/生成IOS bundle资源")]
//    public static void BuildIOSBudle()
//    {
//        BuildBudle();
//
//        EditorHelper7.CreateFolder(EditorHelper7.OUTPUT_IOS_PATH);
//        BuildPipeline.BuildAssetBundles(EditorHelper7.OUTPUT_IOS_PATH, BuildAssetBundleOptions.CollectDependencies, BuildTarget.iOS);
//        AssetDatabase.Refresh();
//    }
//
//    [MenuItem("NEditor/生成Android bundle资源")]
//    public static void BuildAndroidBudle()
//    {
//        BuildBudle();
//
//        EditorHelper7.CreateFolder(EditorHelper7.OUTPUT_ANDROID_PATH);
//        BuildPipeline.BuildAssetBundles(EditorHelper7.OUTPUT_ANDROID_PATH, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android);
//        AssetDatabase.Refresh();
//    }
//
//	//[MenuItem("NEditor/生成全部配表和资源")]
//	public static void BuildBudle(){
//
//		//临时拆分角色模型
//		//RoleEditor.GenerateRoleTempAsset();
//
//		//临时生成场景信息表
//		//SimulateScnData.GenerateStreetScnData ();
//		//SimulateScnData.GenerateTaxasScnData ();
//
//		//收集资源信息，添加bundleName
//		ProcessBundleName();
//
//		//NaInfo 手动收集依赖，参考item处理，暂时不统一收集依赖
//		//收集资源依赖信息
//		//ProcessDependInfo();
//
//		//输出下资源依赖信息
//		//ShowDependInfo ();
//
//		//生成总资源表
//		GenerateResInfoTable();
//
//		//打全部bundle
//		GeneratBundle();
//
//	}
//
//	/// <summary>
//	/// 添加bundleName，收集资源信息
//	/// </summary>
//	private static void ProcessBundleName(){
//		//按固定目录，遍历所有需要打包的资源，
//		//1给这些资源添加bundle目录名称(及后缀)，
//		//2并收集这些资源的resid(bundle目录名称就是resid)，并存到ResInfoList中
//		//3标识资源打包方式(单独，多资源，依赖)，ps:scn就都是单独，role的part是多资源，anim是多资源，目前ui可能是依赖打包
//		//特殊说明：第一遍遍历先知道bundle标签，制定完才能知道依赖关系
//		//路径 ui，scn，role
//		string uiPath = EditorHelper7.EDITOR_GUI_PATH;
//		string scnPath = EditorHelper7.EDITOR_SCENE_PATH;
//		string rolePath = EditorHelper7.EDITOR_ROLE_ASSET_PATH;
//		string itemPath = EditorHelper7.EDITOR_ITEM_PATH;
//		ProcessGUI (uiPath);
//		ProcessSCN (scnPath);
//		ProcessRole (rolePath);
//		ProcessItem (itemPath);
//		//刷新目录使bundleName设置生效
//		AssetDatabase.Refresh ();
//	}
//
//	/// <summary>
//	/// 输出一下所有资源依赖信息
//	/// </summary>
////	private static void ShowDependInfo(){
////
////		for (int i = 0; i < ResInfoList.Count; i++) {
////
////			int dependNum = ResInfoList [i].DependNum;
////
////			for (int j = 0; j < dependNum; j++) {
////				Log.e ("ArtResEditor","ShowDependInfo","i="+i+" 资源路径:" + ResInfoList[i].ReletivePath +" 依赖数:"+dependNum+" 依赖资源id:"+ResInfoList[i].DependResID[j],BeShowLog);
////			}
////				
////		}
////	}
//
//
//	/// <summary>
//	/// 收集资源依赖信息
//	/// </summary>
////	private static void ProcessDependInfo(){
////		//寻找所有资源的依赖
////		for (int i = 0; i < ResInfoList.Count; i++) {
////			if (ResInfoList [i].ReletivePath.Equals ("null")) {
////				ResInfoList [i].DependNum = 0;
////				continue;
////			}
////			string[] dependResids = AssetDatabase.GetDependencies(ResInfoList[i].ReletivePath,false);
////			if (null == dependResids || dependResids.Length == 0) {
////				ResInfoList [i].DependNum = 0;
////				continue;
////			}
////			else{
////				ResInfoList [i].DependNum = dependResids.Length;
////				ResInfoList [i].DependResID = dependResids;
////
////				for (int j = 0; j < ResInfoList [i].DependResID.Length; j++) {
////					Log.e ("ArtResEditor","ProcessDependInfo","i=" + i + "相对路径:"+ResInfoList [i].ReletivePath+" 依赖数:"+ResInfoList [i].DependNum+" 依赖资源:"+ResInfoList [i].DependResID[j],BeShowLog);
////				}
////
////			}
////		}
////	}
//
//	/// <summary>
//	/// 生成资源总表
//	/// </summary>
//	private static void GenerateResInfoTable(){
//		Log.i ("ArtResEditor","GenerateResInfoTable","开始生成总资源表",BeShowLog);
//		IoBuffer buffer = new IoBuffer(102400);
//		buffer.PutInt(ResInfoList.Count);
//		ResInfo info = null;
//		for (int i = 0; i < ResInfoList.Count; i++) {
//			info = ResInfoList [i];
//			buffer.PutString(info.resID);
//			buffer.PutByte (info.UnloadType);
//			buffer.PutByte (info.BundleType);
//			buffer.PutString (info.resName);
//			buffer.PutInt (info.DependNum);
//
//			Log.i("ArtResEditor","GenerateResInfoTable","i="+i+" 总表资源 resid:"+info.resID+" unload:"+info.UnloadType+
//				" btype:"+info.BundleType+" resName:"+info.resName+" depNum："+info.DependNum,BeShowLog);
//
//			for (int j = 0; j < info.DependNum; j++) {
//				Log.i("ArtResEditor","GenerateResInfoTable","----i="+i+" 依赖信息 editor路径:" + ResInfoList[i].ReletivePath +" 依赖数:"+info.DependNum+" j="+j+" 依赖资源bundle名:"+info.DependResID[j],BeShowLog);
//				buffer.PutString (info.DependResID[j]);
//			}
//		}
//
//		ResInfoList.Clear ();
//		byte[] bs = buffer.ToArray ();
//		EditorHelper7.CreateFolder(EditorHelper7.OUTPUT_TABLE_SYSTEM_PATH);
//		FileHelper.WriteBytes2File (EditorHelper7.OUTPUT_TABLE_SYSTEM_PATH+"/apkresinfo.bytes", bs);
//	}
//
//	/// <summary>
//	/// 生成资源bundle
//	/// </summary>
//	private static void GeneratBundle(){
//		//if(!Directory.Exists(EditorHelper7.OutPutPath))AssetDatabase.CreateFolder("Assets", EditorHelper7.OutPutFolderName);
//		EditorHelper7.CreateFolder(EditorHelper7.OUTPUT_ROOT_PATH);
//        BuildPipeline.BuildAssetBundles(EditorHelper7.OUTPUT_ROOT_PATH,BuildAssetBundleOptions.CollectDependencies,EditorUserBuildSettings.activeBuildTarget);
//		AssetDatabase.Refresh();
//	}
//
//	/// <summary>
//	/// 处理AreRes/UI目录
//	/// </summary>
//	/// <param name="path">Path.</param>
//	private static void ProcessGUI(string path){
//		
//		string[] absoluteFolderPath = EditorHelper7.GetSubFolderPaths(path);
//		string atlasFolderPath = "";
//		string prefabFolderPath = "";
//
//		//遍历图集目录
//		for (int i = 0; i < absoluteFolderPath.Length; i++) {
//
//			string uifolderName = EditorHelper7.GetFileNameFromPath(absoluteFolderPath[i]);
//			Log.i("","ProcessGUI","i:"+i+"当前图集文件夹名:"+uifolderName,BeShowLog);
//
//			//处理图集
//			atlasFolderPath = absoluteFolderPath[i]+"/"+"atlas";
//			string[] atlasFilePaths =  EditorHelper7.FindAllFileURLs(atlasFolderPath);
//			for (int j = 0; j < atlasFilePaths.Length; j++) {
//				string absolutePath = atlasFilePaths[j];
//				string reletivePath = EditorHelper7.ChangeToRelativePath(absolutePath);
//				string fileNameWithoutExt = EditorHelper7.GetFileNameFromPath(reletivePath,true);
//				string bundleName = "res/ui/" + uifolderName + "/" + fileNameWithoutExt;
//				EditorHelper7.SetAssetBundleName (reletivePath,bundleName,EditorHelper7.BUNDLE_EXT_NAME);
//
//				//记录ResInfo
//				ResInfo _resInfo = new ResInfo();
//				_resInfo.resID = bundleName+ "."+EditorHelper7.BUNDLE_EXT_NAME;
//				_resInfo.UnloadType = (byte)0;//引用计数卸载
//				_resInfo.BundleType = 0;//单独打包(图集不需要依赖其他资源)
//				_resInfo.resName = fileNameWithoutExt;//这个参数目前其实没用到，暂时保留
//				//_resInfo.DependNum;//这个所有资源设置完bundleName后使用
//				_resInfo.ReletivePath = reletivePath; //这个asset相对路径不打入总资源表，记录下，用来查找依赖用
//				ResInfoList.Add(_resInfo);
//			}
//
//			//处理uiprefab
//			prefabFolderPath = absoluteFolderPath[i]+"/"+"prefabs";
//			string[] prefabFilePaths =  EditorHelper7.FindAllFileURLs(prefabFolderPath);
//			for (int j = 0; j < prefabFilePaths.Length; j++) {
//				string absolutePath = prefabFilePaths[j];
//				string reletivePath = EditorHelper7.ChangeToRelativePath(absolutePath);
//				string fileNameWithoutExt = EditorHelper7.GetFileNameFromPath(reletivePath,true);
//				string bundleName = "res/ui/" + uifolderName + "/" + fileNameWithoutExt;
//				EditorHelper7.SetAssetBundleName (reletivePath,bundleName,EditorHelper7.BUNDLE_EXT_NAME);
//
//				//记录ResInfo
//				ResInfo _resInfo = new ResInfo();
//				_resInfo.resID = bundleName+ "."+EditorHelper7.BUNDLE_EXT_NAME;
//				_resInfo.UnloadType = (byte)0;//引用计数卸载
//				_resInfo.BundleType = 2;//依赖打包
//				_resInfo.resName = fileNameWithoutExt;//这个参数目前其实没用到，暂时保留
//
//
//
//				//手动收集依赖，默认uiprefab只依赖atlas图集
//				bool flag = false;
//				string tempStr = null;
//				string[] dependResids = AssetDatabase.GetDependencies(reletivePath,false);
//
//				for (int k = 0; k < dependResids.Length; k++) {
//					if (dependResids [k].EndsWith (".png")) {
//						//_resInfoitem.DependResID [0] =dependResids[k];
//						tempStr = EditorHelper7.GetAssetBundleName(dependResids[k]);
//
//						flag = true;
//					}
//				}
//
//				if (flag) {//找到依赖资源才修改数据
//					_resInfo.DependNum = 1;
//					_resInfo.DependResID = new string[1];
//					_resInfo.DependResID [0] = tempStr;
//				} else {
//					_resInfo.DependNum = 0;
//					_resInfo.DependResID = null;
//				}
//					
//				_resInfo.ReletivePath = reletivePath; //这个asset相对路径不打入总资源表，记录下，用来查找依赖用
//				ResInfoList.Add(_resInfo);
//
//			}
//		}
//			
//	}
//
//	/// <summary>
//	/// 处理AreRes/Scene目录
//	/// </summary>
//	/// <param name="path">Path.</param>
//	private static void ProcessSCN(string path){
//		
//		string[] absoluteFolderPath = EditorHelper7.GetSubFolderPaths(path);
//
//		//遍历所有场景类型目录(AreRes/Scene/下一级的所有目录 eg:Scn_Street)
//		for (int i = 0; i < absoluteFolderPath.Length; i++) {
//
//			//场景类型目录名 比如Scn_Street
//			string scnTypeFolder = EditorHelper7.GetFileNameFromPath(absoluteFolderPath[i],true);
//
//			//获取一个场景类型(比如Scn_Street)下所有场景(scn1,scn2....)目录绝对地址
//			string[] scnFolderAbsPath = EditorHelper7.GetSubFolderPaths(absoluteFolderPath[i]);
//
//			for (int j = 0; j < scnFolderAbsPath.Length; j++) {
//				string scnName = EditorHelper7.GetFileNameFromPath(scnFolderAbsPath[j],true);//eg:scn1,scn2
//				string scnRealAbsPath = scnFolderAbsPath[j]+"/scn"; //eg:ArtRes/Scene/Scn_Streat/scn1/scn,需要打包资源在scn中
//				string[] scnPartPrefabAbsPath =EditorHelper7.FindAllFileURLs(scnRealAbsPath); //scn中具体prefab绝对路径 eg：bg,building,ground,obj
//
//				for (int k = 0; k < scnPartPrefabAbsPath.Length; k++) {
//					string absolutePath = scnPartPrefabAbsPath[k];
//					string reletivePath = EditorHelper7.ChangeToRelativePath(absolutePath);
//					string fileNameWithoutExt = EditorHelper7.GetFileNameFromPath(reletivePath,true);
//					string bundleName = "res/scene/" + scnTypeFolder+"/" +scnName+ "/" + fileNameWithoutExt;
//					EditorHelper7.SetAssetBundleName (reletivePath,bundleName,EditorHelper7.BUNDLE_EXT_NAME);
//
//
//					//记录ResInfo
//					ResInfo _resInfo = new ResInfo();
//					_resInfo.resID = (bundleName+ "."+EditorHelper7.BUNDLE_EXT_NAME).ToLower();
//					_resInfo.UnloadType = (byte)0;//引用计数卸载
//					_resInfo.BundleType = 0;//单独打包(图集不需要依赖其他资源)
//					_resInfo.resName = fileNameWithoutExt;//这个参数目前其实没用到，暂时保留
//					_resInfo.DependNum = 0;//场景暂时不需要依赖
//					_resInfo.ReletivePath = reletivePath; //这个asset相对路径不打入总资源表，记录下，用来查找依赖用
//					ResInfoList.Add(_resInfo);
//
//				}//k循环结尾
//			}//j循环结尾
//		}//i循环结尾
//
//	}
//
//	private static void ProcessRole(string path){
//		
//		string[] absoluteFolderPath = EditorHelper7.GetSubFolderPaths(path);
//		//遍历所有角色目录(AreRes/Scene/下一级的所有目录 eg:Scn_Street)
//		for (int i = 0; i < absoluteFolderPath.Length; i++) {
//
//			//eg:hg 角色(目录)名
//			string roleFolderName = EditorHelper7.GetFileNameFromPath (absoluteFolderPath[i]);
//
//			//骨骼
//			string  boneAbsPath = absoluteFolderPath[i]+"/basebone.prefab";
//			string  boneRelPath = EditorHelper7.ChangeToRelativePath (boneAbsPath);
//			string  boneBundleName = "res/role/"+roleFolderName+"_basebone";
//			EditorHelper7.SetAssetBundleName (boneRelPath,boneBundleName,EditorHelper7.BUNDLE_EXT_NAME);
//			ResInfo _resInfoBone = new ResInfo();
//			_resInfoBone.resID = boneBundleName+ "."+EditorHelper7.BUNDLE_EXT_NAME;
//			_resInfoBone.UnloadType = (byte)0;//引用计数卸载
//			_resInfoBone.BundleType = (byte)0;//单资源打包
//			_resInfoBone.resName = roleFolderName+"_basebone";//这个参数目前其实没用到，暂时保留，这里暂时写动作组名
//			_resInfoBone.DependNum = 0;//场景暂时不需要依赖
//			_resInfoBone.ReletivePath = boneRelPath; //这个asset相对路径不打入总资源表，记录下，用来查找依赖用
//			ResInfoList.Add(_resInfoBone);
//
//
//			//动作组
//			string animClipFolderAbsPath = absoluteFolderPath[i]+"/AnimationClip";
//			//动作组文件夹路径 eg:AnimationClip/common
//			string[] animGroupFolderAbsPath = EditorHelper7.GetSubFolderPaths(animClipFolderAbsPath);
//			for (int j = 0; j < animGroupFolderAbsPath.Length; j++) {//遍历所有动作组文件夹
//				string animGroupFolderName = EditorHelper7.GetFileNameFromPath(animGroupFolderAbsPath[j]);
//				string bundleName = "res/role/"+roleFolderName + "_anim_" + animGroupFolderName;//eg:hg_anim_common
//
//				//具体动画文件绝对路径 eg:Assets/ArtRes/Role/TempAsset/hg/AnimationClip/common/deal
//				string[] allAnimFileAbsPaths = EditorHelper7.GetSubFilesPaths(animGroupFolderAbsPath[j]);
//				for (int k = 0; k < allAnimFileAbsPaths.Length; k++) {//遍历一个动作组内所有动作文件
//					string absolutePath = allAnimFileAbsPaths[k];
//					string reletivePath = EditorHelper7.ChangeToRelativePath(absolutePath);
//					string fileNameWithoutExt = EditorHelper7.GetFileNameFromPath(reletivePath,true);
//					EditorHelper7.SetAssetBundleName (reletivePath,bundleName,EditorHelper7.BUNDLE_EXT_NAME);
//				}
//
//				//记录一个动作组的ResInfo
//				ResInfo _resInfo = new ResInfo();
//				_resInfo.resID = bundleName+ "."+EditorHelper7.BUNDLE_EXT_NAME;
//				_resInfo.UnloadType = (byte)0;//引用计数卸载
//				_resInfo.BundleType = (byte)1;//多资源打包
//				_resInfo.resName = animGroupFolderName;//这个参数目前其实没用到，暂时保留，这里暂时写动作组名
//				_resInfo.DependNum = 0;//场景暂时不需要依赖
//				_resInfo.ReletivePath = animGroupFolderAbsPath[j]; //这个asset相对路径不打入总资源表，记录下，用来查找依赖用
//				ResInfoList.Add(_resInfo);
//
//			}
//
//			//身体
//			string bodyFolderAbsPath = absoluteFolderPath[i]+"/"+roleFolderName+"_"+"body";
//			string bodyFolderName = EditorHelper7.GetFileNameFromPath(bodyFolderAbsPath);//eg:hg_body
//			string bodyBundleName = "res/role/"+bodyFolderName;
//			string[] bodyFileAbsPath = EditorHelper7.GetSubFilesPaths(bodyFolderAbsPath);
//			for (int j = 0; j < bodyFileAbsPath.Length; j++) {
//				string absolutePath = bodyFileAbsPath[j];
//				string reletivePath = EditorHelper7.ChangeToRelativePath(absolutePath);
//				string fileNameWithoutExt = EditorHelper7.GetFileNameFromPath(reletivePath,true);
//				EditorHelper7.SetAssetBundleName (reletivePath,bodyBundleName,EditorHelper7.BUNDLE_EXT_NAME);
//			}
//
//			//记录一个动作组的ResInfo
//			ResInfo _resInfoBody = new ResInfo();
//			_resInfoBody.resID = bodyBundleName+ "."+EditorHelper7.BUNDLE_EXT_NAME;
//			_resInfoBody.UnloadType = (byte)0;//引用计数卸载
//			_resInfoBody.BundleType = (byte)1;//多资源打包
//			_resInfoBody.resName = bodyFolderName;//这个参数目前其实没用到，暂时保留，这里暂时写动作组名
//			_resInfoBody.DependNum = 0;//暂时不需要依赖
//			_resInfoBody.ReletivePath = bodyFolderAbsPath; //这个asset相对路径不打入总资源表，记录下，用来查找依赖用
//			ResInfoList.Add(_resInfoBody);
//
//
//			//脸部
//			string faceFolderAbsPath = absoluteFolderPath[i]+"/"+roleFolderName+"_"+"face";
//			string faceFolderName = EditorHelper7.GetFileNameFromPath(faceFolderAbsPath);//eg:hg_face
//			string faceBundleName = "res/role/"+faceFolderName;
//			string[] faceFileAbsPath = EditorHelper7.GetSubFilesPaths(faceFolderAbsPath);
//			for (int j = 0; j < faceFileAbsPath.Length; j++) {
//				string absolutePath = faceFileAbsPath[j];
//				string reletivePath = EditorHelper7.ChangeToRelativePath(absolutePath);
//				string fileNameWithoutExt = EditorHelper7.GetFileNameFromPath(reletivePath,true);
//				EditorHelper7.SetAssetBundleName (reletivePath,faceBundleName,EditorHelper7.BUNDLE_EXT_NAME);
//			}
//			//记录一个动作组的ResInfo
//			ResInfo _resInfoFace = new ResInfo();
//			_resInfoFace.resID = faceBundleName+ "."+EditorHelper7.BUNDLE_EXT_NAME;
//			_resInfoFace.UnloadType = (byte)0;//引用计数卸载
//			_resInfoFace.BundleType = (byte)1;//多资源打包
//			_resInfoFace.resName = faceFolderName;//这个参数目前其实没用到，暂时保留，这里暂时写动作组名
//			_resInfoFace.DependNum = 0;//场景暂时不需要依赖
//			_resInfoFace.ReletivePath = faceFolderAbsPath; //这个asset相对路径不打入总资源表，记录下，用来查找依赖用
//			ResInfoList.Add(_resInfoFace);
//
//
//			//材质
//			string materialFolderAbsPath = absoluteFolderPath[i]+"/Materials";
//			//材质文件绝对路径
//			string[] matFileAbsPaths = EditorHelper7.GetSubFilesPaths(materialFolderAbsPath);
//			for (int j = 0; j < matFileAbsPaths.Length; j++) {
//				if (matFileAbsPaths [j].EndsWith (".meta")) {
//					continue;
//				}
//				string absolutePath = matFileAbsPaths[j];
//				string reletivePath = EditorHelper7.ChangeToRelativePath(absolutePath);
//				string fileNameWithoutExt = EditorHelper7.GetFileNameFromPath(reletivePath,true);
//				string matBundleName = "res/role/" + fileNameWithoutExt;
//				EditorHelper7.SetAssetBundleName (reletivePath,matBundleName,EditorHelper7.BUNDLE_EXT_NAME);
//
//				//记录一个动作组的ResInfo
//				ResInfo _resInfo = new ResInfo();
//				_resInfo.resID = matBundleName + "."+EditorHelper7.BUNDLE_EXT_NAME;
//				_resInfo.UnloadType = (byte)0;//引用计数卸载
//				_resInfo.BundleType = (byte)0;//多资源打包
//				_resInfo.resName = fileNameWithoutExt;//这个参数目前其实没用到，暂时保留，这里暂时写动作组名
//				_resInfo.DependNum = 0;//场景暂时不需要依赖
//				_resInfo.ReletivePath = reletivePath; //这个asset相对路径不打入总资源表，记录下，用来查找依赖用
//				//Debug.LogError("------>resInfo.resid:"+_resInfo.resID);
//				ResInfoList.Add(_resInfo);
//			}
//
//		}
//
//	}
//
//
//	private static void ProcessItem(string path){
//		
//		string[] absoluteFolderPath = EditorHelper7.GetSubFolderPaths(path);
//
//		//遍历所有道具文件夹
//		for (int i = 0; i < absoluteFolderPath.Length; i++) {
//			string itemFolderName = EditorHelper7.GetFileNameFromPath (absoluteFolderPath[i]);
//
//
//			string  matFolderPath = absoluteFolderPath[i]+"/material";
//			string[] matAbsPaths =  EditorHelper7.GetSubFilesPaths (matFolderPath);
//			for (int j = 0; j < matAbsPaths.Length; j++) {
//				if (matAbsPaths [j].EndsWith (".meta")) {
//					continue;
//				}
//
//				string absolutePath = matAbsPaths[j];
//				string reletivePath = EditorHelper7.ChangeToRelativePath(absolutePath);
//				string fileNameWithoutExt = EditorHelper7.GetFileNameFromPath(reletivePath,true);
//				string itemBundleName = "res/item/" + itemFolderName+"/"+fileNameWithoutExt;
//				EditorHelper7.SetAssetBundleName (reletivePath,itemBundleName,EditorHelper7.BUNDLE_EXT_NAME);
//
//				//记录一个动作组的ResInfo
//				ResInfo _resInfoitem = new ResInfo();
//				_resInfoitem.resID = itemBundleName+ "."+EditorHelper7.BUNDLE_EXT_NAME;
//				_resInfoitem.UnloadType = (byte)0;//引用计数卸载
//				_resInfoitem.BundleType = (byte)0;//单资源打包
//				_resInfoitem.resName = itemBundleName;//这个参数目前其实没用到，暂时保留，这里暂时写动作组名
//				_resInfoitem.DependNum = 0;//暂时不需要依赖
//				_resInfoitem.ReletivePath = reletivePath; //这个asset相对路径不打入总资源表，记录下，用来查找依赖用
//				ResInfoList.Add(_resInfoitem);
//
//			}
//
//
//			string  prefabFolderPath = absoluteFolderPath[i]+"/prefab";
//			string[] prefabAbsPaths =  EditorHelper7.GetSubFilesPaths (prefabFolderPath);
//			for (int j = 0; j < prefabAbsPaths.Length; j++) {
//				if (prefabAbsPaths [j].EndsWith (".meta")) {
//					continue;
//				}
//
//				string absolutePath = prefabAbsPaths[j];
//				string reletivePath = EditorHelper7.ChangeToRelativePath(absolutePath);
//				string fileNameWithoutExt = EditorHelper7.GetFileNameFromPath(reletivePath,true);
//				string itemBundleName = "res/item/" + itemFolderName+"/"+fileNameWithoutExt;
//				EditorHelper7.SetAssetBundleName (reletivePath,itemBundleName,EditorHelper7.BUNDLE_EXT_NAME);
//
//				//记录一个动作组的ResInfo
//				ResInfo _resInfoitem = new ResInfo();
//				_resInfoitem.resID = itemBundleName+ "."+EditorHelper7.BUNDLE_EXT_NAME;
//				_resInfoitem.UnloadType = (byte)0;//引用计数卸载
//				_resInfoitem.BundleType = (byte)2;//依赖资源打包
//				_resInfoitem.resName = itemBundleName;//这个参数目前其实没用到，暂时保留，这里暂时写动作组名
//
//
//				//NINFO 手动处理下依赖，正常prefab依赖model，mat，现在只依赖mat就可以，模型包含在prefab中，因为每个prefab就使用一个特有model
//
//				bool flag = false;
//				List<string> tempStrList = new List<string>();
//				string[] dependResids = AssetDatabase.GetDependencies(reletivePath,false);
//
//				for (int k = 0; k < dependResids.Length; k++) {
//					if (dependResids [k].EndsWith (".mat")) {
//						//_resInfoitem.DependResID [0] =dependResids[k];
//						tempStrList.Add(EditorHelper7.GetAssetBundleName(dependResids[k]));
//						flag = true;
//					}
//				}
//
//				if (flag) {//找到依赖资源才修改数据
//					_resInfoitem.DependNum = tempStrList.Count;
//					_resInfoitem.DependResID = new string[tempStrList.Count];
//					for(int k=0;k<tempStrList.Count;k++){
//						_resInfoitem.DependResID[k] = tempStrList[k];
//					}
//
//				} else {
//					_resInfoitem.DependNum = 0;
//					_resInfoitem.DependResID = null;
//				}
//
//
//
//				_resInfoitem.ReletivePath = reletivePath; //这个asset相对路径不打入总资源表，记录下，用来查找依赖用
//				ResInfoList.Add(_resInfoitem);
//
//
//			}
//
//		}
//
//	}
//
//
//
//	#region test
//
//	//[MenuItem("NEditor/Test/GenerateBundle4SelectObj")]
//	public static void ExportSelectedScene2AssetBundle(){
//
//		List<AssetBundleBuild> BundleBuildList = new List<AssetBundleBuild> ();
//
//		string path = Path.GetDirectoryName(Application.dataPath) +"/data/res/";
//		string rootPath = "Assets/NEditor/SceneEditor/Resources/Scenes/Prefabs/";
//
//		UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
//		foreach (UnityEngine.Object obj in selection){
//			//Debug.LogError ("=-=-=-==-=-=>objName："+obj.name);
//			/*if (obj is GameObject)*/{
//				string objPath = AssetDatabase.GetAssetPath(obj);
//				string objDirectory = Path.GetDirectoryName(objPath);
//				string objName = Path.GetFileNameWithoutExtension(objPath);
//				string saveDirectory = path;// + objDirectory.Substring(rootPath.Length);
//
//				if (!Directory.Exists(saveDirectory))
//					Directory.CreateDirectory(saveDirectory);
//
//				string savePath = saveDirectory + "/" + objName + ".unity3d";
//				//bool ret=  BuildPipeline.BuildAssetBundle(obj,null, savePath,BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.StandaloneWindows);
//
//				AssetBundleBuild ab = new AssetBundleBuild();
//				ab.assetBundleName = objName+".n";
//				Debug.Log ("objPath---->"+objPath+" bundle.name:"+objName);
//				ab.assetNames = new string[]{objPath};
//				BundleBuildList.Add (ab);
//				string outUrl = path;
//
//
//				BuildPipeline.BuildAssetBundles (outUrl, BundleBuildList.ToArray(), BuildAssetBundleOptions.CollectDependencies,BuildTarget.StandaloneWindows );
//
//				//if(ret==false)
//				//	Debug.LogError("编译错误"+savePath);
//				//else{
//				//	string newPath = saveDirectory + "/" + objName + ".tresource";
//				//EncodeFile(savePath, newPath);
//				//}
//			}
//		}
//	}
//
//
//
//	//[MenuItem("NEditor/Test/TestAssets")]
//	public static void TestAssets(){
//
//		string[] allbundleName = AssetDatabase.GetAllAssetBundleNames ();
//		for (int i = 0; i < allbundleName.Length; i++) {
//			Debug.Log ("--->I:"+i+" name:"+allbundleName[i]);
//		}
//
//		string[] depend = AssetDatabase.GetDependencies("Assets/ArtRes/GameUI/ui_nameA/prefabs/TestWindow.prefab",false);
//		for (int j = 0; j < depend.Length; j++) {
//			Debug.Log ("=======>j:"+j+" depend:"+depend[j]);
//		}
//
//		//		string p1 = Application.dataPath+"/ArtRes/GameUI/ui_nameA/prefabs/TestWindow";
//		//		Debug.Log ("11---"+p1);
//		//
//		//		string path = "Assets/ArtRes/GameUI/ui_nameA/prefabs/TestWindow";
//		//
//		//		string[] depends = AssetDatabase.GetDependencies(p1); 
//		//		if()
//		//		for (int j = 0; j < depends.Length; j++) {
//		//			Debug.Log ("=======>j:"+j+" depend:"+depends[j]);
//		//		}
//	}
//
//	//将选定的资源进行统一设置AssetBundle名
//	//[MenuItem("Tool/SetAssetBundleNameExtension")]  
//	static void SetBundleName() {
//		UnityEngine.Object[] selects = Selection.objects;
//		foreach (UnityEngine.Object item in selects){
//			string path = AssetDatabase.GetAssetPath(item);
//			AssetImporter asset = AssetImporter.GetAtPath(path);
//			asset.assetBundleName = item.name;//设置Bundle文件的名称
//			asset.assetBundleVariant = "asset";//设置Bundle文件的扩展名
//			asset.SaveAndReimport();
//		}
//		AssetDatabase.Refresh();
//	}
//
//	#endregion
//}
//
//public class ResInfo{
//	public string resID;
//	public byte UnloadType;  		//默认0引用计数卸载
//	public byte BundleType;  		//0单独打包，1多资源，2包含依赖  
//	public string resName;
//	public int DependNum;	 		//依赖资源数
//	public string[] DependResID;    //依赖资源的ResID
//
//	//NINFO 多资源打包不需要查找依赖引用，这里就赋值为"null",遇到这个string就不查找依赖
//	//NINFO 18.7.19 设置bundle名时直接查找引用关系，不再二次查找引用关系，这个参数目前只用来输出log
//	public string ReletivePath;		//资源相对路径Asset/.......,这个string不打入总资源表，记录下，用来查找依赖用
//}
