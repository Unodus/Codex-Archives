using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsString
{
    public enum ECaseSensitive
    {
        CaseSensitive,
        NoCaseSensitive,
    }

    public static bool Contains(this string source, string toCheck, ECaseSensitive lECaseSensitive = ECaseSensitive.NoCaseSensitive)
    {
        if (lECaseSensitive == ECaseSensitive.CaseSensitive)
        {
            return source.IndexOf(toCheck, System.StringComparison.Ordinal) >= 0;
        }
        else
        {
            return source.IndexOf(toCheck, System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

    }

}
