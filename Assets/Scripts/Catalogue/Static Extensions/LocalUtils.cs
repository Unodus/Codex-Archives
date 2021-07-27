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
public static class LocalUtils 
{
    #region GUI Utils
    /// <summary>
    /// Converts Screens space to GUI Space.
    /// </summary>
    public static Vector2 ScreenSpaceToGUIPoint(Vector3 screenSpace)
    {
        screenSpace.y = Screen.height - screenSpace.y;

        return GUIUtility.ScreenToGUIPoint(new Vector2(screenSpace.x, screenSpace.y));
    }

    /// <summary>
    /// Returns true if the target is infront of the camera... also returns the 2D screen coords of the object for GUISpace
    /// </summary>
    public static bool WorldPointToGUIPoint(Camera camera, Vector3 targetPos, out Vector2 guiSpace)
    {
        Vector3 forward = camera.transform.TransformDirection(Vector3.forward);
        Vector3 toOther = targetPos - camera.transform.position;

        if (Vector3.Dot(forward, toOther) > 0)
        {
            Vector3 screenSpace = camera.WorldToScreenPoint(targetPos);

            screenSpace.y = Screen.height - screenSpace.y;

            guiSpace = GUIUtility.ScreenToGUIPoint(new Vector2(screenSpace.x, screenSpace.y));
            return true;
        }

        guiSpace = Vector2.zero;
        return false;
    }

    public static void DrawTexture(Rect positionRect, Texture2D textureToDraw, ScaleMode scaleMode = ScaleMode.ScaleToFit, bool bAlphaBlend = false)
    {
        if (null != textureToDraw)
        {
            GUI.DrawTexture(positionRect, textureToDraw, scaleMode, bAlphaBlend);
        }
    }
    #endregion

    #region Creation & Manipulation of Objects
    /// <summary>
    /// Instantiates the prefab.
    /// </summary>
    public static bool InstantiatePrefab(ref GameObject gameObject, string sFilename, string sName = "", Transform Parent = null, Vector3 Position = default(Vector3), Quaternion Rotation = default(Quaternion), bool bOffset = false)
    {
        // instantiate the prefab
        gameObject = GameObject.Instantiate(Resources.Load(sFilename)) as GameObject;

        if (null == gameObject)
        {
            return false;
        }

        if (true == string.IsNullOrEmpty(sName))
        {
            gameObject.name = gameObject.name.Replace("(Clone)", "");
        }
        else
        {
            gameObject.name = sName;
        }
        if (bOffset == true)
        {
            gameObject.transform.parent = Parent;
            gameObject.transform.localPosition = Position;
            gameObject.transform.localRotation = Rotation;
        }
        else
        {
            gameObject.transform.position = Position;
            gameObject.transform.rotation = Rotation;
            gameObject.transform.parent = Parent;
        }
        return true;
    }


    /// <summary>
    /// Loads the resource of Type T
    /// </summary>
    /// 

    public static bool LoadResource<T>(ref T item, string sFile) where T : UnityEngine.Object
    {
        item = Resources.Load<T>(sFile) as T;

        if (null == item)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Creates the game object with attributes.
    /// </summary>
    public static GameObject CreateGameObjectWithAttributes(string sName, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform parent = null)
    {
        GameObject item = new GameObject(sName);
        item.transform.position = position;
        item.transform.rotation = rotation;

        if (parent != null)
        {
            item.transform.parent = parent;
        }
        return item;
    }

    /// <summary>
    /// Find game object.
    /// </summary>
    public static bool FindGameObject(ref GameObject obj, string nameToFind)
    {
        obj = GameObject.Find(nameToFind);

        //Is GameObject Null
        if (null == obj)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Find child object in gameobject.
    /// </summary>
    public static bool FindGameObjectInChild(ref GameObject obj, GameObject parent, string nameToFind)
    {
        //Find transform
        Transform t = parent.transform.Find(nameToFind);

        //Successfully found?
        if (null == t)
        {
            return false;
        }
        else
        {
            //Get GameObject
            obj = t.gameObject;
            return true;
        }
    }

    public static bool FindChildObjectInObject(ref GameObject obj, GameObject parent, string nameToFind)
    {
        Transform[] transforms = parent.GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            if (t.name == nameToFind)
            {
                obj = t.gameObject;
                return true;
            }
        }
        return false;
    }

    public static bool FindChildObjectInObject(ref Transform obj, GameObject parent, string nameToFind)
    {
        Transform[] transforms = parent.GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            if (t.name == nameToFind)
            {
                obj = t;
                return true;
            }
        }
        return false;
    }

