using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XYDEditor;

namespace XYDEditor
{
    public class AtlasesToolkit
    {
        // private static string defaultWhiteTexPath_relative = "Assets/Default_Alpha.png";
        [MenuItem("Assets/Generate Sprite Atlas", false)]
        public static void GenerateSpriteAtlas()
        {
            string[] strs = Selection.assetGUIDs;
            if( strs != null && strs.Length >= 1)
            {
                string path = AssetDatabase.GUIDToAssetPath(strs[0]);
                GenerateSpriteAtlasByDir(path);
            }  
        }

        public static void PictureSplit()
        {
            string[] strs = Selection.assetGUIDs;
            if( strs != null && strs.Length >= 1)
            {
                string path = AssetDatabase.GUIDToAssetPath(strs[0]);
                GenerateSpriteAtlasByDir(path);
            }  
        }

        public static void makeTpSheet(string srcPath, string outKey, bool ignoreOutkey = false)
        {
            StreamReader sr = new StreamReader(srcPath);
            StreamWriter sw = new StreamWriter(srcPath.Replace("_TPS.tpsheet", outKey + ".tpsheet"), false);
            string line;
            while((line = sr.ReadLine()) != null)
            {
                string line_r = line.Replace("_RGBA.png", outKey + ".png");
                string[] sArray = Regex.Split(line_r, ";");
                if(sArray.Length > 1)
                {
                    sArray[0] = sArray[0] + (ignoreOutkey?"":outKey);
                    line_r = "";
                    for (int i = 0; i <sArray.Length; i++){
                        line_r += sArray[i];
                        if ( i != (sArray.Length - 1)){
                            line_r = line_r + ";";
                        }
                    }
                }
                sw.WriteLine(line_r);
            }
            sr.Close();
            sw.Close();
        }
        // [MenuItem("Assets/Generate Sprite Atlas", false)]
        // public static void SeperateRGBAandlpgaChannel(string path)

