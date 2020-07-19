使用方法：
1.导入OutlinePackage
2.在主相机上添加脚本Outline/Scripts/outlineCamera(需要物体上有Camera组件)
3.将Outline/Shaders/目录下的rtShader和displayShader拖给outlineCamera上的对应变量
4.在所有需要描边的物体上添加脚本Outline/Scripts/outlineDrawer(需要物体上有renderer)
6.调整outlineDrawer的包围盒使其能够包围住物体网格且尽量小
5.运行状态下可看到描边效果

参数说明：
outlineCamera.cs：
Outline Scale: 描边宽度
Outline Color: 描边颜色
Margin: RenderTexture上两个物体之间采样保护区域的像素宽度
Rt Width, Rt Height: 创建的RenderTexture的像素宽高,不要在运行时修改
Display Pos, Display Dir: 解决近距离遮挡问题的参数,一般情况使用零向量即可

outlineDrawer.cs:
BdBox Center, BdBox Size: 包围盒中心和大小

使用注意事项：
1.本工具需要手动为每个描边物体调整大小合适的包围盒来确定屏幕范围;
2.不要把描边物体拉得太大或离相机太近,或是创建过大的包围盒,这都会导致物体在屏幕上占据的范围过大,占据屏幕范围超出render texture分辨率的物体描边不会被绘制;
3.两个物体距离过近/穿插时,描边的遮挡关系可能会不正常;
4.根据需要描边的物体大小和数量设置尽量小的Rt Width, Rt Height以降低开销