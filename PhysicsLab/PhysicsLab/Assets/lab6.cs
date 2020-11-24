using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class lab6 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool enable{
        get{
            return _enabled;
        }
        set{
            SetEnabled(value);
            _enabled = value;
        }
    }

    bool _enabled;
    SceneView _sceneView;
    private bool _restoreCamera2DMode;
    private Vector3 _restoreCameraPosition;
    private void SetEnabled(bool value)
    {
        if(_enabled != value)
        {
            if (value)
            {
                _sceneView = SceneView.lastActiveSceneView;
                if(_sceneView == null) return;
                _restoreCamera2DMode = _sceneView.in2DMode;
                _restoreCameraPosition = _sceneView.pivot;

                _sceneView.in2DMode = true;
                _sceneView.LookAt(new Vector3(0, 0, -1000));
                _sceneView.Repaint();

                // SceneView.onSceneGUIDelegate += OnSceneGUI;
            }
            else
            {
                _sceneView.in2DMode = _restoreCamera2DMode;
                _sceneView.LookAt(_restoreCameraPosition);

                // SceneView.onSceneGUIDelegate -= OnSceneGUI;
            }
            _enabled = value;
        }
    }

    Vector3 _mousePosition;
    void OnSceneGUI(SceneView scene)
    {
        Event e = Event.current;
        Vector2 mousePosition = e.mousePosition;

        // View point to world point translation function in my game
        this._mousePosition = SceneScreenToWorldPoint(mousePosition);

        // Block SceneView's built-in behavior
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        // ------------------------
        // Your Custom OnGUI Logic
        // ------------------------

        if (Event.current.type == EventType.MouseDown) Event.current.Use();
        if (Event.current.type == EventType.MouseMove) Event.current.Use();
    }

    private Vector3 SceneScreenToWorldPoint(Vector3 sceneScreenPoint)
    {
        Camera sceneCamera = _sceneView.camera;
        float screenHeight = sceneCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * sceneCamera.aspect;

        Vector3 worldPos = new Vector3(
            (sceneScreenPoint.x / sceneCamera.pixelWidth) * screenWidth - screenWidth * 0.5f,
            ((-(sceneScreenPoint.y) / sceneCamera.pixelHeight) * screenHeight + screenHeight * 0.5f),
            0f
        );

        worldPos += sceneCamera.transform.position;
        worldPos.z = 0f;

        return worldPos;
    }
}
