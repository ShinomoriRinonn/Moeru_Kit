using System.Linq;
using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Rendering;


[System.Serializable]
public class VividLightPostEffect : PostEffectsBase {

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

    public Texture vividTexture = null;
	private Mesh mesh = null;
	private CommandBuffer m_cmdDraw = null;

    // #if UNITY_EDITOR
	public Vector3 offset = Vector2.zero;
    private Vector2 cameraNodeTrans = Vector2.zero;

	[SerializeField]
	public List<Vector3> mOffset = new List<Vector3>();

	void OnEnable() {

	}

	public void ClearVividOffset()
	{
		mOffset.Clear();
	}

	public void PushVividOffset(Vector3 input)
	{
		mOffset.Add(input);
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

	void OnPreRender(){
	}

	void OnGUI(){

	}

	void OnRenderImage (RenderTexture src, RenderTexture dest) {
		if (material != null && vividTexture != null) {

			if (m_cmdDraw == null)
            {
                m_cmdDraw = new CommandBuffer();
                m_cmdDraw.name = "Vivid Draw Mesh";
            }
			m_cmdDraw.Clear();

			for (var i = 0; i < mOffset.Count; i++){
				var offset = mOffset[i];
				var go = XYDUtils.FindGameObject("CameraManager");
				var mainC = go.NodeByName("Main Camera").GetComponent<Camera>();
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
				// scaleVec = Vector2.one;
				// offsetVec = Vector2.zero;

				var position = offset;
				var mesh = new Mesh();
				
				// var vertices = new List<Vector3>();
				// for(var i = 0; i < Math.Ceiling(tileY); i++)
				// {
				// 	vertices.Add(new Vector3(- halfWidth, - halfHeight - i * halfHeight * 2, 0f));
				// 	vertices.Add(new Vector3(- halfWidth, + halfHeight - i * halfHeight * 2, 0f));
				// 	vertices.Add(new Vector3(+ halfWidth, + halfHeight - i * halfHeight * 2, 0f));
				// 	vertices.Add(new Vector3(+ halfWidth, - halfHeight - i * halfHeight * 2, 0f));
				// }

				mesh.vertices = new Vector3[] {
						new Vector3(- halfWidth, - halfHeight - deltaTielY * 2 * halfHeight, 0f),
						new Vector3(- halfWidth, + halfHeight, 0f),
						new Vector3(+ halfWidth, + halfHeight, 0f),
						new Vector3( + halfWidth, - halfHeight - deltaTielY * 2 * halfHeight, 0f),
						// new Vector3(- halfWidth, - halfHeight, 0f),
						// new Vector3(- halfWidth, + halfHeight, 0f),
						// new Vector3(+ halfWidth, + halfHeight, 0f),
						// new Vector3( + halfWidth, - halfHeight, 0f),


						// new Vector3(- halfWidth, - halfHeight * 2, 0f),
						// new Vector3(- halfWidth, - halfHeight, 0f),
						// new Vector3(+ halfWidth, - halfHeight, 0f),
						// new Vector3(+ halfWidth, - halfHeight * 2, 0f),
				};
				// openGL
				mesh.uv = new Vector2[]{
					new Vector2(0, 0),
					new Vector2(0, 1),
					new Vector2(1, 1),
					new Vector2(1, 0),

					// new Vector2(0, 0),
					// new Vector2(0, 0.5f),
					// new Vector2(0.5f, 0.5f),
					// new Vector2(0.5f, 0),
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

			// m_cmdDraw.DrawMesh(m_meshPoint, Matrix4x4.identity, m_matVisualize, 0, (int)VisualizeType.RayPosition);

		}
		Graphics.Blit(src, dest);
	}

}
