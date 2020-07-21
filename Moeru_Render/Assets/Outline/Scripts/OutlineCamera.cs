using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[RequireComponent(typeof(Camera))]
public class OutlineCamera : MonoBehaviour
{

    private List<OutlineDrawer> outlineList = new List<OutlineDrawer>();// 存放所有outlineObj的list
    public List<RenderTexture> rtList = new List<RenderTexture>();//render texture list
    private CommandBuffer m_cb = null;
    private Material rtMat = null;
    private Material displayMat = null;
    private MaterialPropertyBlock rtMatBlock = null;
    private MaterialPropertyBlock displayMatBlock = null;
    private Mesh displayMesh = null;

    // serialized
    [SerializeField, Range(0.0f, 10.0f)]
    private float outlineScale = 3.0f;// 描边宽度
    [SerializeField]
    private Color outlineColor = Color.black;// 描边颜色
    [SerializeField, Range(256, 4096)]
    private int rtWidth = 1024;// rt宽度
    [SerializeField, Range(256, 4096)]
    private int rtHeight = 512;// rt高度
    [SerializeField, Range(5, 50)]
    private int margin = 10;// rt上的保护区域宽度
    [SerializeField]
    private Vector3 displayPos;// 展示平面的位移相对描边物体position的位移
    [SerializeField]
    private Vector3 displayDir;// 展示平面的朝向
    [SerializeField]
    private Shader displayShader;// Outline/Shaders/displayShader.shader
    [SerializeField]
    private Shader rtShader;// Outline/Shaders/rtShader.shader

    void OnEnable()
    {
        // Debug.Log("outlineCamera OnEnable");
        if (m_cb == null) m_cb = new CommandBuffer();
        if (rtMat == null) rtMat = new Material(rtShader);
        if (displayMat == null) displayMat = new Material(displayShader);
        if (rtMatBlock == null) rtMatBlock = new MaterialPropertyBlock();
        if (displayMatBlock == null) displayMatBlock = new MaterialPropertyBlock();
        if (displayMesh == null)// 创建面片
        {
            Vector3[] newVertices = { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f) };
            Vector2[] newUV = { new Vector2(0.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector2(1.0f, 1.0f), new Vector2(1.0f, 0.0f) };
            int[] newTriangles = { 0, 1, 2, 0, 2, 3 };
            displayMesh = new Mesh();
            displayMesh.vertices = newVertices;
            displayMesh.uv = newUV;
            displayMesh.triangles = newTriangles;
        }

        AddRenderTexture();
    }

    void OnDisable()
    {
        foreach (var item in rtList)
        {
            RenderTexture.ReleaseTemporary(item);
        }
        rtList.Clear();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int rtIndex = 0;
        int usedX = 0;

        m_cb.Clear();
        m_cb.SetRenderTarget(rtList[rtIndex]);
        m_cb.ClearRenderTarget(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));

        OutlineDrawer drawer = null;
        Vector4 resolution = new Vector4(Camera.main.pixelWidth, Camera.main.pixelHeight, rtWidth, rtHeight);
        Vector4 bounding = Vector4.zero;
        Vector4 bias = Vector4.one;
        Vector2 area = Vector2.zero;
        for (int i = 0; i < outlineList.Count; i++)
        {
            drawer = outlineList[i];
            bounding = CalcBounding(drawer.GetBoundingPoints());
            area.Set(bounding.y - bounding.x + 1 + 2 * margin, bounding.w - bounding.z + 1 + 2 * margin);
            // 判断是否满足绘制条件: 在屏幕内且覆盖范围没有超过render texture的分辨率
            drawer.drawable = bounding.x < resolution.x && bounding.y > 0 && bounding.z < resolution.y && bounding.w > 0 && area.x <= rtWidth && area.y <= rtHeight;
            if (!drawer.drawable) continue;// 不满足绘制条件则跳过

            if(usedX + area.x > rtWidth)// 如果当前rt放不下,就执行当前cb,并将新的cb的rendertarget设为下一张rt 
            {
                if(rtIndex >= rtList.Count - 1) AddRenderTexture();//如果已用尽list中的所有rt,则创建新的一张
                rtIndex++;
                usedX = 0;
                Graphics.ExecuteCommandBuffer(m_cb);
                m_cb.Clear();
                m_cb.SetRenderTarget(rtList[rtIndex]);
                m_cb.ClearRenderTarget(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
            }

            Debug.Log("### bounding -- " + "x: " + bounding.x + ", y: " + bounding.y + ", z: " + bounding.z + ", w: " + bounding.w);

            // 根据bounding分配一个渲染纹理上的区域
            usedX += margin;
            bias.z = usedX - bounding.x;
            bias.w = margin - bounding.z;
            usedX = (int)(usedX + bounding.y - bounding.x + 1);
            // 设置材质参数并将绘制命令加入command buffer
            rtMatBlock.SetVector("_ViewportBias", bias);
            rtMatBlock.SetVector("_Resolution", resolution);
            drawer.rdr.SetPropertyBlock(rtMatBlock);
            m_cb.DrawRenderer(drawer.rdr, rtMat);  // drawer.rdr 即 model节点下 skinMeshRenderer， rtMat 即 new material(some shader)
            drawer.bounding = bounding;
            drawer.bias = bias;
            drawer.rtIndex = rtIndex;
        }

        Graphics.ExecuteCommandBuffer(m_cb);

        if (rtIndex < rtList.Count - 2)// 如果至少有两张多余rt,则进行删除
        {
            for (rtIndex++; rtIndex < rtList.Count;)
            {
                RenderTexture temp = rtList[rtIndex];
                rtList.Remove(rtList[rtIndex]);
                RenderTexture.ReleaseTemporary(temp);
            }
        }

        m_cb.Clear();
        for (int i = 0; i < outlineList.Count; i++)
        {
            if (outlineList[i].drawable)
            {
                // 设置材质参数并将绘制命令加入command buffer
                displayMatBlock.SetVector("_ViewportBias", outlineList[i].bias);
                displayMatBlock.SetVector("_Bounding", outlineList[i].bounding);
                displayMatBlock.SetTexture("_MainTex", rtList[outlineList[i].rtIndex]);
                displayMatBlock.SetFloat("_Margin", margin);
                displayMatBlock.SetFloat("_OutlineScale", outlineScale);
                displayMatBlock.SetColor("_OutlineColor", outlineColor);
                Vector3 src = outlineList[i].transform.position + displayPos;
                Vector3 dst = src + displayDir;
                m_cb.DrawMesh(displayMesh, Matrix4x4.LookAt(src, dst, Vector3.up), displayMat, 0, -1, displayMatBlock);
            }
                
        }
        Graphics.ExecuteCommandBuffer(m_cb);

        Graphics.Blit(source, destination);
    }

