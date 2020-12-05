// using System.Linq;
// using System.Xml.Serialization;
// using System.Linq.Expressions;
// using UnityEngine;
// using UnityEngine.Rendering;
// using System.Collections;
// using System.Collections.Generic;
// public class OutlineCamera : MonoBehaviour
// {
//     private List<OutlineDrawer> outlineList = new List<OutlineDrawer>();  // 当outlineDrawer Enable时被添加进来
//     private List<RenderTexture> rtList = new List<RenderTexture>(); 
//     private CommandBuffer m_cb = null;
//     private Material rtMat = null;
//     private Material displayMat = null;
//     private MaterialPropertyBlock rtMatBlock = null;
//     private MaterialPropertyBlock displayMatBlock = null;
//     private Mesh displayMesh = null;

//     //
//     [SerializeField, Range(0.0f, 10.0f)]
//     private float outlineScale = 3.0f; // 描边宽度
//     [SerializeField]
//     private Color outlineColor = Color.black; // 描边颜色
//     [SerializeField, Range(256, 4096)]
//     private int rtWidth = 1024;
//     [SerializeField, Range(256, 4096)]
//     private int rtHeight = 512;
//     [SerializeField, Range(5, 50)]
//     private int margin = 10; // rt上的保护区域宽度
//     [SerializeField]
//     private Vector3 displayPos; // 展示平面的位移 相对描边物体position的位移
//     [SerializeField]
//     private Vector3 displayDir; // 展示平面的朝向
//     [SerializeField]
//     private Shader displayShader; // Outline / Shaders / displayShader.shader
//     [SerializeField]
//     private Shader rtShader; // Outline / Shaders / rtShader.shader

//     void OnEnable()
//     {
//         if (m_cb == null) m_cb = new CommandBuffer();
//         if (rtMat == null) rtMat = new Material(rtShader);
//         if (displayMat == null) displayMat = new Material(displayShader);

//         if (rtMatBlock == null) rtMatBlock = new MaterialPropertyBlock();
//         if (displayMatBlock == null) displayMatBlock = new MaterialPropertyBlock();   // 为啥需要这样整，判断OnEnable会重复跑，而不希望执行重复的构造？

//         if (displayMesh == null) // 创建面片
//         {
//             Vector3[] newVertices = { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)};
//             Vector2[] newUV = { new Vector2(0.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector2(1.0f, 1.0f), new Vector2(1.0f, 0.0f)};
//             int[] newTriangles = { 0, 1, 2, 0, 2, 3};
            
//             displayMesh =  new Mesh();
//             displayMesh.vertices = newVertices;
//             displayMesh.uv = newUV;
//             displayMesh.triangles = newTriangles;
//         }

//         AddRenderTexture();
//     }

//     void OnDisable() {
//         foreach ( var item in rtList)
//         {
//             RenderTexture.ReleaseTemporary(item);
//         }
//         rtList.Clear();
//     }

//     void OnRenderImage(RenderTexture source, RenderTexture destination)
//     {
//         int rtIndex = 0;
//         int usedX = 0;

//         m_cb.Clear();
//         m_cb.SetRenderTarget(rtList[rtIndex]);
//         m_cb.ClearRenderTarget(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
//         //至此 把rtList[0]指向的1024x512酱 填充成了depth, color都被清理的，backgroundColor被填充为 (0,0,0,0)透明白，depth被填充为1

//         OutlineDrawer drawer = null;
//         Vector4 resolution = new Vector4(Camera.main.pixelWidth, Camera.main.pixelHeight, rtWidth, rtHeight);
//         Vector4 bounding = Vector4.zero;
//         Vector4 bias = Vector4.zero;
//         Vector2 area = Vector2.zero;
//         for (int i = 0; i< outlineList.Count; i++)
//         {
//             drawer = outlineList[i];
//             bounding = CalcBounding(drawer.GetBoundingPoints());
//             area.Set(bounding.y- bounding.x + 1 + 2 * margin, bounding.x - bounding.z + 1 + 2 * margin); // 注意 (delta +1)描述像素宽度是合理的！
//             drawer.drawable = bounding.x < resolution.x && bounding.y > 0 && bounding.z < resolution.y && bounding.w > 0 && area.x <= rtWidth && area.y <= rtHeight;
//             if (!drawer.drawable) continue;

//             if (usedX + area.x > rtWidth) // 如果当前rt放不下，就执行当前cb，并将新的cb的rendertarget设为下一张rt
//             {
//                 if ( rtIndex >= rtList.Count - 1) AddRenderTexture();
//                 rtIndex ++; // 创建新的RenderTexture并更新Index
//                 usedX = 0;
//                 Graphics.ExecuteCommandBuffer(m_cb);
//                 m_cb.Clear();
//                 m_cb.SetRenderTarget(rtList[rtIndex]);
//                 m_cb.ClearRenderTarget(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
//             }

//             usedX += margin;
//             bias.z = usedX - bounding.x;
//             bias.w = margin - bounding.z;
//             usedX = (int)(usedX + bounding.y - bounding.x + 1);

//             rtMatBlock.SetVector("_ViewportBias", bias);
//             rtMatBlock.SetVector("_Resolution", resolution);
//             drawer.rdr.SetPropertyBlock(rtMatBlock);
//             m_cb.DrawRenderer(drawer.rdr, rtMat);

//             drawer.bounding = bounding;
//             drawer.bias = bias;
//             drawer.rtIndex = rtIndex;

//         }

//         Graphics.ExecuteCommandBuffer(m_cb);
        
//         if(){
//             RenderTexture.ReleaseTemporary(temp);
//         }

//         m_cb.Clear();
//         for (int i = 0; i < outlineList.Count; i++)
//         {
//             if(outlineList[i].drawable)
//             {
//                 displayMatBlock.SetVector("_ViewportBias", outlineList[i].bias);
                                                              
//             }
//         }

        
//     }

//     private void AddRenderTexture()
//     {
//         RenderTexture rt = RenderTexture.GetTemporary(rtWidth, rtHeight, 0, RenderTextureFormat.RGB565, RenderTextureReadWrite.sRGB, 1); // 深度0位
//         rt.filterMode = FilterMode.Point;
//         rt.wrapMode = TextureWrapMode.Clamp;
//         rtList.Add(rt);
//     }

//     private void CalcBounding(Vector3[] boundingPoints)
//     {
//         int X_screen = Camera.main.pixelWidth;
//         int Y_screen = Camera.main.pixelHeight;

//         float minX = Mathf.Infinity, maxX = -Mathf.Infinity, minY = Mathf.Infinity, maxY = -Mathf.Infinity;
//         Vector3 screenPos;
//         for (int i = 0; i < 8; i++)
//         {
//             screenPos = Camera.main.WorldToViewportPoint(boundingPoints[i]);
//             if (screenPos.z < 0)
//             {
//                 screenPos.Set(1.0f - screenPos.x, 1.0f - screenPos.y, 1.0f - screenPos.z); // ??
//             }
//             screenPos.Set(screenPos.x * X_screen, screenPos.y * Y_screen, screenPos.z);

//         }
//     }

// }
