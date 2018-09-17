#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using FrameLogicData;
using FrameEditor;
namespace GEngine.Editor
{
    class EditorViewCreateFrame : SingletonNotMono<EditorViewCreateFrame>, IEditorView
    {
        VoFrameConfigData _frameConfigData;
        private string _tempFrameName;
        private GUIContent _fieldGUIContent;
        private GUIStyle _fieldGUIStyle;
        private bool _nameSame;
        public void GenerateGUIContent()
        {
#if UNITY_EDITOR
            if (_fieldGUIContent == null)
                _fieldGUIContent = new GUIContent(EditorGUIUtility.FindTexture(IconConstContainer.ICON_DEFAULT_ASSET));
#endif
        }
        //private GUIStyle _fieldGUIStyle;
        public void GenerateGUIStyle()
        {
#if UNITY_EDITOR
            if (_fieldGUIStyle == null)
            {
                _fieldGUIStyle = new GUIStyle(EditorStyles.textField);
                _fieldGUIStyle.fontSize = 12;
                _fieldGUIStyle.fontStyle = FontStyle.Italic;
                _fieldGUIStyle.alignment = TextAnchor.MiddleLeft;
                _fieldGUIStyle.imagePosition = ImagePosition.ImageLeft;
            }
#endif
        }

        public void DrawUI(object param)
        {
            _frameConfigData = param as VoFrameConfigData;
            GenerateGUIStyle();
            if (_frameConfigData == null)
                return;
            GUILayout.BeginVertical(EditorStyles.helpBox);
            _fieldGUIContent.text = "关卡名称：";
            _tempFrameName = EditorGUILayout.TextField(_fieldGUIContent, _frameConfigData.name,EditorGuiContentStyle.Instance.fieldGUIStyle);
            if (_tempFrameName != _frameConfigData.name)
            {
                _frameConfigData.name = _tempFrameName;
                if (VoConfigDataManager.Instance.CheckConfigHasSameName(_tempFrameName))
                {
                    Debug.Log("name same " + _tempFrameName);
                    _nameSame = true;
                }
                else
                    _nameSame = false;
                //judge frame name equal 
            }
            if(_nameSame)
                EditorWindowManager.Instance.HelpBox("名字重复");
            if (GUILayout.Button("confirm ", GUILayout.Width(100), GUILayout.Height(20)))
            {
                if ((_tempFrameName == null) || _tempFrameName == ""||VoConfigDataManager.Instance.CheckConfigHasSameName(_tempFrameName))
                {
                    EditorWindowManager.Instance.ShowMsgBox("无法添加");
                }
                else
                    FrameLogic.Instance.AddFrameConfig(_frameConfigData);
            }
            GUILayout.EndVertical();
        }

        public void Enter()
        {
            _nameSame = false;
        }

        public void Exit()
        {
            _nameSame = false;
        }
    }
}
