using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System;
using ImportDataToDbSpace;

using GEngine.Editor;

public class ImportExcelFileLogic : ImportDataLogic
{
	private const char SHEET_VALID_NAME_FLAG = '|';
	private const char SHEET_VALUE_SPLIT_CHAR = '^';

	private const string SHEET_EMPTY_VALUE = "";
	private const string SHEET_CLIENT_FLAG = "c";

	private const string SHEET_TYPE_INT = "int";
	private const string SHEET_TYPE_LONG = "long";
	private const string SHEET_TYPE_FLOAT = "float";
	private const string SHEET_TYPE_STRING = "string";
	private const string SHEET_TYPE_BOOLEAN = "boolean";
	private const string SHEET_TYPE_JSON = "json";

	private const string DATA_TYPE_INT = "INTEGER";
	private const string DATA_TYPE_STR = "TEXT";

	private const string SHEET_LINE_COMMENT_FLAG = "#";

	private const string DEFAULT_INT_VALUE = "0";
	private const string DEFAULT_STRING_VALUE = "0";

	private const string BOOLEAN_ORIGINAL_DEFAULT_TRUE_VALUE = "true";
	private const string BOOLEAN_ORIGINAL_DEFAULT_FALSE_VALUE = "false";

	private const string BOOLEAN_DEFAULT_TRUE_VALUE = "1";
	private const string BOOLEAN_DEFAULT_FALSE_VALUE = "0";

	private const string EXCEL_TEMP_FILE_FLAG = "~$";

	private const int EXCEL_MIN_ROW_NUM = 4;

