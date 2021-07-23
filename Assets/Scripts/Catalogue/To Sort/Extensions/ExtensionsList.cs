using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsList
{
    public static bool CanMoveUp<T>(this List<T> list, uint lIndex)
    {
        return (list.IsValidIndex(lIndex) && lIndex > 0);
    }


    /// <summary>
    /// 
    /// </summary>
    public static bool CanMoveDown<T>(this List<T> list, uint lIndex)
    {
        return (list.IsValidIndex(lIndex) && lIndex <= list.Count - 2);
    }


    /// <summary>
    /// 
    /// </summary>
    public static void MoveUp<T>(this List<T> list, uint lIndex)
    {
        if (list.CanMoveUp(lIndex) == true)
        {
            list.Swap(lIndex, lIndex - 1);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public static void MoveDown<T>(this List<T> list, uint lIndex)
    {
        if (list.CanMoveDown(lIndex) == true)
        {
            list.Swap(lIndex, lIndex + 1);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public static void Swap<T>(this List<T> list, uint lIndex1, uint lIndex2)
    {
        if (lIndex1 == lIndex2)
            return;
        if (list.IsValidIndex(lIndex1) == false || list.IsValidIndex(lIndex2) == false)
        {
            return;
        }
        T aux = list[(int)lIndex1];
        list[(int)lIndex1] = list[(int)lIndex2];
        list[(int)lIndex2] = aux;
    }


    /// <summary>
    /// 
    /// </summary>
    public static bool IsValidIndex<T>(this List<T> list, uint lIndex)
    {
        return (lIndex <= list.Count - 1);
    }
}
