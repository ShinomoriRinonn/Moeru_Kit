using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SerializableList<T> : Collection<T>, ISerializationCallbackReceiver
{
    [SerializeField]
    List<T> items;
    /*
    ここで押さえるべきなのは ISerializationCallbackReceiver の存在です。JsonUtility で JSON に変換す
    るときに、ISerializationCallbackReceiver の OnBeforeSerialize と OnAfterDeserialize が呼び出されま
    す。これを利用して、 ToJson 呼び出すときのみオブジェクトをシリアライズ可能なフィールドへと代入する
    ことで目的が達成できます。
    シリアライズできたのはいいのですが、最終的には JSON 形式ではなく配列のみの表示が好ましいです
（"items" のキーがいらないということ）。

        ... 即collection<T>下Items虽然不支持SerializeField, 但是item可以...
    */
    public void OnBeforeSerialize()
    {
        items = (List<T>)Items;
    }
    public void OnAfterDeserialize()
    {
        Clear();
        foreach (var item in items)
            Add(item);
    }
    public string ToJson(bool prettyPrint = false)
    {
        var result = "[]";
        var json = JsonUtility.ToJson(this, prettyPrint);
        var pattern = prettyPrint ? "^\\{\n\\s+\"items\":\\s(?<array>.*)\n\\s+\\]\n}$" : "^{\"items\":(?<array>.*)}$";
        var regex = new Regex(pattern, RegexOptions.Singleline);
        var match = regex.Match(json);
        if (match.Success)
        {
            result = match.Groups["array"].Value;
            if (prettyPrint)
                result += "\n]";
        }
        return result;
    }
    public static SerializableList<T> FromJson(string arrayString)
    {
        var json = "{\"items\":" + arrayString + "}";
        return JsonUtility.FromJson<SerializableList<T>>(json);
    }
}
