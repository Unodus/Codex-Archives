using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUtils : MonoBehaviour
{

    /// <summary>
    /// Gets the time as string, hours, minutes, seconds
    /// </summary>
    public string GetTimeAsString(long lTime = -1)
    {
        string sTime;
        int iHours;
        int iMinutes;
        int iSeconds;
        //int iFraction;

        iHours = (int)(lTime / 3600);
        iMinutes = (int)((lTime / 60) % 60);
        iSeconds = (int)(lTime % 60);
        //iFraction =  (int)((iTime * 1000) % 1000);

        //if minutes is within 0-9 mins, add a 0 in string
        if (GetIntegerDigitCount(iHours) == 1)
        {
            if (GetIntegerDigitCount(iMinutes) == 1)
            {
                if (GetIntegerDigitCount(iSeconds) == 1)
                {
                    sTime = string.Format("00{0}:0{1}:0{2}", iHours, iMinutes, iSeconds);
                }
                else
                {
                    sTime = string.Format("00{0}:0{1}:{2}", iHours, iMinutes, iSeconds);
                }
            }
            else
            {
                if (GetIntegerDigitCount(iSeconds) == 1)
                {
                    sTime = string.Format("00{0}:{1}:0{2}", iHours, iMinutes, iSeconds);
                }
                else
                {
                    sTime = string.Format("00{0}:{1}:{2}", iHours, iMinutes, iSeconds);
                }
            }
        }
        else if (GetIntegerDigitCount(iHours) == 2)
        {
            if (GetIntegerDigitCount(iMinutes) == 1)
            {
                if (GetIntegerDigitCount(iSeconds) == 1)
                {
                    sTime = string.Format("0{0}:0{1}:0{2}", iHours, iMinutes, iSeconds);
                }
                else
                {
                    sTime = string.Format("0{0}:0{1}:{2}", iHours, iMinutes, iSeconds);
                }
            }
            else
            {
                if (GetIntegerDigitCount(iSeconds) == 1)
                {
                    sTime = string.Format("0{0}:{1}:0{2}", iHours, iMinutes, iSeconds);
                }
                else
                {
                    sTime = string.Format("0{0}:{1}:{2}", iHours, iMinutes, iSeconds);
                }
            }
        }
        else
        {
            if (GetIntegerDigitCount(iMinutes) == 1)
            {
                if (GetIntegerDigitCount(iSeconds) == 1)
                {
                    sTime = string.Format("{0}:0{1}:0{2}", iHours, iMinutes, iSeconds);
                }
                else
                {
                    sTime = string.Format("{0}:0{1}:{2}", iHours, iMinutes, iSeconds);
                }
            }
            else
            {
                if (GetIntegerDigitCount(iSeconds) == 1)
                {
                    sTime = string.Format("{0}:{1}:0{2}", iHours, iMinutes, iSeconds);
                }
                else
                {
                    sTime = string.Format("{0}:{1}:{2}", iHours, iMinutes, iSeconds);
                }
            }
        }
        return sTime;
    }

    /// <summary>
    /// Return how many integers in a number
    /// </summary>
    public int GetIntegerDigitCount(int valueInt)
    {
        double value = valueInt;
        int sign = 0;
        if (value < 0)
        {
            value = -value;
            sign = 1;
        }

        if (value <= 9)
        {
            return sign + 1;
        }

        if (value <= 99)
        {
            return sign + 2;
        }

        if (value <= 999)
        {
            return sign + 3;
        }

        return sign + 4;
    }

    public int[] GetIntArray(int num)
    {
        List<int> listOfInts = new List<int>();
        while (num > 0)
        {
            listOfInts.Add(num % 10);
            num = num / 10;
        }
        listOfInts.Reverse();
        return listOfInts.ToArray();
    }

    public long[] GetLongArray(long num)
    {
        List<long> listOfLongs = new List<long>();
        while (num > 0)
        {
            listOfLongs.Add(num % 10);
            num = num / 10;
        }
        listOfLongs.Reverse();
        return listOfLongs.ToArray();
    }

    public T DeserializeTextAsset<T>(string filename)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(filename, typeof(TextAsset));

        if (textAsset == null)
        {
            Debug.LogError("Could not load text asset " + filename);
        }

        return DeserializeString<T>(textAsset.ToString());
    }

    //filename an object from an XML string.
    public T DeserializeString<T>(string xml)
    {
        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        System.IO.StringReader stringReader = new System.IO.StringReader(xml);
        System.Xml.XmlTextReader xmlReader = new System.Xml.XmlTextReader(stringReader);
        T obj = (T)serializer.Deserialize(xmlReader);

        xmlReader.Close();
        stringReader.Close();

        return obj;
    }
}
