using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public static class StringExtensions
{

    // Remove all diacritics from a string
    public static string RemoveDiacritics(this string text)
    {
        string normalizedString = text.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();
        char character;

        for (int i = 0; i < normalizedString.Length; i++)
        {
            character = normalizedString[i];
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(character);
            }
        }

        return stringBuilder.ToString();
    }
    public static bool Contains(this string source, string toCheck, bool CaseSensitive = false )
    {
        if (CaseSensitive) return source.IndexOf(toCheck, System.StringComparison.Ordinal) >= 0;
        else return source.IndexOf(toCheck, System.StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public static string AddSpacesToSentence(this string text, bool preserveAcronyms)
    {
        if (true == string.IsNullOrEmpty(text))
        {
            return "";
        }

        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);

        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
            {
                if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                    (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                     i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                {
                    newText.Append(' ');
                }
            }
            newText.Append(text[i]);
        }
        return newText.ToString();
    }


    public static IEnumerable<string> SplitString(string str, int maxChunkSize)
    {
        for (int i = 0; i < str.Length; i += maxChunkSize)
            yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
    }

    public static string ConcatString(this string[] str)
    {
        string sFinal = "";
        int iCount = str.Length;
        for (int i = 0; i < iCount; i++)
        {
            sFinal += str[i];
        }

        return sFinal;
    }

    /// <summary>
    /// Truncates the string name to certain length.
    /// </summary>
    public static string Truncate(string sSource, int iLength)
    {
        if (sSource.Length > iLength)
        {
            sSource = sSource.Substring(0, iLength);
        }
        return sSource;
    }

    /// <summary>
    /// Prefixs all objects in transform with sName.
    /// </summary>

    public static void PrefixAllWith(this Transform transform, string sName)
    {
        string sOldName = transform.name;
        transform.name = sName + "_" + sOldName;

        foreach (Transform tran in transform)
        {
            PrefixAllWith(tran, sName);
        }
    }

    /// <summary>
    /// Parses a string to vector3.
    /// </summary>
    public static Vector3 ParseStringToVector3(this string sSourceString)
    {
        string outString;
        Vector3 outVector3;
        string[] splitString;

        outString = sSourceString.Substring(1, sSourceString.Length - 2);

        splitString = outString.Split(","[0]);

        // Build new Vector3 from array elements

        outVector3.x = float.Parse(splitString[0]);
        outVector3.y = float.Parse(splitString[1]);
        outVector3.z = float.Parse(splitString[2]);

        return outVector3;
    }

    /// <summary>
    /// Takes strings formatted with numbers and no spaces before or after the commas.
    /// if B255 is true means the color has come in a 0-255 format, if not, 0-1
    /// </summary>
    /// <returns>
    public static Color ParseStringToColour(this string sCol, bool bFormat255 = false)
    {
        Color output = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        //default to grey, half alpha
        if (sCol == "")
        {
            return output;
        }

        // "1.0,1.0,.35,1.0"
        string[] strings = sCol.Split(',');

        for (int i = 0; i < 4; i++)
        {
            float fColorValue = System.Single.Parse(strings[i]);
            if (true == bFormat255)
            {
                fColorValue = fColorValue / 255f;
            }

            output[i] = fColorValue;
        }
        return output;
    }
    public static bool IsValidDigit(this string str)
    {
        foreach (char c in str)
        {
            if (c < '0' && c > '9')
                return false;
        }

        return true;
    }

    public static bool IsValidDigit(this string str, char[] charsArray)
    {
        int iCount = charsArray.Length;
        foreach (char c in str)
        {
            for (int i = 0; i < iCount; i++)
            {
                if (c < '0' && c > '9' && c != charsArray[i])
                {
                    return false;
                }
            }
        }

        return true;
    }

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
