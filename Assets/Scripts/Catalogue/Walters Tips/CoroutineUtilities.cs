using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Walter 's tip of the day - 11/08/2020

// Did you know you can choose to cache WaitForSeconds' to decrease allocation?
// And, you can actually re-use this globally!


// Use "yield return CoroutineUtilities.Wait(seconds: timeBetweenRounds);" to get the waitforseconds from this dictionary instead!
public class CoroutineUtilities : MonoBehaviour
{
    private static readonly Dictionary<float, WaitForSeconds> WaitsDictionary = new Dictionary<float, WaitForSeconds>();

    /// <summary> Gives you a reusable <see cref="WaitForSeconds"/>.</summary>
    /// <returns> A reusable <see cref="WaitForSeconds"/>. </returns>
     
    public static WaitForSeconds Wait(in float seconds)
    {
        //If the dictionary contains an entry with key 'seconds' it returns the found entry.
        if (WaitsDictionary.TryGetValue(key: seconds, value: out WaitForSeconds __result)) return __result;

        //If not, it adds it and returns the result.
        WaitsDictionary.Add(
            key: seconds,
            value: new WaitForSeconds(seconds: seconds));

        return WaitsDictionary[key: seconds];
    }
}
