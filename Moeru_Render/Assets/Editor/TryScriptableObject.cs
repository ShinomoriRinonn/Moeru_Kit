using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Example/Create ExampleAsset Instance")]

public class TryScriptableObject : ScriptableObject
{
    [SerializeField]
    string str;

    [SerializeField, Range(0, 100)]
    int number;

    [MenuItem("Editor Extension Lab/Create extend Asset Instance and output towards instance")]
    static void CreateExampleAssetInstance()
    {
        var exampleAsset = CreateInstance<TryScriptableObject>();

        AssetDatabase.CreateAsset(exampleAsset, "Assets/Editor/ExampleAsset.asset"); // 这句相当于执行 MenuItem/Assets/Create/Example/Create ExampleAsset Instance
        AssetDatabase.Refresh();
    }

    [MenuItem("Editor Extension Lab/Load Asset instance")]
    static void LoadExampleAsset()
    {
        var exampleAsset = 
            AssetDatabase.LoadAssetAtPath<TryScriptableObject>("Assets/Editor/ExampleAsset.asset");
    }


    /*
    このは理由は ScriptableObject の基底クラスである UnityEngine.Object をシリアライズデータとして
    扱うには、ディスク上にアセットとして保存しなければいけません。Type mismatch 状態は、 インスタンスは
    存在するが、ディスク上にアセットとして存在しない状態を指しています。つまり、そのインスタンスがなんら
    かの状況（Unity 再起動など）で破棄されてしまうとデータにアクセスができなくなります。
    */

    /*
    ただし、今回のように「親」と「子」の関係がある状態で、それぞれ独立したアセットを作成してしまうのは
    管理面で見ても得策ではありません。子の数が増えたり、リストを扱うことになった時にその分アセットを作成
    するのは非常にファイル管理が面倒になってきます。
    そこで サブアセット という機能を使って親子関係であるアセットを 1 つにまとめ上げることができます。
    */

    
}
