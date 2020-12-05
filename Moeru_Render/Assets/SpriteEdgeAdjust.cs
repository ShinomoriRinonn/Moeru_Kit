using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnSceneGUIMode {
    FREE_MOVE,
    FREE_ROTATION,
    HANDLE_FIXED_SIZE,
    HANDLE_LOCAL_MODE,
}

public class SpriteEdgeAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    [ContextMenuItem("根据精灵初始化Mesh数据", "GenerateMesh")]
    [ContextMenuItem("打印sprite数据", "PrintSprite")]
    public Sprite _sprite;
    private Mesh _mesh;
    [HideInInspector][SerializeField] MeshProperty m_meshProperty; 
    public OnSceneGUIMode _onSceneGUI_Mode;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PrintSprite()
    {
        SpriteUtils.DrawVertices(_sprite);
    }

    void GenerateMesh()
    {
        // UnityEngine.Object.Destroy(_mesh);
        _mesh = new Mesh();

        m_meshProperty = new MeshProperty(_sprite.vertices, _sprite.triangles, _sprite.uv);
        var meshVerticesBuffer = new List<Vector3>();
        // foreach(){
            // meshVerticesBuffer.Add(new Vector3())
        // }
        // _mesh.vertices = vertices;

    }
    // ゲームオブジェクトまた
    // は親を選択中の時のみ表示する OnDrawGizmosSelected、 ゲームオブジェクトがアクティブの時に常に表示す
    // る OnDrawGizmos の 2 つがあります。
    private void OnDrawGizmos() {
        if(isDataReady){
            //ギズモの色を変更
            Gizmos.color = new Color32(255, 0, 0, 210);
            ApplyPropertyToMesh();
            Gizmos.DrawWireMesh(
                _mesh, 
                0, 
                this.transform.position + Vector3.zero, 
                this.transform.rotation, 
                this.transform.lossyScale
            );
            // Gizmos.DrawWireCube(transform.position, transform.lossyScale);
        }
    }

    private void ApplyPropertyToMesh() {
        _mesh.vertices = m_meshProperty.vertices.ToArray();
        _mesh.triangles = m_meshProperty.triangles.ToArray();
        _mesh.uv = m_meshProperty.uv.ToArray();

        var normallist = new List<Vector3>();
        for(var i = 0; i<_mesh.vertices.Length; i++){
            normallist.Add(Vector3.back);
        }
        _mesh.normals = normallist.ToArray();
    }

    public bool isDataReady{
        get
        {
            return this._sprite != null && this._mesh != null;
        }
    }

    public MeshProperty meshProperty{
        get{
            return m_meshProperty;
        }
    }
}

[SerializeField]
public class MeshProperty 
{
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uv;

    public MeshProperty()
    {
        Reset();
    }
    public MeshProperty(Vector2[] vertices_, ushort[] triangles_, Vector2[] uv_)
    {
        Reset();
        foreach(var ele in vertices_){
            vertices.Add(ele);
        }
        foreach(var ele in triangles_){
            triangles.Add(ele);
        }
        foreach(var ele in uv_){
            uv.Add(ele);
        }
    }

    public void Reset() {
        if(vertices != null){
            vertices.Clear();
        }else{
            vertices = new List<Vector3>();
        }

        if(triangles != null){
            triangles.Clear();
        }else{
            triangles = new List<int>();
        }

        if(uv != null){
            uv.Clear();
        }else{
            uv = new List<Vector2>();
        }
    }
}