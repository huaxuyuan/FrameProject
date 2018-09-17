/*
 * 
 *      Created by ZhonghuiShao
 * 
 */

using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;


namespace GEngine.Editor
{
    class Utils
    {
        /// <summary>
        /// 打开场景
        /// </summary>
        /// <param name="sceneName">场景路径</param>
        /// <param name="isSaveLast">是否保存当前场景</param>
        /// 
        public static UnityEngine.SceneManagement.Scene sceneData;
        public static bool OpenScene(string scenePath, bool isSaveCur = true)
        {
            if (isSaveCur && sceneData != null) UnityEditor.SceneManagement.EditorSceneManager.SaveScene(sceneData);
            //Debug.LogError("scenePath" + scenePath);
            sceneData = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath);
            return (sceneData != null);
        }
        public static void SaveScene()
        {
                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(sceneData);
        }
        public static void ShowTips(string title,string content,string btnName,bool isDialog = true)
        {
            if (isDialog)
            {
                EditorUtility.DisplayDialog(title,content, btnName);
            }
            else
            {
                Debug.LogError(title + " => " + content);
            }
        }

        /// <summary>
        /// 打开指定的文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void OpenFile(string filePath)
        {
            System.Diagnostics.Process.Start(filePath);
        }


        public static void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
