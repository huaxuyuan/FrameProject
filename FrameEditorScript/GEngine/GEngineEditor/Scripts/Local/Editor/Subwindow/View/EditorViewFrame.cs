using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ConfigData;
using FrameLogicData;
using FrameEditor;
namespace GEngine.Editor
{
    class EditorViewFrame :  SingletonNotMono<EditorViewFrame>, IEditorView
    {
        FrameLogic _frameLogic;
        EditorFrameDetailView _editorFrameDetailView;
        EditorFrameConfigView _editorFrameConfigView;
        EditorGuiContentStyle _editorGUIContentStyle;
        public void Enter()
        {
            //Debug.Log("EditorViewFrame open");
            _frameLogic = FrameLogic.Instance;
            _editorGUIContentStyle = EditorGuiContentStyle.Instance;
            _editorFrameDetailView = new EditorFrameDetailView();
            _editorFrameConfigView = new EditorFrameConfigView();
            _frameLogic.InitializeFrameLogic();
            IconConstContainer.LoadTexture();
            GenerateGUIContent();
            _editorGUIContentStyle.RegisterContent();
        }

        public void Exit()
        {
            EditorWindowManager.Instance.ClearAllWindowVariable();
            _frameLogic.ExitFrameLogic();
        }

        #region GUIContent
        private GUIContent _newBtnGUIContent;
        private GUIContent _editBtnGUIConent;
        private GUIContent _playBtnGUIContent;
        private GUIContent _saveBtnGUIContent;
        private GUIContent _resetBtnGUIContent;
        private GUIContent _recordBtnGUIContent;
        private GUIContent _copyBtnGUIContent;
        private GUIContent _buildBtnGUIContent;
        private GUIContent _deleteBtnGUIContent;
        private GUIContent _searchBtnGUIContent;
        private GUIContent _obstacleBtnGUIContent;

        private GUIContent _litleTitleGUIContent;
        private GUIContent _levelItemGUIContent;
        private GUIContent _foldOutGUIContent;
        //private GUIContent[] _colorsGUIContent;
        private GUIContent _levelTargetGUIContent;
        private GUIContent _fieldGUIContent;
       

        public void GenerateGUIContent()
        {
            // load texture
            //Texture2D newAddTex = AssetDatabase.LoadAssetAtPath<Texture2D>(IconConstContainer.IconNew);
            //Texture2D newDeleteTex = AssetDatabase.LoadAssetAtPath<Texture2D>(IconConstContainer.IconDelete);
            Texture2D btnCubeTex = AssetDatabase.LoadAssetAtPath<Texture2D>(IconConstContainer.IconCube);
            //_newBtnGUIContent = new GUIContent(" 新建", newAddTex, "点击按钮，创建新的场景剧情");
            //_deleteBtnGUIContent = new GUIContent("删除", newDeleteTex, "点击按钮，删除场景剧情");
            //_editSceneGUIConent = new GUIContent("编辑", EditorGUIUtility.FindTexture(IconConstContainer.ICON_EDIT), "点击按钮，控制路径设置区域的展开，收起！");
            //_litleTitleGUIContent = new GUIContent("关卡列表", EditorGUIUtility.FindTexture(IconConstContainer.ICON_TOOL_TITLE));
            _levelItemGUIContent = new GUIContent(btnCubeTex);
            //Texture2D iconGreenTex = AssetDatabase.LoadAssetAtPath<Texture2D>(IconConstContainer.IconGreen);
            //Texture2D iconBlueTex = AssetDatabase.LoadAssetAtPath<Texture2D>(IconConstContainer.IconBlue);
            //Texture2D iconBrownTex = AssetDatabase.LoadAssetAtPath<Texture2D>(IconConstContainer.IconBrown);
            //Texture2D iconPurpleTex = AssetDatabase.LoadAssetAtPath<Texture2D>(IconConstContainer.IconPurple);
            //Texture2D iconRedTex = AssetDatabase.LoadAssetAtPath<Texture2D>(IconConstContainer.IconRed);
            //Texture2D iconYellowTex = AssetDatabase.LoadAssetAtPath<Texture2D>(IconConstContainer.IconYellow);
            _fieldGUIContent = new GUIContent(EditorGUIUtility.FindTexture(IconConstContainer.ICON_DEFAULT_ASSET));
            _newBtnGUIContent = new GUIContent("新 建");
            _editBtnGUIConent = new GUIContent("编 辑");
            _playBtnGUIContent = new GUIContent("试 玩");
            _saveBtnGUIContent = new GUIContent("保 存");
            _resetBtnGUIContent = new GUIContent("重 置");
            _recordBtnGUIContent = new GUIContent("记 录");
            _copyBtnGUIContent = new GUIContent("复 制");
            _buildBtnGUIContent = new GUIContent("发 布");
            _deleteBtnGUIContent = new GUIContent("删 除");
            _obstacleBtnGUIContent = new GUIContent("障 碍");
            _searchBtnGUIContent = new GUIContent("搜 索");
            _foldOutGUIContent = new GUIContent(btnCubeTex);
            _litleTitleGUIContent = new GUIContent(EditorGUIUtility.FindTexture(IconConstContainer.ICON_TOOL_TITLE));
            _levelTargetGUIContent = new GUIContent();
        }
        #endregion


