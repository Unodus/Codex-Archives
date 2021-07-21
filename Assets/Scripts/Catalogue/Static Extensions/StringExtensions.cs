using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions
{
    public static bool ContainsCaseInsensitive(this string source, string toCheck)
    {
        return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    /// <summary>
    /// Get the nummer of lines in the string.
    /// </summary>
    /// <returns>Nummer of lines</returns>
    public static int LineCount(this string str)
    {
        return str.Split('\n').Length;
    }
}
