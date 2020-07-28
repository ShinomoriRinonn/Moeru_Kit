using UnityEngine;
using UnityEditor;

namespace XYDEditor
{
    public class ToonShaderGUI:ShaderGUI
    {
        private MaterialEditor editor = new MaterialEditor();
        void DoAdvanced () {
            GUILayout.Label("Advanced Options", EditorStyles.boldLabel);

            this.editor.EnableInstancingField();
        }
        public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
        {
            
            this.editor = editor;
            DoAdvanced();
        }
    }
}