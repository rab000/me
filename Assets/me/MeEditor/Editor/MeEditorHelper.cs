using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeEditorHelper  {

	#region 编辑器内部路径

	// 编辑器根目录
	public static string EDITOR_ROOT_PATH{
		get{
			return EditorHelper.EDITOR_ASSETS_PATH+"/me";
		}
	} 

	// 场景资源路径
	public static string SCN_PATH{
		get{
			return EDITOR_ROOT_PATH+"/Resources/scn";
		}
	}

	// 地图资源路径
	public static string MAP_PATH{
		get{
			return EDITOR_ROOT_PATH+"/Resources/map";
		}
	}

	// 地图物件路径
	public static string OBJ_PATH{
		get{
			return EDITOR_ROOT_PATH+"/Resources/objs";
		}
	}



	#endregion

	#region 编辑器输出路径

	/// <summary>
	/// 输出ab根目录
	/// </summary>
	/// <value>The output root path.</value>
	public static string OUTPUT_ROOT_PATH{
		get{
			return EditorHelper.GetParentFolderPath(Application.dataPath)+"/data";
		}
	}

	/// <summary>
	/// 配表资源输出目录
	/// </summary>
	/// <value>The output table path.</value>
	public static string OUTPUT_TABLE_PATH{
		get{
			return OUTPUT_ROOT_PATH+"/table";
		}
	}

	/// <summary>
	/// 地图配表输出路径
	/// </summary>
	/// <value>The OUTPU t_ TABL e_ MA p_ PAT.</value>
	public static string OUTPUT_TABLE_MAP_PATH{
		get{
			return OUTPUT_TABLE_PATH+"/map";
		}
	}

	/// <summary>
	/// 场景配表输出路径
	/// </summary>
	/// <value>The OUTPU t_ TABL e_ SCEN e_ PAT.</value>
	public static string OUTPUT_TABLE_SCENE_PATH{
		get{
			return OUTPUT_TABLE_PATH+"/scene";
		}
	}

	/// <summary>
	/// 系统配表输出路径
	/// </summary>
	/// <value>The OUTPU t_ TABL e_ SYSTE m_ PAT.</value>
	public static string OUTPUT_TABLE_SYSTEM_PATH{
		get{
			return OUTPUT_TABLE_PATH+"/system";
		}
	}

	/// <summary>
	/// AB资源输出目录
	/// </summary>
	/// <value>The output res path.</value>
	public static string OUTPUT_RES_PATH{
		get{
			return OUTPUT_ROOT_PATH+"/res/me/";
		}
	}



	#endregion
}
