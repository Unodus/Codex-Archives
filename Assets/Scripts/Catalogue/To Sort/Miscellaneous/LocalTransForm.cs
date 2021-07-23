using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{

    public static class LocalTransform
    {
        public class TTransform // spelled this way so not get confused with transform  
        {
            public Vector3    m_Position;
            public Quaternion m_Rotation;
            public Vector3    m_Scale;
        }

        public static TTransform GetTransformLocal(GameObject lGameObject)
        {
            TTransform lMyTransform = new TTransform();
            lMyTransform.m_Position = lGameObject.transform.localPosition;
            lMyTransform.m_Rotation = lGameObject.transform.localRotation;
            lMyTransform.m_Scale    = lGameObject.transform.localScale;
            return lMyTransform;
        }

        public static void SetTransformLocal(GameObject lGameObject, TTransform lMyTransform)
        {
            lGameObject.transform.localPosition = lMyTransform.m_Position;
            lGameObject.transform.localRotation = lMyTransform.m_Rotation;
            lGameObject.transform.localScale    = lMyTransform.m_Scale;
        }

        public static void SetTransformLocalZeros(GameObject lGameObject)
        {
            lGameObject.transform.localPosition = Vector3.zero;
            lGameObject.transform.localRotation = Quaternion.identity;
            lGameObject.transform.localScale    = Vector3.one;
        }
    }

}
