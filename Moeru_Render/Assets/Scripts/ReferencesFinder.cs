using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;

namespace XYDEditor
{
    public class ReferencesFinder:ScriptableWizard
    {
        static public ReferencesFinder instance;
        private List<string> mList;
        private Vector2 mScroll = Vector2.zero;
        private string mTargetName = "Find References";
        void OnGUI()
        {
            EditorGUIUtility.labelWidth = 80f;
            GUILayout.Label(mTargetName, "LODLevelNotifyText");

            GUILayout.Space(12f);
            if (Event.current.type == EventType.Repaint)
            {
                Texture2D tex = EditorGUIUtility.whiteTexture;
                Rect rect = GUILayoutUtility.GetLastRect();
                GUI.color = new Color(0f, 0f, 0f, 0.25f);
                GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
                GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
                GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
                GUI.color = Color.white;
            }
            if(mList == null)
            {
                return;
            }
            if(mList.Count > 0)
            {
                mScroll = GUILayout.BeginScrollView(mScroll);
                foreach(var path in mList)
                {
                    DrawObject(path);
                }
                GUILayout.EndScrollView();
            }
            else
            {
                    EditorGUILayout.HelpBox("没有任何引用", MessageType.Info);
            }
        }

        void DrawObject(string path)
        {
            GUILayout.BeginHorizontal();
            bool select1 = GUILayout.Button(path.Replace("Assets\\", ""), "TextArea", GUILayout.Height(20f));
            if(select1)
            {
                Object obj = AssetDatabase.LoadMainAssetAtPath(path);
                EditorGUIUtility.PingObject(obj);
            }
            GUILayout.EndHorizontal();
        }

        void OnEnable()
        {
            instance = this;
        }

        void OnDisable()
        {
            instance = null;
        }

        [MenuItem("Assets/Find References In Project", true)]
        public static bool ValidateSelection()
        {
            string[] strs = Selection.assetGUIDs;
            if ( strs != null && strs.Length >=1)
            {
                string path = AssetDatabase.GUIDToAssetPath(strs[0]);
                if (File.Exists(path))
                {
                    return true;
                }
                else{
                    return false;
                }
            }
            else{
                return false;
            }
        }
        [MenuItem("Assets/Find References In Project", false)]
        public static void Show()
        {
            string[] strs = Selection.assetGUIDs;
            if(strs != null && strs.Length >= 1)
            {
                if(instance != null)
                {
                    instance.SetTarget(strs[0]);
                }else{
                    ReferencesFinder comp = DisplayWizard<ReferencesFinder>("Find References");
                    comp.SetTarget(strs[0]);
                }
            }
        }

        public void SetTarget(string guid)
        {
            if (!String.IsNullOrEmpty(guid))
            {
                mList = Find(guid);
            }
        }

        public List<string> Find(string guid)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            mTargetName = path.Substring(path.Replace("/", "\\").LastIndexOf("\\") + 1);
            string fullPath = Application.dataPath;
            string targetPath = fullPath + "/HotRes/Effects";
            Debug.Log(targetPath);
            
            // FileInfo[] fileInfos = Utils.GetAllFileInfos(targetPath);
            FileInfo[] fileInfos = GetAllFileInfos(targetPath);
            List<string> list = new List<string>();
            for (int i = 0; i<fileInfos.Length;i++)
            {
                FileInfo file= fileInfos[i];
                string path2 = Utils.GetRelativeAssetPath(file.FullName);
                EditorUtility.DisplayProgressBar("读取中", "正在搜索...", (float) i / fileInfos.Length);
                Object obj3 = AssetDatabase.LoadMainAssetAtPath(path2);
                if (String.Equals(path2, path) || obj3 == null || list.Contains(path2)) continue;
                string[] dependencies = AssetDatabase.GetDependencies(path2);
                for(int j = 0; j < dependencies.Length; j++)
                {
                    string path3 = dependencies[j];
                    if(!String.Equals(path3, path2) && String.Equals(path3, path))
                    {
                        list.Add(path2);
                    }
                }
            }
            EditorUtility.ClearProgressBar();
            return list;
        }

        public static FileInfo[] GetAllFileInfos(string path, bool includeMeta = false)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dinfo = new DirectoryInfo(path);
                var finfos = dinfo.GetFiles("*.*", SearchOption.AllDirectories);
                if (includeMeta){
                    return finfos;
                }else{
                    var newFInfos = finfos.Select(file => file).Where(file => !file.Name.EndsWith(".meta"));
                    FileInfo[] fileInfos = newFInfos.ToArray();
                    return fileInfos;
                }
            }
            return  new FileInfo[0];
        }
    }
    
}