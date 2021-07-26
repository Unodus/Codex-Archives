using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions 
{


 


    /// <summary>
    /// Change the x value of vector 3
    /// </summary>
    public static Vector3 ChangeX(this Vector3 v, float fValX)
    {
        return new Vector3(fValX, v.y, v.z);
    }

    /// <summary>
    /// Change the y value of vector 3
    /// </summary>
    public static Vector3 ChangeY(this Vector3 v, float fValY)
    {
        return new Vector3(v.x, fValY, v.z);
    }

    /// <summary>
    /// Change the z value of vector 3
    /// </summary>
    public static Vector3 ChangeZ(this Vector3 v, float fValZ)
    {
        return new Vector3(v.x, v.y, fValZ);
    }

}
