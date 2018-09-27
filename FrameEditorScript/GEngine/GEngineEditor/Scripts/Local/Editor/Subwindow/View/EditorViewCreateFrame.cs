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
        private EditorGuiContentStyle _editorGUIContentStyle;
        private bool _selectScene;
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
            #region FrameName
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
                if (CheckFrameNameEqual())
                {
                    EditorWindowManager.Instance.ShowMsgBox("无法添加");
                }
                else
                    FrameLogic.Instance.AddFrameConfig(_frameConfigData);
            }

            #endregion
            #region FrameScene
            _selectScene = EditorGUILayout.Foldout(_selectScene, _editorGUIContentStyle.litleTitleGUIContent, _editorGUIContentStyle.foldOutStyle);
            if(_selectScene)
            {
                foreach(SceneAsset sceneAsset in SceneLogic.Instance.sceneAssetList)
                {
                    _editorGUIContentStyle.itemGUIContent.text = sceneAsset.name;
                    if (GUILayout.Button(_editorGUIContentStyle.itemGUIContent, _editorGUIContentStyle.menuBtnStyle))
                    {

                        if (CheckFrameNameEqual())
                        {
                            EditorWindowManager.Instance.ShowMsgBox("无法添加");
                        }
                        else
                        {
                            SceneLogic.Instance.curSceneAsset = sceneAsset;
                            _frameConfigData.SceneName = sceneAsset.name;
                            SceneLogic.Instance.SaveSceneFile(_frameConfigData.frameConfigData.GetConfigSceneName());
                            
                            FrameLogic.Instance.AddFrameConfig(_frameConfigData);
                        }
                        
                    }

                }
            }
            #endregion
            GUILayout.EndVertical();
        }

        private bool CheckFrameNameEqual()
        {
            if ((_tempFrameName == null) || _tempFrameName == "" || VoConfigDataManager.Instance.CheckConfigHasSameName(_tempFrameName))
                return true;
            return false;
        }
        public void Enter()
        {
            _nameSame = false;
            SceneLogic.Instance.RefreshSceneAsset();
            _editorGUIContentStyle = EditorGuiContentStyle.Instance;
        }

        public void Exit()
        {
            _nameSame = false;
        }
    }
}
