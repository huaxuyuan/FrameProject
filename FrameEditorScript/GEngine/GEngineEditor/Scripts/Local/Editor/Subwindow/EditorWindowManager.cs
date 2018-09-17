
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using System.Collections.Generic;
namespace GEngine.Editor
{
    public class EditorWindowManager : SingletonNotMono<EditorWindowManager>
    {
        public const int WINDOW_TYPE_EDITWINDOW = 1;
        public const int WINDOW_TYPE_CREATE_FRAME = 2;
        public const int WINDOW_TYPE_FRAMECONFIG = 3;

        public const string APP_VERSION = "1.0";

        
        public class WindowVariable
        {
            public Type windowType;
            public GUIContent content;
        }
        private Dictionary<int, WindowVariable> _editorWindowDic = new Dictionary<int, WindowVariable>();

        private List<int> _showWindowList = new List<int>();
        public void RegisterWindow()
        {
            if(_editorWindowDic.Count > 0)
            {
                return;
            }
            // window.Close();
            WindowVariable windowVariable = new WindowVariable();
            windowVariable.windowType = typeof(EditorWindowFrame);
            windowVariable.content = new GUIContent(" 关卡编辑工具", EditorGUIUtility.FindTexture(IconConstContainer.ICON_TOOL_TITLE));

            _editorWindowDic.Add(WINDOW_TYPE_EDITWINDOW, windowVariable);

            windowVariable = new WindowVariable();
            windowVariable.windowType = typeof(EditorWindowCreateFrame);
            windowVariable.content = new GUIContent("创建关卡", EditorGUIUtility.FindTexture(IconConstContainer.ICON_TOOL_TITLE));
            _editorWindowDic.Add(WINDOW_TYPE_CREATE_FRAME, windowVariable);

        }
        public void ShowWindow(int windowType)
        {
            Debug.Log("show window count "+_showWindowList.Count);
            if(_editorWindowDic.ContainsKey(windowType))
            {
                _showWindowList.Add(windowType);
                WindowVariable windowVariable = _editorWindowDic[windowType];
                EditorWindow window = EditorWindow.GetWindow(windowVariable.windowType);
                window.titleContent = windowVariable.content;
                window.Show();
            }
           
        }
        public void HideWindow(int windowType)
        {
            if (!_showWindowList.Contains(windowType))
            {
                return;
            }
            if (_editorWindowDic.ContainsKey(windowType))
            {

                WindowVariable windowVariable = _editorWindowDic[windowType];
                EditorWindow window = EditorWindow.GetWindow(windowVariable.windowType);
                window.Close();
            }
        }

        public bool ShowMsgBox(string msgInfo)
        {
            return EditorUtility.DisplayDialog("异常", msgInfo, "OK");
        }
        public void HelpBox(string msgInfo)
        {
            EditorGUILayout.HelpBox(msgInfo, MessageType.Warning);
        }
        public void ClearAllWindowVariable()
        {
            _showWindowList.Clear();
        }
    }
}
