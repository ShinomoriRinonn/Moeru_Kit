using System.ComponentModel;
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using XYDEditor;

public static class MeshNormalTools
{
    private static List<string> logstringL = new List<string>();
    [MenuItem("Mesh/Noel Runtime")]
    public static void NoelRuntime()
    {
        GameObject go = Selection.activeGameObject;
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        
    }

    [MenuItem("Mesh/CreateCubeMesh")]
    public static void CreateCubeMesh()
    {
        GameObject go = Selection.activeGameObject;
        string path = Application.dataPath + "/HotRes/Prefabs/Obstacle";
        Mesh m = new Mesh();
        m.name = "ScriptedMesh Obstacle";
        // float edgeSize = 0.8f;
        // float halfEdgeSize = 0.4f;

        // 96x156 to 0.9 x 1.4625 to 1.28 x 2.08
        m.vertices = new Vector3[]{
            new Vector3(-0.64f, 1.04f, -0.8f),
            new Vector3(-0.64f, -0.240f, -0.8f),
            new Vector3(-0.64f, -1.04f, 0f),
            new Vector3(0.64f, -1.04f, 0f),
            new Vector3(0.64f, -0.240f, -0.8f),
            new Vector3(0.64f, 1.04f, -0.8f),
        };
        m.triangles = new int[]{
            1,0,4,
            5,4,0,
            2,1,3,
            4,3,1
        };
        m.uv = new Vector2[]{
            new Vector2(0, 1),
            new Vector2(0, 0.3846f),
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 0.3846f),
            new Vector2(1, 1),
        };

