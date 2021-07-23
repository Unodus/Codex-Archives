using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoundsTest : MonoBehaviour
{
    public float m_iSizeLength    = 50;
    public bool m_bMakeLengthSize = false;
    public bool m_bSetToZero      = false;

    public void ReDraw()
    {
        OnDrawGizmos();
    }
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Bounds lBounds = GetChildRendererBounds(gameObject);
        Gizmos.color   = Color.red;
        Gizmos.DrawWireCube(lBounds.center, lBounds.size);

        if (true == m_bMakeLengthSize)
        {
            Vector3 lLocalScale = this.gameObject.transform.localScale;
            float lCurrentSize  = Mathf.Abs(lBounds.max.z - lBounds.min.z);
            float lMultiply     = m_iSizeLength / lCurrentSize;
            lLocalScale         = lLocalScale* lMultiply;

            this.gameObject.transform.localScale = lLocalScale;
            m_bMakeLengthSize = false;
            Undo.RecordObject(this, "Set To Zero");
        }


        if(true == m_bSetToZero)
        {
            m_bSetToZero      = false;
            Vector3 lPosition = this.transform.position;
            lPosition.y      -= lBounds.min.y;
            this.transform.position = lPosition;
            Undo.RecordObject(this, "Set To Zero");
        }
#endif
    }

    public Bounds GetBounds()
    {
        return GetChildRendererBounds(gameObject);
    }


    /// <summary>
    /// 
    /// </summary>
    Bounds GetChildRendererBounds(GameObject go)
    {
        Renderer[] lRenderArray = go.GetComponentsInChildren<Renderer>();

        if (lRenderArray.Length > 0)
        {
            Bounds bounds = lRenderArray[0].bounds;
            for (int i = 1, ni = lRenderArray.Length; i < ni; i++)
            {
                bounds.Encapsulate(lRenderArray[i].bounds);
            }
            return bounds;
        }
        else
        {
            return new Bounds();
        }
    }

}