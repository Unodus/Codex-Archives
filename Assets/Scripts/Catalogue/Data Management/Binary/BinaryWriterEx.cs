using System.IO;
using UnityEngine;

/// <summary>
/// Binary writer ex.
///
/// Extends the BinaryWriter to include several other useful types
/// </summary>
public class BinaryWriterEx : BinaryWriter
{
    public BinaryWriterEx(Stream baseStream) : base(baseStream)
    {
    }

    /// <summary>
    /// Write the specified vector2.
    /// </summary>
    /// <param name='vec'>
    /// Vector2.
    /// </param>
    public void Write(Vector2 vec)
    {
        Write(vec.x);
        Write(vec.y);
    }

    /// <summary>
    /// Write the specified vector3.
    /// </summary>
    /// <param name='vec'>
    /// Vector3.
    /// </param>
    public void Write(Vector3 vec)
    {
        Write(vec.x);
        Write(vec.y);
        Write(vec.z);
    }

    /// <summary>
    /// Write the specified quaternion.
    /// </summary>
    /// <param name='quat'>
    /// Quaternion.
    /// </param>
    public void Write(Quaternion quat)
    {
        Write(quat.x);
        Write(quat.y);
        Write(quat.z);
        Write(quat.w);
    }

    public void Write(Color color)
    {
        Write(color.r);
        Write(color.g);
        Write(color.b);
        Write(color.a);
    }

    public void Write(int[] array)
    {
        Write(array.Length);
        for (int i = 0; i < array.Length; i++)
        {
            Write(array[i]);
        }
    }

    public void Write(float[] array)
    {
        Write(array.Length);
        for (int i = 0; i < array.Length; i++)
        {
            Write(array[i]);
        }
    }

    public void Write(int uid, PlaybackData data)
    {
        Write(uid);
        Write(data.m_Position);
        Write(data.m_Rotation);
        Write(data.m_sParentName);
        Write(data.m_bActiveInHierarchy);
        Write(data.m_bRendererActive);
        Write(data.m_fCurrentTime);
    }
}