using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using XYDEditor;

public class MeshNormalTools
{
    private static List<string> logstringL = new List<string>();
    [MenuItem("Mesh/NormalAdaptive", false)]
    public static void NormalAdaptive()
    {
        // Step 1. 了解Mesh数据的vectex 与 uv
        GameObject go = Selection.activeGameObject;
        // I guess Animation of Skinned, will not use the Normal?
        Mesh mesh = go.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        List<Vector3> vecterBuffer = new List<Vector3>();
        mesh.GetVertices(vecterBuffer);
        DebugLog("======= 提取了vecters们！ =======", logstringL);
        foreach (var ele in vecterBuffer) {
            printVector3(ele, logstringL);
        }

        List<Vector3> normals = new List<Vector3>();
        mesh.GetNormals(normals);
        DebugLog("======= 提取了normals们！ =======", logstringL);
        foreach (var ele in normals) {
            printVector3(ele, logstringL);
        }

        List<List<Vector4>> uvs = new List<List<Vector4>>();
        for (int i = 0; i <= 7; i++){
            List<Vector4> uv = new List<Vector4>();
            mesh.GetUVs(i, uv);
            uvs.Add(uv);
            DebugLog("======= 提取了UV[" + i.ToString() + "], 现在打印哦 =======", logstringL);
            foreach (var ele in uv) {
                printVector4(ele, logstringL);
            }
        }

        writeLogFile(logstringL, Application.streamingAssetsPath + "/../../Logs/meshData.log");

        // Step 2. 改写Normal信息到平均化
        List<Vector3> smoothNormals = SmoothNormals(mesh);
        DebugLog("======= 计算了smoothnormals们！ =======", logstringL);
        foreach (var ele in smoothNormals) {
            printVector3(ele, logstringL);
        }

        mesh.SetNormals(smoothNormals);
        DebugLog("======= smoothNormals 赋值了！ =======", logstringL);

        // Step 3. 创建NormalMap 并给 Material赋值
        UnityEngine.Texture2D tex = createNormalMap(mesh);
        Material mat = go.GetComponent<SkinnedMeshRenderer>().material;
        mat.SetTexture("_OutlineTex", tex);
        go.GetComponent<SkinnedMeshRenderer>().material = mat;
    }

    static UnityEngine.Texture2D createNormalMap(Mesh mesh) {
        int width = 128;
        int height = 128;
        int bufferSize = width * height; //先快乐一下

        List<Vector3> normals = new List<Vector3>();
        Texture2D normalTex = new Texture2D(width, height, TextureFormat.RGB24, false);
        mesh.GetNormals(normals);
        if (normals.Count > bufferSize) {
            DebugLog("====== 法线数超标哩 暂时限定tex 128x128", logstringL);
            return normalTex;
        }
        Color[] texPix = normalTex.GetPixels();
        for (int i = 0; i < normals.Count; i++){
            Vector3 normal = normals[i];
            texPix[i].r = normal2colorspace(normal.x);
            texPix[i].g = normal2colorspace(normal.y);
            texPix[i].b = normal2colorspace(normal.z);
        }
        normalTex.SetPixels(texPix);
        normalTex.Apply();

        byte[] alphabytes = normalTex.EncodeToPNG();
        string savePath = Application.streamingAssetsPath + "/../../OutlineTex.png";
        File.WriteAllBytes(savePath, alphabytes);

        XYDEditor.Utils.ReimportTexture(savePath, width, height);
        return normalTex;
    }

    static float normal2colorspace(float scalar){
        return saturate( (scalar + 1) / 2 );
    }

    static float saturate(float i){
        if(i>1) return 1;
        if(i<0) return 0;
        return i;
    }

    static List<Vector3> SmoothNormals(Mesh mesh) {
        Debug.Log("smoothNormals");
        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);
        var v1 = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index));
        Debug.Log(v1);
        var v2 = v1.GroupBy(pair => pair.Key);
        Debug.Log(v2);


        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Average normals for grouped vertices
        foreach (var group in groups) {

            // Skip single vertices
            if (group.Count() == 1) {
                continue;
            }

            // Calculate the average normal
            var smoothNormal = Vector3.zero;

            foreach (var pair in group) {
                smoothNormal += mesh.normals[pair.Value];
            }

            smoothNormal.Normalize();

            // Assign smooth normal to each vertex
            foreach (var pair in group) {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }

        return smoothNormals;
    }

    public static void DebugLog(string str, List<string> logstringL){

        logstringL.Add(str);
        Debug.Log(str);
    }

    public static void writeLogFile(List<string> logstringL, string filename){
        if(logstringL.Count == 0 ) return;
        StreamWriter fsWrite = new StreamWriter(filename, false, Encoding.UTF8);
        foreach (string log in logstringL) {
            fsWrite.WriteLine(log);
        }
        fsWrite.Close();
    }

    public static void printVector4(Vector4 v, List<string> logstringL) {
        string log = "x: " + v.x + ", y: " + v.y + ", z: " + v.z + ", w: " + v.w;
        logstringL.Add(log);
        Debug.Log(log);
    }
    public static void printVector3(Vector3 v, List<string> logstringL) {
        string log = "x: " + v.x + ", y: " + v.y + ", z: " + v.z;
        logstringL.Add(log);
        Debug.Log(log);
    }

    public static void NormalAdaptiveFer(Mesh mesh)
    {
        List<Vector3> smoothNormals = SmoothNormals(mesh);
        List<Vector3> uv1 = new List<Vector3>();

        for (int vertexIndice = 0; vertexIndice < mesh.vertices.Length; vertexIndice++){
            var normal = mesh.normals[vertexIndice];
            var tangent = mesh.tangents[vertexIndice];
            var smoothedNormals = smoothNormals[vertexIndice];
            var bitangent = (Vector3.Cross(normal, tangent) * tangent.w).normalized;

            var tbn = new Matrix4x4(
                tangent,
                bitangent,
                normal,
                Vector4.zero);
            tbn = tbn.transpose;
            var bakedNormal = tbn.MultiplyVector(smoothedNormals).normalized;
            uv1.Add(bakedNormal);
        }

        mesh.SetUVs(3, uv1);
    }

    // public static void printVector<T>(T t) where T: Vector3 {
        
    // }
}
