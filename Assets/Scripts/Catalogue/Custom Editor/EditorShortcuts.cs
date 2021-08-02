using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorShortcuts : MonoBehaviour
{
    // Toggle the active value of the selected GameObjects using CTRL + W
    [MenuItem("Tools/Toggle Object %w")]
    public static void ToggleObjects()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
            Selection.gameObjects[i].SetActive(!Selection.gameObjects[i].activeSelf);
    }
}
