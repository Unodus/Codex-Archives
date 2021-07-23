using UnityEngine;

public enum Axis
{
    x,
    y,
    z
}

public static class ExtensionsVector3
{
    public static float GetVector3Axis(this Vector3 lVector3, Axis lAxis)
    {
        switch (lAxis)
        {
            case Axis.x: return lVector3.x;
            case Axis.y: return lVector3.y;
            case Axis.z: return lVector3.z;
        }
        return 0;
    }

    public static Vector3 GetReplaceVector3Axis(this Vector3 lVector3, float fValue, Axis lAxis)
    {
        Vector3 lVector3Temp = lVector3;
        switch (lAxis)
        {
            case Axis.x: lVector3Temp.x = fValue; break;
            case Axis.y: lVector3Temp.y = fValue; break;
            case Axis.z: lVector3Temp.z = fValue; break;
        }
        return lVector3Temp;
    }

}
