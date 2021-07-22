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

}
