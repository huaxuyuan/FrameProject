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
