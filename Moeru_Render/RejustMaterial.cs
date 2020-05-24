using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejustMaterial : MonoBehaviour
{
    // Start is called before the first frame update	public bool showInPanelTool = true;

	/// <summary>
	/// Whether normals and tangents will be generated for all meshes.
	/// </summary>
    public int startingRenderQueue = 3000;

	/// <summary>
	/// Whether the soft border will be used as padding.
	/// </summary>

	public bool softBorderPadding = true;
    private SpriteRenderer sr = null;
    void Start()
    {
        // this.gameObject.layer = 
        // sr = this.gameObject.GetComponent<SpriteRenderer>();
        // // print(sr.sharedMaterial);
        // // print(sr.material);

        // // sr.material = new UnityEngine.Material(sr.material);
        // // print(sr.sharedMaterial);
        // // print(sr.material);

        // Component[] list = this.gameObject.GetComponentsInChildren<UnityEngine.Renderer>();
        // for (int i = 0; i < list.Length; i++)
        // {
        //     print("indice: " + i.ToString() + ", "  + list[i].gameObject.name);
        // }

        // MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        // // sr.GetPropertyBlock(mpb);
        // // mpb.SetInt("_ZTest", 8);//UnityEngine.Rendering.CompareFunction.Always);
        // // mpb.SetFloat("_ztestOP", 10f);
        // // Shader._ztestOP
        // // sr.SetPropertyBlock(mpb);
        // ParticleSystemRenderer psr = new ParticleSystemRenderer();
        
        // sr.sortingLayerName = "2";
        // // sr.sortingLayerID = 2;
        // Debug.Log(sr.sortingLayerID);
        // // sr.sortingOrder = 34000;
        // Debug.Log(sr.sortingOrder);
        // // this.gameObject.transform.localt
        // // this.gameObject.transform
    }

    // Update is called once per frame
    void Update()
    {
    }
}
