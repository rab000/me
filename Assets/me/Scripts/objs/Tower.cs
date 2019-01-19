using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : BaseObj {
	
	public byte Camp;

	/// <summary>
	/// 优先级
	/// </summary>
	public int Priority;

	public override string GetType(){
		
		if (null == Type)
			Type = "tower";

		return Type;
	}

}
