using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : BaseObj  {

	//阵营
	public byte Camp;

	public override string GetType(){

		if (null == Type)
			Type = "town";
		return Type;
	}
}
