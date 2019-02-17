//目录结构
//1 scn 里面包含只包含scnData数据，里面包含地图名称，引用的obj的类型及obj其他数据
//2 map 就是地图资源本身
//3 objs 副本之上的动态扩展资源


//注意点 scn中的prefab目的是为了生成scn中的scnData数据
//    scn prefab上map节点名称必须和具体map地图命名一致，这样才能知道scn中到底用的哪个map
//    scn 中具体子文件夹的命名为  scnType_scnName,这样做的好处是直观  


//  MeEditorHelper.OUTPUT_RES_PATH与bundleName的羁绊
//  首先注意xml文件是产生在前者的路径下的
//	1
//  前者设置为OUTPUT_ROOT_PATH+"/res/me";
//	后者设置为"map/" + mapName + "/" + fileNameWithoutExt
//  此时可以正常打包，但是打出来的包的bundleName并不是 me/map/mapName/fileName,缺少了me的开头
//	2
//	如果给后者加上me,bundleName变为"me/map/" + mapName + "/" + fileNameWithoutExt
//	那么打包位置就会变成OUTPUT_ROOT_PATH+"/res/me/me/.......";多了一级me，这时连续两个同名，unity打包就会报错
//	3
//	如果前者设置为OUTPUT_ROOT_PATH+"/res"
//  后者设置为"me/map/" + mapName + "/" + fileNameWithoutExt
//  这样也不行，因为xml文件会直接生成到OUTPUT_ROOT_PATH+"/res"下，这个路径是所有资源的根路径，不是me资源的根路径，而且名称是res，而不是me

//	目前使用的方案是1，在游戏中读取xml中所有资源路径时，给所有资源手动加上"me"的开头
