using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumExtensions
{

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