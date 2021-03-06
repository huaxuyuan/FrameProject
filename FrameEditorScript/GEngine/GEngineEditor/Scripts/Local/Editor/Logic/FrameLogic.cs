﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using FrameLogicData;
using FrameEditorLogic;
using ConfigData;
namespace GEngine.Editor
{
    class FrameLogic : SingletonNotMono<FrameLogic>
    {
        public VoFrameDetailData newFrameDetailData;
        public void InitializeFrameLogic()
        {
            VoConfigDataManager.Instance.InitializeConfigData();
            EditorFrameDetailLogic.Instance.RegisterLogic();
        }
        public void ExitFrameLogic()
        {
            EditorFrameDetailLogic.Instance.UnRegisterLogic();
            VoConfigDataManager.Instance.ExitConfigLogic();
        }
        public List<FrameConfigData> GetFrameConfigList()
        {
            return VoConfigDataManager.Instance.GetFrameConfigDataList();
        }
        public void CreateFrame()
        {
            //ConfigDataManager.Instance.CreateFrameConfigData();
            EditorWindowManager.Instance.ShowWindow(EditorWindowManager.WINDOW_TYPE_CREATE_FRAME);
        }
        public bool EditFrame()
        {
            return true;
        }
        public void DeleteFrame()
        {
            SceneLogic.Instance.RemoveScene();
            VoConfigDataManager.Instance.RemoveCurrentConfigData();
            //ConfigDataManager.Instance.RemoveConfigData();
        }
        public void AddFrameConfig(VoFrameConfigData configData)
        {
            VoConfigDataManager.Instance.AddFrameConfigData(configData);
            EditorFrameConfigLogic.Instance.InitializeFrameLogic();
            EditorWindowManager.Instance.HideWindow(EditorWindowManager.WINDOW_TYPE_CREATE_FRAME);
        }
        public void SelectFrameConfig(FrameConfigData configData)
        {
            if(VoConfigDataManager.Instance.currentFrameConfigData != null)
            {
                if (VoConfigDataManager.Instance.currentFrameConfigData.frameConfigData == configData)
                    return;
            }
            Debug.Log("select frame config name == " + configData.name);
            VoConfigDataManager.Instance.SelectFrameConfigData(configData);
            SceneLogic.Instance.OpenScene(configData);
        }
        public bool CheckCurrentFrameConfig(FrameConfigData configData)
        {
            if (VoConfigDataManager.Instance.currentFrameConfigData != null)
            {
                if (VoConfigDataManager.Instance.currentFrameConfigData.frameConfigData == configData)
                    return true;
            }
            return false;
        }
        public VoFrameConfigData GetCurrentSelectFrameConfigData()
        {
            return VoConfigDataManager.Instance.currentFrameConfigData;
        }
        public VoFrameTotalDetailData GetCurrentFrameTotalDetailData()
        {
            return VoConfigDataManager.Instance.currentFrameTotalDetailData;
        }
        public void SelectFrameDetailData(VoFrameDetailData frameDetail)
        {
            VoConfigDataManager.Instance.currentFrameTotalDetailData.SelectFrameDetailData(frameDetail);
        }
        public VoFrameDetailData GetCurrentFrameDetail()
        {
            return VoConfigDataManager.Instance.currentFrameTotalDetailData.currentDetailData;
        }
        public void CreateFrameDetail()
        {
            newFrameDetailData = VoConfigDataManager.Instance.currentFrameTotalDetailData.CreateVoFrameDetailData();
        }
        public void AddFrameDetailData()
        {
            VoConfigDataManager.Instance.currentFrameTotalDetailData.AddVoFrameDetailData(newFrameDetailData);
            newFrameDetailData = null;
        }
        public void RemoveFrameDetail()
        {

        }
        public void SaveFrame()
        {
            PathLogic.Instance.SavePathLogic();
            VoConfigDataManager.Instance.SaveFrameConfigData();
            EditorWindowManager.Instance.HideWindow(EditorWindowManager.WINDOW_TYPE_EDITWINDOW);
            EditorWindowManager.Instance.HideWindow(EditorWindowManager.WINDOW_TYPE_CREATE_FRAME);
        }
        private void SavePathLogic()
        {
            List<VoPathData> pathList = VoConfigDataManager.Instance.currentFrameTotalDetailData.pathList;
            foreach(VoPathData pathData in pathList)
            {
                pathData.ClearPathVariable();
                //get path component 
                //get component variable pathData.CreatePathNode(); initialize path node variable

                //pathData.CreatePathNode();
                pathData.SavePathNode();
            }
        }
        public void ExitFrame()
        {
            EditorWindowManager.Instance.HideWindow(EditorWindowManager.WINDOW_TYPE_EDITWINDOW);
            EditorWindowManager.Instance.HideWindow(EditorWindowManager.WINDOW_TYPE_CREATE_FRAME);
        }
    }
}
