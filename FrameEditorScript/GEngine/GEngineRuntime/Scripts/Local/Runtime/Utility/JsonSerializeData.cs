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
public class JsonVector3FInt
{
    public int[] d;
    public JsonVector3FInt()
    {
        d = new int[3];
    }
    public JsonVector3FInt(float x, float y, float z)
    {
        d = new int[3];
        d[0] = (int)(x *1000);
        d[1] = (int)(y*1000);
        d[2] = (int)(z*1000);
    }
    public JsonVector3FInt(UnityEngine.Vector3 enginEVector3)
    {
        d = new int[3];
        d[0] = (int)(enginEVector3.x *1000);
        d[1] = (int)(enginEVector3.y * 1000);
        d[2] = (int)(enginEVector3.z *1000);
    }
    public UnityEngine.Vector3 ToEngineVector3()
    {
        UnityEngine.Vector3 result = new UnityEngine.Vector3();
        result.x = (float)((float)d[0] / 1000.0f);
        result.y = (float)((float)d[1] / 1000.0f);
        result.z = (float)((float)d[2] / 1000.0f);

        return result;
    }
    public JsonVector3FInt(UnityEngine.Quaternion qua)
    {
        d = new int[3];
        UnityEngine.Vector3 enginEVector3 = qua.eulerAngles;
        d[0] = (int)(enginEVector3.x * 1000);
        d[1] = (int)(enginEVector3.y * 1000);
        d[2] = (int)(enginEVector3.z * 1000);
    }
    public UnityEngine.Quaternion ToEngineQuaternion()
    {
        UnityEngine.Quaternion rotation = new UnityEngine.Quaternion();
        rotation = UnityEngine.Quaternion.Euler((float)((float)d[0]/1000.0f), 
                                                (float)((float)d[1]/1000.0f), 
                                            (float)((float)d[2]/1000.0f));
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
