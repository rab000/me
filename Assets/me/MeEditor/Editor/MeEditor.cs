﻿using System.Collections;
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

		string[] scnsPath  = EditorHelper.GetSubFolderPaths(scnRootPath);	

		int scnsNum = scnsPath.Length;

		for (int i = 0; i < scnsNum; i++) 
		{

			string scnAssetPath = scnsPath [i]+"/scnData.asset";

			string reletiveScnAssetPath = EditorHelper.ChangeToRelativePath (scnAssetPath);

			string fileNameWithoutExt = EditorHelper.GetFileNameFromPath (scnAssetPath,true);

			string bundleName = "scn/" + fileNameWithoutExt;

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
		string scnPrefabPath = scnSubFolderPath+"/scn.prefab";
		if (!EditorHelper.BeFileExist (scnPrefabPath)) {
			Debug.LogError ("MeEditor.FillOneScnData 未找到scnPrefa  path:"+scnPrefabPath+" 填充场景数据失败");
			return;
		}
		string scnPrefabReletivePath = EditorHelper.ChangeToRelativePath (scnPrefabPath);
		Object obj = AssetDatabase.LoadAssetAtPath<Object> (scnPrefabReletivePath);
		var scnGo = GameObject.Instantiate (obj) as GameObject;

		//根据不同类型类型场景分别填充数据

		FillDiffScnData (scnType,scnName,scnSubFolderPath,scnGo);

		DestroyImmediate(scnGo);
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
		string scnDataAssetPath = scnSubFolderPath + "/scnData.asset";

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

		if (!EditorHelper.BeFileExist (scnDataAssetPath)) 
		{
			Debug.LogError ("MeEditor.FillScnData3C scnData 不存在，创建新数据文件 path:"+scnDataAssetPath+" ");
			scn3CData = ScriptableHelper.CreateOrReplaceAsset<Scn3CData> (scn3CData,scnDataAssetReletivePath);
		} 
		else 
		{
			Debug.LogError ("MeEditor.FillScnData3C scnData存在，读取数据文件,判断是否有数据变更，有就覆盖数据  path:"+scnDataAssetPath);
			var oldScn3CData = AssetDatabase.LoadAssetAtPath<Scn3CData> (scnDataAssetReletivePath);
			bool bSame = Scn3CData.BeSame(scn3CData,oldScn3CData);

			if (!bSame) 
			{
				oldScn3CData.ScnName = scn3CData.ScnName;
				oldScn3CData.MapName = scn3CData.MapName;
				oldScn3CData.TowerDataList = new List<TowerData> ();
				oldScn3CData.TowerDataList.AddRange (scn3CData.TowerDataList.ToArray());
			}

		}

	}

	#endregion
}