        public static void GenerateSpriteAtlasByDir(string atlasDir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(atlasDir);
            if (Directory.Exists(dirInfo.FullName + "\\images")){
                string atlasePathName = "Atlases_sp2/" + dirInfo.Name + "/" + dirInfo.Name;
                Debug.Log(atlasePathName);

                // ---- Step 1. 快乐TexturePacker Command Line 
                // ---- 注意，这里TexturePacker设置了format = unity-texture2d -- multipack，所以这一步输出就是Sprite(?)
                string command = dirInfo.FullName + 
                                            "\\images --format unity-texture2d --multipack --size-constraints POT --max-size 2048 --pack-mode Best --force-squared --padding 0 --disable-rotation --algorithm Polygon --trim-mode Polygon --trim-margin 0 --extrude 0 --border-padding 0 --opt RGBA8888 --scale 1 --sheet "
                                            + dirInfo.FullName + "\\" + dirInfo.Name + "{n}_RGBA.png --data " + dirInfo.FullName + "\\" + 
                                            dirInfo.Name + "{n}_TPS.tpsheet";
                if (dirInfo.Name == "HomeMapBg"){
                    // ---- 这个directory特殊处理一下下... 默认用Polygon  下面的用MaxRects
                    command = dirInfo.FullName + 
                                             "\\images --format unity-texture2d --multipack --size-constraints POT --max-size 2048 --pack-mode Best --force-squared --padding 0 --disable-rotation --algorithm MaxRects --trim-margin 0 --extrude 0 --border-padding 0 --opt RGBA8888 --scale 1 --sheet "
                                            + dirInfo.FullName + "\\" + dirInfo.Name + "{n}_RGBA.png --data " + dirInfo.FullName + "\\" + 
                                            dirInfo.Name + "{n}_TPS.tpsheet";
                }

                string res = BuildTools.processCommand("TexturePacker", command);
                AssetDatabase.Refresh();
                Debug.Log("TexturePacker Res: " + res);


                string tPath2 = dirInfo.FullName + "/" + dirInfo.Name + "0" + "_RGBA" + ".png";
                tPath2 = Utils.GetRelativeAssetPath(tPath2);
                TextureImporter importer2 = TextureImporter.GetAtPath(tPath2) as TextureImporter;
                Debug.Log(tPath2);
                if(importer2 == null){
                    Debug.Log("!!!sss");
                }
                // ---- Step 2. 快乐darkTexture
                int spIdx = 0;
                bool assetCleared = false; 

                // 这个让图像变暗的操作，它们生成了个LUT_DUSK的纹理图
                // 然后写进Material的字段_EnvLut里
                Texture darkTexture = AssetDatabase.LoadAssetAtPath<Texture>(XYDDef.AssetRootDirectory + "Textures/Misc/LUT_DUSK_4.png") as Texture;
                while ((!res.Contains("up-to-data") && !res.Contains("nothing to do")) && File.Exists(dirInfo.FullName + "\\" + dirInfo.Name + spIdx + "_RGBA" + ".png")){
                    Debug.Log("Delete: " + dirInfo.FullName + "/" + dirInfo.Name + spIdx + "_RGBA" + ".png");
                    Utils.SeperateRGBAandlpgaChannel(dirInfo.FullName + "/" + dirInfo.Name + spIdx + "_RGBA" + ".png");
                    makeTpSheet(dirInfo.FullName + "/" + dirInfo.Name + spIdx + "_TPS.tpsheet", "_RGB", true);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    string tPath = dirInfo.FullName + "/" + dirInfo.Name + spIdx + "_RGB" + ".png";
                    tPath = Utils.GetRelativeAssetPath(tPath);
                    AssetDatabase.ImportAsset(tPath);

                    TextureImporter importer = TextureImporter.GetAtPath(tPath) as TextureImporter;
                    importer.spritePixelsPerUnit = 100;

                    AssetDatabase.ImportAsset(tPath);

                    // nani??? ooo!!! 不删掉 被合图的东东就不能新Run了！ 会被检测 nothing to do!
                    File.Delete(dirInfo.FullName + "/" + dirInfo.Name + spIdx + "_RGBA" + ".png");
                    File.Delete(dirInfo.FullName + "/" + dirInfo.Name + spIdx + "_TPS.tpsheet");

                    // ----改写了tpsheet文件并调整扩展名。使unity将rgb识别为sprite

                    Material mat = null;
                    Material clipMat = null;

                    mat = AssetDatabase.LoadAssetAtPath<Material>(XYDDef.AssetRootDirectory + atlasePathName + spIdx + "_Mat.mat") as Material;
                    clipMat = AssetDatabase.LoadAssetAtPath<Material>(XYDDef.AssetRootDirectory + atlasePathName + spIdx + "_Clip_Mat.mat") as Material;
                
                    if (mat == null){
                        mat = new Material(Shader.Find("Custom/Sprite Custom Shader"));
                        AssetDatabase.CreateAsset(mat, XYDDef.AssetRootDirectory + atlasePathName + spIdx + "_Mat.mat");
                        mat = AssetDatabase.LoadAssetAtPath<Material>(XYDDef.AssetRootDirectory + atlasePathName + spIdx + "_Mat.mat") as Material;
                    } else {
                        mat.shader = Shader.Find("Custom/Sprite Custom Shader");
                    }
                    if (clipMat == null){
                        clipMat = new Material(Shader.Find("Unlit/Clip Sprite Renderer"));
                        AssetDatabase.CreateAsset(clipMat, XYDDef.AssetRootDirectory + atlasePathName + spIdx + "_Clip_Mat.mat");
                        clipMat = AssetDatabase.LoadAssetAtPath<Material>(XYDDef.AssetRootDirectory + atlasePathName + spIdx + "_Clip_Mat.mat") as Material;
                    }else{
                        clipMat.shader = Shader.Find("Unlit/Clip Sprite Renderer");
                    }

                    Texture t2d_rgb = AssetDatabase.LoadAssetAtPath<Texture>(XYDDef.AssetRootDirectory + atlasePathName + spIdx + "_RGB.png") as Texture;
                    Texture t2d_alpha = AssetDatabase.LoadAssetAtPath<Texture>(XYDDef.AssetRootDirectory + atlasePathName + spIdx + "_Alpha.png") as Texture;

                    mat.mainTexture = t2d_rgb;
                    mat.SetTexture("_EnvLut", darkTexture);
                    mat.SetFloat("_ColorScale", -0.001f);
                    mat.SetInt("_CustomType", 1);
                    clipMat.mainTexture = t2d_rgb;

                    if(dirInfo.Name != "HomeMapBg") {
                        mat.SetTexture("_AlphaTex", t2d_alpha);
                        clipMat.SetTexture("_AlhpaTex", t2d_alpha);
                    } else {
                        File.Delete(dirInfo.FullName + "/" + dirInfo.Name + spIdx + "_Alhpa" + ".png");
                    }

                // ---- Step 3. 新建了个快乐AtlasePrefab去储存 
                // ---- 以"xxx_rgb.png"为入口  能读取完整信息（由于上面的material.Maintex 指向了该rgb）
                    GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(XYDDef.AssetRootDirectory + atlasePathName + ".prefab") as GameObject;
                    MulSpriteAtlas spAss;
                    if (obj == null){
                        GameObject tmp = new GameObject();
                        tmp.AddComponent<MulSpriteAtlas>();

                        UnityEngine.Object emptyObj = PrefabUtility.CreateEmptyPrefab(XYDDef.AssetRootDirectory + atlasePathName + ".prefab");
                        PrefabUtility.ReplacePrefab(tmp, emptyObj, ReplacePrefabOptions.Default);
                        obj = AssetDatabase.LoadAssetAtPath<GameObject>(XYDDef.AssetRootDirectory + atlasePathName + ".prefab") as GameObject;
                        UnityEngine.Object.DestroyImmediate(tmp);
                    }
                    spAss = obj.GetComponent<MulSpriteAtlas>();
                    if (!assetCleared){
                        spAss.mAtlases.Clear();
                        assetCleared = true;
                    }
                    UnityEngine.Object[] objs2 = AssetDatabase.LoadAllAssetRepresentationsAtPath(XYDDef.AssetRootDirectory + atlasePathName + spIdx + "_RGB.png");
                    // objs2 有着一万个rgb
                    SpriteAtlas singleAtlas = new SpriteAtlas();
                    for (int i = 0; i < objs2.Length; i++){
                        singleAtlas.mSprites.Add(objs2[i] as Sprite);
                    }
                    singleAtlas.material = mat;
                    singleAtlas.clipMaterial = clipMat;
                    spAss.mAtlases.Add(singleAtlas);

                    UnityEditor.EditorUtility.SetDirty(obj);
                    spIdx++;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}
