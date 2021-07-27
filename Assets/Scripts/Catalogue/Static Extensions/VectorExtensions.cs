using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions 
{

    public static Vector2 Rotate(this Vector2 vector, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = vector.x;
        float ty = vector.y;
        vector.x = (cos * tx) - (sin * ty);
        vector.y = (sin * tx) + (cos * ty);
        return vector;
    }

    public static Vector3Int ConvertToVector3(this Vector3 vec3)
    {
        return new Vector3Int((int)vec3.x, (int)vec3.y, (int)vec3.z);
    }

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
