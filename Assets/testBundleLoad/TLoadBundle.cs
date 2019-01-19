using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 测试bundle载入
/// xml中记录项
/// bundle中加载asset注意事项等
/// </summary>
public class TLoadBundle : MonoBehaviour {

	string xmlPath
	{
		get
		{ 
			return Application.streamingAssetsPath;
		}
	}

	void Update () 
	{
		if (Input.GetKeyUp (KeyCode.A)) 
		{
			StartCoroutine (LoadXml(xmlPath));
		}

		if (Input.GetKeyUp (KeyCode.B)) 
		{
		
		}

	}

	public void LoadBundle()
	{
		
	}

	IEnumerator LoadXml(string xmlpath)
	{
		
		WWW www = new WWW(xmlpath);

		yield return www;

		if(null != www.error)Debug.Log("加载xmlBundle www error:"+www.error);
	

		string[] names  =www.assetBundle.GetAllAssetNames();
		for(int i=0;i<names.Length;i++)
		{	
			Debug.Log("---["+i+"]------>"+names[i]);
		}

		AssetBundleManifest xml = (AssetBundleManifest)www.assetBundle.LoadAsset("assetbundlemanifest");//这个名称永远固定，无论bundle名是什么，asset名都是这个

		string[] allAssetsBundles  = xml.GetAllAssetBundles ();

		string[] allDepend =  xml.GetAllDependencies ("assetBundleName");

		string[] directDepend = xml.GetDirectDependencies ("assetBundleName");

		Hash128 hash = xml.GetAssetBundleHash ("assetBundleName");

	}

	//后续测试暂时没必要做，直接看下TU5AssetBundle

}
