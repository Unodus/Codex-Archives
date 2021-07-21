using System;
using System.Collections.Generic;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Linq;


//A Class to 
public class LocalUtils : MonoBehaviour
{

    /// <summary>
    /// Returns time remaining using TimeSpan, DateTime and number of completed tasks out of total number of tasks
    /// </summary>
    public static string GetTimeRemaining(DateTime startTime, DateTime endTime, int iCompletedTasks, int iTotalTasks)
    {
        TimeSpan lTimeTaken = endTime - startTime;
        TimeSpan lTimeRemaining = new TimeSpan(0, 0, 0, 0, lTimeTaken.Milliseconds * (iTotalTasks - iCompletedTasks));
        return lTimeRemaining.ToString(@"hh\:mm\:ss");
    }

    /// <summary>
    /// From The Terrain Creation Project
    /// </summary>
    public static string GetTimeRemaining(float startTime, int iCompletedTasks, int iTotalTasks)
    {
        if (0 == iCompletedTasks)
        {
            return "One million years";
        }
        float fTimeTaken = Time.realtimeSinceStartup - startTime;
        TimeSpan lTimeRemaining = new TimeSpan((long)((fTimeTaken / iCompletedTasks) * (iTotalTasks - iCompletedTasks) * TimeSpan.TicksPerSecond));
        return lTimeRemaining.ToString(@"hh\:mm\:ss");
    }


    /// <summary>
	/// Projects a vector onto a plane. The output is not normalized.
	/// </summary>
	public static Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
    {
        return vector - (Vector3.Dot(vector, planeNormal) * planeNormal);
    }

    //A,B,a,b,x
    //formula is: a + (x - A)(b - a) / (B - A)
    public static float NormaliseValue(float fSmall, float fLarge, float fLowerRange, float fUpperRange, float fValue)
    {
        float fXminusA = fValue - fSmall;
        float fbminusa = fUpperRange - fLowerRange;
        float fBminusA = fLarge - fSmall;

        float fXAmultiplyba = fXminusA * fbminusa;
        float fTopDivideBottom = fXAmultiplyba / fBminusA;

        float fNormalisedValue = fLowerRange + fTopDivideBottom;
        return fNormalisedValue;
    }

