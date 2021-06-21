using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.ShortcutManagement;
#endif
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

    // [MenuItem(itemName: "Tools/Toggle Simulate Physics _%#T")] -- Old implementation

#if UNITY_EDITOR
    [Shortcut(id: nameof(SelectionPhysics) + "/" + nameof(ToggleSimulatePhysics), defaultShortcutModifiers: ShortcutModifiers.Shift, defaultKeyCode: KeyCode.L)]
#endif
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

//_____________

//Walter 's  tip of the day - 05/22/2021

//I'm already repeating myself, but here we go:
//As I've shown in an earlier tip as well, you can execute your own code using shortcuts, incredibly handy if you use custom editor tools, but also extremely useful for simple debugging.
//Problem is; the way I showed back then is stupid, it requires a MenuItem and has horrible synax (if you could call it that) -I only recently learned of the proper way to do it.

//The ShortcutAttribute!

//Just slap [Shortcut(id: "Name")] on your static method, and víola!. (don't worry about encapsulation, it used reflection)
//You can also give it a default KeyCode and modifier, so the user doesn't have to configure it manually and just use that, unless they want to change it!

//I like flipping the defaulyKeyCode and the defaultShortcutModifiers swapped from how you're "supposed" to call the constructor, because that's the order you say them in "Crtl+S" not "S+Crtl"
//However, this means you have to type the parameter names explicitly.

//Personally, I like naming the shortcut exactly the same as the method it's on - makes it easier to find it, should there be a bug with it.
//Now you can just type it out, or use what I prefer: nameof(), it will ensure that even if you rename it, it'll always be the name of the method or class!

