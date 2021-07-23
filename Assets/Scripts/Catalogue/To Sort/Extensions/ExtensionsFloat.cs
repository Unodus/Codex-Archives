using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ExtensionsFloat
{
    public static float Clamp(this float fAmount, float fMin, float fMax)
    {
        if (fAmount < fMin)
        {
            fAmount = fMin;
        }

        if (fAmount > fMax)
        {
            fAmount = fMax;
        }
        return fAmount;
    }

    public static float Clamp(this float fAmount, float fMax)
    {
        return Clamp(fAmount, 0, fMax);
    }

    public static float Loop(this float fAmount, float fMin, float fMax)
    {
        while (fAmount < fMin)
        {
            fAmount += (fMax -fMin);
        }

        while (fAmount > fMax)
        {
            fAmount -= (fMax - fMin);
        }
        return fAmount;
    }

    public static float Loop(this float fAmount, float fMax)
    {
        return Loop(fAmount, 0, fMax);
    }
}

