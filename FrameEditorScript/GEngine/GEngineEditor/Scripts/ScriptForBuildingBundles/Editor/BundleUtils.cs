using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

namespace MyEditorUitls
{

	public class BundleUtils 
	{
        //#if UNITY_ANDROID
        //        public const string BUNDLES_BASE_PATH = "/StreamingAssets/AndroidResource/";
        //#elif UNITY_IPHONE
        //        public const string BUNDLES_BASE_PATH = "/StreamingAssets/IOSResource/";
        //#else
        //        public const string BUNDLES_BASE_PATH = "/StreamingAssets/AndroidResource/";
        //#endif
        //        private const string MANIFEST_FILE_FILTER = "*.manifest";
        //        private const string META_EXTENSION = ".meta";

        //        private static List<GameObject> _prefabList = new List<GameObject>(); 

        //        /// <summary>
        //        /// Gets the prefab.
        //        /// </summary>
        //        /// <returns>The prefab.</returns>
        //        /// <param name="go">Go.</param>
        //        /// <param name="name">Name.</param>
        //        public static GameObject CreatePrefab(GameObject go, string name)
        //		{
        //			Object tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/" + name + ".prefab");
        //			tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);
        //			Object.DestroyImmediate(go);

        //			return (GameObject)tempPrefab;
        //		}

        //		/// <summary>
        //		/// before your mark ,remove all assetbundleName in assetbundle database
        //		/// </summary>
        //		public static void RemoveAllAssetBundleNames()
        //		{
        //			string[] bundleNamesArray = AssetDatabase.GetAllAssetBundleNames ();
        //			foreach(string bundleName in bundleNamesArray)
        //				AssetDatabase.RemoveAssetBundleName(bundleName,true);
        //		}

        //		/// <summary>
        //		/// Marks the asset to bundle.
        //		/// </summary>
        //		/// <param name="assetPath">Asset path.</param>
        //		/// <param name="bundleName">Bundle name.</param>
        //		public static void MarkAssetToBundle(string assetPath, string bundleName)
        //		{
        //			AssetImporter importer = AssetImporter.GetAtPath(assetPath);
        //			if (importer == null)
        //				return;

        //			importer.assetBundleName = bundleName;
        //		}

        //        /// <summary>
        //        /// Delete mainifest files.
        //        /// </summary>
        //        public static void DeleteManifestFile()
        //        {
        //            string[] tempArray = Directory.GetFiles(Application.dataPath + BUNDLES_BASE_PATH, MANIFEST_FILE_FILTER, SearchOption.AllDirectories);
        //            int length = tempArray.Length;
        //            if (length == 0) return;

        //            string tempFilePath = null;
        //            int index = 0;
        //            do
        //            {
        //                tempFilePath = tempArray[index];

        //                File.Delete(tempFilePath);
        //                File.Delete(tempFilePath + META_EXTENSION);

        //                index++;
        //            }
        //            while (index < length);

        //            AssetDatabase.Refresh();
        //        }

        //        /// <summary>
        //        /// Export assetbundle.
        //        /// </summary>
        //        /// <param name="outPath"></param>
        //        /// <param name="target"></param>
        //        /// <param name="option"></param>
        //        public static void ExportBundles(BuildTarget target = BuildTarget.Android, string outPath = BUNDLES_BASE_PATH, BuildAssetBundleOptions option = BuildAssetBundleOptions.None)
        //        {
        //            outPath = Application.dataPath + outPath;
        //            if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);

        //            BuildPipeline.BuildAssetBundles(outPath, option, target);
        //        }

        //        /// <summary>
        //        /// Collect temp prefabs wen building prefabs.
        //        /// </summary>
        //        /// <param name="prefab"></param>
        //        public static void CollectPrefabs(GameObject prefab)
        //        {
        //            _prefabList.Add(prefab);
        //        }

        //        /// <summary>
        //        /// Destory temp prefabs after building bundles.
        //        /// </summary>
        //        public static void DestroyTempPrefabs()
        //        {
        //            if (_prefabList.Count == 0) return;

        //            foreach (var prefab in _prefabList)
        //            {
        //                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(prefab));
        //            }

