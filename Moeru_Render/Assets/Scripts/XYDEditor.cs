using System.IO;
using UnityEngine;
using UnityEditor;

namespace XYDEditor
{
    public static class Utils
    {
        public static string GetRelativeAssetPath(string path)
        {
            path = path.Replace("\\", "/");
            int idx = path.IndexOf("Assets");
            string relativePath = path.Substring(idx);

            return relativePath;
        }

        public static void SeperateRGBAandlpgaChannel(string path)
        {
            string relativePath = GetRelativeAssetPath(path);
            Debug.Log("Seperate Path = " + relativePath);
            SetTextureReadable(relativePath);
            Texture2D sourcetex = AssetDatabase.LoadAssetAtPath<Texture2D>(relativePath) as Texture2D;
            
            if(!sourcetex){
                // XYDLog.Error();
                Debug.Log("Seperate Import Error");
                return;
            }

            TextureImporter ti = TextureImporter.GetAtPath(relativePath) as TextureImporter;
            if(ti == null){
                Debug.Log("Seperate TI null Error");
                return;
            }

            Texture2D mipMapTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGBA32, false);
            mipMapTex.SetPixels(sourcetex.GetPixels());
            mipMapTex.Apply();
            Color[] colors2rdLevel = mipMapTex.GetPixels();
            Color[] colorsAlpha = new Color[colors2rdLevel.Length];

            bool bAlphaExist = false;
            for (int i = 0; i< colors2rdLevel.Length; ++i){
                colorsAlpha[i].r = colors2rdLevel[i].a;
                colorsAlpha[i].g = colors2rdLevel[i].a;
                colorsAlpha[i].b = colors2rdLevel[i].a;
            }

            Color[] colorsRGB = new Color[colors2rdLevel.Length];
            for (int i = 0; i<colors2rdLevel.Length; ++i){
                colorsRGB[i].r = colors2rdLevel[i].r;
                colorsRGB[i].g = colors2rdLevel[i].g;
                colorsRGB[i].b = colors2rdLevel[i].b;
            }

            Texture2D rgbTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGB24, false);
            rgbTex.SetPixels(colorsRGB);
            rgbTex.Apply();

            Texture2D alphaTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGB24, false);
            alphaTex.SetPixels(colorsAlpha);
            alphaTex.Apply();

            int rgbWidth = rgbTex.width;
            int rgbHeight = rgbTex.height;
            byte[] bytes = rgbTex.EncodeToPNG();
            string rgbTexRelativePath = GetRelativeAssetPath(GetRGBTexturePath(path).Replace("_RGBA", ""));
            File.WriteAllBytes(rgbTexRelativePath, bytes);

            int alphaWidth = alphaTex.width;
            int alphaHeight = alphaTex.height;
            byte[] alphabytes = alphaTex.EncodeToPNG();
            string alphaTexRelativePath = GetRelativeAssetPath(GetAlphaTexturePath(path).Replace("_RGBA", ""));
            File.WriteAllBytes(alphaTexRelativePath, alphabytes);

            ReimportTexture(alphaTexRelativePath, alphaWidth, alphaHeight);
            ReimportTexture(rgbTexRelativePath, rgbWidth, rgbHeight);


        }

        public static string GetRGBTexturePath(string path)
        {
            return GetTexturePath(path, "_RGB.");
        }

        public static string GetAlphaTexturePath(string path)
        {
            return GetTexturePath(path, "_Alpha.");
        }

        public static string GetTexturePath(string path, string param)
        {
            string dir = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string result = dir + "/" + filename + param + "png";
            return result;
        }

        public static void SetTextureReadable(string path, bool readable = true, bool uncompressed = true, bool mipmap = false)
        {
            TextureImporter ti = TextureImporter.GetAtPath(path) as TextureImporter;
            if(ti == null){
                Debug.Log("LLLLL");
                return;
            }

            ti.isReadable = readable;
            ti.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            ti.textureCompression = uncompressed ? TextureImporterCompression.Uncompressed : TextureImporterCompression.Compressed;
            ti.mipmapEnabled = mipmap;
            AssetDatabase.ImportAsset(path);
        }

        public static void ReimportTexture(string path, int width, int height)
        {
            AssetDatabase.ImportAsset(path);
            TextureImporter importer = TextureImporter.GetAtPath(path) as TextureImporter;
            if (importer == null){
                Debug.Log("ReimportTexture null Error");
                return ;
            }

            importer.textureType = TextureImporterType.Default;
            importer.maxTextureSize = Mathf.Max(width, height);
            importer.anisoLevel = 0;
            importer.isReadable = false;
            importer.mipmapEnabled = false;
            importer.textureFormat = TextureImporterFormat.AutomaticCompressed;

            AssetDatabase.ImportAsset(path);
        }
    }
}