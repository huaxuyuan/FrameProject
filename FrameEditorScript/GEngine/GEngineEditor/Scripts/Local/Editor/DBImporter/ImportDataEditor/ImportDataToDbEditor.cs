using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ImportDataToDbEditor : EditorWindow {

	private enum FILE_TYPE
	{
		TXT,
		XLSX,
	}

	public const string PLAYERPREFS_KEY_IMPORT_DATA_PATH = "import_editor_path";

	private string _basePath = "E:\\Excel";


	private bool _selectAllFlag = false;

	private string _selectButtonText = "Select All";
	private string _progressText;
	private int _buttonClickCount = 0;

	private static FILE_TYPE fileType = FILE_TYPE.TXT;

	private Vector2 _scrollPos;

//	[MenuItem("Import Data/ Import Txt To DB")]
//	static void AddTxtWindows()
//	{
//		ImportDataToDbEditor window = EditorWindow.GetWindow(typeof(ImportDataToDbEditor)) as ImportDataToDbEditor;
//		window.Show();
//
//		fileType = FILE_TYPE.TXT;
//	}
	[MenuItem("Import Data/ Import Excel To DB")]
	static void AddExcelWindows()
	{
		ImportDataToDbEditor window = EditorWindow.GetWindow(typeof(ImportDataToDbEditor)) as ImportDataToDbEditor;
		window.Show();

		fileType = FILE_TYPE.XLSX;
	}

	#region EditorWindow Cycle Life
	void Awake()
	{
		GetConfigPathFromPlayerPrefs ();
	}

	void Update()
	{
		if(fileType == FILE_TYPE.TXT)
		{
			if(ImportTxtFileLogic.Instance.IsStartWriteDataToDb())
			{
				ImportTxtFileLogic.Instance.ExecuteSqlOp();
			}
		}
		else
		{
			if(ImportExcelFileLogic.Instance.IsStartWriteDataToDb())
			{
				ImportExcelFileLogic.Instance.ExecuteSqlOp();
			}
		}

	}

	void OnGUI()
	{
		if(fileType == FILE_TYPE.TXT) ImportTxtEditorGUILayout ();
		else ImportExcelEditorGUILayout ();
	}

	void OnDestroy()
	{
		SetConfigPathToPlayerPrefs();
	}

	#endregion

	#region GUI Layout Logic

	private void ImportTxtEditorGUILayout()
	{
		if(ImportTxtFileLogic.Instance.GetShowMsgFlag())
		{
			EditorGUILayout.LabelField("Error     Info  : ",ImportTxtFileLogic.Instance.GetMsgStr()) ;

			if(GUILayout.Button("Reset Tool")) ImportTxtFileLogic.Instance.ResetMsg();
		}
		else
		{
			float totalProgress = ImportTxtFileLogic.Instance.GetTotoalProgress();
			bool isStartWriteDb = ImportTxtFileLogic.Instance.IsStartWriteDataToDb();
			if(isStartWriteDb && totalProgress != 0f)
			{
				float currentProgress = ImportTxtFileLogic.Instance.GetCurrentProgress();
				float tableNum = ImportTxtFileLogic.Instance.GetTableNum();
				EditorGUILayout.LabelField("Table       Num : ", tableNum.ToString()) ;
				EditorGUILayout.LabelField("Sql Toatal  Num : ", totalProgress.ToString()) ;
				EditorGUILayout.LabelField("Sql Current Num : ",currentProgress.ToString()) ;
				EditorGUILayout.LabelField("Progress        : ",(currentProgress * 100 / totalProgress).ToString("F2") + " %") ;
			}
			else
			{
				_basePath = (string)EditorGUILayout.TextField("Base Path: ",_basePath) ;
								
				if(GUILayout.Button("Show All Files")) ImportTxtFileLogic.Instance.CheckAllFiles(_basePath,"*.txt");

				if(ImportTxtFileLogic.Instance.allFiles != null && ImportTxtFileLogic.Instance.allFiles.Length > 0)
				{
					if(GUILayout.Button(_selectButtonText))
					{
						if((_buttonClickCount ++) % 2 == 0)
						{
							_selectAllFlag = true;
							_selectButtonText = "Cancel All Select";
						}	
						else
						{	
							_selectAllFlag = false;
							_selectButtonText = "Select All";
						}

						ImportTxtFileLogic.Instance.SetSwitchListState(_selectAllFlag);
					}
					
					if(GUILayout.Button("Import")) ImportTxtFileLogic.Instance.StartImportData();
					
					_scrollPos = GUILayout.BeginScrollView (_scrollPos, GUILayout.Width (500), GUILayout.Height (500));
					
					for(int i = 0; i < ImportTxtFileLogic.Instance.allFiles.Length; i ++)
					{
						ImportTxtFileLogic.Instance.fileImportSwitchList[i] = GUILayout.Toggle(ImportTxtFileLogic.Instance.fileImportSwitchList[i],ImportTxtFileLogic.Instance.allFiles[i].Replace(_basePath + Path.DirectorySeparatorChar,""));
					}
					
					GUILayout.EndScrollView ();
				}
			}
		}
	}

	private void ImportExcelEditorGUILayout()
	{
		if(ImportExcelFileLogic.Instance.GetShowMsgFlag())
		{
			EditorGUILayout.LabelField("Error     Info  : ",ImportExcelFileLogic.Instance.GetMsgStr()) ;

			if(GUILayout.Button("Reset Tool")) ImportExcelFileLogic.Instance.ResetMsg();
		}
		else
		{
			float totalProgress = ImportExcelFileLogic.Instance.GetTotoalProgress();
			bool isStartWriteDb = ImportExcelFileLogic.Instance.IsStartWriteDataToDb();
			if(isStartWriteDb && totalProgress != 0f)
			{
				float currentProgress = ImportExcelFileLogic.Instance.GetCurrentProgress();
//				float tableNum = ImportExcelFileLogic.Instance.GetTableNum();
//				EditorGUILayout.LabelField("Table       Num : ", tableNum.ToString()) ;
				EditorGUILayout.LabelField("Sql Toatal  Num : ", totalProgress.ToString()) ;
				EditorGUILayout.LabelField("Sql Current Num : ",currentProgress.ToString()) ;
				EditorGUILayout.LabelField("Progress        : ",(currentProgress * 100 / totalProgress).ToString("F2") + " %") ;
			}
			else
			{
				_basePath = (string)EditorGUILayout.TextField("Base Path: ",_basePath) ;
				
				if(GUILayout.Button("Show All Files")) ImportExcelFileLogic.Instance.CheckAllFiles(_basePath);

				if(ImportExcelFileLogic.Instance.allFiles != null && ImportExcelFileLogic.Instance.allFiles.Length > 0)
				{
					if(GUILayout.Button(_selectButtonText))
					{
						if((_buttonClickCount ++) % 2 == 0)
						{
							_selectAllFlag = true;
							_selectButtonText = "Cancel All Select";
						}	
						else
						{	
							_selectAllFlag = false;
							_selectButtonText = "Select All";
						}

						ImportExcelFileLogic.Instance.SetSwitchListState(_selectAllFlag);
					}

					if(GUILayout.Button("Import")) ImportExcelFileLogic.Instance.StartImportData();
										
					_scrollPos = GUILayout.BeginScrollView (_scrollPos, GUILayout.Width (Screen.width), GUILayout.Height (Screen.height / 2));

					if(ImportExcelFileLogic.Instance.fileImportSwitchList.Count == ImportExcelFileLogic.Instance.allFiles.Length)
					{
						for(int i = 0; i < ImportExcelFileLogic.Instance.allFiles.Length; i ++)
						{
							ImportExcelFileLogic.Instance.fileImportSwitchList[i] = GUILayout.Toggle(ImportExcelFileLogic.Instance.fileImportSwitchList[i],ImportExcelFileLogic.Instance.allFiles[i].Replace(_basePath + Path.DirectorySeparatorChar,""));
						}
					}

					GUILayout.EndScrollView ();
				}
			}
		}
	}

	#endregion

	#region PlayerPrefs Logic
	/// <summary>
	/// Gets the config path from player prefs.
	/// </summary>
	private void GetConfigPathFromPlayerPrefs()
	{
		string path = PlayerPrefs.GetString (PLAYERPREFS_KEY_IMPORT_DATA_PATH);
		if(path != null && path != "")
			_basePath = path;
	}
	/// <summary>
	/// Sets the config path to player prefs.
	/// </summary>
	/// <param name="targetPath">Target path.</param>
	private void SetConfigPathToPlayerPrefs()
	{
		PlayerPrefs.SetString (PLAYERPREFS_KEY_IMPORT_DATA_PATH,_basePath);
	}

	#endregion
}

