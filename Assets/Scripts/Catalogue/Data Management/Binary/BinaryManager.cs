using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BinaryManager 
{


    #region Bitwise operator helpers	
    /// <summary>
    /// Merge 2 shorts to make a long
    /// </summary>
    public static int MakeLong(short lowPart, short highPart)
    {
        return (int)(((ushort)lowPart) | (uint)(highPart << 16));
    }

    /// <summary>
    /// Get the high value from a long
    /// </summary>
    public static short HighWord(int iBit)
    {
        return (short)(iBit >> 16);
    }

    /// <summary>
    /// Get the low value from a long
    /// </summary>
    public static short LowWord(int iBit)
    {
        return (short)iBit;
    }

    /// <summary>
    /// Numbers the of set bits in iValue.
    /// </summary>
    public static int NumberOfSetBits(int iValue)
    {
        iValue = iValue - ((iValue >> 1) & 0x55555555);
        iValue = (iValue & 0x33333333) + ((iValue >> 2) & 0x33333333);
        return (((iValue + (iValue >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
    }
    #endregion


    #region Binary writing
    /// <summary>
    /// Write a vector3 to binary
    /// </summary>
    public static void Write(System.IO.BinaryWriter file, Vector3 v)
    {
        file.Write(v.x);
        file.Write(v.y);
        file.Write(v.z);
    }

    /// <summary>
    /// Write a quaternion to binary
    /// </summary>
    public static void Write(System.IO.BinaryWriter file, Quaternion q)
    {
        file.Write(q.x);
        file.Write(q.y);
        file.Write(q.z);
        file.Write(q.w);
    }

    /// <summary>
    /// Read a vector3 from binary
    /// </summary>
    public static Vector3 ReadVector3(System.IO.BinaryReader file)
    {
        Vector3 v = new Vector3();
        v.x = file.ReadSingle();
        v.y = file.ReadSingle();
        v.z = file.ReadSingle();
        return v;
    }

    /// <summary>
    /// Read a quaternion from binary
    /// </summary>
    public static Quaternion ReadQuaternion(System.IO.BinaryReader file)
    {
        Quaternion q = new Quaternion();
        q.x = file.ReadSingle();
        q.y = file.ReadSingle();
        q.z = file.ReadSingle();
        q.w = file.ReadSingle();
        return q;
    }

    #endregion

}
