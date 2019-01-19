using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 双方对战类地图数据
/// </summary>
public class Scn3CData :ScriptableObject {

	public string ScnName;

	public string MapName;

	public List<TowerData> TowerDataList;

	/// <summary>
	/// 比较两个Scn3CData是否相同
	/// </summary>
	/// <returns><c>true</c>, if same was been, <c>false</c> otherwise.</returns>
	public static bool BeSame(Scn3CData a,Scn3CData b)
	{
		if (!a.ScnName.Equals (b.ScnName))
			return false;
		
		if (!a.MapName.Equals (b.MapName))
			return false;


		if (null == a.TowerDataList && null != b.TowerDataList)
			return false;
		
		if (null == b.TowerDataList && null != a.TowerDataList)
			return false;

		if (a.TowerDataList.Count != b.TowerDataList.Count)
			return false;

		int count = a.TowerDataList.Count;

		for (int i = 0; i < count; i++) 
		{
			if(a.TowerDataList[i].Camp != b.TowerDataList[i].Camp)
				return false;

			if(a.TowerDataList[i].Pos != b.TowerDataList[i].Pos)
				return false;

			if(a.TowerDataList[i].Priority != b.TowerDataList[i].Priority)
				return false;
		}

		return true;
	}
}
