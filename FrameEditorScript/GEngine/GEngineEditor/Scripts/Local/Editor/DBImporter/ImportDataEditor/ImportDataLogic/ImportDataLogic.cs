using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using ImportDataToDbSpace;

public class ImportDataLogic
{
	public delegate void ImportDbDelegate();

	protected enum SQL_MODE
	{
		MODE_CREATE_TABLE,
		MODE_DROP_TABLE,
		MODE_INSERT,
	}
	protected const string COPY_NAME_SYMBOL = "CopyFile_";

	protected const string FILE_TYPE_TXT = ".txt";

	protected const string EXCEL_97_2003_FILE_FILTER = "*.xls";
//	protected const string EXCEL_BEYONG_2007_FILE_FILTER = "*.xlsx";

	protected const string SHEET_PRIMERY_KEY = "sn";

	protected string basePath;// wait for debug :where initialize...
	protected SqlDataItem tempSqlDataItem;
	protected bool startWriteDbFlag;
	protected float startTime;
	protected float endTime;
	protected float totalProgress;
	protected float currentProgress;

	protected string copyFilePath;

	protected List<SqlDataItem> sqlStrList;

	private int _tablePoint;
	private int _sqlStrPoint;
	private int _tableNum;
	private DataBaseAccess _m_databaseAccess;
	private string _msg = "";
	private bool _showMsgFlag;
	private DataBaseReader _reader = null;

	public string[] allFiles;
	public List<bool> fileImportSwitchList;


	public ImportDataLogic()
	{
		fileImportSwitchList = new List<bool> ();
		sqlStrList = new List<SqlDataItem> ();

		startWriteDbFlag = false;
		_showMsgFlag = false;
	}

	public virtual void StartImportData()
	{
		if(sqlStrList.Count > 0)
		{
			tempSqlDataItem = null;
			startWriteDbFlag = true;
			
			InitDataBase ();
		}
	}

	public virtual void ResetData()
	{
		sqlStrList.Clear ();

		tempSqlDataItem = null;
		_tablePoint = 0;
		_sqlStrPoint = 0;
		_tableNum = 0;
		currentProgress = 0f;
		totalProgress = 0f;

		copyFilePath = null;

		ResetMsg ();
	}

	/// <summary>
	/// Checks all execl files.
	/// </summary>
	public virtual void CheckAllFiles(string basePath,string filterStr)
	{
		if(basePath != "")
		{
			this.basePath = basePath;
			if(Directory.Exists(basePath))
			{
				allFiles = Directory.GetFiles(basePath,filterStr,SearchOption.TopDirectoryOnly);
			}
			else
				Debug.LogError("The Path '" + basePath + "' is not Exists !!!");
		}
		else
			Debug.LogError("Please Input Real Base Path !!!");
	}

	public virtual void CheckAllFiles(string basePath)
	{
		if(basePath != "")
		{
			this.basePath = basePath;
			if(Directory.Exists(basePath))
			{
				allFiles = Directory.GetFiles(basePath,EXCEL_97_2003_FILE_FILTER,SearchOption.TopDirectoryOnly);
			}
			else
				Debug.LogError("The Path '" + basePath + "' is not Exists !!!");
		}
		else
			Debug.LogError("Please Input Real Base Path !!!");
	}

	public bool IsStartWriteDataToDb()
	{
		return startWriteDbFlag;
	}

	/// <summary>
	/// Sets the state of the switch list.
	/// </summary>
	/// <param name="selectAllFlag">If set to <c>true</c> select all flag.</param>
	public void SetSwitchListState(bool selectAllFlag)
	{
		for(int i = 0; i < allFiles.Length; i ++)
			fileImportSwitchList[i] = selectAllFlag;
	}

	/// <summary>
	/// Executes the sql op.
	/// </summary>
	public void ExecuteSqlOp(ImportDbDelegate callBack = null)
	{
		if(tempSqlDataItem == null)
		{
			if(_tablePoint < sqlStrList.Count)
			{
				tempSqlDataItem = sqlStrList[_tablePoint];
			}
			else
			{
				startWriteDbFlag = false;
				_m_databaseAccess.TransactionCommit();
				CloseDbConnect ();
				endTime = Time.realtimeSinceStartup;
				SetMsg("Import Data Success ,it takes " + (endTime - startTime) +" s");
                UnityEditor.EditorUtility.DisplayDialog("导入数据信息", "导入数据成功", "OK");

				if(callBack != null) callBack();
				return;
			}
		}
		
		if(_sqlStrPoint < tempSqlDataItem.strList.Count)
		{
			try
			{
				_reader = new DataBaseReader ();
				_m_databaseAccess.ExecuteQuery(tempSqlDataItem.strList[_sqlStrPoint],ref _reader);
				_reader.clearData();
				
				_sqlStrPoint ++;
			}
			catch (System.Exception ex)
			{
				SetMsg("Data Error :" + tempSqlDataItem.tableName + "表格导入失败  " + ex.ToString().Replace("\n",","));
				SetMsg("Data Error Sql:" + tempSqlDataItem.strList[_sqlStrPoint]);
				startWriteDbFlag = false;
				_m_databaseAccess.TransactionRollBack();
				CloseDbConnect ();
                UnityEditor.EditorUtility.DisplayDialog("导入数据失败", "Data Error :" + tempSqlDataItem.tableName + "表格导入失败  " + ex.ToString().Replace("\n", ","), "OK");

				return;
			}
			currentProgress ++;
		}
		else
		{
			_tablePoint ++;
			_sqlStrPoint = 0;
			tempSqlDataItem = null;
		}
	}