    /// <summary>
    /// 根据包围盒计算出物体在屏幕坐标上占据的范围
    /// </summary>
    /// <param name="boundingPoints"></param>
    /// <returns></returns>
    private Vector4 CalcBounding(Vector3[] boundingPoints)
    {
        int X_screen = Camera.main.pixelWidth;
        int Y_screen = Camera.main.pixelHeight;

        float minX = Mathf.Infinity, maxX = -Mathf.Infinity, minY = Mathf.Infinity, maxY = -Mathf.Infinity;
        Vector3 screenPos;
        for (int i = 0; i < 8; i++)
        {
            screenPos = Camera.main.WorldToViewportPoint(boundingPoints[i]);
            // Debug.Log("### before");
            XYDUtils.printVector3(screenPos);

            if (screenPos.z < 0)
            {
                screenPos.Set(1.0f - screenPos.x, 1.0f - screenPos.y, 1.0f - screenPos.z);
                Debug.Log("### after");
                XYDUtils.printVector3(screenPos);
            }
            screenPos.Set(screenPos.x * X_screen, screenPos.y * Y_screen, screenPos.z);
            minX = Mathf.Min(minX, screenPos.x);
            maxX = Mathf.Max(maxX, screenPos.x);
            minY = Mathf.Min(minY, screenPos.y);
            maxY = Mathf.Max(maxY, screenPos.y);
        }

        return new Vector4(Mathf.Floor(minX), Mathf.Ceil(maxX), Mathf.Floor(minY), Mathf.Ceil(maxY));
    }

    /// <summary>
    /// 创建一张新的rt并加入RtList
    /// </summary>
    private void AddRenderTexture()
    {
        RenderTexture rt = RenderTexture.GetTemporary(rtWidth, rtHeight, 0, RenderTextureFormat.RGB565, RenderTextureReadWrite.sRGB, 1);
        rt.filterMode = FilterMode.Point;// 屏幕像素与rt像素是一一对应的关系,纹理过滤设置为Point
        rt.wrapMode = TextureWrapMode.Clamp;
        rtList.Add(rt);
    }

    /// <summary>
    /// 添加描边物体
    /// </summary>
    /// <param name="drawer"></param>
    /// <returns></returns>
    public bool AddOutlineObject(OutlineDrawer drawer)
    {
        // Debug.Log("AddOutlineObject : " + renderer.gameObject.name);
        foreach (var item in outlineList)
        {
            if (item == drawer)
            {
                Debug.LogWarning("OutlineDrawer already exist in the List!");
                return false;
            }
        }

        outlineList.Add(drawer);

        return true;
    }

    /// <summary>
    /// 移除描边物体
    /// </summary>
    /// <param name="drawer"></param>
    /// <returns></returns>
    public bool RemoveOutlineObject(OutlineDrawer drawer)
    {
        foreach (var item in outlineList)
        {
            if (item == drawer)
            {
                outlineList.Remove(drawer);
                return true;
            }
        }
        Debug.LogWarning("No such OutlineDrawer in outlineList!");
        return false;
    }



}
