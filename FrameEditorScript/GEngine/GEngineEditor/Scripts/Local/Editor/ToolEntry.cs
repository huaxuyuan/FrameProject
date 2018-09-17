

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using GEngine.Editor;
using MyEditorUitls;
using ScriptForBuildingBundles;
namespace GEngine.Editor
{
    public class ToolEntry
    {
        #region Private Fields

        private const string SHORT_CUT_OPEN = " %#O";
        private const string SHORT_CUT_HELP = " %#H";
        public const string VERSION = "1.01";

        private const string ICON_BG_PATH = "Assets/GEngine/Tool/Icon/logo.png";


        private MeshFilter _colliderFilter;
        private Color bgColor = new Color(40.0f / 255.0f, 42.0f / 255.0f, 57.0f / 255.0f);

        #endregion

        #region Public Fields

        public const int MENU_TYPE_NONE = -1;
        public const int MENU_TYPE_EVENT = 0;
        //public const int MENU_TYPE_CONFIG = 1;
        public const int MENU_TYPE_RESOURCE = 1;
        public const int MENU_TYPE_ONEKEY = 2;

        public static bool open = false;
        public static int showNumber;
        #endregion

        [MenuItem("GEngine/ Open" + SHORT_CUT_OPEN)]
        static void Open()
        {
            //showNumber++;
            //Debug.Log("showNumber" + showNumber);
                EditorWindowManager.Instance.RegisterWindow();
                EditorWindowManager.Instance.ShowWindow(EditorWindowManager.WINDOW_TYPE_EDITWINDOW);
        }

        [MenuItem("GEngine/ Help" + SHORT_CUT_HELP)]
        static void Help()
        {
            BundleUtils.CopyConfigData();
            string bundlePath = BundleUtils.GetBundlePathStr();
            AssetDatabase.Refresh();
            BuildAssetBundleLogic.BuildTextConfig(bundlePath, ".szpkg", EditorUserBuildSettings.activeBuildTarget, BuildAssetBundleOptions.ChunkBasedCompression);
            // Utils.OpenFile(Application.dataPath + "/Help.xlsx");
        }

        #region EditorWindow Cycle Life

        void OnEnable()
        {
            open = false;
            //if (mSceneIndex != -1)
            //    UISceneDrawer.ResetSelectSceneItem(mSceneIndex);
        }
        void OnDestroy()
        {
            open = false;
        }


        #endregion
    }
}