	private static ImportExcelFileLogic _instance;
	public static ImportExcelFileLogic Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new ImportExcelFileLogic();
			}
			
			return _instance;
		}
	}

	private List<int> _columnInValidIndexList;
	private List<string> _tableVariableOriginalTypeList;
	private List<string> _tableVariableTypeList;
	private List<string> _tableVariableNameList;
	private List<string> _tableVariableValueList;
	private List<string> _dataPrimaryKeyList;

	private Dictionary<string, List<SheetItem>> _sheetDataDic;// sheet name is the key;

	private SheetItem _currentSheetItem;

	public ImportExcelFileLogic()
	{
		_columnInValidIndexList = new List<int> ();
		_tableVariableOriginalTypeList = new List<string> ();
		_tableVariableTypeList = new List<string> ();
		_tableVariableNameList = new List<string> ();
		_tableVariableValueList = new List<string> ();
		_dataPrimaryKeyList = new List<string> ();

		_sheetDataDic = new Dictionary<string, List<SheetItem>> ();
	}

	public override void ResetData ()
	{
		base.ResetData ();
	
		_sheetDataDic.Clear ();
	}

	public override void StartImportData ()
	{
		startTime = Time.realtimeSinceStartup;
		
		this.ResetData ();
		
		ConvertExcelDataToSql (allFiles,true);
	}

	public void StartImportData(string[] filesArray)
	{
		this.ResetData ();

		ConvertExcelDataToSql (filesArray,false);
	}

	/// <summary>
	/// Checks all execl files.
	/// </summary>
	public override void CheckAllFiles(string basePath,string filterStr)
	{
		base.CheckAllFiles (basePath,filterStr);

		int length = allFiles.Length;
		if(length > 0)
		{
			List<string> tempList = new List<string>();
			string tempStr = null;
			string fileName = null;
			for(int i = 0; i < length;i ++)
			{
				tempStr = allFiles[i];
				fileName = tempStr.Replace(basePath + Path.DirectorySeparatorChar,"");
				if(fileName.Contains(EXCEL_TEMP_FILE_FLAG)) continue;

				tempList.Add(tempStr);
			}

			allFiles = tempList.ToArray();
			
			FileComparer fc = new FileComparer();
			Array.Sort(allFiles,fc);

			length = allFiles.Length;
			fileImportSwitchList.Clear();

			for(int i = 0; i < length;i ++)
				fileImportSwitchList.Add(false);
		}
	}

	public override void CheckAllFiles (string basePath)
	{
		base.CheckAllFiles (basePath);

		int length = allFiles.Length;
		if(length > 0)
		{
			List<string> tempList = new List<string>();
			string tempStr = null;
			string fileName = null;
			for(int i = 0; i < length;i ++)
			{
				tempStr = allFiles[i];
				fileName = tempStr.Replace(basePath + Path.DirectorySeparatorChar,"");
				if(fileName.Contains(EXCEL_TEMP_FILE_FLAG)) continue;
				
				tempList.Add(tempStr);
			}
			
			allFiles = tempList.ToArray();
			
			FileComparer fc = new FileComparer();
			Array.Sort(allFiles,fc);
			
			length = allFiles.Length;
			fileImportSwitchList.Clear();
			
			for(int i = 0; i < length;i ++)
				fileImportSwitchList.Add(false);
		}
	}

	/// <summary>
	/// Converts the excel data to sql.
	/// </summary>
	/// <param name="array">Array.</param>
	/// <param name="isJudge">If set to <c>true</c> is judge.</param>
	private void ConvertExcelDataToSql(string[] array,bool isJudge)
	{
        int length = array.Length;
        if (length == 0) return;

        string filePath = null;
        int i = 0;
        do
        {
            if (isJudge && !fileImportSwitchList[i])
            {
                i++;
                continue;
            }

            filePath = array[i];

            if (!ReadExcelFileByEPPlusDLL(array[i])) return;

            i++;
        }
        while (i < length);

        if (!CombineSheets()) return;

        base.StartImportData();
    }

	#region Read Excel By EPPlus.dll
	public bool ReadExcelFileByEPPlusDLL(string filePath) 
	{ 
		Dictionary<string,List<string>> dataDic = ExcelHelper.ReadAllDataFromExcel (filePath);

		if(dataDic == null) return false;
       
		string fileName = Path.GetFileName (filePath);
		string sheetName = null;
		List<SheetItem> tempList = null;
		List<string> valueList = null;

		int count = 0;
		int rowIndex = 0;

		foreach(KeyValuePair<string,List<string>> data in dataDic)
		{
			sheetName = data.Key;
			if(!sheetName.Contains(SHEET_VALID_NAME_FLAG.ToString())) continue;

			_columnInValidIndexList.Clear();

			valueList = data.Value;
			count = valueList.Count;
			if(count <= 3) 
			{
				Debug.LogError("The Sheet of '" + sheetName + "' in the file " + fileName + "row num less than 4");
				continue;
			}

			// check first line
			if(!CheckSheetFirstLineValid(valueList[0],sheetName,fileName)) 
			{
				Debug.LogError("The Sheet of '" + sheetName + "' is not table for Client in the file :" + fileName);
				continue;
			}

			sheetName = sheetName.Split(SHEET_VALID_NAME_FLAG)[1];

			if(_sheetDataDic.ContainsKey(sheetName))
				tempList = _sheetDataDic[sheetName];
			else
			{
				tempList = new List<SheetItem>();
				_sheetDataDic.Add(sheetName,tempList);
			}

			_currentSheetItem = new SheetItem(data.Key,fileName);
			tempList.Add(_currentSheetItem);

			// check second line
			if(!CheckSheetSecondLineValid(valueList[1],sheetName,fileName)) return false;
			// check third line
			if(!CheckSheetThirdLineValid(valueList[2],sheetName,fileName)) return false;
			// skip forth line
			// check data ,more than five lines
			if(!CheckSheetVariableValue(ref valueList,sheetName,fileName)) return false;
		}
		
		return true;
	}
	
	private bool CheckSheetFirstLineValid(string firstLineStr,string sheetName,string fileName)
	{
		if(String.IsNullOrEmpty(firstLineStr))
		{
			SetMsg("文件: " + fileName + " 中表 " + sheetName + " : 第 1 行数据为空 ！！！");
			return false;
		}
		string[] tempArray = firstLineStr.Split (SHEET_VALUE_SPLIT_CHAR);
		int length = tempArray.Length;
		if(length == 0) return false;

		string colValue = null;
		int colIndex = 0;
		do
		{
			colValue = tempArray[colIndex];
			if(colValue == null || colValue == "") _columnInValidIndexList.Add(colIndex);
			else
			{
				if(!colValue.ToLower().Contains(SHEET_CLIENT_FLAG)) _columnInValidIndexList.Add(colIndex);
			}

			colIndex ++;
		}
		while(colIndex < length);

		return _columnInValidIndexList.Count != length;
	}
	
	private bool CheckSheetSecondLineValid(string secondLineStr,string sheetName,string fileName)
	{
		if(String.IsNullOrEmpty(secondLineStr))
		{
			SetMsg("文件: " + fileName + " 中表 " + sheetName + " : 第 2 行数据为空 ！！！");
			return false;
		}
		string[] tempArray = secondLineStr.Split (SHEET_VALUE_SPLIT_CHAR);
		int length = tempArray.Length;

		_tableVariableTypeList.Clear();
		_tableVariableOriginalTypeList.Clear();

		string colValue = null;
		int colIndex = 0;
		do
		{
			if(_columnInValidIndexList.Contains(colIndex)) 
			{
				colIndex ++;
				continue;
			}

			colValue = tempArray[colIndex].ToLower();
			if(String.IsNullOrEmpty(colValue))
			{
				SetMsg("文件: " + fileName + " 中表 " + sheetName + " : 第 2 行,第 " + (colIndex + 1) + "列数据为空 ！！！");
				return false;
			}

			if(colValue == SHEET_TYPE_INT || colValue == SHEET_TYPE_BOOLEAN || colValue == SHEET_TYPE_LONG)
				colValue = DATA_TYPE_INT;
			else if(colValue == SHEET_TYPE_JSON || colValue == SHEET_TYPE_STRING || colValue == SHEET_TYPE_FLOAT)
				colValue = DATA_TYPE_STR;
			else
			{
				SetMsg("文件: " + fileName + " 中表 " + sheetName + " : 第 2 行,第 " + (colIndex + 1) + "列：" + colValue + " 未知数据类型 ！！！");
				return false;
			}
			
			_tableVariableTypeList.Add(colValue);

			colIndex ++;
		}
		while(colIndex < length);

		colValue = null;
		colIndex = 0;
		do
		{
			colValue = tempArray[colIndex].ToLower();
						
			if(colValue == SHEET_TYPE_INT || colValue == SHEET_TYPE_BOOLEAN || colValue == SHEET_TYPE_LONG)
				colValue = DATA_TYPE_INT;
			else if(colValue == SHEET_TYPE_JSON || colValue == SHEET_TYPE_STRING || colValue == SHEET_TYPE_FLOAT)
				colValue = DATA_TYPE_STR;
			
			_tableVariableOriginalTypeList.Add(colValue);
			
			colIndex ++;
		}
		while(colIndex < length);
		
		return true;
	}
	
	private bool CheckSheetThirdLineValid(string thirdLineStr,string sheetName,string fileName)
	{
		if(String.IsNullOrEmpty(thirdLineStr))
		{
			SetMsg("文件: " + fileName + " 中表 " + sheetName + ": 第 3 行数据为空 ！！！");
			return false;
		}
		string[] tempArray = thirdLineStr.Split (SHEET_VALUE_SPLIT_CHAR);
		int length = tempArray.Length;
				
		_tableVariableNameList.Clear();
		
		string colValue = null;
		int colIndex = 0;
		do
		{
			if(_columnInValidIndexList.Contains(colIndex)) 
			{
				colIndex ++;
				continue;
			}
			
			colValue = tempArray[colIndex];
			if(String.IsNullOrEmpty(colValue)) 
			{
				SetMsg("文件: " + fileName + " 中表 " + sheetName + " : 第 3 行，第 " + (colIndex + 1) + " 列数据为空！！！");
				return false;
			}

			if(_tableVariableNameList.Contains(colValue))
			{
				SetMsg("文件: " + fileName + " 中表 " + sheetName + " : 第 3 行,第 " + (colIndex + 1)+ "列: " + colValue + " 字段名重复 ！！！");
				return false;
			}
			
			_tableVariableNameList.Add(colValue);

			
			colIndex ++;
		}
		while(colIndex < length);

		// set SheetItem variableTypeDic
		length = _tableVariableNameList.Count;
		colIndex = 0;
		do
		{
			_currentSheetItem.variableTypeDic.Add(_tableVariableNameList[colIndex],_tableVariableTypeList[colIndex]);

			colIndex ++;
		}
		while(colIndex < length);
				
		return true;
	}
	
	private bool CheckSheetVariableValue(ref List<string> valueList,string sheetName,string fileName)
	{
		_dataPrimaryKeyList.Clear ();
		
		string tempSnStr = null;
		string valueStr = null;
		
		Dictionary<string,string> tempDic = null;

		int count = valueList.Count;
		if(count <= 4) return true;

		int rowIndex = 4;
		do
		{
			valueStr = valueList[rowIndex];
			if(String.IsNullOrEmpty(valueStr))
			{
				rowIndex ++;
				continue;
			}
			_tableVariableValueList.Clear ();

			string[] tempArray = valueStr.Split (SHEET_VALUE_SPLIT_CHAR);

			string colValue = tempArray[0].Substring(0,1);
			
			if(colValue == SHEET_LINE_COMMENT_FLAG) 
			{
				rowIndex ++;
				continue;
			}
			
			colValue = tempArray[0];
			
			if(_dataPrimaryKeyList.Contains(colValue))
			{
				SetMsg("文件: " + fileName + " 中表 " + sheetName + " : 第 " + (rowIndex + 1)  + " 行,主键sn = " + colValue + "数据重复 ！！！");
				return false;
			}
			
			int tempData = 0;
			if(!int.TryParse(colValue,out tempData))
			{
				SetMsg("文件: " + fileName + " 中表 " + sheetName + " : 第 " + (rowIndex + 1)  + " 行,第 " + 1+ "列数据: " + colValue +"数据类型不符 ！！！");
				return false;
			}
			
			_dataPrimaryKeyList.Add(colValue);
			_tableVariableValueList.Add(colValue);

			int length = tempArray.Length;

			if(length == 1)
			{
				rowIndex ++;
				continue;
			}
	
			int typeIndex = 1;
			int colIndex = 1;
			do
			{
				if(_columnInValidIndexList.Contains(colIndex)) continue;

				colValue = tempArray[0];
				colValue =  String.IsNullOrEmpty(tempArray[colIndex]) ? (_tableVariableOriginalTypeList[colIndex] == DATA_TYPE_INT ? DEFAULT_INT_VALUE : DEFAULT_STRING_VALUE) : tempArray[colIndex];
				
				if(colValue.ToLower() == BOOLEAN_ORIGINAL_DEFAULT_TRUE_VALUE)
					colValue = BOOLEAN_DEFAULT_TRUE_VALUE;
				else if(colValue.ToLower() == BOOLEAN_ORIGINAL_DEFAULT_FALSE_VALUE)
					colValue = BOOLEAN_DEFAULT_FALSE_VALUE;
				
				if(_tableVariableTypeList[typeIndex] == DATA_TYPE_INT)
				{
					if(!int.TryParse(colValue,out tempData))
					{
						SetMsg("文件: " + fileName + " 中表 " + sheetName + " : sn == " + tempArray[0] + " ,第 " + (rowIndex + 1)  + " 行,第 " + (colIndex + 1)+ "列数据: " + colValue +"数据类型不符 ！！！");
						return false;
					}
				}
				
				_tableVariableValueList.Add(colValue);
				
				typeIndex ++;
								
				colIndex ++;
			}
			while(colIndex < length);

			// set sheetItem variableValueDic
			tempSnStr = _tableVariableValueList[0];
			if(_currentSheetItem.variableValueDic.ContainsKey(tempSnStr))
				tempDic = _currentSheetItem.variableValueDic[tempSnStr];
			else
			{
				tempDic = new Dictionary<string, string>();
				_currentSheetItem.variableValueDic.Add(tempSnStr,tempDic);
			}

			colIndex = 0;
			do
			{
				tempDic.Add(_tableVariableNameList[colIndex],_tableVariableValueList[colIndex]);
				
				colIndex ++;
			}
			while(colIndex < length);
						
			rowIndex ++;
		}
		while(rowIndex < count);

		return true;
	}
	#endregion

	private bool CombineSheets()
	{
		Dictionary<string,SheetItem> combineSheetDic = new Dictionary<string, SheetItem> ();
		Dictionary<string,string> tempValueDic = null;

		List<SheetItem> tempList = null;
		List<string> variableNameBADifferList = new List<string>();
		List<string> variableNameABDifferList = new List<string> ();

		SheetItem combineSheetItem = null;
		SheetItem tempSheetItem = null;

		string sheetName = null;
		string differVariableName = null;
		string differVariableType = null;
		string differVariableValue = null;

		int count = 0;
		int listCount = 0;
		int index = 0;
		int k = 0;

		foreach(KeyValuePair<string,List<SheetItem>> data in _sheetDataDic)
		{
			sheetName = data.Key;
			tempList = data.Value;

			combineSheetItem = tempList[0];
			combineSheetDic.Add(sheetName,combineSheetItem);

			count = tempList.Count;
			if(count == 1) continue;

			for(index = 1;index < count;index ++)
			{
				variableNameBADifferList.Clear();
				variableNameABDifferList.Clear();

				tempSheetItem = tempList[index];
				// get differ variable name List
				foreach(string snStr in tempSheetItem.variableValueDic.Keys)
				{
					if(!combineSheetItem.variableValueDic.ContainsKey(snStr))
					{
						foreach(string name in tempSheetItem.variableValueDic[snStr].Keys)
						{
							if(!combineSheetItem.variableTypeDic.ContainsKey(name))
							{
								if(!variableNameBADifferList.Contains(name))
									variableNameBADifferList.Add(name);
							}
						}
					}
					else
					{
						SetMsg("文件: " + tempSheetItem.fileName + " 中，表 " + tempSheetItem.originalSheetName + "中，主键sn == " + snStr + " 为重复数据 !!!");
						return false;
					}
				}

				foreach(string name in combineSheetItem.variableTypeDic.Keys)
				{
					if(!tempSheetItem.variableTypeDic.ContainsKey(name))
						variableNameABDifferList.Add(name);
				}
				//add combine item name str
				listCount = variableNameBADifferList.Count;
				if(listCount > 0)
				{
					for(k = 0;k < listCount;k ++)
					{
						differVariableName = variableNameBADifferList[k];
						differVariableType = tempSheetItem.variableTypeDic[differVariableName];
						differVariableValue = differVariableType == DATA_TYPE_INT ? DEFAULT_INT_VALUE : DEFAULT_STRING_VALUE;

						combineSheetItem.variableTypeDic.Add(differVariableName,differVariableType);
						foreach(string snStr in combineSheetItem.variableValueDic.Keys)
						{
							tempValueDic = combineSheetItem.variableValueDic[snStr];
							tempValueDic.Add(differVariableName,differVariableValue);
						}
					}
				}
				// add temp item name str
				listCount = variableNameABDifferList.Count;
				if(listCount > 0)
				{
					for(k = 0;k < listCount;k ++)
					{
						differVariableName = variableNameABDifferList[k];
						differVariableType = combineSheetItem.variableTypeDic[differVariableName];
						differVariableValue = differVariableType == DATA_TYPE_INT ? DEFAULT_INT_VALUE : DEFAULT_STRING_VALUE;

						tempSheetItem.variableTypeDic.Add(differVariableName,differVariableType);
						foreach(string snStr in tempSheetItem.variableValueDic.Keys)
						{
							tempValueDic = tempSheetItem.variableValueDic[snStr];
							tempValueDic.Add(differVariableName,differVariableValue);
						}
					}
				}
				// combine two sheet data
				foreach(KeyValuePair<string,Dictionary<string,string>> info in tempSheetItem.variableValueDic)
				{
					combineSheetItem.variableValueDic.Add(info.Key,info.Value);
				}
				tempSheetItem.variableTypeDic.Clear();
				tempSheetItem.variableValueDic.Clear();
				tempSheetItem = null;
			}
		}

		// build sql str
		string tempSqlStr = null;
		foreach (KeyValuePair<string, SheetItem> data in combineSheetDic) 
		{
			tempSqlDataItem = BuildSqlDataItem(data.Key);
			// create "drop sql "
			tempSqlStr = BuildSqlStrByDic(SQL_MODE.MODE_DROP_TABLE,data.Key,null,null);
			tempSqlDataItem.strList.Add(tempSqlStr);
			// create "create table"
			tempSqlStr = BuildSqlStrByDic(SQL_MODE.MODE_CREATE_TABLE,data.Key,data.Value.variableTypeDic,null);
			tempSqlDataItem.strList.Add(tempSqlStr);
			// create "insert sql"
			foreach(Dictionary<string,string> itemDic in data.Value.variableValueDic.Values)
			{
				tempSqlStr = BuildSqlStrByDic(SQL_MODE.MODE_INSERT,data.Key,null,itemDic);
				tempSqlDataItem.strList.Add(tempSqlStr);
			}

			totalProgress += (data.Value.variableValueDic.Count + 2);
		}

		return true;
	}
}
