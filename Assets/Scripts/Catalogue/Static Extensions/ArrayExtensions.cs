using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions
{
    // This is an extension method. RandomItem() will now exist on all arrays.
    public static T RandomItem<T>(this T[] array)
    {
        return array[UnityEngine.Random.Range(0, (array.Length - 1))];
    }


    /// <summary>
    /// Compare two arrays.
    /// </summary>
    public static bool CompareArray<TArray>(this TArray[] arr1, TArray[] arr2)
    {
        if (arr1.Length != arr2.Length)
        {
            return false;
        }

        int iLength = arr1.Length;

        EqualityComparer<TArray> comparer = EqualityComparer<TArray>.Default;

        for (int i = 0; i < iLength; ++i)
        {
            if (false == comparer.Equals(arr1[i], arr2[i]))
            {
                return false;
            }
        }

        return true;
    }
}