using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MeEditor : MonoBehaviour {


	[MenuItem ("Editor/BuildScn")]
	static void BuildScn(){

		//检测输出路径
		if(!EditorHelper.BeFolderExist(MeEditorHelper.OUTPUT_RES_PATH)){
			EditorHelper.CreateFolder(MeEditorHelper.OUTPUT_RES_PATH);
		}

		//设置bundleName
		SetMapBundleName();
		SetObjsBundleName ();

		//填充scnData数据
		FillScnData();

		//设置scnData BundleName
		SetScnBundleName ();

		//TODO 平台处理
		//打bundle
		//nafio info 生成的bundle的名称，就取决于MeEditorHelper.OUTPUT_RES_PATH中最后一个文件夹的名称，比如这里就是me.manifest
		BuildPipeline.BuildAssetBundles(MeEditorHelper.OUTPUT_RES_PATH,BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows64);

		AssetDatabase.SaveAssets ();

		AssetDatabase.Refresh();

	}

	static void SetMapBundleName()
	{
		//地图资源根路径
		string mapRootPath = MeEditorHelper.MAP_PATH;

		//具体地图根路径（每个地图文件夹都以地图名命名）
		string[] mapsPath  = EditorHelper.GetSubFolderPaths(mapRootPath);	

		int mapNum = mapsPath.Length;

		for (int i = 0; i < mapNum; i++) 
		{
			string mapName = EditorHelper.GetFileNameFromPath (mapsPath[i]);

			//地图中每个资源的路径
			string[] filesPath = EditorHelper.GetSubFilesPaths(mapsPath[i]);

			int filesNum = filesPath.Length;

			for (int j = 0; j < filesNum; j++) 
			{
				string filePath = filesPath [j];

				string reletiveFilePath = EditorHelper.ChangeToRelativePath (filePath);

				string fileNameWithoutExt = EditorHelper.GetFileNameFromPath (filePath,true);

				string bundleName = "map/" + mapName + "/" + fileNameWithoutExt ;

				EditorHelper.SetAssetBundleName (reletiveFilePath, bundleName,EditorHelper.BUNDLE_EXT_NAME);

			}

		}

	}

	static void SetObjsBundleName(){
		
		//地图物体资源根路径
		string objsRootPath = MeEditorHelper.OBJ_PATH;

		string[] objsPath  = EditorHelper.GetSubFilesPaths(objsRootPath);	

		int objsNum = objsPath.Length;

		for (int i = 0; i < objsNum; i++) 
		{

			string objPath = objsPath [i];

			string reletiveObjsPath = EditorHelper.ChangeToRelativePath (objPath);

			string fileNameWithoutExt = EditorHelper.GetFileNameFromPath (objPath,true);

			string bundleName = "objs/" + fileNameWithoutExt;

			EditorHelper.SetAssetBundleName (reletiveObjsPath, bundleName,EditorHelper.BUNDLE_EXT_NAME);

		}
	}

	static void SetScnBundleName(){
		//地图物体资源根路径
		string scnRootPath = MeEditorHelper.SCN_PATH;

		string[] scnTypesFolderPath  = EditorHelper.GetSubFolderPaths(scnRootPath);	

		int scnTypesNum = scnTypesFolderPath.Length;

		//遍历所有场景类型文件夹
		for (int i = 0; i < scnTypesNum; i++) 
		{
			string scnType = EditorHelper.GetFileNameFromPath(scnTypesFolderPath [i],true);
			
			string[] scnsPath =  EditorHelper.GetSubFolderPaths(scnTypesFolderPath[i]);	

			//遍历所有场景文件夹
			for (int j = 0; j < scnsPath.Length; j++)
			{
				string scnName = EditorHelper.GetFileNameFromPath(scnsPath [j],true);

				string scnAssetPath = scnsPath [j]+"/scnData.asset";

				string reletiveScnAssetPath = EditorHelper.ChangeToRelativePath (scnAssetPath);

				string fileNameWithoutExt = EditorHelper.GetFileNameFromPath (scnAssetPath,true);

				string bundleName = "scn/" +scnType+"/"+scnName +"/"+ fileNameWithoutExt;

				EditorHelper.SetAssetBundleName (reletiveScnAssetPath, bundleName);
			}

		}

	}

	//填充场景数据
	static void FillScnData()
	{
		string scnRootPath = MeEditorHelper.SCN_PATH;

		string[] scnTypesFolderPath  = EditorHelper.GetSubFolderPaths(scnRootPath);	

		int scnTypesNum = scnTypesFolderPath.Length;

		//遍历所有场景类型文件夹
		for (int i = 0; i < scnTypesNum; i++) 
		{
			string scnTypePath = scnTypesFolderPath [i];
			string scnType = EditorHelper.GetFileNameFromPath(scnTypePath);
			string[] scnsPath = EditorHelper.GetSubFolderPaths(scnTypePath);	
			int scnNum = scnsPath.Length;
			//遍历所有场景文件夹
			for (int j = 0; j < scnNum; j++) 
			{
				FillOneScn (scnsPath[j],scnType);
			}
		}

	}


	//填充一个具体场景
	static void FillOneScn(string scnPath,string scnType)
	{
		string scnName = EditorHelper.GetFileNameFromPath(scnPath);

		//创建场景临时对象
		string scnPrefabPath = scnPath+"/scn.prefab";
		if (!EditorHelper.BeFileExist (scnPrefabPath)) {
			Debug.LogError ("MeEditor.FillOneScnData 未找到scnPrefa  path:"+scnPrefabPath+" 填充场景数据失败");
			return;
		}
		string scnPrefabReletivePath = EditorHelper.ChangeToRelativePath (scnPrefabPath);
		Object obj = AssetDatabase.LoadAssetAtPath<Object> (scnPrefabReletivePath);
		var scnGo = GameObject.Instantiate (obj) as GameObject;

		//根据不同类型类型场景分别填充数据

		FillDiffScnData (scnType,scnName,scnPath,scnGo);

		DestroyImmediate(scnGo);
	}

	//设置不同类型的场景数据
	static void FillDiffScnData(string scnType,string scnName,string scnSubFolderPath,GameObject scnPrefab){

		switch(scnType)
		{
		case "scntype3c"://对战类地图解析
			
			FillScnData3C (scnType,scnName,scnSubFolderPath,scnPrefab);

			break;

		}
	}

	#region process diff ScnData

	//填充3c场景
	static void FillScnData3C(string scnType,string scnName,string scnSubFolderPath,GameObject scnPrefab)
	{

		var trm = scnPrefab.transform;

		var mapTrm = trm.GetChild (0);

		string mapName = mapTrm.name;

		var objsTrm = trm.Find ("objs");

		Tower[] towers = objsTrm.GetComponentsInChildren<Tower> ();

		//获取或创建ScnData
		string scnDataAssetPath = scnSubFolderPath + "/scnData.asset";
		//Debug.LogError ("------>scnDataAssetPath:"+scnDataAssetPath);
		string scnDataAssetReletivePath = EditorHelper.ChangeToRelativePath (scnDataAssetPath);


		Scn3CData scn3CData = new Scn3CData();
		scn3CData.ScnName = scnName;
		scn3CData.MapName = mapName;
		scn3CData.TowerDataList = new List<TowerData> ();
		for (int i = 0; i < towers.Length; i++) {
			TowerData td = new TowerData ();
			td.Camp = towers[i].Camp;
			td.Pos = towers [i].transform.position;
			td.Priority = towers [i].Priority;
			scn3CData.TowerDataList.Add (td);
		}

		Town[] towns = objsTrm.GetComponentsInChildren<Town> ();
		scn3CData.TownDataList = new List<TownData> ();
		for (int i = 0; i < towns.Length; i++) 
		{
			TownData td = new TownData ();
			td.Camp = towns[i].Camp;
			td.Pos = towns [i].transform.position;
			scn3CData.TownDataList.Add (td);
		}

		if (!EditorHelper.BeFileExist (scnDataAssetPath)) 
		{
			Debug.LogError ("MeEditor.FillScnData3C scnData 不存在，创建新数据文件 path:"+scnDataAssetPath+" ");
			scn3CData = ScriptableHelper.CreateOrReplaceAsset<Scn3CData> (scn3CData,scnDataAssetReletivePath);
		} 
		else 
		{
			
			Scn3CData oldScn3CData = AssetDatabase.LoadAssetAtPath<Scn3CData> (scnDataAssetReletivePath);
			bool bSame = Scn3CData.BeSame(scn3CData,oldScn3CData);
			Debug.LogError ("MeEditor.FillScnData3C scnData存在，读取数据文件,判断是否有数据变更，有就覆盖数据  path:"+scnDataAssetPath +" bSame:"+bSame);
			if (!bSame) 
			{
				oldScn3CData.ScnName = scn3CData.ScnName;
				oldScn3CData.MapName = scn3CData.MapName;
				oldScn3CData.TowerDataList = new List<TowerData> ();
				oldScn3CData.TowerDataList.AddRange (scn3CData.TowerDataList.ToArray());
				oldScn3CData.TownDataList = new List<TownData> ();
				oldScn3CData.TownDataList.AddRange (scn3CData.TownDataList.ToArray());

				Debug.LogError ("MeEditor.FillScnData3C scnName:"+oldScn3CData.ScnName+" mapName:"+oldScn3CData.MapName+" ListCount:"+oldScn3CData.TowerDataList.Count);

			}



		}

	}

	#endregion
}
