using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions 
{
    
    // Extension that rotates a transform using a 2D direction (on the Z axis), use the correction value depending on your sprite orientation (0 is facing right)
    public static Transform LookAtDirection2D(this Transform transform, Vector2 dir, float correction = 0)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + correction);
        return transform;
    }






    /// It zeros out the locals 
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
