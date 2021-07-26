using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Degrees
{
    static int DEGREES_IN_CIRCLE_HALF = 180;
    static int DEGREES_IN_CIRCLE_TOTAL = 360;

    public static float Limit(float lAmount)
    {
        while (lAmount > DEGREES_IN_CIRCLE_HALF) lAmount -= DEGREES_IN_CIRCLE_TOTAL;
        while (lAmount < -DEGREES_IN_CIRCLE_HALF) lAmount += DEGREES_IN_CIRCLE_TOTAL;
        return lAmount;
    }
    public static Vector3 Limit(Vector3 lAmount)
    {
        lAmount.x = Limit(lAmount.x);
        lAmount.y = Limit(lAmount.y);
        lAmount.z = Limit(lAmount.z);
        return lAmount;
    }
    public static float GetRealRotateAmount(float fFrom, float fTo)
    {
        fFrom = (fFrom % DEGREES_IN_CIRCLE_TOTAL);
        fTo = (fTo % DEGREES_IN_CIRCLE_TOTAL);
        float fClockWise = 0f;
        float fCounterClockWise = 0f;

        if (fFrom <= fTo)
        {
            fClockWise = fTo - fFrom;
            fCounterClockWise = fFrom + (DEGREES_IN_CIRCLE_TOTAL - fTo);
        }
        else
        {
            fClockWise = (DEGREES_IN_CIRCLE_TOTAL - fFrom) + fTo;
            fCounterClockWise = fFrom - fTo;
        }

        if ((fClockWise <= fCounterClockWise))
        {
            return (fClockWise % DEGREES_IN_CIRCLE_TOTAL);
        }
        else
        {
            return -(fCounterClockWise % DEGREES_IN_CIRCLE_TOTAL);
        }
    }
    public static Vector3 LerpRotation(Vector3 From, Vector3 To, float fTime)
    {
        Vector3 lerp = Vector3.zero;
        float x = GetRealRotateAmount(From.x, To.x);
        float y = GetRealRotateAmount(From.y, To.y);
        float z = GetRealRotateAmount(From.z, To.z);

        lerp.x = Mathf.Lerp(From.x, (From.x + x), fTime);
        lerp.y = Mathf.Lerp(From.y, (From.y + y), fTime);
        lerp.z = Mathf.Lerp(From.z, (From.z + z), fTime);
        return lerp;
    }
    public static Vector3 ClampRotation(Vector3 lOriginal, Vector3 lmin, Vector3 lmax)
    {
        lOriginal -= new Vector3(DEGREES_IN_CIRCLE_HALF, DEGREES_IN_CIRCLE_HALF, DEGREES_IN_CIRCLE_HALF);
        lOriginal.x = lOriginal.x.Clamp(lmin.x - DEGREES_IN_CIRCLE_HALF, lmax.x - DEGREES_IN_CIRCLE_HALF);
        lOriginal.y = lOriginal.y.Clamp(lmin.y - DEGREES_IN_CIRCLE_HALF, lmax.y - DEGREES_IN_CIRCLE_HALF);
        lOriginal.z = lOriginal.z.Clamp(lmin.z - DEGREES_IN_CIRCLE_HALF, lmax.z - DEGREES_IN_CIRCLE_HALF);
        lOriginal += new Vector3(DEGREES_IN_CIRCLE_HALF, DEGREES_IN_CIRCLE_HALF, DEGREES_IN_CIRCLE_HALF);

        return lOriginal;
    }

}


