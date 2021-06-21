using UnityEngine;

using UnityEditor;
using UnityEditor.EditorTools;

[EditorTool(displayName: "Move Snappable", targetType: typeof(Snappable))]
public class MoveSnappable : EditorTool
{
    public Texture2D toolIcon;

    private Snappable _mySnappable;
    private Transform _myTransform;

    private bool _isSnappingEnabled = true;
    private readonly float _rotationSnapAngle = 45; //in degrees

    public override GUIContent toolbarIcon =>
        new GUIContent
        {
            image = toolIcon,
            text = "Move Snappable",
            tooltip = "Allows you to move Snappables and have them snap to the closest socket automatically"
        };

    private void OnEnable()
    {
        _lastControl = 0;
        _currentControl = 0;
    }

    public override void OnToolGUI(EditorWindow window)
    {
        _mySnappable = (Snappable)target;

        if (_mySnappable == null) return;

        _myTransform = _mySnappable.transform;

        DrawAllSockets();
        DrawAllPlugs();

        DrawTranslationHandle();
        DrawRotationHandle();

        DrawToggleSnapButton();

        DrawShortestConnectionLine(); //Temporary?
    }

    #region Drawing

    private void DrawShortestConnectionLine()
    {
        if (!GetBestConnection(plug: out ushort __plug, socket: out ushort __socket,
            connectedSnappable: out Snappable __connectedSnappable))
            return; //Should cache the best connection but screw it I'm lazy, you can do it, It's open source.

        Vector3 __a = _mySnappable.WorldPoints[__plug];
        Vector3 __b = __connectedSnappable.WorldPoints[__socket];

        Handles.color = Color.yellow;
        Handles.DrawDottedLine(__a, __b, screenSpaceSize: 5f);
    }

    private int _lastControl;
    private int _currentControl;

    private void DrawTranslationHandle()
    {
        Quaternion __rotation = _myTransform.rotation;
        Vector3 __position = _myTransform.position;

        EditorGUI.BeginChangeCheck();

        __position = Handles.PositionHandle(__position, __rotation);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_myTransform, name: "Move Snappable");

