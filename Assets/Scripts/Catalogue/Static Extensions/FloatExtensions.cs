using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions 
{

    #region Data Helpers
    private static bool CheckFloat(this float fInputValue)
    {
        if (fInputValue == float.MaxValue)
        {
            Debug.Log("Value was float.MaxValue");
            return false;
        }

        if (fInputValue == float.NaN)
        {
            Debug.Log("Value was float.NaN");
            return false;
        }

        if (true == float.IsInfinity(fInputValue))
        {
            Debug.Log("Value was float.Infinity");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Create new Vector3 with the same value in all axis
    /// </summary>
    public static Vector3 ToVector3(this float s)
    {
        return new Vector3(s, s, s);
    }

    #endregion

    public static float RotationNormalizedDeg(this float rotation)
    {
        rotation = rotation % 360f;
        if (rotation < 0)
            rotation += 360f;
        return rotation;
    }


    public static float Clamp(this float fAmount, float fMin, float fMax)
    {
        if (fAmount < fMin) fAmount = fMin;
        if (fAmount > fMax) fAmount = fMax;
        return fAmount;
    }

    public static float Clamp(this float fAmount, float fMax)
    {
        return Clamp(fAmount, 0, fMax);
    }

    public static float Loop(this float fAmount, float fMin, float fMax)
    {
        while (fAmount < fMin) fAmount += (fMax - fMin);
        while (fAmount > fMax) fAmount -= (fMax - fMin);
        return fAmount;
    }

    public static float Loop(this float fAmount, float fMax)
    {
        return Loop(fAmount, 0, fMax);
    }

}
