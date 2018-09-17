/// <summary>
/// EPplus excel helper: Write or Read *.xlsx File
/// Created by shaozhonghui at 2015.12.03
/// Details Address:
///  http://tedgustaf.com/blog/2012/11/create-excel-20072010-spreadsheets-with-c-and-epplus/
/// </summary>

using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;

using OfficeOpenXml;
//using OfficeOpenXml.Drawing;
//using OfficeOpenXml.Drawing.Chart;
//using OfficeOpenXml.Style;

namespace GEngine.Editor
{
    public class ExcelHelper
    {
        private const char SPILT_CHAR = '^';

        private const string COPY_NAME_SYMBOL = "FileCopy_";

        /// <summary>
        /// Writes the data to excel.
        /// Only one sheet to Excel
        /// </summary>
        /// <param name="excelFilePath">Excel file path.</param>
        /// <param name="sheetName">Sheet name.</param>
        /// <param name="list">List : value is separated by symbol ',' </param>
        public static void WriteDataToExcel(string excelFilePath, string sheetName, ref List<string> list)
        {
            FileStream fs = null;
            ExcelPackage ep = null;
            try
            {
                fs = new FileStream(excelFilePath, FileMode.Create);
                ep = new ExcelPackage(fs);

                ExcelWorksheet worksheet = ep.Workbook.Worksheets.Add(sheetName);

                string[] tempArray = null;
                int count = list.Count;
                if (count > 0)
                {
                    int length = 0;
                    int colIndex = 0;
                    int rowIndex = 0;
                    do
                    {
                        tempArray = list[rowIndex].Split(SPILT_CHAR);
                        length = tempArray.Length;
                        if (length == 0)
                        {
                            rowIndex++;
                            continue;
                        }

                        colIndex = 0;
                        do
                        {
                            worksheet.Cells[rowIndex + 1, colIndex + 1].Value = tempArray[colIndex];

                            colIndex++;
                        }
                        while (colIndex < length);

                        rowIndex++;
                    }
                    while (rowIndex < count);
                }

                ep.Save();
                fs.Close();
                AssetDatabase.Refresh();
               // UnityEngine.Debug.LogError("Write Excel Success :" + excelFilePath);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Error :" + e.Message);
            }
        }
        /// <summary>
        /// Writes the data to excel.
        /// More Sheets to Excel
        /// </summary>
        /// <param name="excelFilePath">Excel file path.</param>
        /// <param name="dic">Dic.</param>
        public static void WriteDataToExcel(string excelFilePath, ref Dictionary<string, List<string>> dic)
        {
            FileStream fs = null;
            ExcelPackage ep = null;
            try
            {
                fs = new FileStream(excelFilePath, FileMode.Create);
                ep = new ExcelPackage(fs);

                string[] tempArray = null;
                int count = 0;
                int length = 0;
                int colIndex = 0;
                int rowIndex = 0;

                ExcelWorksheet worksheet = null;

                foreach (KeyValuePair<string, List<string>> data in dic)
                {
                    count = data.Value.Count;

                    if (count == 0) continue;

                    worksheet = ep.Workbook.Worksheets.Add(data.Key);

                    colIndex = 0;
                    rowIndex = 0;
                    do
                    {
                        tempArray = data.Value[rowIndex].Split(SPILT_CHAR);
                        length = tempArray.Length;
                        if (length == 0)
                        {
                            rowIndex++;
                            continue;
                        }

                        colIndex = 0;
                        do
                        {
                            worksheet.Cells[rowIndex + 1, colIndex + 1].Value = tempArray[colIndex];

                            colIndex++;
                        }
                        while (colIndex < length);

                        rowIndex++;
                    }
                    while (rowIndex < count);
                }

                ep.Save();
                fs.Close();
                AssetDatabase.Refresh();
                //UnityEngine.Debug.LogError("Write Excel Success :" + excelFilePath);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Error :" + e.Message);
            }
        }

        /// <summary>
        /// Writes the data to excel.
        /// </summary>
        /// <param name="excelFilePath">Excel file path.</param>
        /// <param name="dic">Dic.</param>
        public static void WriteDataToExcel(string excelFilePath, ref Dictionary<string, Dictionary<string, List<string>>> dic)
        {
            FileStream fs = null;
            ExcelPackage ep = null;
            try
            {
                fs = new FileStream(excelFilePath, FileMode.Create);
                ep = new ExcelPackage(fs);

                int count = 0;
                int length = 0;
                int colIndex = 0;
                int rowIndex = 0;

                ExcelWorksheet worksheet = null;

                foreach (KeyValuePair<string, Dictionary<string, List<string>>> data in dic)
                {
                    count = data.Value.Count;

                    if (count == 0) continue;

                    worksheet = ep.Workbook.Worksheets.Add(data.Key);

                    rowIndex = 0;
                    foreach (KeyValuePair<string, List<string>> info in data.Value)
                    {
                        count = info.Value.Count;
                        if (count == 0) continue;

                        colIndex = 0;
                        do
                        {
                            worksheet.Cells[rowIndex + 1, colIndex + 1].Value = info.Value[colIndex];

                            colIndex++;
                        }
                        while (colIndex < count);

                        rowIndex++;
                    }
                }

                ep.Save();
                fs.Close();
                AssetDatabase.Refresh();

                //UnityEngine.Debug.LogError("Write Excel Success :" + excelFilePath);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Error :" + e.Message);
            }
        }

