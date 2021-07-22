using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtils : MonoBehaviour
{
    public bool IsDivisble(int x, int n)
    {
        return (x % n) == 0;
    }

    /// <summary>
    /// Rotate beyond 360/-360 and have it set within 0-360
    /// /// This functions only does angle bounds management, not actual rotation. 
    /// </summary>
    public float WrapAngle(float fDegrees)
    {
        float fOrientation = fDegrees % 360f;
        if (fOrientation < 0f)
        {
            fOrientation += 360f;
        }

        return fOrientation;
    }

    /// <summary>
    /// Rotate beyond 360/-360 and have it set within 0-360
    /// This functions only does angle bounds management, not actual rotation. (int version)
    /// </summary>
    public int WrapAngle(int iDegrees)
    {
        int iOrientation = iDegrees % 360;
        if (iOrientation < 0)
        {
            iOrientation += 360;
        }

        return iOrientation;
    }


    /// <summary>
    /// convert a degree value to a vector
    /// </summary>
    public Vector3 RotateY(Vector3 upVectorZAxis, float fAngle)
    {
        float fSin = Mathf.Sin(fAngle);
        float fCos = Mathf.Cos(fAngle);

        float fTX = upVectorZAxis.x;
        float fTZ = upVectorZAxis.z;

        upVectorZAxis.x = (fCos * fTX) + (fSin * fTZ);
        upVectorZAxis.z = (fCos * fTZ) - (fSin * fTX);

        return upVectorZAxis;
    }

    /// <summary>
    /// Sqr distance line segment to point.
    /// </summary>
    public float SqrDistanceLineSegmentToPoint(Vector3 segmentA, Vector3 segmentB, Vector3 pointC)
    {
        float fLength = (segmentB - segmentA).sqrMagnitude;
        if (fLength == 0)
        {
            return (pointC - segmentA).sqrMagnitude;
        }

        float fDistAlongLine = Vector3.Dot(pointC - segmentA, segmentB - segmentA) / fLength;

        if (fDistAlongLine < 0)
        {
            return (pointC - segmentA).sqrMagnitude;
        }
        else if (fDistAlongLine > 1)
        {
            return (pointC - segmentB).sqrMagnitude;
        }

        Vector3 projection = segmentA + fDistAlongLine * (segmentB - segmentA);
        return (pointC - projection).sqrMagnitude;
    }

    /// <summary>
    /// Determines if two values are approximately equal
    /// </summary>
    public bool AlmostEqual(float fFirstValue, float fSecondValue, float fBias)
    {
        if (Mathf.Abs(fFirstValue - fSecondValue) < fBias)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Determines if two values are approximately equal for Vectors
    /// </summary>
    public bool AlmostEqual(Vector4 firstValue, Vector4 secondValue, float fBias)
    {
        if (false == AlmostEqual(firstValue.x, secondValue.x, fBias))
        {
            return false;
        }

        if (false == AlmostEqual(firstValue.y, secondValue.y, fBias))
        {
            return false;
        }

        if (false == AlmostEqual(firstValue.z, secondValue.z, fBias))
        {
            return false;
        }

        if (false == AlmostEqual(firstValue.w, secondValue.w, fBias))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether this vector is between the specified min & max.
    /// </summary>
    public bool IsBetweenVector(Vector3 value, Vector3 min, Vector3 max)
    {
        return (value.x > min.x) && (value.x < max.x)
            && (value.y > min.y) && (value.y < max.y)
            && (value.z > min.z) && (value.z < max.z);
    }

    /// <summary>
    /// Determines whether i is odd.
    /// </summary>
    public bool IsOdd(int iInteger)
    {
        return (iInteger % 2 == 1);
    }

    /// <summary>
    /// Gets the vector3 from direction.
    /// </summary>
    public Vector3 GetVector3FromDirection(float fAngle)
    {
        fAngle *= Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(fAngle), 0.0f, Mathf.Cos(fAngle)).normalized;
    }

    /// <summary>
    /// Returns a vector 3 with a 2D direction from angle 
    /// </summary>
    public Vector3 Create2DVectorXZ(float fAngle)
    {
        fAngle -= 180.0f;
        fAngle *= Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(fAngle), 0.0f, Mathf.Cos(fAngle)).normalized;
    }

    /// <summary>
    /// Returns the angle between 2 vectors -180 - 180
    /// </summary>
    public float Angle180(Vector3 dir1, Vector3 dir2)
    {
        float fAngle = Vector3.Angle(dir1, dir2);
        if (Vector3.Cross(dir1, dir2).y >= 0.0f)
        {
            fAngle = -fAngle;
        }
        return fAngle;
    }

    /// <summary>
    // Returns the angle between 2 vectors 0 - 360
    /// </summary>
    public float Angle360(Vector3 dir1, Vector3 dir2)
    {
        float fAngle = Vector3.Angle(dir1, -dir2);
        if (Vector3.Cross(dir1, dir2).x <= 0.0f)
        {
            fAngle = -fAngle;
        }
        return fAngle + 180.0f;
    }
}
