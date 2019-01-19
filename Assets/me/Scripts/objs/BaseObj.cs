using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObj : MonoBehaviour {

	public string Type;

	public virtual string GetType(){
		return null;
	}
}
