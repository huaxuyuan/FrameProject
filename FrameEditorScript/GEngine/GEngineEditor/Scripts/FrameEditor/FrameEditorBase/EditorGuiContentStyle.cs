using GEngine.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace FrameEditor
{
    public class EditorGuiContentStyle : SingletonNotMono<EditorGuiContentStyle>
    {
        public GUIContent fieldGUIContent;
        public GUIContent litleTitleGUIContent;
        public GUIContent itemGUIContent;
        public void RegisterContent()
        {
            fieldGUIContent = new GUIContent(EditorGUIUtility.FindTexture(IconConstContainer.ICON_DEFAULT_ASSET));
            litleTitleGUIContent = new GUIContent(EditorGUIUtility.FindTexture(IconConstContainer.ICON_TOOL_TITLE));
            itemGUIContent = new GUIContent(EditorGUIUtility.FindTexture(IconConstContainer.ICON_ITEM_STAR));//ICON_ITEM_STAR
        }
        public GUIStyle fieldGUIStyle;
        public GUIStyle menuBtnStyle;
        public GUIStyle littleLabelStyle;
        public GUIStyle selectLableStyle;
        public GUIStyle inputFieldStyle;
        public GUIStyle foldOutStyle;
        public GUIStyle popUpStyle;
        public GUIStyle toogleStyle;
        public GUIStyle imageOnlyStyle;

        public float menuBtnWidth = (Screen.width - 60) / 6;
        public float defaultBtnWidth = (Screen.width - 60) / 12;
        public float _menuBtnHeight = 20;
        public float defaultRightSpace = Screen.width / 2 + 100;
        public float defaultMiddleSpace = Screen.width /4;
        public void RegisterGUIStyle()
        {
            menuBtnWidth = (Screen.width - 60) / 6;
            defaultBtnWidth = (Screen.width - 60) / 12;
            defaultRightSpace = Screen.width / 2 + 100;
            defaultMiddleSpace = Screen.width / 4 + 20;
            if (fieldGUIStyle == null)
            {
                fieldGUIStyle = new GUIStyle(EditorStyles.textField);
                fieldGUIStyle.fontSize = 12;
                fieldGUIStyle.fontStyle = FontStyle.Italic;
                fieldGUIStyle.alignment = TextAnchor.MiddleLeft;
                fieldGUIStyle.imagePosition = ImagePosition.ImageLeft;
            }
            if (imageOnlyStyle == null)
            {
                imageOnlyStyle = new GUIStyle(GUI.skin.button);
                imageOnlyStyle.imagePosition = ImagePosition.ImageOnly;
                imageOnlyStyle.fixedHeight = 50;
                imageOnlyStyle.fixedWidth = 50;
            }
            if (menuBtnStyle == null)
            {
                //基础操作菜单按钮
                menuBtnStyle = new GUIStyle(GUI.skin.button);
                menuBtnStyle.alignment = TextAnchor.MiddleCenter;
                menuBtnStyle.fontSize = 15;
                menuBtnStyle.fontStyle = FontStyle.Bold;
                menuBtnStyle.imagePosition = ImagePosition.ImageLeft;
                menuBtnStyle.normal.textColor = Color.black;
                menuBtnStyle.active.textColor = Color.white;
            }
            if (littleLabelStyle == null)
            {
                //关卡列表
                littleLabelStyle = new GUIStyle(EditorStyles.label);
                littleLabelStyle.alignment = TextAnchor.MiddleLeft;
                littleLabelStyle.fontSize = 15;
                littleLabelStyle.normal.textColor = Color.white;
                littleLabelStyle.active.textColor = Color.cyan;
                littleLabelStyle.imagePosition = ImagePosition.ImageLeft;
            }//_levelSelectItemStyle

            if (selectLableStyle == null)
            {
                //关卡列表
                selectLableStyle = new GUIStyle(EditorStyles.label);
                selectLableStyle.alignment = TextAnchor.MiddleLeft;
                selectLableStyle.fontSize = 15;
                selectLableStyle.normal.textColor = Color.red;
                selectLableStyle.active.textColor = Color.cyan;
                selectLableStyle.imagePosition = ImagePosition.ImageLeft;
            }//_levelSelectItemStyle
            if (inputFieldStyle == null)
            {
                //输入框
                inputFieldStyle = new GUIStyle(EditorStyles.textField);
                inputFieldStyle.fontSize = 15;
            }
            if (foldOutStyle == null)
            {
                foldOutStyle = new GUIStyle(EditorStyles.foldout);
                foldOutStyle.fontSize = 15;
                //_foldOutStyle.fontStyle = FontStyle.Bold;
                foldOutStyle.alignment = TextAnchor.MiddleLeft;
                foldOutStyle.imagePosition = ImagePosition.ImageLeft;
            }

            if (popUpStyle == null)
            {
                //下拉菜单
                popUpStyle = new GUIStyle(EditorStyles.popup);
                popUpStyle.alignment = TextAnchor.MiddleLeft;
                popUpStyle.fontSize = 15;
                popUpStyle.fixedHeight = 20;
            }
            if (toogleStyle == null)
            {
                toogleStyle = new GUIStyle(EditorStyles.label);
                popUpStyle.alignment = TextAnchor.MiddleLeft;
                toogleStyle.imagePosition = ImagePosition.ImageLeft;
                toogleStyle.fontSize = 15;
                toogleStyle.fixedHeight = 50;
                toogleStyle.fixedWidth = 50;
            }

        }

    }
}
