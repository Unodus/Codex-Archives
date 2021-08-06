using System.IO;
using UnityEngine;

public class BinaryReaderEx : BinaryReader
{
    public BinaryReaderEx(Stream baseStream) : base(baseStream)
    {
    }

    public Vector2 ReadVector2()
    {
        Vector2 data = Vector2.zero;
        data.x = ReadSingle();
        data.y = ReadSingle();
        return data;
    }

    public Vector3 ReadVector3()
    {
        Vector3 data = Vector3.zero;
        data.x = ReadSingle();
        data.y = ReadSingle();
        data.z = ReadSingle();
        return data;
    }

    public int[] ReadIntArray()
    {
        int[] data = new int[ReadInt32()];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = ReadInt32();
        }
        return data;
    }

    public float[] ReadFloatArray()
    {
        float[] data = new float[ReadInt32()];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = ReadSingle();
        }
        return data;
    }

    public Quaternion ReadQuaternion()
    {
        Quaternion data = Quaternion.identity;
        data.x = ReadSingle();
        data.y = ReadSingle();
        data.z = ReadSingle();
        data.w = ReadSingle();
        return data;
    }

    public Color ReadColor()
    {
        Color colour;
        colour.r = ReadSingle();
        colour.g = ReadSingle();
        colour.b = ReadSingle();
        colour.a = ReadSingle();
        return colour;
    }


    public PlaybackData ReadPlaybackData()
    {
        PlaybackData data = new PlaybackData
        {
            m_Position = ReadVector3(),
            m_Rotation = ReadQuaternion(),
            m_sParentName = ReadString(),
            m_bActiveInHierarchy = ReadBoolean(),
            m_bRendererActive = ReadBoolean(),
            m_fCurrentTime = ReadSingle()
        };

        return data;
    }



}