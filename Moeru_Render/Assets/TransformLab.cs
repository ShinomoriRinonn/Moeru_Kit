using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TransformLab : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCamera; 
    void Start()
    {
        mainCamera = Camera.main;
        // gameObject.transform.LookAt(mainCamera.transform);

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(mainCamera.transform);
    }
}
