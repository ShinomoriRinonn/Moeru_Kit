using UnityEngine;
using System.Collections.Generic;

namespace UnityEngine
{
    public static class XYDUtils
    {
        static List<string> logstring = new List<string>(); 
        public static void printVector3(Vector3 i)
        {
            string log = "x: " + i.x + ", y: " + i.y + ", z: " + i.z;
            logstring.Add(log);
            Debug.Log(log); 
        }

        public static void writeLog()
        {
            string root = Application.dataPath;
            System.IO.File.WriteAllLines(root, logstring);
        }
    }

}