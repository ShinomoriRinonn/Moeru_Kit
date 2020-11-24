using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ParentChildScriptableObject : ScriptableObject
{
    const string PATH = "Assets/Editor/New ParentChildScriptableObject.asset";

    [SerializeField]
    ChildScriptableObject child;

    [MenuItem("Assets/Create Binded ScriptableObject")]
    static void CreateScriptableObject()
    {
        //親をインスタンス化
        var parent = ScriptableObject.CreateInstance<ParentScriptableObject>();

        //子をインスタンス化
        parent.child = ScriptableObject.CreateInstance<ChildScriptableObject>();
        parent.child.name = "i am child.";
        // parent.child.hideFlags = HideFlags.None;

        //親をアセットとして保存
        AssetDatabase.CreateAsset(parent, PATH);

        //親にchildオブジェクトを追加
        AssetDatabase.AddObjectToAsset(parent.child, PATH);

        //インポート処理を走らせて最新状態にする
        AssetDatabase.ImportAsset(PATH);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Set to HideFlags.None")]
    static void SetHideFlags()
    {
        //AnimatorController を選択した状態でメニューを実行
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        //サブアセット含めすべて取得
        foreach (var item in AssetDatabase.LoadAllAssetsAtPath(path))
        {
            //フラグをすべて None にして非表示設定を解除
            item.hideFlags = HideFlags.None;
        }
        //再インポートして最新にする
        AssetDatabase.ImportAsset(path);
    }

    [MenuItem("Assets/Remove ChildScriptableObject")]
    static void Remove()
    {
        var parent = AssetDatabase.LoadAssetAtPath<ParentScriptableObject>(PATH);
        //アセットの CarentScriptableObject を破棄
        Object.DestroyImmediate(parent.child, true);
        //破棄したら Missing 状態になるので null を代入
        parent.child = null;
        //再インポートして最新の情報に更新
        AssetDatabase.ImportAsset(PATH);
    }

}
