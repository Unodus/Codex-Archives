using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor
{
    private SerializedProperty m_LocalPosition;
    private SerializedProperty m_LocalRotation;
    private SerializedProperty m_LocalScale;
    private static float s_fUnityUnitsToMeters = 1f;

    private void OnEnable()
    {
        m_LocalPosition = serializedObject.FindProperty("m_LocalPosition");
        m_LocalRotation = serializedObject.FindProperty("m_LocalRotation");
        m_LocalScale = serializedObject.FindProperty("m_LocalScale");
    }

    /// <summary>
    /// Draw the inspector widget.
    /// </summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Vector3 pos = m_LocalPosition.vector3Value;
        Vector3 posUnityUnits = pos;
        Vector3 posMeters = pos * s_fUnityUnitsToMeters;
        Vector3 posFeet = Utils.MathConversion.MetersToFeet(posMeters);
        Vector3 rot = m_LocalRotation.quaternionValue.eulerAngles;
        Vector3 scale = m_LocalScale.vector3Value;

        EditorGUILayout.BeginHorizontal();
        {
            if (DrawButton("P", "Reset Position", IsResetPositionValid(pos), 20f))
            {
                pos = Vector3.zero;
            }
            EditorGUILayout.BeginVertical();
            {
                Vector3 previousUnityUnits = posUnityUnits;
                posUnityUnits = EditorGUILayout.Vector3Field("Local Position", previousUnityUnits);
                if (posUnityUnits != previousUnityUnits)
                {
                    pos = posUnityUnits;
                }

                Vector3 previousMeters = posMeters;
                posMeters = EditorGUILayout.Vector3Field("Local Position (m)", previousMeters);

                if (posMeters != previousMeters)
                {
                    pos = posMeters / s_fUnityUnitsToMeters;
                }
                Vector3 previousFeet = posFeet;
                posFeet = EditorGUILayout.Vector3Field("Local Position (ft)", previousFeet);

                if (posFeet != previousFeet)
                {
                    pos = Utils.MathConversion.FeetToMetres(posFeet);
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        // Rotation
        EditorGUILayout.BeginHorizontal();
        {
            if (DrawButton("R", "Reset Rotation", IsResetRotationValid(rot), 20f))
            {
                rot = Vector3.zero;
            }
            else
            {
                rot = EditorGUILayout.Vector3Field("Local Rotation", m_LocalRotation.quaternionValue.eulerAngles);
            }
        }
        EditorGUILayout.EndHorizontal();

        // Scale
        EditorGUILayout.BeginHorizontal();
        {
            if (DrawButton("S", "Reset Scale", IsResetScaleValid(scale), 20f))
            {
                scale = Vector3.one;
            }
            scale = EditorGUILayout.Vector3Field("Local Scale", m_LocalScale.vector3Value);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            s_fUnityUnitsToMeters = EditorGUILayout.FloatField("Unity Units to Meters", s_fUnityUnitsToMeters);
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        // If something changes, set the transform values
        if (true == GUI.changed)
        {
            RegisterUndo("Transform Change", targets);
            m_LocalPosition.vector3Value = Validate(pos);
            m_LocalRotation.quaternionValue = Quaternion.Euler(Validate(rot));
            m_LocalScale.vector3Value = Validate(scale);
        }

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Helper function that draws a button in an enabled or disabled state.
    /// </summary>

    private static bool DrawButton(string sTitle, string sTooltip, bool bEnabled, float fMinWidth)
    {
        if (true == bEnabled)
        {
            // Draw a regular button
            return GUILayout.Button(new GUIContent(sTitle, sTooltip), GUILayout.Width(fMinWidth));
        }
        else
        {
            // Button should be disabled -- draw it darkened and ignore its return value
            Color color = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, 0.25f);
            GUILayout.Button(new GUIContent(sTitle, sTooltip), GUILayout.Width(fMinWidth));
            GUI.color = color;
            return false;
        }
    }

    /// <summary>
    /// Helper function that determines whether its worth it to show the reset position button.
    /// </summary>

    private static bool IsResetPositionValid(Vector3 pos)
    {
        return (pos.x != 0f || pos.y != 0f || pos.z != 0f);
    }

    /// <summary>
    /// Helper function that determines whether its worth it to show the reset rotation button.
    /// </summary>

    private static bool IsResetRotationValid(Vector3 rot)
    {
        return (rot.x != 0f || rot.y != 0f || rot.z != 0f);
    }

    /// <summary>
    /// Helper function that determines whether its worth it to show the reset scale button.
    /// </summary>

    private static bool IsResetScaleValid(Vector3 scale)
    {
        return (scale.x != 1f || scale.y != 1f || scale.z != 1f);
    }

    /// <summary>
    /// Helper function that removes not-a-number values from the vector.
    /// </summary>

    private static Vector3 Validate(Vector3 vector)
    {
        vector.x = float.IsNaN(vector.x) ? 0f : vector.x;
        vector.y = float.IsNaN(vector.y) ? 0f : vector.y;
        vector.z = float.IsNaN(vector.z) ? 0f : vector.z;
        return vector;
    }

    private static void RegisterUndo(string sName, params Object[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            Undo.RecordObjects(objects, sName);
        }
    }
}