    public static void SetLayerRecursively(GameObject obj, int iNewLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = iNewLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, iNewLayer);
        }
    }

    public static void SetLayerRecursively(GameObject obj, int iNewLayer, int iSkipLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = (obj.layer == iSkipLayer) ? iSkipLayer : iNewLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, iNewLayer, iSkipLayer);
        }
    }

    public static string TruncateLongString(string sString, int iMaxLength)
    {
        string sNewString = sString.Substring(0, Mathf.Min(sString.Length, iMaxLength));
        return sNewString;
    }

    public static string GetNumbersFromString(string sString)
    {
        return Regex.Replace(sString, @"[^0-9]+", "");
    }

    public static T GetComponentOrAdd<T>(GameObject obj, bool bShowErrorMessage = true) where T : UnityEngine.Component
    {
        if (null == obj)
        {
            if (true == bShowErrorMessage)
            {
                Debug.LogError("GetComponentOrAdd has been passed a null gameobject to add component " + typeof(T).ToString() + " to!");
            }
            return null;
        }

        T component = obj.GetComponent<T>();

        if (null == component)
        {
            component = obj.AddComponent<T>();
        }

        return component;
    }

    public static GameObject GetFirstMeshRendererGameObject(GameObject parent)
    {
        Transform child = parent.transform;
        MeshRenderer renderer = null;
        while ((child.childCount > 0) && (null == renderer))
        {
            child = child.GetChild(0);
            renderer = child.GetComponent<MeshRenderer>();
        }

        return child.gameObject;
    }

    public static GameObject[] FindGameObjectsWithName(string sName)
    {
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        GameObject[] arr = new GameObject[gameObjects.Length];
        int iFluentNumber = 0;
        int iLength = gameObjects.Length;
        for (int i = 0; i < iLength; i++)
        {
            if (true == gameObjects[i].name.Contains(sName))
            {
                arr[iFluentNumber] = gameObjects[i];
                iFluentNumber++;
            }
        }
        Array.Resize(ref arr, iFluentNumber);
        return arr;
    }

    public static GameObject[] FindGameObjectWithNameInParent(string sName, Transform tParent, bool bFindInactiveObjects = true)
    {
        Transform[] children = tParent.gameObject.GetComponentsInChildren<Transform>(bFindInactiveObjects);
        int iLength = children.Length;
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < iLength; i++)
        {
            if (true == children[i].name.Contains(sName))
            {
                objs.Add(children[i].gameObject);
            }
        }

        return objs.ToArray();
    }

    private static string m_sFormerObjectName = "";
    public static GameObject ReturnObjectFromHierarchy(GameObject obj, string sItemName)
    {
        if (null == obj)
        {
            return null; // No parent object to search.
        }

        if (true == string.IsNullOrEmpty(sItemName))
        {
            return null; //nothing to search for
        }

        Transform transformItem = obj.transform.FindDeepChild(sItemName);
        if (null != transformItem)
        {
            return transformItem.gameObject;
        }

        //This is so items that are not yet loaded, but looked for in an update loop, don't spam the console.
        if (sItemName != m_sFormerObjectName)
        {
            Debug.LogError("Item : " + sItemName + " : Cannot be found");
            m_sFormerObjectName = sItemName;
        }

        return null;
    }

    public static Transform[] FindAllChildrenContainingString(GameObject obj, string sString, bool bIncludeInactiveChildren = false)
    {
        List<Transform> listOfTransforms = new List<Transform>();

        foreach (Transform child in obj.GetComponentsInChildren<Transform>(bIncludeInactiveChildren))
        {
            if (true == child.name.Contains(sString))
            {
                listOfTransforms.Add(child);
            }
        }

        if (listOfTransforms.Count > 0)
        {
            return listOfTransforms.ToArray();
        }

        return null;
    }

    public static List<Transform> FindAllChildrenContainingStringAsList(GameObject obj, string sString, bool bIncludeInactiveChildren = false)
    {
        List<Transform> listOfTransforms = new List<Transform>();

        foreach (Transform child in obj.GetComponentsInChildren<Transform>(bIncludeInactiveChildren))
        {
            if (true == child.name.Contains(sString))
            {
                listOfTransforms.Add(child);
            }
        }

        if (listOfTransforms.Count > 0)
        {
            return listOfTransforms;
        }

        return null;
    }

    public static Transform FindFirstChildContainingString(GameObject obj, string sString, bool bIncludeInactiveChildren = false, bool bCaseSensitive = true)
    {
        if (null != obj)
        {
            foreach (Transform child in obj.GetComponentsInChildren<Transform>(bIncludeInactiveChildren))
            {
                if (true == bCaseSensitive)
                {
                    if (true == child.name.Contains(sString))
                    {
                        return child;
                    }
                }
            }
        }

        return null;
    }

    public static T ReturnItemFromHierarchy<T>(GameObject obj, string sItemName)
    {

#if UNITY_EDITOR
        // if use you do <GameObject>  then it will return null, change to <Transform>(item, string).gameobject ;

        string lTemplateToString = typeof(T).ToString();
        lTemplateToString = lTemplateToString.Substring(lTemplateToString.LastIndexOf('.') + 1);
        string lGameObjectToString = typeof(GameObject).Name;
        if (lTemplateToString == lGameObjectToString)
        {
            Debug.LogError("if you use    <GameObject>   as the Template then it does not seem to return correctly, only nulls");
        }
#endif

        Transform transformItem = obj.transform.FindDeepChild(sItemName);
        if (null != transformItem)
        {
            T returnItem = transformItem.GetComponent<T>();

            if (null != returnItem)
            {
                if (true == returnItem.Equals(default(T)))
                {
                    Debug.LogError(typeof(T).ToString() + " not found: " + sItemName);
                }
                return returnItem;
            }
            else
            {
                Debug.LogError(typeof(T).ToString() + " not found: " + sItemName);
                return default(T);
            }
        }
        else
        {
            Debug.LogError("Object not found: " + sItemName);
            return default(T);
        }
    }

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }


    public static void ParentAndTrimName(Transform parent, Transform child)
    {
        child.parent = parent;
        child.name = child.name.Replace("(Clone)", "");
    }

    //This allows new lines to come from the CSV files
    public static string ManageNewLineText(string sInText)
    {
        sInText = Regex.Unescape(sInText);
        return sInText;
    }
    //Allows try parse directly into property setter
    public static float ParseOrDefault(string sText)
    {
        float fTemp;
        if (true == float.TryParse(sText, out fTemp))
        {
            return fTemp;
        }
        else
        {
            return -1f;
        }

    }

#if UNITY_EDITOR
    public static void ClearLogConsole()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
        Type logEntries = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
        clearConsoleMethod.Invoke(new object(), null);
    }
#endif

    public static float AngleSigned(Vector3 from, Vector3 to, Vector3 axis)
    {
        return Mathf.Atan2(
            Vector3.Dot(axis, Vector3.Cross(from, to)),
            Vector3.Dot(from, to)
        ) * Mathf.Rad2Deg;
    }

    public static Rect RectTransformToWorldRect(RectTransform uiElement)
    {
        Vector3[] worldCorners = new Vector3[4];
        uiElement.GetWorldCorners(worldCorners);
        return new Rect
        (
            worldCorners[0].x,
            worldCorners[0].y,
            worldCorners[2].x - worldCorners[0].x,
            worldCorners[2].y - worldCorners[0].y
        );
    }
}










