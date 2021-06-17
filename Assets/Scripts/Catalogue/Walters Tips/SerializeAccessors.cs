using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializeAccessors : MonoBehaviour
{
    //Walter's tip of the day, 10/10/2020

    //Did you know you can serialize Accessors/Properties in Unity?
    //However it does require it has a setter.

    #region Non-Serialized

    public int NonSerializedV1 { get; set; }
    
    [SerializeField]
    public int NonSerializedV2 { get; set; }

    [field: SerializeField]
    public int NonSerializedV3 { get; } = 69;




    #endregion

    // ^ None of these will show in the inspector


    #region Serialized

    [field: SerializeField]
    public int SerializedV1 { get; set; }

    [field: SerializeField]
    public int SerializedV2 { get; private set; }

    [field: SerializeField]
    public int SerializedV3 { get; private set; } = 69;

    #endregion

    // ^ This allows you to set properties from the inspector, so you don't have to do ugly hacks like this anymore: 

    #region Serialized_Old

    [SerializeField] private int SerializedV4;
    public int NonSerializedV4
    {
        get => SerializedV4;
        private set => SerializedV4 = value;
    }

    #endregion

    // You can do it by simply adding field: inside of [SerializeField]. [field: SerializeField]

}
