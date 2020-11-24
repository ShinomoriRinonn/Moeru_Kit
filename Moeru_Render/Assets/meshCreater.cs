using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshCreater : MonoBehaviour
{
    // Start is called before the first frame update
    private Mesh _mesh;
    private MeshFilter _mf;
    private MeshRenderer _mr;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_mesh == null){
            _mesh = new Mesh();
            _mesh.vertices = new Vector3[]{
                new Vector3(-0.5f, -0.5f, 0),
                new Vector3(-0.5f, 0.5f, 0),
                new Vector3(0.5f, 0.5f, 0),
                new Vector3(0.5f, -0.5f, 0)
            };
            _mesh.uv = new Vector2[]{
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
            };

            _mesh.triangles = new int[]{0, 1, 2, 0, 2, 3};
            // gameObject.AddChild(_mesh);
            _mf = gameObject.GetComponent<MeshFilter>();
            _mr = gameObject.GetComponent<MeshRenderer>();

            _mf.sharedMesh = _mesh;
        
            _mr.sharedMaterial.SetTextureScale("_MainTex", new Vector2(1,2));
            // _mr.sharedMaterial.mainTextureScale = new Vector2(1,2);
            // _mr.
        }
    }
}
