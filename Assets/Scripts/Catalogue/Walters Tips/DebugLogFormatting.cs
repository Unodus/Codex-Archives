using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogFormatting : MonoBehaviour
{

    //    Walter's tip of the day: 10/11/2020

    //Did you know Debug.Log can be formatted with rich text?
    //https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html

    [SerializeField] private new string name;
    void Start()
    {
        //You can format you debugs with Bold, Italicized or Coloured text

        Debug.Log(message: "\n" +
            " <b> Bold </b> text \n" +
            " <i> Italicized  </i> text \n" +
            " <color=orange> Coloured  </color> text");


        //You can also change sizes of (parts of) the text.
        Debug.Log(message: $"Player name = <size=20>{name}</size>");

        //(I also used $ and {} to add variables to the string.

    }

}
