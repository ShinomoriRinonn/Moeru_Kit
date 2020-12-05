using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
public static class SpriteUtils
{
    public static void DrawVertices(Sprite sp)
    {
        var olds = sp.vertices;
        var news = olds.Clone();

        foreach (var vertice in olds){
            var v3 = new Vector3(vertice.x, vertice.y, 0);
            var dst = new Vector3(v3.x, v3.y, 1);
            UnityEngine.Debug.DrawLine(v3, dst, Color.yellow, 100);
        }
        Debug.Log("======================>>>        " + olds.Length.ToString());
    }

}
