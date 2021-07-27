using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions 
{


    /// <summary>
    /// It zeros out the locals 
    /// </summary>
    /// <param name="lTransform"></param>
    /// 





    public static void ResetLocalTransform(this Transform lTransform)
    {
        lTransform.localPosition = Vector3.zero;
        lTransform.localScale = Vector3.one;
        lTransform.localRotation = Quaternion.identity;
    }

    public static void ResetTransform(this Transform lTransform)
    {
        lTransform.position = Vector3.zero;
        lTransform.rotation= Quaternion.identity;
        lTransform.localScale = Vector3.one;
    }

    public static void SetTransform(this Transform lTransform, Vector3 lLocalPosition)
    {
        lTransform.localPosition = lLocalPosition;
        lTransform.localRotation = Quaternion.identity;
    }

    public static void SetTransformLocal(this Transform lGameObject, Transform lMyTransform)
    {
        lGameObject.localPosition = lMyTransform.position;
        lGameObject.localRotation = lMyTransform.rotation;
        lGameObject.localScale = lMyTransform.localScale;
    }

    public static void SetTransformLocals(this Transform lTransform, Quaternion lLocalRotation)
    {
        lTransform.localPosition = Vector3.zero;
        lTransform.localRotation = lLocalRotation;
    }

    public static void SetTransformLocals(this Transform lTransform, Vector3 lLocalPosition, Quaternion lRotation)
    {
        lTransform.localPosition = lLocalPosition;
        lTransform.localRotation = lRotation;
    }



    //Breadth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            //if you search for "" it will return the object your searching in, so check that string is empty and return nothing
            if (true == string.IsNullOrEmpty(aName))
            {
                return null; //this is intended
            }

            var result = aParent.Find(aName);
            if (result != null)
                return result;
            foreach (Transform child in aParent)
            {
                result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }
            return null;
        }

      
}
