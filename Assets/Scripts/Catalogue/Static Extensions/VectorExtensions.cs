using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions 
{
    // axisDirection - unit vector in direction of an axis (eg, defines a line that passes through zero)
    // point - the point to find nearest on line for
    public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
    {
        if (!isNormalized) axisDirection.Normalize();
        var d = Vector3.Dot(point, axisDirection);
        return axisDirection * d;
    }

    // lineDirection - unit vector in direction of line
    // pointOnLine - a point on the line (allowing us to define an actual line in space)
    // point - the point to find nearest on line for
    public static Vector3 NearestPointOnLine(
        this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
    {
        if (!isNormalized) lineDirection.Normalize();
        var d = Vector3.Dot(point - pointOnLine, lineDirection);
        return pointOnLine + (lineDirection * d);
    }

    // Get the directions from the start point to the end point
    public static Vector2 GetDirection(this Vector2 startPoint, Vector2 endPoint)
    {
        return (endPoint - startPoint).normalized;
    }

    // Use this method to know the progress of an element between two points taking into account the direction of the movement
    // Returns 0 at the start position, 1 at the end position, and negative numbers if the movements goes in the opposite direction
    public static float PathProgress(Vector2 startPosition, Vector2 endPosition, Vector2 currentPosition)
    {
        Vector2 totalDisplacement = endPosition - startPosition;
        Vector2 currentDisplacement = currentPosition - startPosition;

        return Vector2.Dot(currentDisplacement, totalDisplacement) / totalDisplacement.sqrMagnitude;
    }

    public static Vector2 RandomPointOnCircumference(Vector2 centerPoint, float radius = 1.0f, float fromAngle = 0.0f, float toAngle = 360.0f)
    {
        float angle = UnityEngine.Random.Range(fromAngle, toAngle) * Mathf.Deg2Rad;
        Vector2 point = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

        return point;
    }

    public static Vector2 Rotate(this Vector2 vector, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = vector.x;
        float ty = vector.y;
        vector.x = (cos * tx) - (sin * ty);
        vector.y = (sin * tx) + (cos * ty);
        return vector;
    }

    public static Vector3Int ConvertToVector3(this Vector3 vec3)
    {
        return new Vector3Int((int)vec3.x, (int)vec3.y, (int)vec3.z);
    }

    /// <summary>
    /// Change the x value of vector 3
    /// </summary>
    public static Vector3 ChangeX(this Vector3 v, float fValX)
    {
        return new Vector3(fValX, v.y, v.z);
    }

    /// <summary>
    /// Change the y value of vector 3
    /// </summary>
    public static Vector3 ChangeY(this Vector3 v, float fValY)
    {
        return new Vector3(v.x, fValY, v.z);
    }

    /// <summary>
    /// Change the z value of vector 3
    /// </summary>
    public static Vector3 ChangeZ(this Vector3 v, float fValZ)
    {
        return new Vector3(v.x, v.y, fValZ);
    }

}
