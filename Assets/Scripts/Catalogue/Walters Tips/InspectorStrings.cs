using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorStrings : MonoBehaviour
{
    //    Walter's (small) tip of the day - 06/04/2021
    //Did you know Unity has different ways of showing string inside the inspector?
    //Instead of jamming it all into a tiny little box you can show multiple lines.

    //All it takes is an extra attribute!

    //For a set amount use this: [Multiline(lines: 3)]

    [Multiline(lines: 3)]
    [SerializeField] private string multiLineDescription = "";

    //    Do you not know how large you need it?
    //Well, there's one with a scrollbar too! [TextArea] 

    [TextArea]
    [SerializeField] private string textAreaDescription = "";

    // You can also change the limits on the [TextArea] like this:
    [TextArea(minLines: 1, maxLines: 10)]
    [SerializeField] private string customTextAreaDescription = "";
    // It will then resize until it reaches the maxLines, only after that amount it will use the scrollbar.


    // Note: Unfortunately none of them work on Properties, but you can probably make a custom one if you really need it.
}
