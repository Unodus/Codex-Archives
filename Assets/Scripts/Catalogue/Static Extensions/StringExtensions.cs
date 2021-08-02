using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringExtensions
{
    /// Truncates the string name to certain length.
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    // Named format strings from object attributes. Eg:
    // string blaStr = aPerson.ToString("My name is {FirstName} {LastName}.")
    // From: http://www.hanselman.com/blog/CommentView.aspx?guid=fde45b51-9d12-46fd-b877-da6172fe1791
    public static string ToString(this object anObject, string aFormat)
    {
        return ToString(anObject, aFormat, null);
    }

    public static string ToString(this object anObject, string aFormat, IFormatProvider formatProvider)
    {
        StringBuilder sb = new StringBuilder();
        Type type = anObject.GetType();
        Regex reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
        MatchCollection mc = reg.Matches(aFormat);
        int startIndex = 0;
        foreach (Match m in mc)
        {
            Group g = m.Groups[2]; //it's second in the match between { and }
            int length = g.Index - startIndex - 1;
            sb.Append(aFormat.Substring(startIndex, length));

            string toGet = string.Empty;
            string toFormat = string.Empty;
            int formatIndex = g.Value.IndexOf(":"); //formatting would be to the right of a :
            if (formatIndex == -1) //no formatting, no worries
            {
                toGet = g.Value;
            }
            else //pickup the formatting
            {
                toGet = g.Value.Substring(0, formatIndex);
                toFormat = g.Value.Substring(formatIndex + 1);
            }

            //first try properties
            PropertyInfo retrievedProperty = type.GetProperty(toGet);
            Type retrievedType = null;
            object retrievedObject = null;
            if (retrievedProperty != null)
            {
                retrievedType = retrievedProperty.PropertyType;
                retrievedObject = retrievedProperty.GetValue(anObject, null);
            }
            else //try fields
            {
                FieldInfo retrievedField = type.GetField(toGet);
                if (retrievedField != null)
                {
                    retrievedType = retrievedField.FieldType;
                    retrievedObject = retrievedField.GetValue(anObject);
                }
            }

            if (retrievedType != null) //Cool, we found something
            {
                string result = string.Empty;
                if (toFormat == string.Empty) //no format info
                {
                    result = retrievedType.InvokeMember("ToString",
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                        , null, retrievedObject, null) as string;
                }
                else //format info
                {
                    result = retrievedType.InvokeMember("ToString",
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                        , null, retrievedObject, new object[] { toFormat, formatProvider }) as string;
                }
                sb.Append(result);
            }
            else //didn't find a property with that name, so be gracious and put it back
            {
                sb.Append("{");
                sb.Append(g.Value);
                sb.Append("}");
            }
            startIndex = g.Index + g.Length + 1;
        }
        if (startIndex < aFormat.Length) //include the rest (end) of the string
        {
            sb.Append(aFormat.Substring(startIndex));
        }
        return sb.ToString();
    }



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
