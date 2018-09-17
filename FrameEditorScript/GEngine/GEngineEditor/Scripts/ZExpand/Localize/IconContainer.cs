/*
 * 
 *      Created by ZhonghuiShao
 * 
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace GEngine.Editor
{
    public class IconConstContainer
    {
        public const string ICON_TOOL_TITLE = "d_UnityEditor.SceneHierarchyWindow";
        public const string ICON_SCENE_ASSET = "SceneAsset Icon";
        public const string ICON_REFRESH = "d_Refresh";
        public const string ICON_TABLE_BAR = "Gamemanager Icon";
        public const string ICON_DEFAULT_ASSET = "DefaultAsset Icon";
        public const string ICON_DELETE = "d_TreeEditor.Trash";
        public const string ICON_EDIT = "d_EditCollider";
        public const string ICON_ADD = "d_Toolbar Plus";
        public const string ICON_LIST_DELETE = "d_Toolbar Minus";
        public const string ICON_SETTING = "d_TerrainInspector.TerrainToolSettings";
        public const string ICON_FOLDER = "Folder Icon";
        public const string ICON_TEXTASSET = "TextAsset Icon";
        public const string ICON_OUT = "SceneLoadOut";
        public const string ICON_ONEKEY = "AudioListener Icon";
        public const string ICON_START = "d_preAudioPlayOn";
        public const string ICON_ITEM_STAR = "Favorite Icon";

        // 01.26 ICON
        public const string IconNew = "Assets/GEngineEditor/Icon/btn_add.png";
        public const string IconRefresh = "Assets/GEngineEditor/Icon/btn_ref.png";
        public const string IconDelete = "Assets/GEngineEditor/Icon/btn_delete.png";
        public const string IconImport = "Assets/GEngineEditor/Icon/btn_import.png";
        public const string IconSave = "Assets/GEngineEditor/Icon/btn_save.png";
        public const string IconCube = "Assets/GEngineEditor/Icon/btn_cube.png";
        public const string IconNone = "Assets/GEngineEditor/Icon/label_none.png";
        public const string IconBuild = "Assets/GEngineEditor/Icon/btn_build.png";
        public const string IconPic = "Assets/GEngineEditor/Icon/btn_pic.png";
        public const string IconBtnNext = "Assets/GEngineEditor/Icon/btn_next.png";

        //Add by twj
        //public const string IconGreen = "Assets/GEngineEditor/Icon/tex_green.png";
        //public const string IconBlue = "Assets/GEngineEditor/Icon/tex_blue.png";
        //public const string IconBrown = "Assets/GEngineEditor/Icon/tex_brown.png";
        //public const string IconPurple = "Assets/GEngineEditor/Icon/tex_purple.png";
        //public const string IconRed = "Assets/GEngineEditor/Icon/tex_red.png";
        //public const string IconYellow = "Assets/GEngineEditor/Icon/tex_yellow.png";


        public static Color bgColor = new Color(40.0f / 255.0f, 42.0f / 255.0f, 57.0f / 255.0f);
        public static Dictionary<string, Texture2D> textureDic = new Dictionary<string, Texture2D>();
        public static void LoadTexture()
        {
            if(!textureDic.ContainsKey(IconBtnNext))
                textureDic.Add(IconBtnNext, AssetDatabase.LoadAssetAtPath<Texture2D>(IconBtnNext));
            if (!textureDic.ContainsKey(IconPic))
                textureDic.Add(IconPic, AssetDatabase.LoadAssetAtPath<Texture2D>(IconPic));

        }
    }
}
