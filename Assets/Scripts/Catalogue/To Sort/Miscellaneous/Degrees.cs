using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public static class Degrees180
    {
        public static Vector3 Limit(Vector3 lAmount)
        {
            lAmount.x = Limit(lAmount.x);
            lAmount.y = Limit(lAmount.y);
            lAmount.z = Limit(lAmount.z);
            return lAmount;
        }

        public static float Limit(float lAmount)
        {
            while (lAmount > LocalUtils.DEGREES_IN_CIRCLE_HALF)
            {
                lAmount -= LocalUtils.DEGREES_IN_CIRCLE_TOTAL;
            }

            while (lAmount < -LocalUtils.DEGREES_IN_CIRCLE_HALF)
            {
                lAmount += LocalUtils.DEGREES_IN_CIRCLE_TOTAL;
            }
            return lAmount;
        }
    }

    public static class Degrees360
    {

        static public float GetRealRotateAmount(float fFrom, float fTo)
        {
            fFrom = (fFrom % LocalUtils.DEGREES_IN_CIRCLE_TOTAL);
            fTo = (fTo % LocalUtils.DEGREES_IN_CIRCLE_TOTAL);
            float fClockWise = 0f;
            float fCounterClockWise = 0f;

            if (fFrom <= fTo)
            {
                fClockWise = fTo - fFrom;
                fCounterClockWise = fFrom + (LocalUtils.DEGREES_IN_CIRCLE_TOTAL - fTo);
            }
            else
            {
                fClockWise = (LocalUtils.DEGREES_IN_CIRCLE_TOTAL - fFrom) + fTo;
                fCounterClockWise = fFrom - fTo;
            }

            if ((fClockWise <= fCounterClockWise))
            {
                return (fClockWise % LocalUtils.DEGREES_IN_CIRCLE_TOTAL);
            }
            else
            {
                return -(fCounterClockWise % LocalUtils.DEGREES_IN_CIRCLE_TOTAL);
            }
        }


        static public Vector3 LerpRotation(Vector3 From, Vector3 To, float fTime)
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
            lOriginal -= new Vector3(LocalUtils.DEGREES_IN_CIRCLE_HALF, LocalUtils.DEGREES_IN_CIRCLE_HALF, LocalUtils.DEGREES_IN_CIRCLE_HALF);
            lOriginal.x = lOriginal.x.Clamp(lmin.x - LocalUtils.DEGREES_IN_CIRCLE_HALF, lmax.x - LocalUtils.DEGREES_IN_CIRCLE_HALF);
            lOriginal.y = lOriginal.y.Clamp(lmin.y - LocalUtils.DEGREES_IN_CIRCLE_HALF, lmax.y - LocalUtils.DEGREES_IN_CIRCLE_HALF);
            lOriginal.z = lOriginal.z.Clamp(lmin.z - LocalUtils.DEGREES_IN_CIRCLE_HALF, lmax.z - LocalUtils.DEGREES_IN_CIRCLE_HALF);
            lOriginal += new Vector3(LocalUtils.DEGREES_IN_CIRCLE_HALF, LocalUtils.DEGREES_IN_CIRCLE_HALF, LocalUtils.DEGREES_IN_CIRCLE_HALF);

            return lOriginal;
        }
    }
}
