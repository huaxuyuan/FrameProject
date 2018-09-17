using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class BuildStaticeData
{
    public int ID;
    public string resourceName;
    public int totalRow;
    public int totalCol;

    public BuildStaticeData()
    {
    }

    //public string GetExcelStr()
    //{
    //    string split = "^";
    //    return (split + ID + split + resourceType + split + assetbundleName + split + GetResourceStr() + split + version + split + commonResource);
    //}

}
public class BuildMobileData
{
    public int mobileID;
    public int buildStaticID;
    public int sceneID;
    public int row;
    public int col;

    public string GetExcelStr()
    {
        string split = "^";
        return (split + mobileID + split + buildStaticID + split + sceneID + split + row + split + col);
    }

}
public class BuildExcelHelper
{
    private Dictionary<int, BuildStaticeData> buildStaticData;
    private Dictionary<string, BuildStaticeData> buildStaticNameToDataDic;
    private Dictionary<int, BuildMobileData> buildMobileData;
    private Dictionary<string, int> buildStaticResourceToID;
    private List<string> sheetHeaderList;
    private string _excelName;
    char[] excelSplit = new char[] { '^' };
    private int headLineNum = 10;
    private int _mobileID;
    private float length;
    private float width;
    public Vector3 mapBeginPos;
    public Vector3 originPos;
    public  void InitializeBuildStatic(string excelName)
    {
        _excelName = excelName;
        buildStaticData = new Dictionary<int, BuildStaticeData>();
        buildStaticResourceToID = new Dictionary<string, int>();
        buildStaticNameToDataDic = new Dictionary<string, BuildStaticeData>();
        List<string> listTemp = null;
        string directoryName = GetExcelFilePath();
        listTemp = ExcelHelper.ReadOneSheetDataFromExcel(directoryName, "BuildingStatic");
        string dataStr;
        for (int index = headLineNum; index < listTemp.Count; ++index)
        {
            dataStr = listTemp[index];
            string[] dataArray = dataStr.Split(excelSplit);
            BuildStaticeData resourceData = new BuildStaticeData();
            resourceData.ID = int.Parse(dataArray[1]);
            resourceData.resourceName = dataArray[4];
            resourceData.totalRow = int.Parse(dataArray[6]);
            resourceData.totalCol = int.Parse(dataArray[5]);
           // Debug.Log("resource Data "+resourceData.resourceName.Replace("_prefab",""));
            //resourceData.PrintResourceData();
            buildStaticResourceToID.Add(resourceData.resourceName, resourceData.ID);
            buildStaticNameToDataDic.Add(resourceData.resourceName, resourceData);
            buildStaticData.Add(resourceData.ID, resourceData);
        }
    }
    public void InitializeBuildMobileData(string excelName,float tileLength,Vector3 beginPos,Vector3 originpos)
    {
        width = tileLength;
        length = tileLength;
        mapBeginPos = beginPos;
        originPos = originpos;

        _excelName = excelName;
        sheetHeaderList = new List<string>();
        string directoryName = GetExcelFilePath();
        //Debug.Log("directoryName " + directoryName);
        List<string> listTemp = null;
        listTemp = ExcelHelper.ReadOneSheetDataFromExcel(directoryName, "SceneBuildMobile");
        for (int index = 0; index < headLineNum; ++index)
        {
            sheetHeaderList.Add(listTemp[index]);
        }
        buildMobileData = new Dictionary<int, BuildMobileData>();
        _mobileID = 0;
    }
    public void AddBuildMobileData(string resourceName, int row, int col)
    {
        if (buildStaticResourceToID.ContainsKey(resourceName))
        {
            _mobileID++;
            BuildMobileData mobileData = new BuildMobileData();
            mobileData.mobileID = _mobileID;
            mobileData.buildStaticID = buildStaticResourceToID[resourceName];
            mobileData.row = row;
            mobileData.col = col;
            mobileData.sceneID = 1;
            buildMobileData.Add(mobileData.mobileID, mobileData);
        }
        else
        {
            Debug.LogError("add build mobile data error "+resourceName);
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
        foreach (BuildMobileData resourceData in buildMobileData.Values)
        {
            writeList.Add(resourceData.GetExcelStr());
        }
        ExcelHelper.WriteDataToExcel(directoryName, "SceneBuildMobile", ref writeList);
        Debug.Log(directoryName);
    }
    private string GetExcelFilePath()
    {
        string directoryName = Path.GetDirectoryName(Application.dataPath);
        string[] directoryArray = directoryName.Split('/');
        directoryName = directoryName.Replace(directoryArray[directoryArray.Length - 1], "");
        directoryName += "Config/2Schema/" + _excelName;
        return directoryName;
    }

    public void AddBuildMobileData(Vector3 tilePos, string resourceName)
    {
        Vector3 pos = Vector3.zero;
        if (buildStaticResourceToID.ContainsKey(resourceName))
        {
            Vector3 renderPos = pos - mapBeginPos - GetBuildOffset(resourceName);
            _mobileID++;
            BuildMobileData mobileData = new BuildMobileData();
            mobileData.mobileID = _mobileID;
            mobileData.buildStaticID = buildStaticResourceToID[resourceName];
            mobileData.row = (int)(renderPos.x / length);
            mobileData.col = (int)(renderPos.z / width);
            mobileData.sceneID = 1;
            buildMobileData.Add(mobileData.mobileID, mobileData);
        }
        else
        {
            Debug.LogError("add build mobile data error " + resourceName);
        }
    }
    public Vector3 GetBuildOffset(string resourceName)
    {
        BuildStaticeData staticData = buildStaticNameToDataDic[resourceName];
        return new Vector3((staticData.totalRow) * length / 2, 0, (staticData.totalCol) * width / 2) - originPos;
    }
}