        /// <summary>
        /// Reads all data from excel.
        /// </summary>
        /// <returns>The all data from excel.</returns>
        /// <param name="excelFilePath">Excel file path.</param>
        public static Dictionary<string, List<string>> ReadAllDataFromExcel(string excelFilePath)
        {
            if (!File.Exists(excelFilePath))
            {
                UnityEngine.Debug.LogError("Not Find File :" + excelFilePath);
                return null;
            }

            FileInfo fi = null;
            ExcelPackage ep = null;
            Dictionary<string, List<string>> tempDic = null;

            string copyFilePath = null;

            try
            {
                fi = new FileInfo(excelFilePath);
                ep = new ExcelPackage(fi);
            }
            catch
            {
                string fileName = Path.GetFileName(excelFilePath);
                copyFilePath = excelFilePath.Replace(fileName, COPY_NAME_SYMBOL + fileName);
                File.Copy(excelFilePath, copyFilePath, true);
                fi = new FileInfo(copyFilePath);
                ep = new ExcelPackage(fi);
            }
            finally
            {
                tempDic = new Dictionary<string, List<string>>();

                List<string> tempList = null;
                string tempStr = null;
                string tempValue = null;

                object value = null;

                foreach (ExcelWorksheet sheet in ep.Workbook.Worksheets)
                {
                    tempList = new List<string>();
                    tempDic.Add(sheet.Name, tempList);

                    for (int i = sheet.Dimension.Start.Row; i <= sheet.Dimension.End.Row; i++)
                    {
                        tempStr = "";
                        for (int j = sheet.Dimension.Start.Column; j <= sheet.Dimension.End.Column; j++)
                        {
                            value = sheet.Cells[i, j].Value;
                            tempValue = value == null ? "" : value.ToString();

                            tempStr += tempValue + SPILT_CHAR.ToString(); ;
                        }
                        if (tempStr != "")
                        {
                            tempStr = tempStr.Substring(0, tempStr.Length - 1);
                            tempList.Add(tempStr);
                        }
                    }
                }

                fi = null;
                ep.Dispose();
                ep = null;

                if (copyFilePath != null)
                {
                    File.Delete(copyFilePath);
                    copyFilePath = null;
                    AssetDatabase.Refresh();
                }
            }

            return tempDic;
        }

        /// <summary>
        /// Reads the one sheet data from excel.
        /// </summary>
        /// <returns>The one sheet data from excel.</returns>
        /// <param name="excelFilePath">Excel file path.</param>
        /// <param name="sheetName">Sheet name.</param>
        public static List<string> ReadOneSheetDataFromExcel(string excelFilePath, string sheetName)
        {
            if (!File.Exists(excelFilePath))
            {
                UnityEngine.Debug.LogError("Not Find File :" + excelFilePath);
                return null;
            }

            FileInfo fi = null;
            ExcelPackage ep = null;
            List<string> tempList = null;

            string copyFilePath = null;

            try
            {
                fi = new FileInfo(excelFilePath);
                ep = new ExcelPackage(fi);
            }
            catch
            {
                string fileName = Path.GetFileName(excelFilePath);
                copyFilePath = excelFilePath.Replace(fileName, COPY_NAME_SYMBOL + fileName);
                File.Copy(excelFilePath, copyFilePath, true);
                fi = new FileInfo(copyFilePath);
                ep = new ExcelPackage(fi);
            }
            finally
            {
                string tempStr = null;
                string tempValue = null;

                object value = null;

                foreach (ExcelWorksheet sheet in ep.Workbook.Worksheets)
                {
                    if (sheet.Name != sheetName) continue;

                    tempList = new List<string>();

                    for (int i = sheet.Dimension.Start.Row; i <= sheet.Dimension.End.Row; i++)
                    {
                        tempStr = "";
                        for (int j = sheet.Dimension.Start.Column; j <= sheet.Dimension.End.Column; j++)
                        {
                            value = sheet.Cells[i, j].Value;
                            tempValue = value == null ? "" : value.ToString();

                            tempStr += tempValue + SPILT_CHAR.ToString();

                        }
                        if (tempStr != "")
                        {
                            tempStr = tempStr.Substring(0, tempStr.Length - 1);
                            tempList.Add(tempStr);
                        }
                    }

                    break;
                }

                fi = null;
                ep.Dispose();
                ep = null;

                if (copyFilePath != null)
                {
                    File.Delete(copyFilePath);
                    copyFilePath = null;
                    AssetDatabase.Refresh();
                }
            }

            return tempList;
        }
    }
}