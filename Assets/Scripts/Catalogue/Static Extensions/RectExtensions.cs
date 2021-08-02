using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectExtensions 
{
    public static bool MouseInsideRectTransform(this RectTransform rectTransform, Camera cam)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, cam.ScreenToWorldPoint(Input.mousePosition));
    }

    public static bool MouseInsideRectTransform(this RectTransform rectTransform )
    {
        return rectTransform.MouseInsideRectTransform(Camera.main);
    }

    public static bool Intersects(this Rect source, Rect rect)
    {
        return !((source.x > rect.xMax) || (source.xMax < rect.x) || (source.y > rect.yMax) || (source.yMax < rect.y));
    }
}
