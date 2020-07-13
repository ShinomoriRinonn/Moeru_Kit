using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineDrawer:MonoBehaviour{

    [SerializeField]
    private Vector3 bdboxCenter = Vector3.zero; // center
    [SerializeField]
    private Vector3 bdboxSize = Vector3.one;    // size

    [HideInInspector]
    public Renderer rdr = null;
    [HideInInspector]
    public bool drawable = false;           //?
    [HideInInspector]
    public Vector4 bounding = Vector4.zero;
    [HideInInspector]
    public Vector4 bias = Vector4.zero;
    [HideInInspector]
    public int rtIndex = -1;

    static private Vector3[] dirs = new Vector3[] { 
        new Vector3(0.5f, 0.5f, 0.5f), 
        new Vector3(0.5f, 0.5f, -0.5f),
        new Vector3(0.5f, -0.5f, 0.5f),
        new Vector3(0.5f, -0.5f, -0.5f),
        new Vector3(-0.5f, 0.5f, 0.5f),
        new Vector3(-0.5f, 0.5f, -0.5f),
        new Vector3(-0.5f, -0.5f, 0.5f),
        new Vector3(-0.5f, -0.5f, -0.5f)
    };

    // 这里存的是 worldPosition 哦
    static private Vector3[] boundingPoints = new Vector3[8]; // 猜测每遍历一个Drawer就在这个buffer里操作一下 

    void OnEnable()
    {
        if (rdr == null) rdr = GetComponent<Renderer>();
        if (Camera.main == null) return;
        OutlineCamera oc = Camera.main.GetComponent<OutlineCamera>();
        if (oc == null) return;
        oc.AddOutlineObject(this);    // add to buffer
    }

    void OnDisable()
    {
        if (Camera.main != null)
        {
            OutlineCamera oc = Camera.main.GetComponent<OutlineCamera>();
            if(oc != null)
            {
                oc.RemoveOutlineObject(this);    // remove from buffer
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(bdboxCenter, bdboxSize);
    }

    // Get World Coordinate of 8 Bounding Points 
    public Vector3[] GetBoundingPoints()
    {
        Vector4 homoPos = Vector4.zero;
        Vector4 worldPos = Vector4.zero;
        for (int i =0; i <8;i++)
        {
            homoPos.Set(
                bdboxCenter.x + bdboxSize.x * dirs[i].x,
                bdboxCenter.y + bdboxSize.y * dirs[i].y,
                bdboxCenter.z + bdboxSize.z * dirs[i].z,
                1.0f
            );

            worldPos = transform.localToWorldMatrix * homoPos;
            boundingPoints[i].Set(worldPos.x, worldPos.y, worldPos.z);
        }
        return boundingPoints;

    }


}