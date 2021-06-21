using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//    Walter's tip of the day: 10/16/2020

//Did you know MenuItem's can have a shortcut attached?
//[MenuItem(itemName: "Name _%#T")]

//    The _%#T means (Command/Ctrl)+Shift+T in this case.
//Just replace the letter with another one and now you can do things in the Unity Editor via shortcuts.
//It will even show up in the Shortcuts Menu so you can change it at will!

//This is a script I made to simulate Physics in the editor.
//Handy for placing things like rocks on the ground without having to do it manually!(Just remember to remove the Rigidbodies on the rocks afterwards)

public class SelectionPhysics : MonoBehaviour
{
    private static bool _isSimulating;

    [MenuItem(itemName: "Tools/Toggle Simulate Physics _%#T")]
    private static void ToggleSimulatePhysics()
    {
        if (_isSimulating)
        {
            Physics.autoSimulation = true;
            EditorApplication.update -= UpdatePhysics;
            _isSimulating = false;
        }
        else
        {
            Physics.autoSimulation = false;
            EditorApplication.update += UpdatePhysics;
            _isSimulating = true;
        }
    }

    private static void UpdatePhysics()
    {
        Physics.Simulate(Time.fixedDeltaTime);
    }
}
