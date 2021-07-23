using UnityEditor;
using UnityEngine;
using System.IO;

/// <summary>
/// Owner: Kyle Hatch
/// Created: 24/02/2017
/// 
/// Descirption: Inspector for screenshot taking
/// </summary>

[CustomEditor(typeof(Screenshot))]
public class ScreenshotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Screenshot myTarget = (Screenshot)target;

        EditorGUILayout.LabelField("Current System: ", myTarget.captureMethod.ToString());  

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("App ASync"))
        {
            myTarget.captureMethod = CaptureMethod.AppCapture_Asynch;
        }

        if (GUILayout.Button("App Sync"))
        {
            myTarget.captureMethod = CaptureMethod.AppCapture_Synch;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("ReadPixels ASync"))
        {
            myTarget.captureMethod = CaptureMethod.ReadPixels_Asynch;
        }

        if (GUILayout.Button("ReadPixels Sync"))
        {
            myTarget.captureMethod = CaptureMethod.ReadPixels_Synch;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Render Tex ASync"))
        {
            myTarget.captureMethod = CaptureMethod.RenderToTex_Asynch;
        }

        if (GUILayout.Button("Render Tex Sync"))
        {
            myTarget.captureMethod = CaptureMethod.RenderToTex_Synch;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (myTarget.captureMethod == CaptureMethod.AppCapture_Asynch)
        {
            myTarget.Supersize = EditorGUILayout.IntField("Supersize:", myTarget.Supersize);
        }
        else
        {
            myTarget.Dimensions = EditorGUILayout.RectField("Dimensions:", myTarget.Dimensions);
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Current Shot: ", myTarget.m_iShotNumber.ToString());
        EditorGUILayout.LabelField("Screenshot Dir: ", myTarget.m_sRootLocation + Screenshot.SCREENSHOT_FOLDER_LOCATION + Path.DirectorySeparatorChar);

        GUILayout.Space(10);

        if (true == EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Take Shot"))
            {
                myTarget.TakeShot();
            }
        }
    }
}
