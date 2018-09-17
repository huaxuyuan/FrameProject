using System;
using System.Collections.Generic;
using FrameLogicData;
using UnityEditor;
using UnityEngine;
using FrameEditor;
using GEngine.Editor;

namespace FrameEditor
{
    public class EditorFrameConfigView
    {
        private VoFrameTotalDetailData _frameTotalDetailData;
        private List<VoFrameDetailData> _addDetailData;
        private List<VoFrameDetailData> _delDetailData;
        private VoFrameConfigData _frameConfigData;
        private EditorGuiContentStyle _editorGUIContentStyle;
        private EditorFrameDetailView _editorFrameDetailView;
        public EditorFrameConfigView()
        {
            _addDetailData = new List<VoFrameDetailData>();
            _delDetailData = new List<VoFrameDetailData>();
        }
        public void OnEnter(VoFrameConfigData frameConfigData,EditorFrameDetailView detailView)
        {
            _addDetailData.Clear();
            _delDetailData.Clear();
            _editorFrameDetailView = detailView;
            _frameTotalDetailData = VoConfigDataManager.Instance.currentFrameTotalDetailData;
            _frameConfigData = frameConfigData;
            _editorGUIContentStyle = EditorGuiContentStyle.Instance;
        }
        public void UIDraw()
        {
            if (_frameTotalDetailData == null)
            {
                return;
            }
            if(_addDetailData.Count > 0)
            {
                foreach(VoFrameDetailData detailData in _addDetailData)
                {
                    _frameTotalDetailData.f.Add(detailData);
                }
                _addDetailData.Clear();
            }
            if(_delDetailData.Count >0)
            {
                foreach (VoFrameDetailData detailData in _delDetailData)
                {
                    _frameTotalDetailData.f.Remove(detailData);
                }
                _delDetailData.Clear();
            }
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("关卡名称："+_frameConfigData.name,EditorGuiContentStyle.Instance.littleLabelStyle , GUILayout.Width(200), GUILayout.Height(20));
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            if (_frameTotalDetailData.f.Count > 0)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                foreach (VoFrameDetailData detailData in _frameTotalDetailData.f)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(detailData.DetailName);
                    if (GUILayout.Button("删除事件", GUILayout.Width(_editorGUIContentStyle.defaultBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
                    {
                        _delDetailData.Add(detailData);
                    }
                        EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(_editorGUIContentStyle.defaultRightSpace);
            if (GUILayout.Button("添加起始事件", GUILayout.Width(_editorGUIContentStyle.defaultBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
            {
                GenericMenu eventTypeMenu = new GenericMenu();
                foreach (VoFrameDetailData frameDetailData in _frameTotalDetailData.d)
                {
                    if (_frameTotalDetailData.f.Contains(frameDetailData))
                    {
                        continue;
                    }
                    eventTypeMenu.AddItem(
                        new GUIContent(frameDetailData.DetailName),
                        false,
                        AddFrameFirstData,
                    frameDetailData);
                }
                eventTypeMenu.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
              GUILayout.Space(_editorGUIContentStyle.defaultMiddleSpace);
            if (GUILayout.Button("新建内容", GUILayout.Width(_editorGUIContentStyle.defaultBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
            {
                FrameLogic.Instance.CreateFrameDetail();
                _editorFrameDetailView.OnEnter(FrameLogic.Instance.newFrameDetailData);
            }
              GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
        private void AddFrameFirstData(object obj)
        {
            VoFrameDetailData detailData = obj as VoFrameDetailData;
            _addDetailData.Add(detailData);
        }
        public void OnExit()
        {

        }
    }
}
