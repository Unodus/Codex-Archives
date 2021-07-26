using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class EnumExtensions
{

    public static T LoopIncrement<T>(this T lIndex) where T : struct, IConvertible
    {
        int lEnumInt = lIndex.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
        lEnumInt++;
        int lEnumCount = Enum.GetNames(typeof(T)).Length;
        if (lEnumInt > lEnumCount - 1)
        {
            lEnumInt = 0;
        }
        return (T)(object)lEnumInt;
    }

    public static T LoopDecrement<T>(this T lIndex) where T : struct, IConvertible
    {
        int lEnumInt = lIndex.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
        lEnumInt--;
        int lEnumCount = Enum.GetNames(typeof(T)).Length;
        if (lEnumInt < 0)
        {
            lEnumInt = lEnumCount - 1;
        }
        return (T)(object)lEnumInt;
    }


    public static T ClampDecrement<T>(this T lIndex) where T : struct, IConvertible
    {
        int lEnumInt = lIndex.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
        lEnumInt--;
        if (lEnumInt < 0)
        {
            lEnumInt = 0;
        }
        return (T)(object)lEnumInt;
    }


    public static T ClampIncrement<T>(this T lIndex) where T : struct, IConvertible
    {
        int lEnumInt = lIndex.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
        lEnumInt++;
        int lEnumCount = Enum.GetNames(typeof(T)).Length;
        if (lEnumInt > lEnumCount - 1)
        {
            lEnumInt = lEnumCount - 1;
        }
        return (T)(object)lEnumInt;
    }

    public static bool CompareEnumList<T>(this List<T> lList1, List<T> lList2)
    {
        if (lList1.Count != lList2.Count) return false;
        for (int i = 0; i < lList1.Count; i++)
        {
            if (lList1[i].Equals(lList2[i])) return false;
        }
        return true;
    }

    /// <summary>
    /// Ints to enum.
    /// </summary>
    public static T IntToEnum<T>(this int lInt, out bool lValid)
    {
        if (System.Enum.IsDefined(typeof(T), lInt) == true)
        {
            lValid = true;
            return (T)System.Enum.Parse(typeof(T), lInt.ToString(), true);
        }
        T[] lTemp = (T[])System.Enum.GetValues(typeof(T));
        lValid = false;
        return lTemp[0];
    }


    /// <summary>
    /// Enums to int.
    /// </summary>
    public static int EnumToInt<T>(this T lEnum)
    {
        System.Enum lEnumTest = System.Enum.Parse(typeof(T), lEnum.ToString()) as System.Enum;
        return Convert.ToInt32(lEnumTest);
    }


    public static bool IsEnumInEnumItem<T1, T2>(this T2 lT2Item)
    {
        int lT2Number = lT2Item.EnumToInt<T2>();

        T1[] lEnums = (T1[])System.Enum.GetValues(typeof(T1));
        foreach (T1 lT1Item in lEnums)
        {
            int lT1Number = lT1Item.EnumToInt<T1>() ;

            if (lT1Number.Equals(lT2Number))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Allows movement to next and previous entries in an enum.
    /// Found at: https://stackoverflow.com/questions/642542/how-to-get-next-or-previous-enum-value-in-c-sharp
    /// </summary>


    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    //checks to see if an enumerated value contains a type
    public static bool Has<T>(this System.Enum type, T value)
    {
        try
        {
            return (((int)(object)type &
              (int)(object)value) == (int)(object)value);
        }
        catch
        {
            return false;
        }
    }
}