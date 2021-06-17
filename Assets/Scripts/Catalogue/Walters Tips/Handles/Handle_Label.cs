using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



public sealed class Handle_Label : MonoBehaviour
{
    // Walter's tip of the day, 10/11/2020
    //Did you know you can add so-called custom "Handles" to your scripts using a custom editor?
    //Handles are what Unity uses for their own Gizmos. (The arrows when you move an object from inside the editor, for example)
    //It can allow you to show things in the editor when you select the object the script is on, it's very useful for debugging.

    [SerializeField] private string text;

    #region Custom Editor

    #if UNITY_EDITOR // <- Won't build if you leave out these checks
    [CustomEditor(typeof(Handle_Label))]


    private sealed class LabelExampleEditor : Editor
    {
        private Handle_Label _labelExample;
        private void OnEnable() => _labelExample = (Handle_Label)target;

        private void OnSceneGUI()
        {
            if (_labelExample == null) return; // if label is null for whatever reason, empty out quickly

            Handles.color = Color.white;  // sets color of the handle

            //draw the scripts text in the scene view

            Handles.Label(position: _labelExample.transform.position, text: $"Text: { _labelExample.text }");

        }
    }


    #endif

    #endregion


    //https://docs.unity3d.com/ScriptReference/Handles.html
    //If you go to the documentation there are all sorts of handles you can use.
    //I'll show some use-cases now:
    //-
    //This is an explosion which uses;
    //    https://docs.unity3d.com/ScriptReference/Handles.RadiusHandle.html 
    //for it's area of effect.
    //And coloured https://docs.unity3d.com/ScriptReference/Handles.DrawDottedLine.html
    //for the objects it detects(sorted and colourized from green to red by distance).

    //This is an Enemy Character which has a FOV and distance in which it checks for the player.
    //It uses https://docs.unity3d.com/ScriptReference/Handles.DrawSolidArc.html
    //to display it's Field Of View, it changes colour depending on whether the player is detected or not.


    //    And last but not least:
    //We have simple "level area group" system.To quickly define areas of the level inside the inspector.
    //It uses https://docs.unity3d.com/ScriptReference/Handles.Label.html
    //to show each area's name.
    //It uses https://docs.unity3d.com/ScriptReference/Handles.FreeMoveHandle.html
    //to move each area's offset position.
    //It uses https://docs.unity3d.com/ScriptReference/Handles.DrawSolidRectangleWithOutline.html
    //to draw a rectangle in the shape and colour of each area.
    //And some https://docs.unity3d.com/ScriptReference/Handles.ScaleValueHandle.html with https://docs.unity3d.com/ScriptReference/Handles.RectangleHandleCap.html caps
    //to allow you to resize the areas from inside the scene view.
}
