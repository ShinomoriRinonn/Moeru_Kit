using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLDraw : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 screenSize = Vector2.zero;
    void Start()
    {
        
    }

    void  v() {
        DrawCircleSurfaceLocal();
        Debug.Log("drawCircle");
    }

    private void DrawCircleSurfaceLocal()
    {
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        DrawCircleSurface();
        GL.PopMatrix();
    }

    private void DrawCircleSurfaceScreen()
    {
        //circleRadius = 1000; 以像素为单位
        GL.PushMatrix();
        //GL.LoadPixelMatrix();
        GL.LoadPixelMatrix(0, screenSize.x,0,screenSize.y);
        DrawCircleSurface();
        GL.PopMatrix();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int circleRadius = 3;
    public int circleCount = 6;
    public int zValue = 0; // view 系
        
    private void DrawCircleSurface()
    {
        float angleDelta = 2 * Mathf.PI / circleCount;
 
        GL.Begin(GL.TRIANGLES);
        GL.Color(Color.yellow);

        for (int i = 0; i < circleCount; i++)
        {
            float angle = angleDelta * i;
            float angleNext = angle + angleDelta;

            GL.Vertex3(0, 0, zValue);
            GL.Vertex3(Mathf.Cos(angle) * circleRadius, Mathf.Sin(angle) * circleRadius, zValue);
            GL.Vertex3(Mathf.Cos(angleNext) * circleRadius, Mathf.Sin(angleNext) * circleRadius, zValue);
            Debug.Log("gl. i : " + i);
        }

        GL.End();
    }
}
