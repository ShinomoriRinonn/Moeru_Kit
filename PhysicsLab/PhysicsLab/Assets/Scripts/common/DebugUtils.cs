using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
public static class DebugUtils
{
    public static void DumpToConsole(object obj)
    {
        Type type = obj.GetType();

        var test = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        // var output = new ExpandoObject();
        var output = "";
        var name = "";
        if(obj as GameObject){
            name = (obj as GameObject).name;
        }else if(obj as Component){
            name = (obj as Component).name;
        }
        output += String.Format("<color=#EA45AA>{0}</color>", name);
        foreach(var f in test){
            // output += String.Format("Name: {0} Value: {1}", f.Name, f.GetValue(obj));
            output += String.Format("<color=#EA45AA>{0}</color>", f.Name);
            output += String.Format("<color=#E97C44>: {0}</color>", f.GetValue(obj));
            output += "\n";
        }
        Debug.Log(output);
    }

    public static void TraceStackInfo()
    {
        string trackStr = new System.Diagnostics.StackTrace().ToString();
        Debug.Log ("Stack Info:" + trackStr);
    }

}
