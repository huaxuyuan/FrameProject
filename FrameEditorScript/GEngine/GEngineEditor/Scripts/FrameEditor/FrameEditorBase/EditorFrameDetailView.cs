using FrameLogicData;
using GEngine.Editor;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using FrameEditorLogic;
using System;

namespace FrameEditor
{
    public class EditorFrameDetailView
    {
        public VoFrameDetailData currentFrameDetailData;
        private List<EditorFrameEventViewBase> _editorEventViewList;
        private EditorFrameConditionViewBase _editorConditionView;
        private List<EditorFrameEventViewBase> _tempAddEventViewList;
        private List<EditorFrameEventViewBase> _tempDeleteEventViewList;
        private List<int> _eventTypeList;
        private EditorGuiContentStyle _editorGUIContentStyle;
        private string _frameDetailName;
        private bool _nameSame;
        public EditorFrameDetailView()
        {
            _editorEventViewList = new List<EditorFrameEventViewBase>();
            _tempAddEventViewList = new List<EditorFrameEventViewBase>();
            _tempDeleteEventViewList = new List<EditorFrameEventViewBase>();
            _eventTypeList = new List<int>();
        }
        public void OnEnter(VoFrameDetailData frameEventData)
        {
            _editorGUIContentStyle = EditorGuiContentStyle.Instance;
            if (_editorConditionView != null)
            {
                EditorFrameDetailLogic.Instance.RecycleConditionView(_editorConditionView);
                _editorConditionView = null;
            }
            _nameSame = false;
            foreach (EditorFrameEventViewBase eventView in _editorEventViewList)
                EditorFrameDetailLogic.Instance.RecycleEventView(eventView);
            _editorEventViewList.Clear();
            _eventTypeList.Clear();
            currentFrameDetailData = frameEventData;
            Debug.Log("EditorFrameDetailView OnEnter");
            EditorFrameDetailLogic.Instance.SetFrameDetailData(currentFrameDetailData);
            if(currentFrameDetailData.condition != null)
            {
                _editorConditionView = EditorFrameDetailLogic.Instance.GetEditorConditionView(currentFrameDetailData.condition.ConditionType);
                _editorConditionView.OnEnter(currentFrameDetailData.condition);
                
            }
            EditorFrameEventViewBase eventViewData;
            foreach(VoFrameEventData frameEvent in currentFrameDetailData.eventList)
            {
                eventViewData = EditorFrameDetailLogic.Instance.GetEditorEventView(frameEvent.eventType);
                eventViewData.OnEnter(frameEvent);
                _eventTypeList.Add(frameEvent.eventType);
                _editorEventViewList.Add(eventViewData);
            }

        }
        public  void UIDraw()
        {
            if (currentFrameDetailData == null)
                return;
            EditorGuiContentStyle.Instance.fieldGUIContent.text = "内容名称:";
            _frameDetailName = EditorGUILayout.TextField(EditorGuiContentStyle.Instance.fieldGUIContent, currentFrameDetailData.DetailName, EditorGuiContentStyle.Instance.fieldGUIStyle);

           if (_frameDetailName != currentFrameDetailData.DetailName)
            {
                currentFrameDetailData.DetailName = _frameDetailName;
                if (VoConfigDataManager.Instance.CheckFrameDetailNameSame(_frameDetailName))
                {
                    _nameSame = true;
                }
                else
                    _nameSame = false;
            }
            if (_nameSame)
                EditorWindowManager.Instance.HelpBox("内容名字相同！");
            GUILayout.Space(10);
            if (_editorConditionView != null)
            {
                //draw condition type
                EditorGUILayout.LabelField(EditorFrameDetailLogic.Instance.GetConditionMenuStr(_editorConditionView.conditionType));
                GUILayout.Space(10);
                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Space(10);
                _editorConditionView.UIDraw();

                GUILayout.BeginHorizontal();
                GUILayout.Space(_editorGUIContentStyle.defaultRightSpace);
                if (GUILayout.Button("删除条件", GUILayout.Width(_editorGUIContentStyle.defaultBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
                {
                    currentFrameDetailData.RemoveConditionData();
                    EditorFrameDetailLogic.Instance.RecycleConditionView(_editorConditionView);
                    _editorConditionView = null;
                }
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(_editorGUIContentStyle.defaultRightSpace);
                if (GUILayout.Button("添加条件", GUILayout.Width(_editorGUIContentStyle.defaultBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
                {
                    GenericMenu eventTypeMenu = new GenericMenu();
                    foreach (int type in Enum.GetValues(typeof(FrameConstDefine.ConditionType)))
                    {
                        eventTypeMenu.AddItem(
                            new GUIContent(EditorFrameDetailLogic.Instance.GetConditionMenuStr(type)), 
                            false, 
                            AddConditionCallBack, 
                            type);
                    }
                    eventTypeMenu.ShowAsContext();

                }
                GUILayout.EndHorizontal();
                //add condition button
            }
            if(_tempDeleteEventViewList.Count > 0)
            {
                foreach(EditorFrameEventViewBase eventView in _tempDeleteEventViewList)
                {
                    EditorFrameDetailLogic.Instance.RecycleEventView(eventView);
                    _editorEventViewList.Remove(eventView);
                    _eventTypeList.Remove(eventView.eventType);
                }
                _tempDeleteEventViewList.Clear();
            }
            if(_tempAddEventViewList.Count >0)
            {
                foreach(EditorFrameEventViewBase eventView in _tempAddEventViewList)
                {
                    _editorEventViewList.Add(eventView);
                    _eventTypeList.Add(eventView.eventType);
                }
                _tempAddEventViewList.Clear();
            }
            if(_editorEventViewList.Count > 0)
            {
                foreach(EditorFrameEventViewBase eventView in _editorEventViewList)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField(EditorFrameDetailLogic.Instance.GetEventShowStr(eventView.eventType));
                    GUILayout.Space(10);
                    eventView.UIDraw();
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(_editorGUIContentStyle.defaultRightSpace);
                    if (GUILayout.Button("删除事件", GUILayout.Width(_editorGUIContentStyle.defaultBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
                    {
                        currentFrameDetailData.RemoveEventData(eventView.frameEventData);
                        
                        _tempDeleteEventViewList.Add(eventView);
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
            }
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(_editorGUIContentStyle.defaultRightSpace);
            if (GUILayout.Button("添加事件", GUILayout.Width(_editorGUIContentStyle.defaultBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
            {

                GenericMenu eventTypeMenu = new GenericMenu();
                foreach (int eventType in Enum.GetValues(typeof(FrameConstDefine.EventType)))
                {
                    if(_eventTypeList.Contains(eventType))
                    {
                        continue;
                    }
                    eventTypeMenu.AddItem(
                        new GUIContent(EditorFrameDetailLogic.Instance.GetEventMenuStr(eventType)),
                        false,
                        AddEventCallBack,
                        eventType);
                }
                eventTypeMenu.ShowAsContext();

            }
            GUILayout.EndHorizontal();
            //GUILayout.Label(currentFrameDetailData.DetailDescribe, _litleTileStyle, GUILayout.Width(70), GUILayout.Height(20));
            if (FrameLogic.Instance.newFrameDetailData == currentFrameDetailData)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(_editorGUIContentStyle.defaultMiddleSpace);
                if (GUILayout.Button("添加内容", GUILayout.Width(_editorGUIContentStyle.defaultBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
                {
                    if (_frameDetailName == null || _frameDetailName == ""||VoConfigDataManager.Instance.CheckFrameDetailNameSame(currentFrameDetailData.DetailName))
                    {
                        EditorWindowManager.Instance.ShowMsgBox("名字异常, 无法添加");
                        return;
                    }
                    FrameLogic.Instance.AddFrameDetailData();
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(_editorGUIContentStyle.defaultMiddleSpace);
                if (GUILayout.Button("保存内容", GUILayout.Width(_editorGUIContentStyle.defaultBtnWidth), GUILayout.Height(_editorGUIContentStyle._menuBtnHeight)))
                {
                    if (_frameDetailName == null || _frameDetailName == "" || VoConfigDataManager.Instance.CheckFrameDetailNameSame(_frameDetailName))
                    {
                        EditorWindowManager.Instance.ShowMsgBox("名字异常, 无法添加");
                        return;
                    }
                    SaveFrameDetailConfig();
                }
                GUILayout.EndHorizontal();
            }

        }
        public void OnExit()
        {

        }

        private void AddConditionCallBack(object obj)
        {
            int conditionType = (int)obj;
            Debug.Log("condition type == "+conditionType);
            VoFrameConditionData conditionData = EditorFrameDetailLogic.Instance.AddCondition(conditionType);
            _editorConditionView = EditorFrameDetailLogic.Instance.GetEditorConditionView(currentFrameDetailData.condition.ConditionType);
            _editorConditionView.OnEnter(currentFrameDetailData.condition);
        }
        private void AddEventCallBack(object obj)
        {
            int eventType = (int)obj;
            Debug.Log("event type == " + eventType);
            VoFrameEventData frameEventData = currentFrameDetailData.AddFrameEventData(eventType);
            EditorFrameEventViewBase eventViewData = EditorFrameDetailLogic.Instance.GetEditorEventView(frameEventData.eventType);
            eventViewData.OnEnter(frameEventData);
            _tempAddEventViewList.Add(eventViewData);
        }

        private void SaveFrameDetailConfig()
        {
            Debug.Log("SaveFrameDetailConfig");
            foreach (EditorFrameEventViewBase eventFrame in _editorEventViewList)
                eventFrame.SaveData();
            if (_editorConditionView != null)
                _editorConditionView.SaveData();
            currentFrameDetailData.SaveFrameDetailData();
            VoConfigDataManager.Instance.SaveFrameDetailData();
        }

    }
}