        //            _prefabList.Clear();
        //        }
        public static void CopyConfigData()
        {
            string copyPath = GetConfigPath();
            string destinationPath = Application.dataPath + "/Res/tempConfig";
            CopyDir(copyPath, destinationPath);
        }
        private static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加
                if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                {
                    aimPath += System.IO.Path.DirectorySeparatorChar;
                }
                // 判断目标目录是否存在如果不存在则新建
                if (!System.IO.Directory.Exists(aimPath))
                {
                    System.IO.Directory.CreateDirectory(aimPath);
                }
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles（srcPath）；
                string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    Debug.Log("file path " + file);
                    if (System.IO.Directory.Exists(file))
                    {
                        CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                    }
                    // 否则直接Copy文件
                    else
                    {
                        System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file), true);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    

        public static string GetConfigPath()
        {
            string directoryName = Path.GetDirectoryName(Application.dataPath);
            string[] directoryArray = directoryName.Split('/');
            directoryName = directoryName.Replace(directoryArray[directoryArray.Length - 1], "");
            directoryName += "FrameConfig/";
            return directoryName;
        }
        /// <summary>
        /// 清空Unity工程中所有AssetsLabel
        /// </summary>
        public static void ClearAllAssetsLabels()
        {
            string[] bundleNamesArray = AssetDatabase.GetAllAssetBundleNames();
            foreach (string bundleName in bundleNamesArray)
                AssetDatabase.RemoveAssetBundleName(bundleName, true);
        }

        /// <summary>
        /// 为资源指定AssetsLabel
        /// </summary>
        /// <param name="assetFilePath">资源路径</param>
        /// <param name="bundleName">名字</param>
        public static void SetAssetsLabel(string assetFilePath, string bundleName)
        {
            AssetImporter importer = AssetImporter.GetAtPath(assetFilePath);
            if (importer == null)
                return;

            importer.assetBundleName = bundleName;
        }

        /// <summary>
        /// 启动Assetbundle输出逻辑
        /// </summary>
        /// <param name="outPath">输出路径</param>
        /// <param name="target">输出平台</param>
        /// <param name="options">设置选项</param>
        public static void StartBuildAssetBundles(string outPath, BuildTarget target, BuildAssetBundleOptions options)
        {
            if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);

            BuildPipeline.BuildAssetBundles(outPath, options, target);

            AssetDatabase.Refresh();
        }

        public static void StartBuildAssetBundles(string outPath, AssetBundleBuild[] builds, BuildTarget target, BuildAssetBundleOptions options)
        {
            if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);
            for (int index = 0; index < builds.Length; ++index)
            {
                Debug.Log(" " + builds[index].assetBundleName);
            }
            BuildPipeline.BuildAssetBundles(outPath, builds, options, target);

            AssetDatabase.Refresh();
        }

        public static void DeleteNoUseFile(string folderPath, string pattern)
        {
            if (!Directory.Exists(folderPath)) return;

            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
            if (files == null || files.Length == 0) return;

            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].EndsWith(pattern))
                {
                    File.Delete(files[i]);
                }
            }

            AssetDatabase.Refresh();

            string shellPath = Application.dataPath.Replace("Assets", "Shell/");
            UnityRunShell(shellPath + "copyAssetbundle.sh");
        }
        private static void UnityRunShell(string shellFile)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = shellFile;
            //		process.StartInfo.Arguments = "";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.ErrorDialog = true;
            process.StartInfo.CreateNoWindow = false;
            try
            {
                process.Start();
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(" " + e.Message);
            }

            process.WaitForExit();
            process.Close();
        }
        public static string GetFileHash(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    return string.Empty;
                }
                var md5 = System.Security.Cryptography.MD5.Create();
                var stream = File.OpenRead(fileName);
                byte[] hash = md5.ComputeHash(stream);
                System.Text.StringBuilder strHash = new System.Text.StringBuilder();
                for (int j = 0; j < hash.Length; ++j)
                {
                    strHash.Append(hash[j].ToString("x2"));
                }
                stream.Close();
                stream.Dispose();
                return strHash.ToString();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            return string.Empty;
        }

        public static string GetBundlePathStr()
        {
            string bundlePath = Application.streamingAssetsPath;
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                bundlePath = Application.streamingAssetsPath + "/Android";
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                bundlePath = Application.streamingAssetsPath + "/IOS";
            }
            return bundlePath;
        }

    }
}