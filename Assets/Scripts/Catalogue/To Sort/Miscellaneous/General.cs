/* General helper files which could not be put into another group
 * 
 * 
 * 
 * //TODO To review at end of project.
 */


using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine.SceneManagement;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Helpers
{

    public static class General
    {
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
                    if (false == desc.name.Contains("Chute", ExtensionsString.ECaseSensitive.NoCaseSensitive))
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
                switch(lType)
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
                Debug.LogError("Textures not found lSourceMaterials :" + Helpers.General.GetGameObjectPath(lSource) + "    MaterialName :" + sSourceMaterialName);
                bError = true;
            }

            if (-1 == iTargetIndex)
            {
                Debug.LogError("Textures not found in lTargetMaterials :" + Helpers.General.GetGameObjectPath(lTarget) + "    MaterialName :" + sTargetMaterialName);
                bError = true;
            }
            if(true == bError)
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
        public static void  CheckAnimationStringExistsInAnimator(Animator lAnimator, string lAnimationName)
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
                if(true == lItem.name.ContainsCaseInsensitive(sName) )
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


        /// <summary>
        /// This is NOT the number in build. it is the current number loaded 
        /// when running a single test scene it will be 1 , 
        /// // when running the full, with networking it will be over 4
        /// </summary>

        public static bool IsTestScene()
        {
#if !UNITY_EDITOR
            return false;
#else

            int lCount  = SceneManager.sceneCount;
            for (int i = 0; i < lCount; i++)
            {
                string lPath = SceneManager.GetSceneAt(i).path;
                if (true == lPath.Contains("Test") || true == lPath.Contains("AssetBundle"))
                {
                    return true;
                }
            }
            return false;
#endif
        }
    }
}