using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private int count = 0;

    // Update is called once per frame
    void Update()
    {
        var speed = 360 / (30 * Time.deltaTime); // 10秒360
        this.count = this.count + 1;
        // this.transform.Rotate(0, speed * Time.deltaTime, 0);
        // var baseQuat = Quaternion.Euler(0, 90, -60);
        // this.transform.rotation = baseQuat;
        // this.transform.Rotate(0, 30, 0, Space.World);

        // var model = gameObject.transform.Find("model").gameObject;
        this.transform.rotation = Quaternion.Euler(0, 90, -60);
        this.transform.Rotate(0, speed * Time.deltaTime * this.count, 0, Space.World                                                                          );
        // XYDUtils.printVector3(this.transform.forward);
    }
}
