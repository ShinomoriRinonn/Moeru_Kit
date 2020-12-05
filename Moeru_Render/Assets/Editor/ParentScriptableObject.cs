using UnityEngine;
using UnityEditor;
public class ParentScriptableObject : ScriptableObject
{
    [MenuItem("GameObject/Create Material")]
    static void CreateMaterial()
    {
        // Create a simple material asset

        Material material = new Material(Shader.Find("Specular"));
        AssetDatabase.CreateAsset(material, "Assets/MyMaterial.mat");

        // Add an animation clip to it
        AnimationClip animationClip = new AnimationClip();
        animationClip.name = "My Clip";
        AssetDatabase.AddObjectToAsset(animationClip, material);

        // Reimport the asset after adding an object.
        // Otherwise the change only shows up when saving the project
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animationClip));

        // Print the path of the created asset
        Debug.Log(AssetDatabase.GetAssetPath(material));
    }   

    // const string PATH = "Assets/Editor/New ParentScriptableObject.asset";
    // [SerializeField]
    // public ChildScriptableObject child;
    // [MenuItem("Assets/Create ScriptableObject")]
    // static void CreateScriptableObject()
    // {
    //     //親をインスタンス化
    //     var parent = ScriptableObject.CreateInstance<ParentScriptableObject>();
    //     //子をインスタンス化
    //     parent.child = ScriptableObject.CreateInstance<ChildScriptableObject>();
    //     //親をアセットとして保存
    //     AssetDatabase.CreateAsset(parent, PATH);
    //     //インポート処理を走らせて最新の状態にする
    //     AssetDatabase.ImportAsset(PATH);
    // }
    const string PATH = "Assets/Editor/New ParentScriptableObject.asset";
    [SerializeField]
    public ChildScriptableObject child;
    [MenuItem("Assets/Create ScriptableObject")]
    static void CreateScriptableObject()
    {
        //親をインスタンス化
        var parent = ScriptableObject.CreateInstance<ParentScriptableObject>();
        //子をインスタンス化
        parent.child = ScriptableObject.CreateInstance<ChildScriptableObject>();
        //親をアセットとして保存
        AssetDatabase.CreateAsset(parent, PATH);
        //親に child オブジェクトを追加
        AssetDatabase.AddObjectToAsset(parent.child, PATH);
        //インポート処理を走らせて最新の状態にする
        // AssetDatabase.ImportAsset(PATH);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parent.child));
        Debug.Log(AssetDatabase.GetAssetPath(parent.child));
        Debug.Log(AssetDatabase.GetAssetPath(parent));
        Debug.Log(PATH);
    }
}