        // m.vertices = new Vector3[]{
        //     new Vector3(-halfEdgeSize, -halfEdgeSize, 0),
        //     new Vector3(-halfEdgeSize, halfEdgeSize, 0),
        //     new Vector3(halfEdgeSize, halfEdgeSize, 0),
        //     new Vector3(halfEdgeSize, -halfEdgeSize, 0),
        //     new Vector3(-halfEdgeSize, -halfEdgeSize, -edgeSize),
        //     new Vector3(-halfEdgeSize, halfEdgeSize, -edgeSize),
        //     new Vector3(halfEdgeSize, halfEdgeSize, -edgeSize),
        //     new Vector3(halfEdgeSize, -halfEdgeSize, -edgeSize),
        // };
        // m.triangles = new int[]{
        //     4,7,6,
        //     6,5,4,
        //     7,3,2,
        //     2,6,7,
        //     3,0,1,
        //     1,2,3,
        //     0,4,5,
        //     5,1,0,
        //     0,3,7,
        //     7,4,0,
        //     5,6,2,
        //     2,1,5
        // };
        // m.colors = new Color[]{
        //     new Color(1,1,1,1),
        //     new Color(1,1,1,1),
        //     new Color(1,1,1,1),
        //     new Color(1,1,1,1),
        //     new Color(1,1,1,1),
        //     new Color(1,1,1,1),
        //     new Color(1,1,1,1),
        //     new Color(1,1,1,1),
        // };
        m.RecalculateBounds();
        m.RecalculateNormals();
        MeshFilter mf = go.GetComponent<MeshFilter>();
        mf.sharedMesh = m;
        Mesh newMesh = (Mesh)UnityEngine.Object.Instantiate(m);
        string destPath = path + "/" + "obmesh_mesh.asset";
        Debug.Log("Generate Mesh Data " + destPath);
        AssetDatabase.CreateAsset(newMesh, Utils.GetRelativeAssetPath(destPath));
        mf.sharedMesh = newMesh;
        AssetDatabase.SaveAssets();
    }

    public static string getDirName(string path){
        string[] sps = path.Split('/');
        string parentPath = path.Remove(path.Length - sps[sps.Length - 1].Length - 1); // 去除尾部'/' 
        return parentPath;
    }

    [MenuItem("Mesh/cal")]
    public static void cal()
    {
        var c1 = new Vector3(-0.1495f, -0.1986f, -0.9685f);
        var c2 = new Vector3(-0.8256f,  0.5640f,  0.0117f);
        var c3 = new Vector3(-0.5439f, -0.8015f,  0.2484f);

        var dot12 = Vector3.Dot(c1, c2);
        var dot23 = Vector3.Dot(c2, c3);
        var dot31 = Vector3.Dot(c3, c1);
        Debug.Log("dot12: " + dot12);
        Debug.Log("dot23: " + dot23);
        Debug.Log("dot31: " + dot31);
    }
    [MenuItem("Mesh/ShadowRenderer Generator/All Enemies")] // save mesh.asset then reconfig prefab
    public static void ShadowRendererAllEnemy()
    {
        string path = Application.dataPath + "/HotRes/Char/enemy";
        if (Directory.Exists(path))
        {
            DirectoryInfo info = new DirectoryInfo(path);
            DirectoryInfo[] directoryInfos = info.GetDirectories("monster_*", SearchOption.AllDirectories);
            for (int i = 0; i < directoryInfos.Length; i++)
            {
                DirectoryInfo info2 = directoryInfos[i];

                FileInfo[] modelFiles = info2.GetFiles("monster_*PB.prefab");
                // FileInfo[] weaponFiles = info2.GetFiles("weapon_*.prefab");
                // FileInfo[] srcFiles =  modelFiles.Concat(weaponFiles).ToArray();
                
                foreach(FileInfo fI in modelFiles)
                {
                    string filePath = fI.FullName;
                    string relativePath = Utils.GetRelativeAssetPath(filePath);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);
                    GameObject go = (GameObject)UnityEngine.Object.Instantiate(prefab);
                    ShadowRendererSetup(go);
                    PrefabUtility.SaveAsPrefabAsset(go, relativePath);
                }
            }
            AssetDatabase.SaveAssets();
        }

    }
    private static string shadowRenderNodeName = "shade_shadow";
    
    
    [MenuItem("Mesh/ShadowRenderer Generator/Selected")]
    public static void ShadowRendererSelected()
    {
        string[] strs = Selection.assetGUIDs;
        string filePath = AssetDatabase.GUIDToAssetPath(strs[0]);

        string relativePath = Utils.GetRelativeAssetPath(filePath);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);
        GameObject go = (GameObject)UnityEngine.Object.Instantiate(prefab);
        ShadowRendererSetup(go);
        PrefabUtility.SaveAsPrefabAsset(go, relativePath);

        string parentPath = getDirName(filePath);
        Debug.Log("parentPath: " + parentPath + ", goName: " + go.name + "start process...");
    }

    public static void ShadowRendererSetup(GameObject go)
    {
        var smrs = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        var mrs = go.GetComponentsInChildren<MeshRenderer>();

        SetupShadowComponents(go, smrs);
        SetupShadowComponents(go, mrs);

        // update rs
        smrs = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        mrs = go.GetComponentsInChildren<MeshRenderer>();

        SetupColorShadow2ColorShade(go, smrs);
        SetupColorShadow2ColorShade(go, mrs);
    }

    [MenuItem("Mesh/Outline Editor/Offline process All Model")]
    public static void AttachOutlineScript()
    {
        string path = Application.dataPath + "/HotRes/Char/enemy";
        if (Directory.Exists(path))
        {
            DirectoryInfo info = new DirectoryInfo(path);
            DirectoryInfo[] directoryInfos = info.GetDirectories("monster_*", SearchOption.AllDirectories);
            for (int i = 0; i < directoryInfos.Length; i++)
            {
                DirectoryInfo info2 = directoryInfos[i];

                FileInfo[] modelFiles = info2.GetFiles("monster_*PB.prefab");
                foreach(FileInfo fI in modelFiles)
                {
                    Debug.Log("#####################\n开始处理！ " + Path.GetFileNameWithoutExtension(fI.FullName));
                    string filePath = fI.FullName;
                    string dirName = fI.Directory.FullName;
                    Debug.Log("1、DirName = " + dirName );
                    string relativePath = Utils.GetRelativeAssetPath(filePath);
                    Debug.Log("2、File to Normal Adapter: " + relativePath);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);
                    // GameObject go = (GameObject)UnityEngine.Object.Instantiate(prefab);
                    AttachOutlineSetup(prefab);
                    // PrefabUtility.SaveAsPrefabAsset(go, relativePath);
                }
            }
            
            AssetDatabase.SaveAssets();
        }
    }
    [MenuItem("Mesh/Outline Editor/Offline process selected")]
    public static void AttachOutlineScriptSelected()
    {
        string[] strs = Selection.assetGUIDs;
        string filePath = AssetDatabase.GUIDToAssetPath(strs[0]);

        string relativePath = Utils.GetRelativeAssetPath(filePath);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);
        GameObject go = (GameObject)UnityEngine.Object.Instantiate(prefab);
        AttachOutlineSetup(go);
        PrefabUtility.SaveAsPrefabAsset(go, relativePath);

        string parentPath = getDirName(filePath);
        Debug.Log("parentPath: " + parentPath + ", goName: " + go.name + "start process...");
    }
    
    public static void AttachOutlineSetup(GameObject go)
    {
        // 寻找shader为的render
        var render = go.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach(var smr in render){
            if(! smr.sharedMaterial) continue;
            if( smr.sharedMaterial.shader.ToString().Equals("Unlit/Char/Toon_Outline (UnityEngine.Shader)")){
                var drawer = smr.gameObject.GetComponent<OutlineDrawer>();
                if (drawer == null){
                    drawer = smr.gameObject.AddComponent<OutlineDrawer>();
                }
                drawer.bdboxCenter.Set(0, 0, 1);
                drawer.bdboxSize.Set(3, 3, 3);
            }
            // Debug.Log("###########    " + smr.sharedMaterial.shader.ToString());
        }

    }

    public static void SetupShadowComponents(GameObject go, Renderer[] render){
        foreach(var smr in render){
            Debug.Log(smr.gameObject.name);
            Boolean hasShadowNode = smr.gameObject.transform.Find(shadowRenderNodeName);
            if (smr.gameObject.name != shadowRenderNodeName && !hasShadowNode) {
                var shadowNode = (GameObject)UnityEngine.Object.Instantiate(smr.gameObject);
                
                var material = AssetDatabase.LoadAssetAtPath<Material>(Utils.GetRelativeAssetPath(Application.dataPath + "/HotRes/Materials/Common/chara_shadow.mat"));
                var shadowsmr = shadowNode.GetComponent<Renderer>();
                shadowsmr.material = material;

                shadowNode.name = shadowRenderNodeName;
                // smr.gameObject.AddChild(shadowNode);
                shadowNode.transform.parent = smr.gameObject.transform;
            }
        }
    }

    public static void SetupColorShadow2ColorShade(GameObject go, Renderer[] render){
        foreach(var smr in render){
            if(! smr.sharedMaterial) continue;
            if( smr.sharedMaterial.shader.ToString().Equals("Unlit/Char/Toon_Outline_Shdaow (UnityEngine.Shader)")){
                var newshade = Shader.Find("Unlit/Char/Toon_Outline");
                Debug.Log(newshade.ToString());
                smr.sharedMaterial.shader = newshade;
            }
            // Debug.Log("###########    " + smr.sharedMaterial.shader.ToString());
        }
    }

    [MenuItem("Mesh/Normal Editor/Offline process All Model")] // save mesh.asset then reconfig prefab
    public static void NormalAdapterRuntime()
    {
        string path = Application.dataPath + "/HotRes/Char/enemy";
        if (Directory.Exists(path))
        {
            DirectoryInfo info = new DirectoryInfo(path);
            DirectoryInfo[] directoryInfos = info.GetDirectories("monster_*", SearchOption.AllDirectories);
            for (int i = 0; i < directoryInfos.Length; i++)
            {
                DirectoryInfo info2 = directoryInfos[i];

                FileInfo[] modelFiles = info2.GetFiles("monster_*PB.prefab");
                // FileInfo[] weaponFiles = info2.GetFiles("weapon_*.prefab");
                // FileInfo[] srcFiles =  modelFiles.Concat(weaponFiles).ToArray();
                
                foreach(FileInfo fI in modelFiles)
                {
                    Debug.Log("#####################\n开始处理！ " + Path.GetFileNameWithoutExtension(fI.FullName));
                    string filePath = fI.FullName;
                    string dirName = fI.Directory.FullName;
                    Debug.Log("1、DirName = " + dirName );
                    string relativePath = Utils.GetRelativeAssetPath(filePath);
                    Debug.Log("2、File to Normal Adapter: " + relativePath);
                    GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);
                    processModelMesh(go, dirName);
                }
            }
            
            AssetDatabase.SaveAssets();
        }

    }
    [MenuItem("Mesh/Normal Editor/Offline process Selected Model")] // save mesh.asset then reconfig prefab
    public static void processSelectedModelMesh()
    {
        string[] strs = Selection.assetGUIDs;
        string path = AssetDatabase.GUIDToAssetPath(strs[0]);
        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        string parentPath = getDirName(path);
        Debug.Log("parentPath: " + parentPath + ", goName: " + go.name + "start process...");

        processModelMesh(go, parentPath);
    }

    public static void processModelMesh(GameObject go, string dirName)
    {
        // string destPath = Path.GetPathRoot(filePath) + Path.GetFileNameWithoutExtension() + "_mesh.mesh";
        foreach(SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            Mesh mesh = smr.sharedMesh;
            Debug.Log("TAH?" + mesh);
            Debug.Log(mesh);
            if(mesh){
                string destPath = Utils.GetRelativeAssetPath(dirName) + "/" + smr.gameObject.name + "_mesh.asset";
                string testPath = Utils.GetRelativeAssetPath(dirName) + "/" + smr.gameObject.name + ".FBX";
                ModelImporter test = AssetImporter.GetAtPath(testPath) as ModelImporter;
                Debug.Log("3、Dest Path: " + destPath);
                Debug.Log("3、Importer Path: " + testPath);
                Debug.Log(test);
                NormalAdapterFerReconfig(smr, destPath, testPath);
            }
        }
        foreach(MeshFilter mf in go.GetComponentsInChildren<MeshFilter>())
        {
            Mesh mesh = mf.sharedMesh;
            if(mesh){
                string destPath = Utils.GetRelativeAssetPath(dirName) + "/" + mf.gameObject.name + "_mesh.asset";
                string testPath = Utils.GetRelativeAssetPath(dirName) + "/" + mf.gameObject.name + ".FBX";
                ModelImporter test = AssetImporter.GetAtPath(testPath) as ModelImporter;
                Debug.Log("3、Dest Path: " + destPath);
                NormalAdapterFerReconfig(mf, destPath, testPath);
            }
        }
        Debug.Log("#####################\n结束！ ");
    }

    [MenuItem("Mesh/MaterialChecker", false)]
    public static void MaterialChecker()
    {
        GameObject go = Selection.activeGameObject;
        var smr = go.GetComponent<SkinnedMeshRenderer>();
        var outlineWidthf = smr.sharedMaterial.GetFloat("_OutLineWidth");
        // smr.material.
        Debug.Log("##################### Outline = " + outlineWidthf.ToString());
        var useTgaf = smr.sharedMaterial.GetInt("_useTgaAlpha");
        Debug.Log("##################### useTgaf = " + useTgaf.ToString());
        Debug.Log("##################### _Mode = " + smr.sharedMaterial.GetInt("_Mode").ToString());
        Debug.Log("##################### _ZWrite = " + smr.sharedMaterial.GetFloat("_ZWrite").ToString());
    }


    [MenuItem("Mesh/NormalAdaptive", false)]
    public static void NormalAdaptive()
    {
        GameObject go = Selection.activeGameObject;
        Mesh mesh = go.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        NormalAdapterFer2(mesh);
        // DebugLog("=======  节点名是！  =======", logstringL);
        
        // I guess Animation of Skinned, will not use the Normal?
    }
    [MenuItem("Mesh/AntiNormalAdaptive", false)]
    public static void AntiNormalAdaptive()
    {
        GameObject go = Selection.activeGameObject;
        Mesh mesh = go.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        // mesh.SetUVs(3, new List<Vector3>());
        mesh.uv4 = new Vector2[mesh.vertexCount];
        mesh.uv5 = new Vector2[mesh.vertexCount];
        // DebugLog("=======  节点名是！  =======", logstringL);
        
        // I guess Animation of Skinned, will not use the Normal?
    }
    public static void NormalAdapterFerReconfig(UnityEngine.Object meshContainer, string destPath, string testPath)
    {
        try{
            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(testPath) as Mesh;
            Debug.Log("printMesh: " + mesh);
            if(meshContainer is MeshFilter){
                (meshContainer as MeshFilter).sharedMesh = mesh;
            } else if ( meshContainer is SkinnedMeshRenderer){
                (meshContainer as SkinnedMeshRenderer).sharedMesh = mesh;
            }
            Debug.Log("afterSetMesh To original Fbx.");
            List<Vector3> normalImported = new List<Vector3>();
            mesh.GetNormals(normalImported);

            Debug.Log("after RecordNormal.");

            ModelImporter test = AssetImporter.GetAtPath(testPath) as ModelImporter;
            if (test) {
                test.importNormals = ModelImporterNormals.Calculate;
                test.normalCalculationMode = ModelImporterNormalCalculationMode.AngleWeighted;
                test.normalSmoothingSource = ModelImporterNormalSmoothingSource.FromAngle;
                test.normalSmoothingAngle = 180;
            }
            
            AssetDatabase.ImportAsset(testPath, ImportAssetOptions.ImportRecursive | ImportAssetOptions.ForceUpdate);
            Debug.Log("After Reimport Fbx");
            
            List<Vector3> smoothedNormals = new List<Vector3>();
            mesh.GetNormals(smoothedNormals);
            List<Vector4> tangents = new List<Vector4>();
            SeperateNormalsToTangents(smoothedNormals, tangents);
            mesh.SetTangents(tangents);
            
            mesh.SetNormals(normalImported);
            Debug.Log("Fbx Instance Mesh All Set.");

            Mesh newMesh = (Mesh)UnityEngine.Object.Instantiate(mesh);
            AssetDatabase.CreateAsset(newMesh, destPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Saved NewMesh in .asset");

            if(meshContainer is MeshFilter){
                (meshContainer as MeshFilter).sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(destPath);
            } else if ( meshContainer is SkinnedMeshRenderer){
                (meshContainer as SkinnedMeshRenderer).sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(destPath);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Changed Linkage");

        } catch(Exception e){
            Debug.Log(e.Message);
        }
    }

    public static void SeperateNormalsToTangents(List<Vector3> normals, List<Vector4> tangents)
    {
        for (int i = 0; i < normals.Count; i++)
        {
            Vector3 normal = normals[i];

            Vector4 n1 = new Vector4(normal.x, normal.y, normal.z, 0);
            tangents.Add(n1);
        }
    }
    public static void SeperateNormalsToUV(List<Vector3> normals, List<Vector4> uv1)
    {
        for (int i = 0; i < normals.Count; i++)
        {
            Vector3 normal = normals[i];

            Vector4 n1 = new Vector4(normal.x, normal.y, normal.z, 0);
            uv1.Add(n1);
        }
    }

    public static Vector3 normal2uv(Vector3 normal)
    {
        return new Vector3( (normal.x + 1)/2, (normal.y + 1)/2, (normal.z + 1)/2 );
    }
    public static void NormalAdapterFer2(Mesh mesh)
    {
        List<Vector3> smoothNormals = SmoothNormals(mesh);
        List<Vector3> uv1 = new List<Vector3>();
        List<Vector3> uv2 = new List<Vector3>();
        //（目前接触过模型最多5000顶点，故打印到 一个正方形内 128 * 128 (0~127)（128~....）

        for (int vertexIndice = 0; vertexIndice < mesh.vertices.Length; vertexIndice++){
            Vector3 smoothNormal = smoothNormals[vertexIndice];
            Vector3 vertice = mesh.vertices[vertexIndice];
            Vector3 normal = mesh.normals[vertexIndice];
            Vector4 tangent = mesh.tangents[vertexIndice];
            var bitangent = Vector3.Cross(normal, tangent).normalized;
            var tbn = new Matrix4x4(
                tangent,
                bitangent,
                normal,
                Vector4.zero);
            tbn = tbn.transpose;
            var bakedNormal = tbn.MultiplyVector(smoothNormal).normalized;
            printVector3(bakedNormal, logstringL);
            uv1.Add(bakedNormal);

            var x = vertexIndice % 128;

            var y = (vertexIndice - x)  / 12;
            uv2.Add(new Vector3(x/128, y/128));
        }

        mesh.SetUVs(3, uv1);
        mesh.SetUVs(4, uv2);
        // mesh.SetNormals(smoothNormals);//test
        writeLogFile(logstringL, Application.streamingAssetsPath + "/../../Logs/bakedNormal.log");
    }

    [MenuItem("Mesh/TestMatrix")]
    public static void testMatrixInUnityC()
    {
        GameObject go = Selection.activeGameObject;
        Mesh mesh = go.GetComponent<SkinnedMeshRenderer>().sharedMesh;   
        List<Vector3> smoothNormals = SmoothNormals(mesh);
        logstringL = new List<string>();
        for (int vertexIndice = 0; vertexIndice < mesh.vertices.Length; vertexIndice++){
            Vector3 smoothNormal = smoothNormals[vertexIndice];
            Vector3 vertice = mesh.vertices[vertexIndice];
            Vector3 normal = mesh.normals[vertexIndice];
            Vector4 tangent = mesh.tangents[vertexIndice];
            var bitangent = Vector3.Cross(normal, tangent).normalized;
            var tbn = new Matrix4x4(
                tangent,
                bitangent,
                normal,
                Vector4.zero);
            tbn = tbn.transpose;
            var bakedNormal = tbn.MultiplyVector(smoothNormal).normalized;

            printString("###############", logstringL);
            printMatrix(tbn, logstringL);

            printString(" -- ", logstringL);
            printVector4(tangent, logstringL);
            printVector3(bitangent, logstringL);
            printVector3(normal, logstringL);

            printString(" -- smooth -- ", logstringL);
            printVector3(smoothNormal, logstringL);
            printVector3(bakedNormal, logstringL);
            printString(" ", logstringL);

        }
        writeLogFile(logstringL, Application.streamingAssetsPath + "/../../Logs/matrixData.log");
    }

    public static void NormalAdapterFer(Mesh mesh)
    {
        List<Vector3> smoothNormals = SmoothNormals(mesh);
        List<Vector3> uv1 = new List<Vector3>();

        for (int vertexIndice = 0; vertexIndice < mesh.vertices.Length; vertexIndice++){
            Vector3 smoothNormal = smoothNormals[vertexIndice];
            Vector3 vertice = mesh.vertices[vertexIndice];
            Vector3 normal = mesh.normals[vertexIndice];
            Vector4 tangent = mesh.tangents[vertexIndice];
            Vector3 binormal = (Vector3.Cross(normal, tangent) * tangent.w).normalized;
            var tbn = new Matrix4x4(tangent, binormal, normal, Vector4.zero);
            tbn = tbn.transpose;
            Vector3 normal_tbn = tbn.MultiplyVector(smoothNormal).normalized;
            //-1, 1 to 0,1
            // Vector3 uv = normal2uv(normal_tbn);
            DebugLog("======= 提取了BoneWeight们！ =======", logstringL);
            uv1.Add(normal_tbn);
        }

        mesh.SetUVs(3, uv1);
    }


    [MenuItem("Mesh/EditSkeletonMesh", false)]
    static void editSkeleton() {
        GameObject go = Selection.activeGameObject;
        Mesh mesh = go.GetComponent<SkinnedMeshRenderer>().sharedMesh;

        List<BoneWeight> bw = new List<BoneWeight>();
        List<BoneWeight> bw1 = new List<BoneWeight>();
        mesh.GetBoneWeights(bw);
        DebugLog("======= 提取了BoneWeight们！ =======", logstringL);
        foreach (var ele in bw) {
            BoneWeight weights = new BoneWeight();
            weights.boneIndex0 = ele.boneIndex0;
            weights.weight0 = 0;

            weights.boneIndex1 = ele.boneIndex1;
            weights.weight1 = 0;

            weights.boneIndex2 = ele.boneIndex2;
            weights.weight2 = 0;

            weights.boneIndex3 = ele.boneIndex3;
            weights.weight3 = 0;

            bw1.Add(weights);
        }
        mesh.boneWeights = bw1.ToArray();
    }

    [MenuItem("Mesh/PrintMesh", false)]
    static void printMesh() {
        GameObject go = Selection.activeGameObject;
        DebugLog("=======  节点名是！  =======", logstringL);
        
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
            if (ele.magnitude < 0.9f){
                Debug.Log("xx");
            }
            printVector3(ele, logstringL);
        }
        List<Vector4> tangents = new List<Vector4>();
        mesh.GetTangents(tangents);
        DebugLog("======= 提取了tangents们！ =======", logstringL);
        foreach (var ele in tangents) {
            printVector4(ele, logstringL);
        }
        List<BoneWeight> bw = new List<BoneWeight>();
        mesh.GetBoneWeights(bw);
        DebugLog("======= 提取了BoneWeight们！ =======", logstringL);
        foreach (var ele in bw) {
            printBoneWeight(ele, logstringL);
        }

        List<List<Vector4>> uvs = new List<List<Vector4>>();
        for (int i = 0; i <= 7; i++){
            List<Vector4> uv = new List<Vector4>();
            mesh.GetUVs(i, uv);
            uvs.Add(uv);
            DebugLog("======= 提取了UV[" + i.ToString() + "]  =======", logstringL);
            foreach (var ele in uv) {
                printVector4(ele, logstringL);
            }
        }

        // DebugLog("======= 提取了Colors =======", logstringL);
        // Color[] colors = mesh.colors;
        // for (int i = 0; i <= colors.Length; i++){
        //     Color color = colors[i];
        //     printColor(color);
        // }

        writeLogFile(logstringL, Application.streamingAssetsPath + "/../../Logs/meshData.log");
    }

    static UnityEngine.Texture2D createNormalMap(Mesh mesh) {
        int width = 128;
        int height = 128; // 测试一下!
        int bufferSize = width * height;
        
        List<Vector3> normals = new List<Vector3>();
        mesh.GetNormals(normals);
        if(normals.Count > bufferSize) {
            DebugLog("======= 法线数超标 暂时tex 限定128x128=======", logstringL);
            return new Texture2D(width, height);
        }
        return new Texture2D(width, height, TextureFormat.RGB24, false);
    }

    static List<Vector3> SmoothNormals(Mesh mesh) {
        // Debug.Log("smoothNormals");
        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);
        // var v1 = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index));
        // // Debug.Log(v1);
        // // foreach(var da in v1)
        // // {
        // //     Debug.Log(da);
        // // }
        // var v2 = v1.GroupBy(pair => pair.Key);
        // Debug.Log(v2);
        // int indice = 0;
        // foreach(var da in v2)
        // {
            
        //     foreach (var da2 in da)
        //     {
        //         Debug.Log(da2);
        //     }
        //     Debug.Log("----" + indice.ToString());
        //     indice = indice + 1;
        // }

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
        logstringL.Clear();
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
    public static void printColor(Color v){
        string log = "r: " + v.r + ", g: " + v.g + ", b: " + v.b + ", a: " + v.a;
        logstringL.Add(log);
        Debug.Log(log);
    }

    public static void printMatrix(Matrix4x4 input, List<string> logstringL){
        printVector4(input.GetRow(0), logstringL);
        printVector4(input.GetRow(1), logstringL);
        printVector4(input.GetRow(2), logstringL);
        printVector4(input.GetRow(3), logstringL);
    }

    public static void printString(string str, List<string> logstringL){
        DebugLog(str, logstringL);
    }

    public static void printBoneWeight(BoneWeight bw, List<string> logstringL){
        // string log = "x: " + v.x + ", y: " + v.y + ", z: " + v.z;
        string log = "boneIndex0: " + bw.boneIndex0 + ", boneIndex1: " +　bw.boneIndex1 + ", boneIndex2: " +　bw.boneIndex2 + ", boneIndex3: " + bw.boneIndex3;
        log += "weight0: " + bw.weight0 + ", weight1: " + bw.weight1 + ", weight2: " + bw.weight2 + ", weight3: " + bw.weight3;
        logstringL.Add(log);
        Debug.Log(log);
    }
}
