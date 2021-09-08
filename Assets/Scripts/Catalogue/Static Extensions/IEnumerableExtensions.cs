using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IEnumerableExtensions
{
    public static bool ExceedsThreshold(this IEnumerable<bool> bools, int threshold)
    {
        return bools.Count(b => b) > threshold;
    }

    public static CoroutineHandle RunCoroutine(this MonoBehaviour owner, IEnumerator coroutine)
    {
        return new CoroutineHandle(owner, coroutine);
    }
}