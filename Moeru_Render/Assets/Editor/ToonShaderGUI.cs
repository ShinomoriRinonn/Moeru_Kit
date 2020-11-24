using UnityEngine;
using UnityEditor;

namespace XYDEditor
{
    public class ToonShaderGUI:ShaderGUI
    {
        private MaterialEditor editor;

        MaterialProperty mainTex = null;
        public static GUIContent txtG = new GUIContent("string content");
        void DoAdvanced () {
            GUILayout.Label("Advanced Options", EditorStyles.boldLabel);
            this.editor.EnableInstancingField();
        }

        void DoTexture() {
            GUILayout.Label("Textures: " , EditorStyles.boldLabel);
            this.editor.TextureProperty(mainTex, "MainTex哦");
            this.editor.TexturePropertyMiniThumbnail(new Rect(200, 100, 100, 100), mainTex, "MiniThumbnail", "Tooptips!!!!");
            this.editor.TexturePropertySingleLine(txtG, mainTex);
        }
        public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
        {
            this.editor = editor;
            // mainTex = FindProperty("_MainTex", properties, false);
            // DoTexture();
            DoAdvanced();

        }
    }
}