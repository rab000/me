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

		//打bundle
		BuildPipeline.BuildAssetBundles("path:",BuildAssetBundleOptions.None,BuildTarget.StandaloneOSXIntel64);

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

				string bundleName = "res/map" + mapName + "/" + fileNameWithoutExt ;

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

			string bundleName = "res/objs/" + fileNameWithoutExt;

			EditorHelper.SetAssetBundleName (reletiveObjsPath, bundleName,EditorHelper.BUNDLE_EXT_NAME);

		}
	}

	static void SetScnBundleName(){
		//地图物体资源根路径
		string scnRootPath = MeEditorHelper.SCN_PATH;

		string[] scnsPath  = EditorHelper.GetSubFolderPaths(scnRootPath);	

		int scnsNum = scnsPath.Length;

		for (int i = 0; i < scnsNum; i++) 
		{

			string scnAssetPath = scnsPath [i]+"/scnData.asset";

			string reletiveScnAssetPath = EditorHelper.ChangeToRelativePath (scnAssetPath);

			string fileNameWithoutExt = EditorHelper.GetFileNameFromPath (scnAssetPath,true);

			string bundleName = "res/scn/" + fileNameWithoutExt;

			EditorHelper.SetAssetBundleName (reletiveScnAssetPath, bundleName,EditorHelper.BUNDLE_EXT_NAME);

		}

	}

	//填充场景数据
	static void FillScnData()
	{
		string scnRootPath = MeEditorHelper.SCN_PATH;

		string[] scnsPath  = EditorHelper.GetSubFolderPaths(scnRootPath);	

		int scnsNum = scnsPath.Length;

		for (int i = 0; i < scnsNum; i++) 
		{
			FillOneScnData (scnsPath[i]);
		}

	}

	static void FillOneScnData(string scnSubFolderPath)
	{

		//首先获取类型
		string scnSubFolerName = EditorHelper.GetFileNameFromPath(scnSubFolderPath);
		string[] scnTypeAndName = scnSubFolerName.Split ('_');
		string scnType = scnTypeAndName[0];
		string scnName = scnTypeAndName[1];

		//创建场景临时对象
		string scnPrefabPath = scnSubFolderPath+"scn.prefab";
		if (!EditorHelper.BeFileExist (scnPrefabPath)) {
			Debug.LogError ("MeEditor.FillOneScnData 未找到scnPrefa  path:"+scnPrefabPath+" 填充场景数据失败");
			return;
		}
		string scnPrefabReletivePath = EditorHelper.ChangeToRelativePath (scnPrefabPath);
		Object obj = AssetDatabase.LoadAssetAtPath<Object> (scnPrefabReletivePath);
		var scnGo = GameObject.Instantiate (obj) as GameObject;

		//根据不同类型类型场景分别填充数据

		FillDiffScnData (scnType,scnName,scnSubFolderPath,scnGo);

	}

	static void FillDiffScnData(string scnType,string scnName,string scnSubFolderPath,GameObject scnPrefab){

		switch(scnType)
		{
		case "scntype3c"://对战类地图解析
			
			FillScnData3C (scnType,scnName,scnSubFolderPath,scnPrefab);

			break;

		}
	}

	#region process diff ScnData

	static void FillScnData3C(string scnType,string scnName,string scnSubFolderPath,GameObject scnPrefab)
	{

		var trm = scnPrefab.transform;

		var mapTrm = trm.GetChild (0);

		string mapName = mapTrm.name;

		var objsTrm = trm.Find ("objs");

		Tower[] towers = objsTrm.GetComponentsInChildren<Tower> ();

		//获取或创建ScnData
		string scnDataAssetPath = scnSubFolderPath + "scnData.asset";

		Scn3CData sd = null;

		if (!EditorHelper.BeFileExist (scnDataAssetPath)) 
		{
			sd = ScriptableHelper.CreateOrReplaceAsset<Scn3CData> (sd,scnDataAssetPath);
		} 
		else 
		{
			sd = AssetDatabase.LoadAssetAtPath<Scn3CData> (scnDataAssetPath);
		}

		sd.ScnName = scnName;

		sd.MapName = mapName;

		sd.TowerDataList = new List<TowerData> ();

		for (int i = 0; i < towers.Length; i++) {
			TowerData td = new TowerData ();
			td.Camp = towers[i].Camp;
			td.Pos = towers [i].transform.position;
			td.Priority = towers [i].Priority;
			sd.TowerDataList.Add (td);
		}

	}

	#endregion
}
