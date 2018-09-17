using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class ResourceData
{
    public int ID;
    public string assetbundleName;
    public List<string> resourceArray;
    public string resourceType;
    public string version;
    public int commonResource;
    public int where;

    public ResourceData()
    {
        resourceArray = new List<string>();
    }

    public string GetResourceStr()
    {
        string resourceStr = "";
        for (int index = 0; index < resourceArray.Count; ++index)
        {
            resourceStr += resourceArray[index] + ";";
        }
        resourceStr = resourceStr.Substring(0, resourceStr.Length - 1);
        return resourceStr;
    }

    public string GetExcelStr()
    {
        string split = "^";
        string result = (split + ID + split + resourceType + split + assetbundleName + split + GetResourceStr() + split + version + split + commonResource + split + where);
        Debug.Log("result " + result);
        return result;
    }

    public void PrintResourceData()
    {
        string split = "^";
        Debug.Log(split + ID + split + resourceType + split + assetbundleName + split + GetResourceStr() + split + version + split + commonResource + split + where);
    }
}

public class ResourceExcelHelper
{
    private Dictionary<string, ResourceData> resourceDataDic;
    private List<string> sheetHeaderList;
    private string _excelName;
    private char[] excelSplit = new char[] { '^' };
    private int headLineNum = 10;

    public void InitializeResourceData(string excelName)
    {
        _excelName = excelName;
        resourceDataDic = new Dictionary<string, ResourceData>();
        sheetHeaderList = new List<string>();
        string directoryName = GetExcelFilePath();
        List<string> listTemp = null;
        listTemp = ExcelHelper.ReadOneSheetDataFromExcel(directoryName, "ResourceStatic");
        for (int index = 0; index < headLineNum; ++index)
        {
            sheetHeaderList.Add(listTemp[index]);
        }
        string dataStr;
        for (int index = headLineNum; index < listTemp.Count; ++index)
        {
            dataStr = listTemp[index];
            string[] dataArray = dataStr.Split(excelSplit);
            ResourceData resourceData = new ResourceData();
            resourceData.ID = int.Parse(dataArray[1]);
            resourceData.resourceType = dataArray[2];
            resourceData.assetbundleName = dataArray[3];
            string[] resourceArray = dataArray[4].Split(';');
            for (int resourceIndex = 0; resourceIndex < resourceArray.Length; ++resourceIndex)
            {
                resourceData.resourceArray.Add(resourceArray[resourceIndex]);
            }
            resourceData.version = dataArray[5];
            resourceData.commonResource = int.Parse(dataArray[6]);
            resourceData.where = int.Parse(dataArray[7]);
            resourceData.PrintResourceData();
            resourceDataDic.Add(resourceData.assetbundleName, resourceData);
        }
    }

    public void WriteData()
    {
        string directoryName = GetExcelFilePath();
        List<string> writeList = new List<string>();
        for (int sheetHeaderIndex = 0; sheetHeaderIndex < sheetHeaderList.Count; ++sheetHeaderIndex)
        {
            writeList.Add(sheetHeaderList[sheetHeaderIndex]);
        }
        foreach (ResourceData resourceData in resourceDataDic.Values)
        {
            writeList.Add(resourceData.GetExcelStr());
        }
        ExcelHelper.WriteDataToExcel(directoryName, "ResourceStatic", ref writeList);
        Debug.Log(directoryName);
    }

    private bool HasData(string assetbundleName)
    {
        return resourceDataDic.ContainsKey(assetbundleName);
    }

    public ResourceData GetResourceData(string assetbundleName)
    {
        if (!resourceDataDic.ContainsKey(assetbundleName))
        {
            ResourceData resourceData = new ResourceData();
            resourceData.assetbundleName = assetbundleName;
            resourceDataDic.Add(assetbundleName, resourceData);
            resourceData.ID = resourceDataDic.Values.Count;
            resourceData.commonResource = 0;
            resourceData.where = 0;
        }
        return resourceDataDic[assetbundleName];
    }

    private string GetExcelFilePath()
    {
        string directoryName = Path.GetDirectoryName(Application.dataPath);
        string[] directoryArray = directoryName.Split('/');
        directoryName = directoryName.Replace(directoryArray[directoryArray.Length - 1], "");
        directoryName += "ConfigExcel/mobileData/" + _excelName;
        return directoryName;
    }
}