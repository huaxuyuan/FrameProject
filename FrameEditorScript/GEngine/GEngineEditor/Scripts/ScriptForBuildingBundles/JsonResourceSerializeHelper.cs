using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class ResourceMD5Value
{
    public string p;
    public string v;
    public List<string> resourceStr;
}

public class JsonResourceSerializeHelper
{
    public static List<ResourceMD5Value> resourceMD5ValueList = new List<ResourceMD5Value>();
    public static Dictionary<string, string> packageToMD5Dic = new Dictionary<string, string>();
    public static Dictionary<string, List<string>> packageToResouceList = new Dictionary<string, List<string>>();

    public static void DeserializeResourceMD5()
    {
        resourceMD5ValueList = new List<ResourceMD5Value>();
        packageToResouceList = new Dictionary<string, List<string>>();
        string jsonPathName = GetJsonFilePath();
        if (!File.Exists(jsonPathName))
        {
            return;
        }
        string str = File.ReadAllText(jsonPathName);
        resourceMD5ValueList = JsonMapper.ToObject<List<ResourceMD5Value>>(str);
        foreach (ResourceMD5Value resourceValue in resourceMD5ValueList)
        {
            //Debug.Log("resourceValue.p " + resourceValue.p + " resourceValue.v " + resourceValue.v);
            AddPackageAndMD5(resourceValue.p, resourceValue.v, resourceValue.resourceStr);
        }
        //JsonReader
    }

    public static void AddPackageAndMD5(string packageName, string md5Value, List<string> resourceList)
    {
        if (!packageToMD5Dic.ContainsKey(packageName))
        {
            packageToResouceList.Add(packageName, resourceList);
            packageToMD5Dic.Add(packageName, md5Value);
        }
        else
        {
            packageToMD5Dic[packageName] = md5Value;
            if (packageToResouceList.ContainsKey(packageName))
                packageToResouceList[packageName] = resourceList;
            else
                packageToResouceList.Add(packageName, resourceList);
        }
    }

    public static void SeriablizeResourceMD5()
    {
        resourceMD5ValueList.Clear();
        foreach (KeyValuePair<string, string> pairValue in packageToMD5Dic)
        {
            ResourceMD5Value resourceValue = new ResourceMD5Value();
            resourceValue.p = pairValue.Key;
            resourceValue.v = pairValue.Value;
            resourceValue.resourceStr = packageToResouceList[pairValue.Key];
            resourceMD5ValueList.Add(resourceValue);
        }
        string strValue = JsonMapper.ToJson(resourceMD5ValueList);

        string byteFileName = GetJsonFilePath();
        if (!File.Exists(byteFileName))
        {
            File.WriteAllText(byteFileName, strValue);
        }
        else
        {
            File.Delete(byteFileName);
            File.WriteAllText(byteFileName, strValue);
        }
    }

    private static string GetJsonFilePath()
    {
        string directoryName = System.IO.Path.GetDirectoryName(Application.dataPath);
        string[] directoryArray = directoryName.Split('/');
        directoryName = directoryName.Replace(directoryArray[directoryArray.Length - 1], "");
        directoryName += "ResourceMD5/resource_md5.bytes";
        return directoryName;
    }
}