using System.Collections;
using System.Collections.Generic;

public class JsonVector2Int
{
    public int[] d;
    public JsonVector2Int()
    {
        d = new int[2];
    }
    public JsonVector2Int(int x, int y)
    {
        d = new int[2];
        d[0] = x;
        d[1] = y;
    }
}
public class JsonVector2Float
{
    public float[] d;
    public JsonVector2Float()
    {
        d = new float[2];
    }
    public JsonVector2Float(float x, float y)
    {
        d = new float[2];
        d[0] = x;
        d[1] = y;
    }
}
public class JsonVector3Int
{
    public int[] d;
    public JsonVector3Int()
    {
        d = new int[3];
    }
    public JsonVector3Int(int x, int y,int z)
    {
        d = new int[3];
        d[0] = x;
        d[1] = y;
        d[2] = z;
    }
}
public class JsonVector3Float
{
    public float[] d;
    public JsonVector3Float()
    {
        d = new float[3];
    }
    public JsonVector3Float(float x, float y, float z)
    {
        d = new float[3];
        d[0] = x;
        d[1] = y;
        d[2] = z;
    }
    public JsonVector3Float(UnityEngine.Vector3 enginEVector3)
    {
        d = new float[3];
        d[0] = enginEVector3.x;
        d[1] = enginEVector3.y;
        d[2] = enginEVector3.z;
    }
    public UnityEngine.Vector3 ToEngineVector3()
    {
        UnityEngine.Vector3 result = new UnityEngine.Vector3();
        result.x = d[0];
        result.y = d[1];
        result.z = d[2];

        return result;
    }
    public JsonVector3Float(UnityEngine.Quaternion qua)
    {
        d = new float[3];
        UnityEngine.Vector3 vector3 = qua.eulerAngles;
        d[0] = vector3.x;
        d[1] = vector3.y;
        d[2] = vector3.z;
    }
    public UnityEngine.Quaternion ToEngineQuaternion()
    {
        UnityEngine.Quaternion rotation = new UnityEngine.Quaternion();
        rotation = UnityEngine.Quaternion.Euler(d[0], d[1], d[2]);
        return rotation;
    }
}
public class JsonVector4Int
{
    public int[] d;
    public JsonVector4Int()
    {
        d = new int[4];
    }
    public JsonVector4Int(int x, int y, int z,int w)
    {
        d = new int[4];
        d[0] = x;
        d[1] = y;
        d[2] = z;
        d[3] = w;
    }
}
public class JsonVectorFloat
{
    public float[] d;
    public JsonVectorFloat()
    {
        d = new float[4];
    }
    public JsonVectorFloat(float x, float y, float z, float w)
    {
        d = new float[4];
        d[0] = x;
        d[1] = y;
        d[2] = z;
        d[3] = w;
    }
}
