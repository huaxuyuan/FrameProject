using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class Utility
{
    public static void WriteFileLogic(string filePath, string fileName, string info)
    {
        string byteFileName = filePath + fileName;
        Debug.Log("WriteFileLogic "+byteFileName);
        if (!File.Exists(byteFileName))
        {
            File.WriteAllText(byteFileName, info);
        }
        else
        {
            File.Delete(byteFileName);
            File.WriteAllText(byteFileName, info);
        }
    }
    public static void RemoveFileLogic(string filePath, string fileName)
    {
        string byteFileName = filePath + fileName;
        if (!File.Exists(byteFileName))
        {
            return;
        }
        File.Delete(byteFileName);
          
        
    }
    public static string ReadFileLogic(string filePath, string fileName)
    {
        string byteFileName = filePath + fileName;
        UnityEngine.Debug.Log("read file logic "+byteFileName);
        if(File.Exists(byteFileName))
        {
            return File.ReadAllText(byteFileName);
        }
        return "";
    }
}