            _myTransform.position = __position;
        }

        if (_isSnappingEnabled == false) return;

        //Check if we've switched to not translating.
        if (_lastControl != 0 && _currentControl == 0)
        {
            ConnectToSnappable();
        }

        _lastControl = _currentControl;
        _currentControl = GUIUtility.hotControl;
    }

    private void DrawRotationHandle()
    {
        Quaternion __rotation = _myTransform.rotation;
        Vector3 __position = _myTransform.position;

        EditorGUI.BeginChangeCheck();

        float __scaledSize = HandleUtility.GetHandleSize(__position) * 1.25f;

        Handles.color = Handles.xAxisColor;
        __rotation = Handles.Disc(__rotation, __position,
            axis: _myTransform.right, size: __scaledSize, cutoffPlane: false, snap: _rotationSnapAngle);

        Handles.color = Handles.yAxisColor;
        __rotation = Handles.Disc(__rotation, __position,
            axis: _myTransform.up, size: __scaledSize, cutoffPlane: false, snap: _rotationSnapAngle);

        Handles.color = Handles.zAxisColor;
        __rotation = Handles.Disc(__rotation, __position,
            axis: _myTransform.forward, size: __scaledSize, cutoffPlane: false, snap: _rotationSnapAngle);

        if (!EditorGUI.EndChangeCheck()) return;

        Undo.RecordObject(_myTransform, name: "Rotate Snappable");

        _myTransform.rotation = __rotation;
    }

    private void DrawToggleSnapButton()
    {
        Handles.BeginGUI();
        if (GUILayout.Button(text: $"Snapping = {(_isSnappingEnabled ? "On" : "Off")}", GUILayout.Width(100)))
        {
            _isSnappingEnabled = !_isSnappingEnabled;
        }

        Handles.EndGUI();
    }

    private void DrawAllPlugs()
    {
        Matrix4x4 __cachedMatrix = Handles.matrix;
        Handles.matrix = _mySnappable.transform.localToWorldMatrix;
        Handles.color = Color.white;

        for (int __index = 0; __index < _mySnappable.WorldPoints.Length; __index++)
        {
            Vector3 __position = _mySnappable.LocalPoints[__index].position;
            Vector3 __direction = _mySnappable.LocalPoints[__index].direction.normalized;

            __position -= (__direction * 0.125f);

            Handles.ConeHandleCap(
                controlID: 0,
                position: __position,
                rotation: Quaternion.LookRotation(__direction, upwards: _mySnappable.transform.up),
                size: 0.25f,
                EventType.Repaint
            );
        }

        Handles.matrix = __cachedMatrix;
    }

    private void DrawAllSockets()
    {
        Matrix4x4 __cachedMatrix = Handles.matrix;

        foreach (Snappable __snappable in Snappable.Instances)
        {
            if (__snappable == _mySnappable) continue; //Skip self.

            Handles.matrix = __snappable.transform.localToWorldMatrix;
            Handles.color = Color.grey;

            for (int __index = 0; __index < __snappable.WorldPoints.Length; __index++)
            {
                Vector3 __position = __snappable.LocalPoints[__index].position;
                Vector3 __direction = __snappable.LocalPoints[__index].direction.normalized;

                //__position -= (__direction * 0.125f);

                Handles.CylinderHandleCap(
                    controlID: 0,
                    position: __position,
                    rotation: Quaternion.LookRotation(__direction, upwards: Vector3.up),
                    size: 0.25f,
                    EventType.Repaint
                );

            }
        }

        Handles.matrix = __cachedMatrix;
    }

    #endregion

    private void ConnectToSnappable()
    {
        if (GetBestConnection(
            plug: out ushort __plug,
            socket: out ushort __socket,
            connectedSnappable: out Snappable __connectedSnappable))
        {
            _myTransform.position -=
                (_mySnappable.WorldPoints[__plug] - __connectedSnappable.WorldPoints[__socket]);

            _myTransform.rotation = Quaternion.Euler(x: 0, y: _myTransform.rotation.eulerAngles.y, z: 0);
            //RotateAround(_targetTransform, pivotPoint:  __b, Quaternion.identity);
        }
    }

    /// <summary> Gets the closest socket that is within range. </summary>
    /// <param name="plug"> The index of the closest point on the TARGET snappable. </param>
    /// <param name="socket"> The index of the closest point on the CLOSEST snappable. </param>
    /// <param name="closestSnappable"> The closest snappable. </param>
    /// <returns> True if we found a socket we can connect to, false if we haven't . </returns>
    private bool GetBestConnection(out ushort plug, out ushort socket, out Snappable connectedSnappable)
    {
        bool __foundResult = false;

        plug = 0;
        socket = 0;
        connectedSnappable = null;

        /*
        closestSnappable = BestSnappable;

        if(closestSnappable == null) return false; //If the closest snappable is too far away we don't need to do any further checks.
        */

        float __closestDistance = float.PositiveInfinity;

        foreach (Snappable __snappable in Snappable.Instances)
        {
            if (__snappable == _mySnappable) continue; //Skip self.

            for (ushort __plugIndex = 0; __plugIndex < _mySnappable.WorldPoints.Length; __plugIndex++)
            {
                Vector3 __myPoint = _mySnappable.WorldPoints[__plugIndex];

                for (ushort __socketIndex = 0; __socketIndex < __snappable.WorldPoints.Length; __socketIndex++)
                {
                    Vector3 __otherPoint = __snappable.WorldPoints[__socketIndex];
                    float __distance = Vector3.Distance(a: __myPoint, b: __otherPoint);

                    if (__distance > _mySnappable.snapRadius)
                        continue; //If the distance isn't outside the SNAP radius..

                    if (__distance < __closestDistance) //And the distance is closer than the current closest.
                    {
                        //Set it as the new best option.
                        __closestDistance = __distance;

                        __foundResult = true;

                        plug = __plugIndex;
                        socket = __socketIndex;
                        connectedSnappable = __snappable;
                    }
                }
            }
        }

        return __foundResult;
    }

}