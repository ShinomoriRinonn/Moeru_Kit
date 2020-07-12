using UnityEngine;
using System.Collections;

public class EdgeDetectNormalsAndDepth : PostEffectsBase {

	public Shader edgeDetectShader;
	private Material edgeDetectMaterial = null;
	public Material material {  
		get {
			// Debug.Log("shadername: " + edgeDetectShader.name);
			edgeDetectMaterial = CheckShaderAndCreateMaterial(edgeDetectShader, edgeDetectMaterial);
			// UnityEngine.Debug.Log("############# edeDetectMaterial");
			string tac = edgeDetectMaterial == null ? "### no material" : "### yes we can";
			// Debug.Log(tac);
			
			return edgeDetectMaterial;
		}  
	}

	[Range(0.0f, 1.0f)]
	public float edgesOnly = 0.0f;

	public Color edgeColor = Color.black;

	public Color backgroundColor = Color.white;

	public float sampleDistance = 1.0f;

	public float sensitivityDepth = 1.0f;

	public float sensitivityNormals = 1.0f;
	
	Mesh CreateMesh(float width, float height)
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        //note: unity is left-hand system
        m.vertices = new Vector3[] {
            new Vector3(-width/2, -height/2, 0),
            new Vector3(-width/2, height/2, 0),
            new Vector3(width/2, height/2, 0),
            new Vector3(width/2, -height/2, 0)
        };
        m.uv = new Vector2[] {
            new Vector2 (0, 0),
            new Vector2 (0, 1),
            new Vector2 (1, 1),
            new Vector2 (1, 0)
        };
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3};
        m.RecalculateNormals();
        m.RecalculateBounds();
        return m;
    }
	

	void OnEnable() {
		// GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
		Mesh m = CreateMesh(1920, 1080);
		GameObject.
	}

	// [ImageEffectOpaque]
	void OnRenderImage (RenderTexture src, RenderTexture dest) {
		if (material != null) {
			Debug.Log("###################### OnRenderImage !!!!!!!!!!!!!");

			material.SetFloat("_EdgeOnly", edgesOnly);
			material.SetColor("_EdgeColor", edgeColor);
			material.SetColor("_BackgroundColor", backgroundColor);
			material.SetFloat("_SampleDistance", sampleDistance);
			material.SetVector("_Sensitivity", new Vector4(sensitivityNormals, sensitivityDepth, 0.0f, 0.0f));

			Graphics.Blit(src, dest, material);
		} else {
			Debug.Log("######################### No material on OnRenderImage !!!!!!!!!!!!!");
			Graphics.Blit(src, dest);
		}
	}
}
