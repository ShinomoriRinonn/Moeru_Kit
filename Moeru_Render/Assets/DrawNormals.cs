using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Renderer))]
public class DrawNormals : MonoBehaviour {

    // serialized
    [SerializeField]
    public Vector3 bdboxCenter = Vector3.zero;
    [SerializeField]
    public Vector3 bdboxSize = Vector3.one;
    [SerializeField]
    public float normalSize = 0;
    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        // Gizmos.DrawWireCube(bdboxCenter, bdboxSize);
        var mf = this.GetComponent<MeshFilter>();
        // var mesh = render;
        var mesh = mf.sharedMesh;;

        for (var i = 0; i < mesh.vertexCount; i++ ){
            var from = mesh.vertices[i];
            var to = mesh.vertices[i] + mesh.normals[i] * normalSize;
            Gizmos.DrawLine(from, to);
        }
    }

}
