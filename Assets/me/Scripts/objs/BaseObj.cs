using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 专用于标识me中的object
/// 只记录类型，不记录数据，也不在me外使用
/// </summary>
public class BaseObj : MonoBehaviour {

	public string Type;

	public virtual string GetType(){
		return null;
	}
}
