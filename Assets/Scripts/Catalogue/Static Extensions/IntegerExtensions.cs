using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntegerExtensions 
{
    public static int Clamp(this int iAmount, int iMin, int iMax)
    {
        if (iAmount < iMin) iAmount = iMin;
        if (iAmount > iMax) iAmount = iMax;
        return iAmount;
    }

    public static int Clamp(this int iAmount, int iMax)
    {
        return Clamp(iAmount, 0, iMax);
    }

    public static int IncrementClamp(this int iAmount, int iMax)
    {
        iAmount++;
        return Clamp(iAmount, 0, iMax);
    }

    public static int IncrementClamp(this int iAmount, int iMin, int iMax)
    {
        iAmount++;
        return Clamp(iAmount, iMin, iMax);
    }

    public static int DecrementClamp(this int iAmount, int iMax)
    {
        iAmount--;
        return Clamp(iAmount, 0, iMax);
    }
    public static int DecrementClamp(this int iAmount, int iMin, int iMax)
    {
        iAmount--;
        return Clamp(iAmount, iMin, iMax);
    }

    public static int Loop(this int iAmount, int iMin, int iMax)
    {
        if (iAmount < iMin) iAmount = iMax;
        if (iAmount > iMax) iAmount = iMin;
        return iAmount;
    }

    public static int Loop(this int iAmount, int iMax)
    {
        return Loop(iAmount, 0, iMax);
    }

    public static int IncrementLoop(this int iAmount, int iMax)
    {
        iAmount++;
        return Loop(iAmount, 0, iMax);
    }

    public static int IncrementLoop(this int iAmount, int iMin, int iMax)
    {
        iAmount++;
        return Loop(iAmount, iMin, iMax);
    }

    public static int DecrementLoop(this int iAmount, int iMax)
    {
        iAmount--;
        return Loop(iAmount, 0, iMax);
    }

    public static int DecrementLoop(this int iAmount, int iMin, int iMax)
    {
        iAmount--;
        return Loop(iAmount, iMin, iMax);
    }


}