    public static bool FindChildObjectInObject(ref GameObject obj, Transform parent, string nameToFind)
    {
        Transform[] transforms = parent.GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            if (t.name == nameToFind)
            {
                obj = t.gameObject;
                return true;
            }
        }
        return false;
    }

    public static bool FindChildObjectInObject(ref Transform obj, Transform parent, string nameToFind)
    {
        Transform[] transforms = parent.GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            if (t.name == nameToFind)
            {
                obj = t;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// See's if Gamobject has a component.
    /// </summary>
    public static bool ReturnComponent<T>(ref T com, GameObject go) where T : Component
    {
        com = go.GetComponent<T>();

        //Is Component Null?
        if (null == com)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Finds the component within object. Doesn't work yet
    /// </summary>
    public static UnityEngine.Object[] FindComponentWithinObject(GameObject obj, Type t)
    {
        UnityEngine.Object[] newObj = obj.GetComponentsInChildren(t);
        return newObj;
    }

    /// <summary>
    /// Finds all gameobjects with tag and alphabatise them.
    /// </summary>
    public static GameObject[] FindObsWithTag(string tag)
    {
        GameObject[] foundObs = GameObject.FindGameObjectsWithTag(tag);
        Array.Sort(foundObs, CompareGameObjectNames);
        return foundObs;
    }

    /// <summary>
    /// Array Sort param for objects
    /// </summary>
    public static int CompareObjectNames(UnityEngine.Object x, UnityEngine.Object y)
    {
        return x.name.CompareTo(y.name);
    }

    /// <summary>
    /// Array Sort param for gameobject
    /// </summary>
    public static int CompareGameObjectNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }

    /// <summary>
    /// Array Sort param for transforms
    /// </summary>
    public static int CompareTransform(Transform A, Transform B)
    {
        return A.name.CompareTo(B.name);
    }

    /// <summary>
    /// Finds all gameobjects within parent.
    /// </summary>
    public static GameObject[] FindObsWithinParent(GameObject go)
    {
        int count = go.transform.childCount;
        GameObject[] obj = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            obj[i] = go.transform.GetChild(i).gameObject;
        }
        Array.Sort(obj, CompareGameObjectNames);
        return obj;
    }
    #endregion

    #region General Stuff


#if UNITY_EDITOR
    [MenuItem("Helpers/Add Box Collider")]
#endif
    public static void AddBoxColliderToAllWithChildren(GameObject lGameObject)
    {
        BoxCollider lBoxCollider = lGameObject.AddComponent<BoxCollider>();


        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        Renderer thisRenderer = lGameObject.transform.GetComponent<Renderer>();
        if (null != thisRenderer)
        {
            bounds.Encapsulate(thisRenderer.bounds);
        }
        lBoxCollider.center = bounds.center - lGameObject.transform.position;
        lBoxCollider.size = bounds.size;

        Transform[] allDescendants = lGameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform desc in allDescendants)
        {
            Renderer childRenderer = desc.GetComponent<Renderer>();
            if (null != childRenderer)
            {
                if (false == desc.name.Contains("Chute", false))
                {
                    bounds.Encapsulate(childRenderer.bounds);
                }
            }
        }
        lBoxCollider.center = lGameObject.transform.position - bounds.center;
        lBoxCollider.size = bounds.size;
    }

    public static int GetMaterialIndexForString(GameObject lGameObject, string sName, CopyMaterialType lType)
    {
        if (null == lGameObject)
        {
            Debug.LogError("GetMaterialIndexForString gameobject passed in is null.");
            return -1;
        }

        if (true == string.IsNullOrEmpty(sName))
        {
            Debug.LogError("GetMaterialIndexForString null or empty name has been passed in.");
            return -1;
        }

        Renderer lRenderer = lGameObject.GetComponent<Renderer>();
        return GetMaterialIndexForString(lRenderer.materials, sName, lType);
    }


    public static int GetMaterialIndexForString(Material[] lMaterials, string sName, CopyMaterialType lType)
    {
        for (int i = 0; i < lMaterials.Length; i++)
        {
            switch (lType)
            {
                case CopyMaterialType.Contains:
                    if (true == lMaterials[i].name.Contains(sName))
                    {
                        return i;
                    }
                    break;

                case CopyMaterialType.FullName:
                    if (lMaterials[i].name == sName)
                    {
                        return i;
                    }
                    break;
                case CopyMaterialType.EndsWith:
                    if (true == lMaterials[i].name.EndsWith(sName))
                    {
                        return i;
                    }
                    break;

            }
        }
        return -1;
    }

    public enum CopyMaterialType
    {
        FullName,
        Contains,
        EndsWith
    };



    public static void CopyMaterial(GameObject lSource, string sSourceMaterialName, CopyMaterialType lSourceType, GameObject lTarget, string sTargetMaterialName, CopyMaterialType lTargetType)
    {
        Material[] lSourceMaterials = lSource.GetComponent<Renderer>().materials;
        Material[] lTargetMaterials = lTarget.GetComponent<Renderer>().materials;

        int iSourceIndex = GetMaterialIndexForString(lSourceMaterials, sSourceMaterialName, lSourceType);
        int iTargetIndex = GetMaterialIndexForString(lTargetMaterials, sTargetMaterialName, lTargetType);

        bool bError = false;
        if (-1 == iSourceIndex)
        {
            Debug.LogError("Textures not found lSourceMaterials :" + GetGameObjectPath(lSource) + "    MaterialName :" + sSourceMaterialName);
            bError = true;
        }

        if (-1 == iTargetIndex)
        {
            Debug.LogError("Textures not found in lTargetMaterials :" + GetGameObjectPath(lTarget) + "    MaterialName :" + sTargetMaterialName);
            bError = true;
        }
        if (true == bError)
        {
            return;
        }

        lTargetMaterials[iTargetIndex] = lSourceMaterials[iSourceIndex];
        lTarget.GetComponent<Renderer>().materials = lTargetMaterials;
    }


    public static void CopyMaterialIndex(GameObject lSource, int iSourceIndex, GameObject lTarget, int iTargetIndex)
    {
        Material[] lSourceMaterials = lSource.GetComponent<Renderer>().materials;
        Material[] lTargetMaterials = lTarget.GetComponent<Renderer>().materials;

        lTargetMaterials[iTargetIndex] = lSourceMaterials[iSourceIndex];
        lTarget.GetComponent<Renderer>().materials = lTargetMaterials;
    }

