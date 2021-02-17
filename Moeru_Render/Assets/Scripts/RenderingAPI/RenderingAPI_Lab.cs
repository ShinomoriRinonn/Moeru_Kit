/**
    前言.
    个人认为 前端视觉工作 的工作内容，应分为 
    1. 有能力描述 视觉效果（不拘泥于文字，应为文字、图像，绝对任意的）      
    2. 有能力操作图形Api以尽可能还原 上述效果          
    3. 工作意义上 优先级排序 = 工期 >> 实现的可行性（做到对应机种上、效率） >> 表现效果的还原度

    RenderingLab 主要以代码片的形式 记录对图形Api、图形技术的使用
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RenderingAPI_Lab : MonoBehaviour
{
    // Start is called before the first frame update
    private ComputeBuffer _cb; 
    void Start()
    {
        _cb = new ComputeBuffer(10, 10);

        /*
        This class is used to copy resource data from the GPU to the CPU without any stall(GPU or CPU), but adds a few frames of latencyc.
        */
        AsyncGPUReadback.Request(_cb); // <---- AsyncGPUReadback 为静态类型
        // AsyncGPUReadback.RequestIntoNativeArray(); 
        AsyncGPUReadback.WaitAllRequests();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
