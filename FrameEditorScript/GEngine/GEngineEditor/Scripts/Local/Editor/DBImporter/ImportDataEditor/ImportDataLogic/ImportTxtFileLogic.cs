using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using ImportDataToDbSpace;

public class ImportTxtFileLogic : ImportDataLogic
{
	// note symbol
	private const string NOTE_SYMBOL = "##";
	// type
	private const string DATA_TYPE_C = "c";
	private const string DATA_TYPE_S = "s";
	private const string DATA_TYPE_CS = "cs";
	// data type
	private const string DATA_TYPE_INT = "INTEGER";
	private const string DATA_TYPE_STR = "VARCHAR";
	// Data Start Line Index
	private const int DATA_START_LINE = 3;
	// vaule callback key
	private const int LINE_ZERO = 0;
	private const int LINE_ONE = 1;
	private const int LINE_TWO = 2;
	private const int LINE_THREE = 3;

	private static ImportTxtFileLogic _instance;
	public static ImportTxtFileLogic Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new ImportTxtFileLogic();
			}

			return _instance;
		}
	}
	
	private List<string> _dataPrimaryKeyList;


	private List<string> _tableVariableNameList;
	private List<string> _tableVariableValueList;
	private List<string> _tableVariableTypeList;
	private List<int> _skipColumnsDataIndexList;
	private string[] _variableOriginalNameArray;
	private string[] _variableOriginalTypeArray;
	
	private string _strLine;

	private int _lineIndex;

	public ImportTxtFileLogic()
	{
		_skipColumnsDataIndexList = new List<int> ();
		_dataPrimaryKeyList = new List<string>();
		_tableVariableTypeList = new List<string> ();

		_tableVariableNameList = new List<string> ();
		_tableVariableValueList = new List<string> ();
	}

	public override void ResetData()
	{
		base.ResetData ();
	}

	/// <summary>
	/// Starts the import excel data.
	/// </summary>
	public override void StartImportData()
	{
		startTime = Time.realtimeSinceStartup;
		
		this.ResetData ();

		ConvertTxtDataToSql (allFiles,true);
	}
	/// <summary>
	/// Starts the import text data.
	/// </summary>
	/// <param name="filesArray">Files array.</param>
	public void StartImportData(string[] filesArray)
	{
		if(filesArray.Length > 0)
		{
			ResetData();

			ConvertTxtDataToSql (filesArray,false);
		}
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
			fileImportSwitchList.Clear();
			for(int i = 0; i < length;i ++)
				fileImportSwitchList.Add(false);
			
			FileComparer fc = new FileComparer();
			Array.Sort(allFiles,fc);
		}
	}

	/// <summary>
	/// Converts the text data to sql.
	/// </summary>
	/// <param name="array">Array.</param>
	/// <param name="isJudge">If set to <c>true</c> is judge.</param>
	private void ConvertTxtDataToSql(string[] array,bool isJudge)
	{
		string fileName = null;
		string tableName = null;
		string[] sLineArray = null;

		FileInfo fileInfo = null;

		StreamReader sReader = null; 
		
		for(int i = 0;i < array.Length; i ++)
		{
			if(isJudge && !fileImportSwitchList[i]) continue;

			_strLine = "";
			_lineIndex = 3;

			ResetTempBufferData();

			fileInfo = new FileInfo(array[i]);
			tableName = fileInfo.Name.Replace(FILE_TYPE_TXT,"");
			tempSqlDataItem = BuildSqlDataItem(tableName);

			sReader = GetStreamReader(array[i]);
			// check table title
			if(CheckTableTitleValid(ref sReader))
			{
				startWriteDbFlag = false;
				CloseStreamReader(ref sReader);
				return;
			}
			// check variable value
			while (_strLine != null)
			{
				sLineArray = GetValidLineStrArray (ref sReader);

				if(sLineArray == null)
				{
					_lineIndex ++;
					continue;
				}

				if(SetTableVariableValue(ref sLineArray))
				{
					startWriteDbFlag = false;
					CloseStreamReader(ref sReader);
					return;
				}

				_lineIndex ++;
			}
			
			CloseStreamReader(ref sReader);
		}

		base.StartImportData();
	}

	private StreamReader GetStreamReader(string filePath)
	{
		StreamReader sReader = null;
		try	{	sReader = new StreamReader(filePath);	}
		catch(Exception e)
		{
			copyFilePath = filePath.Replace(basePath + Path.DirectorySeparatorChar,"");
			copyFilePath = basePath + Path.DirectorySeparatorChar + COPY_NAME_SYMBOL + copyFilePath;
			File.Copy(filePath,copyFilePath,true);
			sReader = new StreamReader(copyFilePath);
		}

		return sReader;
	}

	private string[] GetValidLineStrArray(ref StreamReader sReader)
	{
		_strLine = sReader.ReadLine();
		
		if(_strLine == null || _strLine == "")// check the line content is null or not
			return null;
		
		string[] sLineArray = _strLine.Split('\t');
		
		if(sLineArray[0].Contains(NOTE_SYMBOL))//check the line is note or not
			return null;

		return sLineArray;
	}

	private bool CheckTableTitleValid(ref StreamReader sReader)
	{
		bool oneLineRes = SetTableVariableName (ref sReader);
		bool twoLineRes = SetTableVariableType (ref sReader);
		bool threeLineRes = SetTableType (ref sReader);

		return oneLineRes || twoLineRes || threeLineRes;
	}
	private bool SetTableVariableName(ref StreamReader sReader)
	{
		string[] sLineArray = GetValidLineStrArray (ref sReader);

		if(sLineArray == null)
		{
			Debug.LogError("The Table :" + tempSqlDataItem.tableName + " 第 1 行数据为空!!!");
			return true;
		}
		
		_variableOriginalNameArray = sLineArray;
	
		for(int k = 0;k < sLineArray.Length ; k ++)
		{
			if(!sLineArray[k].Contains(NOTE_SYMBOL))
			{
				_tableVariableValueList.Add(sLineArray[k]);
			}
			else
				_skipColumnsDataIndexList.Add(k);
			
		}

		return CheckVariableNameValid ();
	}

	private bool SetTableVariableType(ref StreamReader sReader)
	{
		string[] sLineArray = GetValidLineStrArray (ref sReader);

		if(sLineArray == null)
		{
			Debug.LogError("The Table :" + tempSqlDataItem.tableName + " 第 2 行数据为空!!!");
			return true;
		}

		_variableOriginalTypeArray = sLineArray;
		_tableVariableValueList.Clear ();
		for(int k = 0;k < sLineArray.Length ; k ++)
		{
			if(!_skipColumnsDataIndexList.Contains(k))
			{
				_tableVariableValueList.Add(sLineArray[k]);
			}
		}

		return CheckVariableTypeValid ();
	}

	private bool SetTableType(ref StreamReader sReader)
	{
		string[] sLineArray = GetValidLineStrArray (ref sReader);

		if(sLineArray == null)
		{
			Debug.LogError("The Table :" + tempSqlDataItem.tableName + " 第 3 行数据为空!!!");
			return true;
		}

		string tempStr = null;
		for(int k = 0;k < sLineArray.Length ; k ++)
		{
			tempStr = sLineArray[k];
			if(!_skipColumnsDataIndexList.Contains(k) && tempStr == DATA_TYPE_S)
			{
				_skipColumnsDataIndexList.Add(k);
			}
		}

		bool res = sLineArray.Length ==_skipColumnsDataIndexList.Count;// check the table is used for client or server

		if(!res)
		{
			_tableVariableNameList.Clear();
			_tableVariableTypeList.Clear();
			for(int index = 0;index < _variableOriginalNameArray.Length;index ++)
			{
				if(_skipColumnsDataIndexList.Contains(index)) continue;

				_tableVariableNameList.Add(_variableOriginalNameArray[index]);
				_tableVariableTypeList.Add(_variableOriginalTypeArray[index]);
			}
			// create "drop sql "
			string tempSqlStr = BuildSqlStrByList(SQL_MODE.MODE_DROP_TABLE,tempSqlDataItem.tableName,null,null);
			tempSqlDataItem.strList.Add(tempSqlStr);
			// create "create table"
			tempSqlStr = BuildSqlStrByList(SQL_MODE.MODE_CREATE_TABLE,tempSqlDataItem.tableName,_tableVariableNameList.ToArray(),null,_tableVariableTypeList.ToArray());
			tempSqlDataItem.strList.Add(tempSqlStr);
			totalProgress += 2;
		}
		else
			Debug.LogError("The Table :" + tempSqlDataItem.tableName + " is used for Server !!!");

		return res;
	}

	private bool SetTableVariableValue(ref string[] sLineArray)
	{
		_tableVariableValueList.Clear ();
		for(int k = 0;k < sLineArray.Length ; k ++)
		{
			if(!_skipColumnsDataIndexList.Contains(k))
			{
				_tableVariableValueList.Add(sLineArray[k]);
			}
		}
		// create "insert sql"
		string tempSqlStr = BuildSqlStrByList(SQL_MODE.MODE_INSERT,tempSqlDataItem.tableName,_tableVariableNameList.ToArray(),_tableVariableValueList.ToArray());
		tempSqlDataItem.strList.Add(tempSqlStr);
		totalProgress += 1;

		return CheckVariableValueValid ();
	}

	private bool CheckVariableNameValid()
	{
		List<string> tempList = new List<string> ();
		string tempStr = null;
		string tableName = tempSqlDataItem.tableName;
		int count = _tableVariableValueList.Count;
		for(int i = 0; i < count;i ++)
		{
			tempStr = _tableVariableValueList[i];
			if(tempStr != "")
			{
				if( tempStr != NOTE_SYMBOL)
				{
					if(tempList.Contains(tempStr))
					{
						SetMsg("Data Error : " + tableName + "表格" + tempStr + "字段名重复!!!");
						return true;
					}
					else
						tempList.Add(tempStr);
				}
			}
			else
			{
				SetMsg("Data Error : " + tableName + "表格,第 " + (i + 1) + "个字段为空!!!");
				return true;
			}
				
		}

		return false;
	}

	private bool CheckVariableTypeValid()
	{
		string tableName = tempSqlDataItem.tableName;
		int count = _tableVariableValueList.Count;
		for(int i = 0; i < count;i ++)
		{
			if(_tableVariableValueList[i] == "")// check data null is null or not
			{
				SetMsg("Data Error :" + tableName + "表格第" + 2 + "行,第" + (i + 1) +"列数据为空!!!");
				return true;
			}
		}

		return false;
	}

	private bool CheckVariableValueValid()
	{
		string tempStr = null;
		string tableName = tempSqlDataItem.tableName;
		int count = _tableVariableValueList.Count;
		for(int i = 0; i < count ;i ++)
		{
			tempStr = _tableVariableTypeList[i];
			if(tempStr == DATA_TYPE_INT)// check the type of data 
			{
				tempStr = _tableVariableValueList[i];
				int tempData = 0;
				if(!int.TryParse(tempStr,out tempData))
				{
					SetMsg("Data Error :" + tableName + "表格第" + (_lineIndex + 1) + "行,第" + (i + 1) +"列数据：" + tempStr + " 数据类型不符!!!");
					return true;
				}
			}
			else if(tempStr == "")// check data is null
			{
				SetMsg("Data Error :" + tableName + "表格第" + (_lineIndex + 1) + "行,第" + (i + 1) +"列数据为数据为空!!!");
				return true;
			}
		}

		tempStr = _tableVariableValueList [0];
		if(_dataPrimaryKeyList.Contains(tempStr))// check the primary key
		{
			SetMsg("Data Error :" + tableName + "表格第" + (_lineIndex + 1) + "行,数据id=" + tempStr + "重复!!!");
			return true;
		}
		else
			_dataPrimaryKeyList.Add(tempStr);


		return false;
	}

	private void CloseStreamReader(ref StreamReader reader)
	{
		reader.Close();
		reader.Dispose();

		if(copyFilePath != null)
		{
			File.Delete(copyFilePath);
			copyFilePath = null;
		}
	}

	private void ResetTempBufferData()
	{
		_skipColumnsDataIndexList.Clear();
		_dataPrimaryKeyList.Clear();
		_tableVariableTypeList.Clear();
		_tableVariableNameList.Clear ();
		_tableVariableValueList.Clear ();

		_variableOriginalNameArray = null;
		_variableOriginalTypeArray = null;
	}
}