#if UNITY_EDITOR
    private static List<KeyValuePair<Animator, string>> m_CheckAnimationList = new List<KeyValuePair<Animator, string>>();
#endif
    public static void CheckAnimationStringExistsInAnimator(Animator lAnimator, string lAnimationName)
    {
#if UNITY_EDITOR
        KeyValuePair<Animator, string> lNew = new KeyValuePair<Animator, string>(lAnimator, lAnimationName);
        if (false == m_CheckAnimationList.Contains(lNew))
        {

            AnimationClip[] animationClips = lAnimator.runtimeAnimatorController.animationClips;
            bool lFound = false;
            foreach (AnimationClip animClip in animationClips)
            {
                if (lAnimationName == animClip.name)
                {
                    lFound = true;
                    break;
                }
            }
            if (false == lFound)
            {
                Debug.LogError("The animation name :" + lAnimationName + " cannot be found in the Animator:" + lAnimator);
            }
            m_CheckAnimationList.Add(lNew);
        }
#endif
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


    public static bool DictionaryComparer<TKey, TValue>(Dictionary<TKey, TValue> x, Dictionary<TKey, TValue> y)
    {
        if (x.Count != y.Count)
            return false;

        foreach (var lX in x)
        {
            bool lFoundAndSame = false;
            foreach (var lY in y)
            {
                if (lX.Key.Equals(lY.Key))
                {
                    if (lX.Value.Equals(lY.Value))
                    {
                        lFoundAndSame = true;
                        break;
                    }
                }
            }
            if (lFoundAndSame == false)
            {
                return false;
            }
        }

        return true;

    }

    public static string GetGameObjectPath(GameObject lGameObject)
    {
        return GetGameObjectPath(lGameObject.transform);
    }


    public static string GetGameObjectPath(Transform lTransform)
    {
        string path = lTransform.name;
        while (lTransform.parent != null)
        {
            lTransform = lTransform.parent;
            path = lTransform.name + "/" + path;
        }
        return path;
    }


    public static List<int> GetIntsFromString(string lString)
    {
        List<int> lList = new List<int>();
        string[] numbers = Regex.Split(lString, @"\D+");
        foreach (string value in numbers)
        {
            if (!string.IsNullOrEmpty(value))
            {
                int i = int.Parse(value);
                lList.Add(i);
            }
        }
        return lList;
    }


    public static int GetIntFromString(string lString)
    {
        List<int> lList = GetIntsFromString(lString);
        if (lList.Count > 0)
        {
            return lList[0];
        }
        return -1;
    }

    public static GameObject AddPivotParentToGameObject(GameObject GameObjectRef)
    {
        GameObject oldParent = GameObjectRef.transform.parent.gameObject;
        GameObject PivotGameObject = new GameObject("PivotCreatedViaCode");
        PivotGameObject.SetParent(GameObjectRef);
        PivotGameObject.SetTransformLocals();
        PivotGameObject.SetParent(null);
        PivotGameObject.SetParent(oldParent);
        GameObjectRef.SetParent(PivotGameObject);
        return PivotGameObject;
    }

    public static T FindGameObjectParent<T>(GameObject lGameObject)
    {
        if (lGameObject.transform.parent != null)
        {
            if (lGameObject.transform.parent.GetComponent<T>() != null)
            {
                return lGameObject.transform.parent.GetComponent<T>();
            }
            else
            {
                return FindGameObjectParent<T>(lGameObject.transform.parent.gameObject);
            }
        }
        return default(T);
    }


    public static GameObject FindChildGameObjectWithName(GameObject lGameObject, string sName)
    {
        Transform[] lTransformList = lGameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform lItem in lTransformList)
        {
            if (true == lItem.name.ContainsCaseInsensitive(sName))
            {
                return lItem.gameObject;
            }
        }
        return null;
    }

    public static GameObject FindChildGameObjectWithNameEndsWith(GameObject lGameObject, string sName)
    {
        Transform[] lTransformList = lGameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform lItem in lTransformList)
        {
            if (true == lItem.name.EndsWith(sName))
            {
                return lItem.gameObject;
            }
        }
        return null;
    }

    public static GameObject FindChildGameObjectFullPathnameWithNameEndsWith(GameObject lGameObject, string sName)
    {
        Transform[] lTransformList = lGameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform lItem in lTransformList)
        {

            if (true == GetGameObjectPath(lItem).EndsWith(sName))
            {
                return lItem.gameObject;
            }
        }
        return null;
    }

    public static GameObject FindChildGameObjectContains(GameObject lGameObject, string sName)
    {
        Transform[] lTransformList = lGameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform lItem in lTransformList)
        {
            if (true == lItem.name.ContainsCaseInsensitive(sName))
            {
                return lItem.gameObject;
            }
        }
        return null;
    }

    public static List<GameObject> FindChildGameObjectsContains(GameObject lGameObject, string sName)
    {
        List<GameObject> lList = new List<GameObject>();
        Transform[] lTransformList = lGameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform lItem in lTransformList)
        {
            if (true == lItem.name.ContainsCaseInsensitive(sName))
            {
                lList.Add(lItem.gameObject);
            }
        }
        return lList;
    }


    public static void Swap<T>(ref T l1, ref T l2)
    {
        T lTemp = l1;
        l1 = l2;
        l2 = lTemp;
    }


    public static bool FindGameObjectsFromRootWithName(GameObject lGameObject, ref GameObject lFoundObject, string lName)
    {
        lGameObject = lGameObject.transform.root.gameObject;
        Transform[] lTransformList = lGameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform lItem in lTransformList)
        {
            if (lItem.gameObject.name.Equals(lName) == true)
            {
                lFoundObject = lItem.gameObject;
                return true;
            }
        }

        return false;
    }

    #endregion

    #region 

    #region Colour
    public class Colour256
    {
        public static float Colour256ToFloat(int lAmout)
        {
            return (float)lAmout / (float)256;
        }

        public static Color Colour256ToColour(int lRed, int lGreen, int lBlue)
        {
            return new Color(Colour256ToFloat(lRed), Colour256ToFloat(lGreen), Colour256ToFloat(lBlue));
        }
    }
    #endregion


    /// <summary>
    /// Gets the random colour.
    /// </summary>
    public static Color GetRandomColour()
    {
        
        return new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
    }

    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }


    /// <summary>
    /// Sets the color from float. Has to be 0-225 and include .0f
    /// </summary>
    public static Color Colour(float fRed, float fGreen, float fBlue, float fAlpha)
    {
        fRed = Mathf.Clamp(fRed, 0.0f, 255.0f);
        fGreen = Mathf.Clamp(fGreen, 0.0f, 255.0f);
        fBlue = Mathf.Clamp(fBlue, 0.0f, 255.0f);
        fAlpha = Mathf.Clamp(fAlpha, 0.0f, 255.0f);
        return new Color(fRed / 255.0f, fGreen / 255.0f, fBlue / 255.0f, fAlpha / 255.0f);
    }

    public static Color Colour(Vector4 vector)
    {
        vector.x = Mathf.Clamp(vector.x, 0.0f, 255.0f);
        vector.y = Mathf.Clamp(vector.y, 0.0f, 255.0f);
        vector.z = Mathf.Clamp(vector.z, 0.0f, 255.0f);
        vector.w = Mathf.Clamp(vector.w, 0.0f, 255.0f);
        return new Color(vector.x, vector.y, vector.z, vector.w);
    }

    public static  Color ToVector4(Color color)
    {
        return new Vector4(color.r, color.g, color.b, color.a);
    }
    #endregion

    #region Camera
    /// <summary>
    /// Add a culling mask to the camera
    /// </summary>
    public static void AddCullLayer(GameObject camera, string sNewLayerName)
    {
        AddCullLayer(camera, LayerMask.NameToLayer(sNewLayerName));
    }

    /// <summary>
    /// Add a culling mask to the camera
    /// </summary>
    public static void AddCullLayer(GameObject camera, int iNewLayer)
    {
        if (camera != null && camera.GetComponent<Camera>() != null)
        {
            camera.GetComponent<Camera>().cullingMask |= (1 << iNewLayer);
        }
    }

    /// <summary>
    /// Remove a culling mask to the camera
    /// </summary>
    public static void RemoveCullLayer(GameObject camera, string sNewLayerName)
    {
        RemoveCullLayer(camera, LayerMask.NameToLayer(sNewLayerName));
    }

    /// <summary>
    /// Remove a culling mask to the camera
    /// </summary>
    public static void RemoveCullLayer(GameObject camera, int iNewLayer)
    {
        if (camera != null && camera.GetComponent<Camera>() != null)
        {
            camera.GetComponent<Camera>().cullingMask &= ~(1 << iNewLayer);
        }
    }

    #endregion

    #region Lists
    public static void TrimList<T>(ref List<T> list)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i] == null)
            {
                list.RemoveAt(i);
                i--;
            }
        }
        list.TrimExcess();
    }

    public static void TrimList(ref List<GameObject> list)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i] == null || list[i].gameObject == null)
            {
                list.RemoveAt(i);
                i--;
            }
        }
        list.TrimExcess();
    }
    #endregion

    #region Meshes
    public static void CombineMesh(GameObject item)
    {
        MeshFilter[] meshFilters = item.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        item.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        item.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        item.transform.gameObject.SetActive(true);
    }
    #endregion

    /// <summary>
    /// Shutdown the application, absloutely.
    /// </summary>
    public static void ShutdownApplication()
    {
        if (true == Application.isEditor)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
    public static void DrawRay(Vector3 origin, Vector3 direction, Color col, float fLenght)
    {
        if (true == Application.isEditor)
        {
            Debug.DrawRay(origin, direction, col, fLenght, false);
        }


    }

    public static bool IsNull<T>(T t, UnityEngine.Object obj = null)
    {
        if (null != t)
        {
            return true;
        }
        else
        {
            Debug.LogError(t.ToString() + " is null", obj);
            return false;
        }
    }


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










