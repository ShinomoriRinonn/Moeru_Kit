using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/*
    主题：
        想想办法 解决 onRenderImage输入下，DepthTexture丢失的问题

    次目标：
        OnPostRender	OnPostRender is called after a camera has finished rendering the Scene.
        OnPreCull	OnPreCull is called before a camera culls the Scene.
        OnPreRender	OnPreRender is called before a camera starts rendering the Scene.
        OnRenderImage	OnRenderImage is called after all rendering is complete to render image.
        OnRenderObject	OnRenderObject is called after camera has rendered the Scene.
        OnWillRenderObject

        7个玩意儿都干了啥
*/

public class CameraRunning : PostEffectsBase
{

	public Shader shader;
	private Material _mat = null;
	private List<Material> mats = new List<Material>();
    public Material material {  
		get {
			_mat = CheckShaderAndCreateMaterial(shader, _mat);
			string tac = _mat == null ? "### no material" : "### yes we can";
			return _mat;
		}  
	}
	[SerializeField]
	public float tileY = 1;
// #if UNITY_EDITOR
	public Vector3 offset = Vector2.zero;
    private Vector2 cameraNodeTrans = Vector2.zero;

	[SerializeField]
	public List<Vector3> mOffset = new List<Vector3>();
    public Texture vividTexture = null;
	private Mesh mesh = null;
	private CommandBuffer m_cmdDraw = null;
    Camera cam;
    bool revertFogState = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        prepareBufferWithChapterNum(1);
    }

	public void prepareBufferWithChapterNum(int chapterNum)
	{
		mats.Clear();
		for(var i =0; i< chapterNum; i++)
		{
			Material mat = null;
			mats.Add(CheckShaderAndCreateMaterial(shader, mat));
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPostRender()
    {
        Debug.Log("OnPostRender");
        /*
        OnPostRender is called after a camera has finished rendering the Scene.
        This message is sent to all scripts attached to the camera.
        */
        RenderSettings.fog = revertFogState;

        GL.invertCulling = false;
    }
    void OnPreCull()
    {
        /*
        OnPreCull is called before a camera culls the Scene.
        Culling determines which objects are visible to the camera. 
        OnPreCull is called just before this process. 
        This message is sent to all scripts attached to the camera.
        */
        // Debug.Log("OnPreCull");
        // cam.ResetWorldToCameraMatrix();
        // cam.ResetProjectionMatrix();
        // cam.projectionMatrix = cam.projectionMatrix * Matrix4x4.Scale(new Vector3(1, -1, 1));
    }
    void OnPreRender()
    {
        Debug.Log("OnPreRender");
        revertFogState = RenderSettings.fog;
        RenderSettings.fog = enabled;

        // GL.invertCulling = true;
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        /*
        OnRenderImage is called after all rendering is complete to render image.
        Postprocessing effects. 
        It allows you to modify final image by processing it with shader based filters. 
        The incoming image is source render texture. 
        The result should end up in destination render texture. 
        You must always issue a Graphics.Blit() or render a fullscreen quad if your override this method.
        */
        Debug.Log("OnRenderImage");
        // Copy the source Render Texture to the destination,
        // applying the material along the way.

        if (material != null && vividTexture != null) {

			if (m_cmdDraw == null)
            {
                m_cmdDraw = new CommandBuffer();
                m_cmdDraw.name = "Vivid Draw Mesh";
            }
			m_cmdDraw.Clear();

			for (var i = 0; i < mOffset.Count; i++){
				var offset = mOffset[i];
				var go = GameObject.FindGameObjectWithTag("CameraManager");
				var mainC = go.transform.Find("Main Camera").GetComponent<Camera>();
				cameraNodeTrans.Set(go.transform.localPosition.x, go.transform.localPosition.y);

				var shouldSize = Screen.height / (2 * 100f);
				var cameraRate = mainC.orthographicSize / shouldSize ;

				// var tileY = 2f;
				var deltaTielY = tileY - 1;

				var halfWidth = vividTexture.width / (2 * 100f);
				var halfHeight = vividTexture.height / (2 * 100f);

				var screenWidth = Screen.width / 100f;
				var screenHeight = Screen.height / 100f;

				var left = (offset.x - halfWidth) / (screenWidth * cameraRate) + 0.5f;
				var right = (offset.x + halfWidth) / (screenWidth * cameraRate) + 0.5f;
				var down = (offset.y - halfHeight * (1 + 2 * deltaTielY) - cameraNodeTrans.y) / (screenHeight *cameraRate)+ 0.5f;
				var up = (offset.y + halfHeight - cameraNodeTrans.y) / (screenHeight*cameraRate) + 0.5f;

				var scaleVec = new Vector2(right - left, up - down);
				var offsetVec = new Vector2(left, down);

				var position = offset;
				var mesh = new Mesh();

				mesh.vertices = new Vector3[] {
						new Vector3(- halfWidth, - halfHeight - deltaTielY * 2 * halfHeight, 0f),
						new Vector3(- halfWidth, + halfHeight, 0f),
						new Vector3(+ halfWidth, + halfHeight, 0f),
						new Vector3( + halfWidth, - halfHeight - deltaTielY * 2 * halfHeight, 0f),
				};
				// openGL
				mesh.uv = new Vector2[]{
					new Vector2(0, 0),
					new Vector2(0, 1),
					new Vector2(1, 1),
					new Vector2(1, 0),
				};

				int[] newTriangles = { 0, 1, 2, 0, 2, 3 };
				mesh.triangles = newTriangles;

				var material = mats[i];
				material.SetTextureScale("_MainTex", scaleVec);
				material.SetTextureOffset("_MainTex", offsetVec);

				// Debug.Log("Texture WH : " + halfWidth +  ", "+ halfHeight );

				material.SetTexture("_BlendTex", vividTexture);
				// material.SetTextureScale("_BlendTex", new Vector2(1f, tileY));
				material.SetTextureOffset("_BlendTex", Vector2.zero);

				
				Matrix4x4 mt = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);// new Vector3(1, tileY, 1));
				m_cmdDraw.DrawMesh(mesh, mt, material);
				// Graphics.ExecuteCommandBuffer(m_cmdDraw);
				// m_cmdDraw.Clear();
			}
			Graphics.ExecuteCommandBuffer(m_cmdDraw);

        }
		Graphics.Blit(source, destination);
    }
    void OnRenderObject()
    {
        Debug.Log("OnRenderObject");
    }
    void OnWillRenderObject()
    {
        Debug.Log("OnWillRenderObject");
    }
}
