using UnityEngine;
using System.Collections;
//using UnityEditor;
using System;
using System.IO;

/// <summary>
/// Unity5 export AB Demo
/// 测试资源为Cube.prefab  mat.mat  tex.png     
/// Cube.prefab依赖mat.mat,
/// mat上的贴图是tex.png(可以认为这两个一起加载，一起卸载，所以不需要再建立一级依赖)
/// 
/// 打包前的初始设置
/// Cube.prefab assetbundleName 设置为cube 后缀为n
/// mat.mat     assetbundleNmae 设置为mat  后缀为n
/// 
/// 坑：关于新增打包，如果名称改了，unity不会清理旧的资源(会导致打出的output中有冗余资源)
/// </summary>
//public class TU5Assetbundle : MonoBehaviour {
//
//
//	/// <summary>
//	/// 输出assetbundle的文件夹名
//	/// 打包时回生成跟这个输出文件夹同名的xml和bundle文件
//	/// 里面记录全部资源及依赖信息，所以这个输出文件夹名很重要
//	/// </summary>
//	static string OutPutFolderName = "output";
//
//	
//	/// <summary>
//	/// 输出assetbundle的目录的完整路径
//	/// </summary>
//	/// <value>The out put path.</value>
//	static string OutPutPath{
//
//		get{
//			return Application.dataPath +"/"+OutPutFolderName;
//		}
//	}
//
//	/// <summary>
//	/// 导出所有设置了assetbundleName的资源
//	/// 这种打包非单独打包，mainAsset永远是null
//	/// LoadAsset需要使用资源原始名(不加后缀)来加载
//	/// 或者使用相对资源路径(例如assets/temp/tu5/cube0.prefab 这一段就是www.assetBundle.GetAllAssetNames得到的)
//	/// </summary>
//	[MenuItem("NEditor/Test/U5ExportAB")]
//	static void CreateAB(){
//
//		if(!Directory.Exists(OutPutPath))AssetDatabase.CreateFolder("Assets", OutPutFolderName);
//
//		BuildPipeline.BuildAssetBundles(OutPutPath,BuildAssetBundleOptions.CollectDependencies,BuildTarget.StandaloneWindows);
//		//BuildPipeline.
//		AssetDatabase.Refresh();
//
//
//	}
//	
//
//	void Update(){
//
//		//测试载入
//		if(Input.GetKeyUp(KeyCode.X)){
//
//			StartCoroutine(Load ());
//
//		}
//	}
//	
//
//	/// <summary>
//	/// 加载包含依赖资源的bundle
//	/// </summary>
//	IEnumerator Load(){
//	
//		Debug.Log("step1----------------加载xml的assetbundle");
//
//		//注意:1 pc上www需要加"file:///"  ，
//		//     2 这里最后一个output是xml bundle的资源文件名
//		string path = "file:///"+OutPutPath + "/output";
//
//		WWW www = new WWW(path);
//
//		yield return www;
//
//		if(null != www.error)Debug.Log("加载xmlBundle www error:"+www.error);
//
////		查看下xml中asset的名称，里面固定是名称为assetbundlemanifest的asset资源
////		string[] names  =www.assetBundle.GetAllAssetNames();
////		for(int i=0;i<names.Length;i++){	
////			Debug.Log("---["+i+"]------>"+names[i]);
////		}
//
//		AssetBundleManifest xml = (AssetBundleManifest)www.assetBundle.LoadAsset("assetbundlemanifest");//这个名称永远固定，无论bundle名是什么，asset名都是这个
//
//
//		Debug.Log("step2----------------加载Cube的依赖资源");
//
//		//这个名称cube是自己设置的assetbundle的名称，包含后缀(不是原始prefab名称)
//		string[] depends = xml.GetAllDependencies("cube.n");
//
//		AssetBundle[] dependAB = new AssetBundle[depends.Length]; 
//
//		for(int i=0;i<depends.Length;i++){
//
//			Debug.Log("加载依赖 ["+i+"]------>"+depends[i]);
//
//			string dependABPath = "file:///"+OutPutPath + "/"+depends[i];
//
//			www = new WWW(dependABPath);
//
//			yield return www;
//
//			if(null != www.error)Debug.Log("加载依赖bundle www error:"+www.error);
//
//			//?这里有个问题，是否加载到assetBundle就可以了，貌似应该加载到asset才行，否则依赖丢失
//			//经过测试，只要有下面一句赋值就不会丢失依赖，如果只是加载了www，而不使用www.assetBundle，那么依赖实际上没有被加载出来
//			dependAB[i] = www.assetBundle;
//
//		}
//	
//		Debug.Log("step2----------------加载Cube的assetbundle资源");
//
//		path = "file:///"+OutPutPath + "/cube.n";
//
//		www = new WWW(path);
//		
//		yield return www;
//		
//		if(null != www.error)Debug.Log("www error:"+www.error);
//
//		string[] names  =www.assetBundle.GetAllAssetNames();
//
//		//Debug.Log("mainAsset:"+www.assetBundle.mainAsset.name);//mainAsset是null的
//
//		for(int i=0;i<names.Length;i++){	
//			Debug.Log("ff---["+i+"]------>"+names[i]);
//			//？这个asset名字奇怪是原始prefab的路径，还有后缀名.prefab
//			//ff---[0]------>assets/temp/tu5/cube0.prefab,   holyFuck，这还怎么开心的加载asset，用这么个奇葩路径加载？循环获取这个路径？
//			//只要prefab名和ab名同名就可以解决这个问题了
//		}
//		//asset名不是ab资源名，是原始prefab的相对路径，所以只能这么加载了
//		//UnityEngine.Object obj = www.assetBundle.LoadAsset(names[0]);
//		//还可以使用原始prefab名，只用名称不加路径，不加后缀，也能加载到
//		UnityEngine.Object obj = www.assetBundle.LoadAsset("Cube");
//		if(null==obj)Debug.Log("obj === null");
//
//		GameObject go = GameObject.Instantiate(obj) as GameObject;
//
//
//	}
//}
