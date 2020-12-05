 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 
 // Create Empty GameObject as the parent, change the transform anything but not the Vector3.zero
 // Create a Cube as the someObject and attach the script to it
 public class TransformPointExample : MonoBehaviour {
     public GameObject someObject;
     public Transform parent;
     public Vector3 thePosition;
     public void Start()
     {
         string log = "";
         log = "Current Transform World Space# " + transform.position + ", Current Transform Local Space# " + transform.localPosition + ", Point# " + (Vector3.right * 2).ToString() + "\r\n";
         thePosition = transform.TransformPoint(Vector3.right * 2);
         GameObject clone = Instantiate(someObject, thePosition, someObject.transform.rotation);
         clone.transform.SetParent(parent);
         log += "thePosition# " + thePosition + ", Current Transform Inverse Transform# " + transform.InverseTransformPoint(thePosition);
         transform.localPosition = Vector3.zero;
         log += ", local position Vector3.zero Transform Inverse Transform# " + transform.InverseTransformPoint(thePosition);
         Debug.Log(log);
     }
 }