	public float GetCurrentProgress()
	{
		return currentProgress;
	}

	public float GetTotoalProgress()
	{
		return totalProgress;
	}

	public int GetTableNum()
	{
		return _tableNum;
	}

	protected SqlDataItem BuildSqlDataItem(string tableName)
	{
		SqlDataItem sqlDataItem = new SqlDataItem(tableName);
		sqlStrList.Add(sqlDataItem);
		
		return sqlDataItem;
	}
	
	protected void SetMsg(string msg)
	{
		_showMsgFlag = true;
		_msg = msg;
		Debug.LogError (msg);
	}

	public void ResetMsg()
	{
		_showMsgFlag = false;
		_msg = "";
	}

	public string GetMsgStr()
	{
		return _msg;
	}

	public bool GetShowMsgFlag()
	{
		return _showMsgFlag;
	}

	/// <summary>
	/// Inits the data base.
	/// </summary>
	private void InitDataBase()
	{
		if(_m_databaseAccess == null)
			_m_databaseAccess = new DataBaseAccess("data source="+Application.dataPath + "/StreamingAssets/database.db");
	}
	/// <summary>
	/// Closes the db connect.
	/// </summary>
	private void CloseDbConnect()
	{
		if(_m_databaseAccess != null)
		{
			_m_databaseAccess.TransactionDispose();
			_m_databaseAccess.CloseDataBase();
			_m_databaseAccess = null;
		}	
	}
	protected string BuildSqlStrByList(SQL_MODE mode,string tableName,string[] variableNameArray,string[] variableValueArray,string[] dataTypeArray = null)
	{
		string sqlStr = "";
		switch(mode)
		{
		case SQL_MODE.MODE_INSERT:
		{
			sqlStr = "INSERT INTO " + tableName + "("  ;
			for(int i = 0; i < variableNameArray.Length; i++)
			{
				sqlStr += variableNameArray[i];
				if(i < variableNameArray.Length - 1)
					sqlStr += ",";
			}
			sqlStr += ") VALUES (";
			for(int i = 0; i < variableValueArray.Length; i++)
			{
				sqlStr += "'" + variableValueArray[i] + "'";
				if(i < variableValueArray.Length - 1)
					sqlStr += ",";
			}
			sqlStr += ")";
			break;
		}
		case SQL_MODE.MODE_DROP_TABLE:
		{
			sqlStr = "DROP TABLE IF EXISTS " + tableName;
			break;
		}
		case SQL_MODE.MODE_CREATE_TABLE:
		{
			sqlStr = "CREATE TABLE " + tableName + "(" + variableNameArray[0] + " " + dataTypeArray[0] + " PRIMARY KEY,";
			
			for(int i = 1; i < variableNameArray.Length; i++)
			{
				sqlStr += variableNameArray[i] + " " + dataTypeArray[i] + ",";
			}
			
			sqlStr = sqlStr.Substring(0,sqlStr.Length - 1);
			sqlStr += ")";
			
			break;
		}
		}
		
		return sqlStr;
	}

	protected string BuildSqlStrByDic(SQL_MODE mode,string tableName,Dictionary<string,string> variableTypeDic,Dictionary<string,string> variableValueDic)
	{
		string sqlStr = "";
		switch(mode)
		{
		case SQL_MODE.MODE_INSERT:
		{
			sqlStr = "INSERT INTO " + tableName + "("  ;
			foreach(string name in variableValueDic.Keys)
			{
				sqlStr += name + ",";
			}

			sqlStr = sqlStr.Substring(0,sqlStr.Length - 1);
			sqlStr += ") VALUES (";
			foreach(string value in variableValueDic.Values)
			{
				sqlStr += "'" + value + "',";
			}

			sqlStr = sqlStr.Substring(0,sqlStr.Length - 1);
			sqlStr += ")";
			break;
		}
		case SQL_MODE.MODE_DROP_TABLE:
		{
			sqlStr = "DROP TABLE IF EXISTS " + tableName;
			break;
		}
		case SQL_MODE.MODE_CREATE_TABLE:
		{
			sqlStr = "CREATE TABLE " + tableName + "(";

			foreach(KeyValuePair<string,string> data in variableTypeDic)
			{
				sqlStr += data.Key + " " + data.Value;
				if(data.Key.ToLower() == SHEET_PRIMERY_KEY)
					sqlStr += " PRIMARY KEY,";
				else
					sqlStr += ",";
			}
						
			sqlStr = sqlStr.Substring(0,sqlStr.Length - 1);
			sqlStr += ")";
			
			break;
		}
		}
		
		return sqlStr;
	}
}
