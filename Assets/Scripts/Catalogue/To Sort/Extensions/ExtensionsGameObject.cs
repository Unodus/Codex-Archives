
using UnityEngine;

public static class ExtensionsGameObject
{
    /// <summary>
    /// It zeros out the locals 
    /// </summary>
    /// <param name="lGameObject"></param>
    /// 

    public static void SetTransformLocals(this GameObject lGameObject, Vector3 lLocalPosition, Quaternion lRotation)
    {
        lGameObject.transform.SetTransformLocals(lLocalPosition, lRotation);
    }
    public static void SetTransformLocals(this Transform lTransform, Vector3 lLocalPosition, Quaternion lRotation)
    {
        lTransform.localPosition = lLocalPosition;
        lTransform.localRotation = lRotation;
        lTransform.localScale = Vector3.one;
    }

    public static void SetTransformLocals(this GameObject lGameObject)
    {
        lGameObject.transform.SetTransformLocals();
    }


    /// <summary>
    /// It zeros out the locals 
    /// </summary>
    /// <param name="lTransform"></param>
    public static void SetTransformLocals(this Transform lTransform)
    {
        lTransform.localPosition = Vector3.zero;
        lTransform.localRotation = Quaternion.identity;
        lTransform.localScale    = Vector3.one;
    }

    /// <summary>
    /// It zeros out the locals but takes in lLocalPosition
    /// </summary>
    /// <param name="lGameObject"></param>
    /// <param name="lLocalPosition"></param>
    public static void SetTransformLocals(this GameObject lGameObject, Vector3 lLocalPosition)
    {
        lGameObject.transform.SetTransformLocals(lLocalPosition);
    }


    /// <summary>
    /// It zeros out the locals but takes in lLocalPosition
    /// </summary>
    /// <param name="lTransform"></param>
    /// <param name="lPosition"></param>
    public static void SetTransformLocals(this Transform lTransform, Vector3 lLocalRotation)
    {
        lTransform.localPosition = lLocalRotation;
        lTransform.localRotation = Quaternion.identity;
        lTransform.localScale = Vector3.one;
    }


    /// <summary>
    /// It zeros out the locals but takes in lLocalPosition
    /// </summary>
    /// <param name="lGameObject"></param>
    /// <param name="lLocalRotation"></param>
    public static void SetTransformLocals(this GameObject lGameObject, Quaternion lLocalRotation)
    { 
        lGameObject.transform.SetTransformLocals(lLocalRotation);
    }


    /// <summary>
    /// It zeros out the locals but takes in lLocalPosition
    /// </summary>
    /// <param name="lTransform"></param>
    /// <param name="lLocalRotation"></param>
    public static void SetTransformLocals(this Transform lTransform, Quaternion lLocalRotation)
    {
        lTransform.localPosition = Vector3.zero;
        lTransform.localRotation = lLocalRotation;
        lTransform.localScale = Vector3.one;
    }

    /// <summary>
    /// quick way to set objects parent
    /// </summary>
    /// <param name="lThis"></param>
    /// <param name="lOther"></param>
    public static void SetParent(this GameObject lThis, GameObject lOther)
    {
        if (null == lOther)
        {
            lThis.transform.SetParent(null);
        }
        else
        {
            lThis.transform.SetParent(lOther.transform);
        }
    }
}
