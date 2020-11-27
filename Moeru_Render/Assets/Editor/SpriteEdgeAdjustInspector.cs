using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteEdgeAdjust))]
public class SpriteEdgeAdjustInspector : Editor
{
    Vector3 snap;
    /*
        move only
    */
    // private void OnSceneGUI() {
    //     Tools.current = Tool.None;
    //     var component = target as SpriteEdgeAdjust;

    //     var transform = component.transform;
    //     transform.position = Handles.PositionHandle(transform.position, transform.rotation);
    // }

    /*
        move and rotation 
    */
    // private void OnSceneGUI() {
    //     var component = target as SpriteEdgeAdjust;
    //     var transform = component.transform;

    //     if(Tools.current == Tool.Move){
    //         transform.position = Handles.PositionHandle(transform.position, transform.rotation);
    //     }else if(Tools.current == Tool.Rotate){
    //         transform.rotation = Handles.RotationHandle(transform.rotation, transform.position);
    //     } 
    // }
    private void OnSceneGUI(){
        var component = target as SpriteEdgeAdjust;
        switch(component._onSceneGUI_Mode)
        {
            case OnSceneGUIMode.FREE_MOVE:
                OnSceneGUI_0();
                break;
            case OnSceneGUIMode.FREE_ROTATION:
                OnSceneGUI_1();
                break;
            case OnSceneGUIMode.HANDLE_FIXED_SIZE:
                OnSceneGUI_3();
                break;
            case OnSceneGUIMode.HANDLE_LOCAL_MODE:
                OnSceneGUI_2();
                break;

        }
    }
    private void OnSceneGUI_0() {
        var component = target as SpriteEdgeAdjust;
        var transform = component.transform;
        var size = 1f;

        transform.position = Handles.FreeMoveHandle(
            transform.position,
            transform.rotation,
            size,
            snap,
            Handles.RectangleCap
        );
    }

    private void OnSceneGUI_1() {
        var component = target as SpriteEdgeAdjust;
        var transform = component.transform;
        var size = 1f;

        transform.rotation =
            Handles.FreeRotateHandle (
            transform.rotation,
            transform.position,
            size);
        
    }
    /*
        handle self-making three axis.es
    */

    private void OnEnable() {
        // 进行一个SnapSettings值的取
        var snapX = EditorPrefs.GetFloat("MoveSnapX", 1f);
        var snapY = EditorPrefs.GetFloat("MoveSnapY", 1f);
        var snapZ = EditorPrefs.GetFloat("MoveSnapZ", 1f);

        snap = new Vector3(snapX, snapY, snapZ);
    }

    private void OnSceneGUI_2() {
        Tools.current = Tool.None;
        var component = target as SpriteEdgeAdjust;
        component.transform.position = PositionHandle(component.transform, 1f);
    }

    private void OnSceneGUI_3() {
        Tools.current = Tool.None;
        var component = target as SpriteEdgeAdjust;
        var size = HandleUtility.GetHandleSize(component.transform.position);
        component.transform.position = PositionHandle(component.transform, size);
    }

    Vector3 PositionHandle(Transform transform, float size)
    {
        var position = transform.position;
        // 注意是对position做叠加变化 而不是对transform.position （后者应在return 前后做赋值改写
        Handles.color = Color.red;
        position = Handles.Slider(position, transform.right, size, Handles.ArrowCap, snap.x);    // X axis

        Handles.color = Color.green;
        position = Handles.Slider(position, transform.up, size, Handles.ArrowCap, snap.y);       // Y axis

        Handles.color = Color.blue;
        position = Handles.Slider(position, transform.forward, size, Handles.ArrowCap, snap.z);  // Z axis

        return position;
    }

    // FreeMoveHandle



}