        private Vector2 _leftPartScrollView;
        public void DrawUI(object param)
        {
            if (_frameLogic == null)
                return;
            _editorGUIContentStyle.RegisterGUIStyle();
            #region TopPart
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            _litleTitleGUIContent.text = "基础操作区";
            EditorGUILayout.LabelField(_litleTitleGUIContent, _editorGUIContentStyle.littleLabelStyle, GUILayout.Height(_editorGUIContentStyle._menuBtnHeight));
            DrawMenu();
            GUILayout.Space(20);
            EditorGUILayout.EndVertical();
            #endregion

            EditorGUILayout.BeginHorizontal();
            #region LeftPart
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(Screen.width / 3.2f));
            _litleTitleGUIContent.text = "关卡列表";
            EditorGUILayout.LabelField(_litleTitleGUIContent, _editorGUIContentStyle.littleLabelStyle, GUILayout.Width(Screen.width / 4), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight));
            DrawLevelList();
            EditorGUILayout.EndVertical();
            #endregion

            #region RightPart
            EditorGUILayout.BeginVertical();
            _leftPartScrollView = EditorGUILayout.BeginScrollView(_leftPartScrollView, false, true);
            VoFrameConfigData configData = FrameLogic.Instance.GetCurrentSelectFrameConfigData();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            _litleTitleGUIContent.text = "关卡通用信息编辑区";
            //EditorGUILayout.LabelField(_litleTitleGUIContent, _litleTileStyle, GUILayout.Width(Screen.width * 3 / 4), GUILayout.Height(20));
            _frameConfigInfoFoldOut = EditorGUILayout.Foldout(_frameConfigInfoFoldOut, _litleTitleGUIContent, _editorGUIContentStyle.foldOutStyle);
            if(_frameConfigInfoFoldOut || configData != null)
            {
                
                if(configData != null)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    DrawFrameConfig(configData);
                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            _litleTitleGUIContent.text = "事件详细配置区";
            //EditorGUILayout.LabelField(_litleTitleGUIContent, _litleTileStyle, GUILayout.Width(Screen.width * 3 / 4), GUILayout.Height(20));
            _frameDetailFoldOut = EditorGUILayout.Foldout(_frameDetailFoldOut, _litleTitleGUIContent, _editorGUIContentStyle.foldOutStyle);
            if(_editorFrameDetailView.currentFrameDetailData != null)
            {
                _frameDetailFoldOut = true;
            }
            if (_frameDetailFoldOut)
            {

                if (configData != null)
                {
                    VoFrameDetailData currentFrameDetailData = FrameLogic.Instance.GetCurrentFrameDetail();
                    if (currentFrameDetailData != null)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        _editorFrameDetailView.UIDraw();
                        //DrawFrameDetailData(currentFrameDetailData);
                        EditorGUILayout.EndVertical();
                    }

                }
            }
            
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            #endregion
            EditorGUILayout.EndHorizontal();
        }

        #region 主菜单
        private float _menuBtnHeight = 20;
        private void DrawMenu()
        {
            GUILayout.Space(10);
            //第一行
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            if (GUILayout.Button(_newBtnGUIContent, _editorGUIContentStyle.menuBtnStyle, GUILayout.Width(_editorGUIContentStyle.menuBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
            {
                //新 建
                _frameLogic.CreateFrame();
            }
            GUILayout.Space(10);
            if (GUILayout.Button(_editBtnGUIConent, _editorGUIContentStyle.menuBtnStyle, GUILayout.Width(_editorGUIContentStyle.menuBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
            {
                //编 辑
                if(!_frameLogic.EditFrame())
                {
                    //select one frame warning
                }

            }
            GUILayout.Space(10);

            if (GUILayout.Button(_deleteBtnGUIContent, _editorGUIContentStyle.menuBtnStyle, GUILayout.Width(_editorGUIContentStyle.menuBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
            {
                //删 除
                //delete warning
                _frameLogic.DeleteFrame();
            }
            GUILayout.Space(10);

            if (GUILayout.Button(_saveBtnGUIContent, _editorGUIContentStyle.menuBtnStyle, GUILayout.Width(_editorGUIContentStyle.menuBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
            {
                //_frameLogic.SaveFrame();
                //保 存
                _frameLogic.SaveFrame();
            }
            GUILayout.Space(10);

            EditorGUILayout.EndHorizontal();

        }
        #endregion

        #region 关卡列表
        private Vector2 _levelListScrollPos;
        private bool _mainFoldOut;
        private bool _frameDetailFoldOut;
        private bool _frameConfigInfoFoldOut;
        private bool _frameDetailInfoFoldOut;
        private void DrawLevelList()
        {
           // GUILayout.Space(10);

            _levelListScrollPos = EditorGUILayout.BeginScrollView(_levelListScrollPos, false, true);

            _foldOutGUIContent.text = "主线关卡";
            _mainFoldOut = EditorGUILayout.Foldout(_mainFoldOut, _foldOutGUIContent, _editorGUIContentStyle.foldOutStyle);
            if (_mainFoldOut)
            {
                List<FrameConfigData> frameConfigList = FrameLogic.Instance.GetFrameConfigList();
                if(frameConfigList.Count > 0)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(Screen.width / 3.5f));
                    foreach (FrameConfigData configData in frameConfigList)
                    {
                        //GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        if (!FrameLogic.Instance.CheckCurrentFrameConfig(configData))
                        {
                            if (GUILayout.Button(configData.name, _editorGUIContentStyle.littleLabelStyle))
                            {
                                FrameLogic.Instance.SelectFrameConfig(configData);
                                _editorFrameConfigView.OnEnter(VoConfigDataManager.Instance.currentFrameConfigData,_editorFrameDetailView);
                            }
                        }
                        else
                        {
                            _foldOutGUIContent.text = configData.name;
                            _frameDetailInfoFoldOut = EditorGUILayout.Foldout(_frameDetailInfoFoldOut, _foldOutGUIContent, _editorGUIContentStyle.foldOutStyle);
                            if (_frameDetailInfoFoldOut)
                            {
                                //_frameDetailInfoFoldOut
                               // GUILayout.Space(2);
                                GUILayout.BeginVertical();
                                GUILayout.Space(2);
                                DrawFrameTotalDetialList(configData);
                                GUILayout.EndVertical();

                            }
                        }
                        //GUILayout.EndHorizontal();
                        //GUILayout.Space(10);
                    }
                    GUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawLevelItem(int levelType, int frameID)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region 通用关卡信息
        private void DrawFrameConfig(VoFrameConfigData frameConfigData)
        {
            

            _editorFrameConfigView.UIDraw();
        }
        #endregion
        #region 通用关卡详细信息
        private void DrawFrameTotalDetialList(FrameConfigData configData)
        {
            VoFrameTotalDetailData totalDetailData = FrameLogic.Instance.GetCurrentFrameTotalDetailData();
            if (totalDetailData == null)
                return;
            if (totalDetailData.d.Count == 0)
                return;
            foreach(VoFrameDetailData frameDetailData in totalDetailData.d)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                if (GUILayout.Button(frameDetailData.DetailName, _editorGUIContentStyle.littleLabelStyle))
                {
                    FrameLogic.Instance.SelectFrameDetailData(frameDetailData);
                    _editorFrameDetailView.OnEnter(frameDetailData);
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            }
        }
        private void DrawFrameDetailData(VoFrameDetailData currentFrameDetailData)
        {
            _editorFrameDetailView.UIDraw();

        }

        #endregion

        #region 独立障碍配置区
        private void DrawOwnObstacleArea()
        {

        }
        #endregion

    }
}
