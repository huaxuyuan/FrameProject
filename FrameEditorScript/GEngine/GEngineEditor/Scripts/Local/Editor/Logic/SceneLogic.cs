using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;
using FrameLogicData;
using ConfigData;
namespace GEngine.Editor
{
    public class SceneLogic : SingletonNotMono<SceneLogic>
    {
        public const string DEFAULT_SCENE_PATH = "Assets/Res/baseScene";
        public const string DEFAULT_SCENEBAK_PATH = "Assets/Res/editorScene";
        public  List<SceneAsset> sceneAssetList = new List<SceneAsset>();
        public  SceneAsset curSceneAsset;
        public Scene currentOpenScene;
        public void RefreshSceneAsset()
        {
            if (!Directory.Exists(DEFAULT_SCENE_PATH))
            {
                EditorUtility.DisplayDialog("RefreshSceneAsset", "Path is invalid ! ->" + DEFAULT_SCENE_PATH, "OK");
                return;
            }

            string[] files = Directory.GetFiles(DEFAULT_SCENE_PATH, "*.unity", SearchOption.AllDirectories);
            if (files == null || files.Length == 0) return;

            sceneAssetList.Clear();
            for (int i = 0; i < files.Length; i++)
            {
                string pathTemp = files[i].Replace(Application.dataPath, "Assets");
                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathTemp);
                if (sceneAsset == null) continue;

                sceneAssetList.Add(sceneAsset);
            }
        }
        public void SaveSceneFile(string frameName)
        {
            if(string.IsNullOrEmpty(frameName))
            {
                Debug.LogError("frame name error "+frameName);
                return;
            }
            if(curSceneAsset == null)
            {
                Debug.LogError("current scene asset null");
                return;
            }
            string path = AssetDatabase.GetAssetPath(curSceneAsset);
            Scene scene = EditorSceneManager.OpenScene(path);
            //EditorApplication.OpenScene(path);

            string newPath = DEFAULT_SCENEBAK_PATH + "/" + frameName + ".unity";

            EditorSceneManager.SaveScene(scene,newPath);
            currentOpenScene = EditorSceneManager.OpenScene(newPath);
        }
        public void OpenScene(FrameConfigData frameConfigData)
        {
            string newPath = DEFAULT_SCENEBAK_PATH + "/" + frameConfigData.GetConfigSceneName() + ".unity";
            currentOpenScene = EditorSceneManager.OpenScene(newPath);
        }
        public void RemoveScene()
        {
            if(currentOpenScene == null)
            {
                Debug.LogError("currentOpenScene == null");
                return;
            }
            //SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(currentOpenScene.path);
            RemoveFile(currentOpenScene.path);
            RemoveFile(currentOpenScene.path+".meta");
            //string path = Application.dataPath.Replace("Assets","") + currentOpenScene.path;
            //Debug.Log("asset path == "+path);
            //if (string.IsNullOrEmpty(path)) return;

            //if (File.Exists(path))
            //{
            //    File.Delete(path);
            //}
            
        }
        private void RemoveFile(string AssetPath)
        {
            string path = Application.dataPath.Replace("Assets", "") + AssetPath;
            //Debug.Log("asset path == " + path);
            if (string.IsNullOrEmpty(path)) return;

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
