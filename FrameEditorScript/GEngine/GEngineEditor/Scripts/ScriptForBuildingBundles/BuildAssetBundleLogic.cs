using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MyEditorUitls;
using System.IO;

namespace ScriptForBuildingBundles
{
    public class BuildAssetBundleLogic
    {
        public static void BuildTextConfig(string bundlePath, string suffix, BuildTarget target, BuildAssetBundleOptions options)
        {

            BundleUtils.ClearAllAssetsLabels();
            JsonResourceSerializeHelper.DeserializeResourceMD5();
            ResourceExcelHelper resourceExcelHelper = new ResourceExcelHelper();
            resourceExcelHelper.InitializeResourceData("ResourceMobile.xlsx");
            string _abSuffix = string.IsNullOrEmpty(suffix) ? ".szpkg" : suffix;
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> dicExcel = new Dictionary<string, List<string>>();
            string assetBundleName;

            string gameObjectPath = Application.dataPath + "/Res/tempConfig";
            string[] arrStrPath = Directory.GetFiles(gameObjectPath, "*", SearchOption.AllDirectories);
            List<Object> gamePrefabObjList = new List<Object>();
            foreach (string path in arrStrPath)
            {
                //Debug.Log("game prefab path == " + path);
                string strTempPath = path.Replace(@"\", "/");
                //截取我们需要的路径
                strTempPath = strTempPath.Substring(strTempPath.IndexOf("Assets"));
                //根据路径加载资源
                Object objTemp = AssetDatabase.LoadAssetAtPath(@strTempPath, typeof(Object));

                 gamePrefabObjList.Add(objTemp);
                
            }

            foreach (Object obj in gamePrefabObjList)
            {
                Debug.Log(obj);
                if (!(obj is TextAsset))
                    continue;
                string name = obj.name.ToLower();
                //BehaviorDesigner.Runtime.ExternalBehavior prefab = obj as BehaviorDesigner.Runtime.ExternalBehavior;
                TextAsset prefab = obj as TextAsset;
                string assetPath = AssetDatabase.GetAssetPath(prefab);
                string[] assetArray = assetPath.Split('/');
                assetBundleName = assetArray[assetArray.Length - 2].ToLower() + _abSuffix;

                List<string> pathArray;
                if (!dic.ContainsKey(assetBundleName))
                {
                    pathArray = new List<string>();
                    dic.Add(assetBundleName, pathArray);
                }
                List<string> prefabArray;
                if (!dicExcel.ContainsKey(assetBundleName))
                {
                    prefabArray = new List<string>();
                    dicExcel.Add(assetBundleName, prefabArray);
                }
                pathArray = dic[assetBundleName];
                prefabArray = dicExcel[assetBundleName];
                pathArray.Add(assetPath);
                prefabArray.Add(prefab.name);
               BundleUtils.SetAssetsLabel(assetPath, assetArray[assetArray.Length - 2].ToLower() + _abSuffix);
                //Debug.Log("path "+assetPath+" assetbundle name "+ assetArray[assetArray.Length - 2].ToLower());
            }
            if (dic.Count == 0)
                return;
            AssetBundleBuild[] bundleMap = new AssetBundleBuild[dic.Count];
            int index = 0;
            string[] pathData = new string[0];
            foreach (KeyValuePair<string, List<string>> pairValue in dic)
            {
                bundleMap[index].assetBundleName = pairValue.Key;
                pathData = new string[pairValue.Value.Count];
                for (int pathIndex = 0; pathIndex < pathData.Length; ++pathIndex)
                {
                    pathData[pathIndex] = pairValue.Value[pathIndex];
                }
                bundleMap[index].assetNames = pathData;
                index++;
            }

            BundleUtils.StartBuildAssetBundles(bundlePath, bundleMap, target, options);
            BundleUtils.DeleteNoUseFile(bundlePath, suffix);

            foreach (KeyValuePair<string, List<string>> pairValue in dicExcel)
            {
                ResourceData resourceData = resourceExcelHelper.GetResourceData(pairValue.Key);
                resourceData.resourceType = "Config";
                resourceData.version = BundleUtils.GetFileHash(bundlePath + "/" + pairValue.Key); ; ;
                if (pairValue.Key.Contains("common"))
                {
                    resourceData.commonResource = 1;
                }
                else
                    resourceData.commonResource = 0;
                resourceData.resourceArray = new List<string>();

                for (int pathIndex = 0; pathIndex < pathData.Length; ++pathIndex)
                {
                    resourceData.resourceArray.Add(pairValue.Value[pathIndex]);
                    Debug.Log("pair value " + pairValue.Value[pathIndex]);
                }
                JsonResourceSerializeHelper.AddPackageAndMD5(pairValue.Key, resourceData.version, resourceData.resourceArray);
            }
            resourceExcelHelper.WriteData();
            JsonResourceSerializeHelper.SeriablizeResourceMD5();
            BundleUtils.ClearAllAssetsLabels();
        }

    }
}
