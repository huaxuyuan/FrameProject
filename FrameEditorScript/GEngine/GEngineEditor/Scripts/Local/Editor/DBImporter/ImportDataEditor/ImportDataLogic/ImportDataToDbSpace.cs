using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Mono.Data.Sqlite;

namespace ImportDataToDbSpace
{
	public class SheetItem
	{
		public string fileName;
		public string originalSheetName;

		public Dictionary<string,string> variableTypeDic;// variable name is the key,variable type is the value
		public Dictionary<string,Dictionary<string,string>> variableValueDic;// sn is the key,valueDic is the value; variable name is the key,variable value is the value

		public SheetItem(string sheetName,string fileName)
		{
			this.fileName = fileName;
			this.originalSheetName = sheetName;

			variableTypeDic = new Dictionary<string, string> ();
			variableValueDic = new Dictionary<string, Dictionary<string, string>> ();
		}
	}

	public class SqlDataItem
	{
		public List<string> strList;
		public string tableName;
		
		public SqlDataItem(string tableName)
		{
			strList = new List<string> ();
			this.tableName = tableName;
		}
	}
	
	public class FileComparer : IComparer
	{
		int IComparer.Compare(object o1, object o2)
		{
			string path1 = (string)o1;
			string path2 = (string)o2;
			FileInfo fi1 = new FileInfo (path1);
			FileInfo fi2 = new FileInfo (path2);
			return fi2.LastWriteTime.CompareTo(fi1.LastWriteTime);
		}
	}
	
	public class DataBaseAccess
	{
		private SqliteConnection m_dbConnection;
		private SqliteCommand m_dbCommand;
		private SqliteTransaction m_dbTrans;
		public DataBaseAccess(string connectionStr)
		{
			m_dbConnection = null;
			m_dbCommand = null;
			try
			{
				m_dbConnection = new SqliteConnection(connectionStr);
				m_dbConnection.Open();
				m_dbCommand = new SqliteCommand(m_dbConnection);
				BeginTransaction();
				//			Debug.Log("connect db success !! :  "+connectionStr);
			}
			catch(Exception e)
			{
				string temp = e.ToString();
				Debug.LogError("connect db exception :"+temp);
			}
		}
		
		public void BeginTransaction()
		{
			if(m_dbConnection != null)
			{
				m_dbTrans = m_dbConnection.BeginTransaction();
				m_dbCommand.Transaction = m_dbTrans;
			}
		}
		public void TransactionCommit()
		{
			if(m_dbTrans != null)
			{
				m_dbCommand.Transaction = null;
				m_dbTrans.Commit();
			}
		}
		public void TransactionDispose()
		{
			if(m_dbTrans != null)
			{
				m_dbTrans.Dispose();
			}
		}
		public void TransactionRollBack()
		{
			if(m_dbTrans != null)
			{
				m_dbTrans.Rollback();
			}
		}
		
		public void CloseDataBase()
		{
			if(m_dbCommand != null)
			{
				m_dbCommand.Dispose();
			}
			m_dbCommand = null;
			
			if(m_dbConnection != null)
			{
				m_dbConnection.Dispose();
			}
			m_dbConnection = null;
			
		}
		
		public void ExecuteQuery(string sqlQuery, ref DataBaseReader reader)
		{
			if(m_dbCommand == null)
			{
				m_dbCommand = new SqliteCommand(m_dbConnection);
			}
			
			m_dbCommand.CommandText = sqlQuery;
			SqliteDataReader m_dbReader = m_dbCommand.ExecuteReader();
			reader.SetReader( m_dbReader);
		}
	}
	
	public class DataBaseReader
	{
		private	SqliteDataReader m_dbReader;
		public DataBaseReader()
		{
			m_dbReader = null;
		}
		public void SetReader( SqliteDataReader data)
		{
			m_dbReader = data;
		}
		
		public int GetIntValue( string str)
		{
			//		Debug.LogError (str);
			return  m_dbReader.GetInt32(m_dbReader.GetOrdinal(str));
		}
		
		public float GetFloatValue( string str)
		{	
			return  m_dbReader.GetFloat(m_dbReader.GetOrdinal(str));
		}
		
		public double GetDoubleValue( string str)
		{
			return  m_dbReader.GetDouble(m_dbReader.GetOrdinal(str));
		}
		
		public string GetStringValue( string str)
		{ 
			return m_dbReader.GetString(m_dbReader.GetOrdinal(str));
		}
		public bool IsNotEnd()
		{
			return m_dbReader.Read();
		}
		
		public void clearData()
		{
			if(m_dbReader != null)
			{
				m_dbReader.Dispose();
			}
			m_dbReader = null;	
		}
	}
}