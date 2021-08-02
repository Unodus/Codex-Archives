using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtenstions
{

    public static T Random<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            Debug.LogError("Cannot select a random item from an empty list");
            return default(T);
        }

        return list[UnityEngine.Random.Range(0, list.Count)];
    }


    public static void AddMany<T>(this List<T> list, params T[] elements)
    {
        list.AddRange(elements);
    }

    public static void MoveUp<T>(this List<T> list, uint lIndex)
    {
        if (list.CanMoveUp(lIndex) == true) list.Swap(lIndex, lIndex - 1);
    }
    public static void MoveDown<T>(this List<T> list, uint lIndex)
    {
        if (list.CanMoveDown(lIndex) == true) list.Swap(lIndex, lIndex + 1);
    }

    public static bool CanMoveUp<T>(this List<T> list, uint lIndex)
    {
        return (list.IsValidIndex(lIndex) && lIndex > 0);
    }
    public static bool CanMoveDown<T>(this List<T> list, uint lIndex)
    {
        return (list.IsValidIndex(lIndex) && lIndex <= list.Count - 2);
    }

    public static void Swap<T>(this List<T> list, uint lIndex1, uint lIndex2)
    {
        if (lIndex1 == lIndex2) return;
        if (list.IsValidIndex(lIndex1) == false || list.IsValidIndex(lIndex2) == false) return;
        T aux = list[(int)lIndex1];
        list[(int)lIndex1] = list[(int)lIndex2];
        list[(int)lIndex2] = aux;
    }
    public static bool IsValidIndex<T>(this List<T> list, uint lIndex)
    {
        return (lIndex <= list.Count - 1);
    }
}
