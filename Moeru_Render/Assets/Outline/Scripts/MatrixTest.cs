// using System;
// using UnityEngine;
// using System.Collections;

// public class MatrixTest : MonoBehaviour
// {
//     public GameObject target;
//     public Matrix4x4 rotaM;
//     private Quaternion initQ; 
//     void Start()
//     {
        
//     }

//     void OnEnable()
//     {
//         // Vector3 inputVec = new Vector3(1, 0, 0);

//         // Vector3 initV = new Vector3(1, 1, 0); // 这是个啥 ==> 3点钟方向 即欧拉绕z转正向45
//         var d3 = (float)Math.Sqrt(3)/2;
//         // Debug.Log(d3);
//         Vector3 oldDir = new Vector3(d3, 0.5f, 0);
//         Vector3 ss = 2 * oldDir.normalized ;
//         XYDUtils.printVector3(ss);
//         Quaternion initQ = Quaternion.Euler(0, 0, 30);


//         // Quaternion initQ = Quaternion.LookRotation(initV.normalized);
//         this.initQ = initQ;
//         this.target.transform.rotation = initQ;
//         // Debug.Log("Now this Euler(0, 0, 30)");

//         var rotateMat = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 45));
//         // rotaM = rotateMat;
//         var newDir = rotateMat.MultiplyVector(oldDir);
        
//         Debug.Log("########## priiiiiiiiint");
//         XYDUtils.printVector3(oldDir);
//         XYDUtils.printVector3(newDir);

//         var newDir2 = Quaternion.Euler(0, 0, 45) * oldDir;
//         XYDUtils.printVector3(newDir2);
//         // Quaternion rotatedQ = Quaternion.LookRotation(newDir.normalized);
//         // this.target.transform.rotation = rotatedQ;
//         // Debug.Log("Now After rotate 45 degree by z axis");
//     }

//     void OnDisable()
//     {
//         var d3 = (float)Math.Sqrt(3)/2;
//         Debug.Log(d3);
//         Vector3 initV = new Vector3(d3, 1/2, 0);
//         initV.Set(0, 1, 0);

//         Quaternion initQ = Quaternion.LookRotation(initV);
//         this.target.transform.rotation = initQ;
//         this.target.transform.rotation = this.initQ;
//     }
// }