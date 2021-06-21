using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTest : MonoBehaviour
{
    //    Walter's tip of the day: 10/13/2020

    //Have you ever wanted to debug your script on encountering certain situations, but you're too lazy to write unit tests?
    //Or have you ever play tested your game and found all the grinding too boring, but you're too lazy to make a cheat command system?

    //Well have I got something for you.

    //https://docs.unity3d.com/ScriptReference/ContextMenu.html

    //Simply right-click your MonoBehaviour and execute a method by force.


    [SerializeField] private int myValue = 100;
    public int Value
    {
        get => myValue;
        private set => myValue = Mathf.Clamp(value: (myValue = value), min: 0, max: 100);
    }

    [ContextMenu(itemName: "Cheat: + 10")]
    private void Cheat10Points() => Value += 10;
}
