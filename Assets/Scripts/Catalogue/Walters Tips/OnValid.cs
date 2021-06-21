using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Walter 's tip of the day: 10/15/2020

//Don 't forget MonoBehaviours and ScriptableObjects can implement a method called "OnValidate"
//https://docs.unity3d.com/ScriptReference/ScriptableObject.OnValidate.html
//https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnValidate.html

//It gets called when the script is loaded, or every time a value is changed in the inspector.
//You can use this to validate your scripts variables.

//For example, when you want to make sure an integer doesn't go below 0? Just make it a uint, but how do you do this for Vectors?

//Well, like this:


public class OnValid : MonoBehaviour
{
    [SerializeField] private Vector2 nonNegativeVector;

#if UNITY_EDITOR

    private void OnValidate()
    {
        //Makes sure X and Y don't go below 0. (in the inspector at least)
        if (nonNegativeVector.x < 0)
        {
            nonNegativeVector.x = 0;
        }
        if (nonNegativeVector.y < 0)
        {
            nonNegativeVector.y = 0;
        }
    }

#endif
}
