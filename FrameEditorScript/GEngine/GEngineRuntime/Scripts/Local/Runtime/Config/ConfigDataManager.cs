
using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;
namespace ConfigData
{


    public class FrameTotalConfigData
    {
        public int type;
        public int mID;
        public List<FrameConfigData> d = new List<FrameConfigData>();
        public FrameTotalConfigData()
        {
        }
        public FrameConfigData CreateFrameConfigData()
        {
            FrameConfigData configData = new FrameConfigData();
            configData.ID = (mID+1);
            configData.tID = configData.ID;
            return configData;
        }
        public void AddFrameConfigData(FrameConfigData saveData)
        {
            FrameConfigData sameConfigData = null;
            foreach(FrameConfigData configData in d)
            {
                if(configData.ID == saveData.ID)
                {
                    sameConfigData = configData;
                    
                }
            }
            if(sameConfigData != null)
                d.Remove(sameConfigData);
            if (saveData.ID > mID)
                mID = saveData.ID;
            d.Add(saveData);
        }
        public void RemoveFrameConfigData(int frameID)
        {
            FrameConfigData deleteConfigData = null;
            foreach (FrameConfigData configData in d)
            {
                if (configData.ID == frameID)
                {
                    deleteConfigData = configData;
                    break;
                }
            }
            if(deleteConfigData == null)
            {
                Debug.LogError("not contains this configData");
                return;
            }
            d.Remove(deleteConfigData);
            if (frameID == mID)
                mID--;
        }
    }
    public class FrameConfigData
    {
    
        public int ID;// frame id used sort
        public int tID;//frame id used to logic default ID == tID
        public string name;// frame name
        public string describe;

        public FrameConfigData()
        {
            name = "empty";
        }
    }
    public class FrameTotalDetailData
    {
        public int maxID;
        public List<int> f;
        public List<FrameDetailData> d;
        public List<FramePathData> p;
        public int maxPathID;

        public FrameTotalDetailData()
        {
            maxID = 0;
            d = new List<FrameDetailData>();
            f = new List<int>();
            p = new List<FramePathData>();
        }
        public FrameDetailData CreateFrameDetailData()
        {
            FrameDetailData detailData = new FrameDetailData();
            detailData.ID = (maxID+1);
            return detailData;
        }
        public void AddFrameDetailData(FrameDetailData detailData)
        {
            if (detailData == null)
                return;
            d.Add(detailData);
            if (maxID < detailData.ID)
                maxID = detailData.ID;
        }
        public void RemoveFrameDetailData(FrameDetailData detailData)
        {
            if (detailData == null)
                return;
            if (!d.Contains(detailData))
                return;
            foreach(FrameDetailData detail in d)
            {
                if (detail.n.Contains(detailData.ID))
                    detail.n.Remove(detailData.ID);
            }
            d.Remove(detailData);
            if (maxID == detailData.ID)
                maxID--;
        }
        public void AddFrameLauchDetailData(FrameDetailData detailData)
        {
            if (!f.Contains(detailData.ID))
                f.Add(detailData.ID);
        }
        public void RemoveFrameLauchDetailData(FrameDetailData detailData)
        {
            if(f.Contains(detailData.ID))
            {
                f.Remove(detailData.ID);
            }
        }

        public void ClearFrameDetailData()
        {
            f.Clear();
            d.Clear();
            p.Clear();
        }
        public FramePathData CreateFramePathData()
        {
            FramePathData pathData = new FramePathData();
            pathData.pathID = (maxPathID++);
            p.Add(pathData);
            return pathData;
        }
        public void RemoveFramePathData(FramePathData pathData)
        {
            if (!p.Contains(pathData))
                return;
            if (pathData.pathID == maxPathID)
                maxPathID--;
            p.Remove(pathData);
        }
        public void AddFramePathData(FramePathData pathData)
        {
            if(!p.Contains(pathData))
            {
                p.Add(pathData);
            }
        }
    }
    public class FrameDetailData
    {
        public string detailName;
        public int ID;
        public int maxEventSortID;
        public FrameConditionData c;
        public List<FrameEventData> e;
        public List<int> n;//下一个有效的细节数据
        public FrameDetailData()
        {
            e = new List<FrameEventData>();
            n = new List<int>();
        }
        public FrameEventData CreateFrameEventData(int eventType)
        {
            FrameEventData eventData = new FrameEventData();
            eventData.eventType = eventType;
            eventData.sortID = (maxEventSortID+1);
            AddFrameEventData(eventData);
            return eventData;
        }
        public void AddFrameEventData(FrameEventData eventData)
        {
            e.Add(eventData);
            if (eventData.sortID > maxEventSortID)
                maxEventSortID = eventData.sortID;
            e.Sort();
        }
        public void RemoveFrameEventData(FrameEventData eventData)
        {
            if (!e.Contains(eventData))
            {
                Debug.LogError("event data not exist " + eventData.eventParam);
                return;
            }
            e.Remove(eventData);
            if (eventData.sortID == maxEventSortID)
                maxEventSortID--;
            e.Sort();
        }
        public FrameConditionData CreateConditionData(int conditionType)
        {
            FrameConditionData conditionData = new FrameConditionData();
            conditionData.conditionType = conditionType;
            this.c = conditionData;
            return conditionData;
        }
        public void RemoveConditionData()
        {
            this.c = null;
        }
        public void ResortEventData(FrameEventData f, FrameEventData t)
        {
            f.sortID = f.sortID + t.sortID;
            t.sortID = f.sortID - t.sortID;
            f.sortID = f.sortID - t.sortID;

            e.Sort();
        }
        public void AddNextDetailData(FrameDetailData detailData)
        {
            if(!n.Contains(detailData.ID))
            {
                n.Add(detailData.ID);
            }
        }
        public void RemoveNextDetailData(FrameDetailData detailData)
        {
            if (n.Contains(detailData.ID))
                n.Remove(detailData.ID);
        }

    }
    public class FramePathData
    {
        public string pathName;
        public int pathID;
        public string targetObjPath;
        public bool lookTarget;
        public List<FramePathNode> framePathNodeList;
        public FramePathData()
        {
            pathName = "";
            framePathNodeList = new List<FramePathNode>();
        }
        public FramePathNode CreatPathNode()
        {
            FramePathNode pathNode = new FramePathNode();
            framePathNodeList.Add(pathNode);
            return pathNode;
        }
        public void ClearPathNode()
        {
            framePathNodeList.Clear();
        }

    }
    public class FramePathNode
    {
        public JsonVector3FInt position;
        public JsonVector3FInt rotation;
        public JsonVector3FInt prePosition;
        public JsonVector3FInt preRotation;
        public int positionCurveType;
        public int rotationCurveType;
        public bool chained;
        public FramePathNode()
        {
            position = new JsonVector3FInt();
            rotation = new JsonVector3FInt();
            prePosition = new JsonVector3FInt();
            preRotation = new JsonVector3FInt();
        }

        
    }
    public class FrameConditionData
    {
        public int conditionType;
        public string conditionParam;
    }
    public class FrameEventData : IComparable
    {
        public int sortID = 0;
        public int eventType = 0;
        public string eventParam = "";

        public int CompareTo(object obj)
        {
            FrameEventData frameEventData = obj as FrameEventData;
            return -(sortID - frameEventData.sortID);
        }
    }
    public class ConfigDataManager : SingletonNotMono<ConfigDataManager>
    {
        public const int FRAME_TYPE_MAIN = 1;

        public List<int> frameTypeList = new List<int>();
        public Dictionary<int, FrameTotalConfigData> frameTotalConfigDataDic = new Dictionary<int, FrameTotalConfigData>();
        private List<FrameTotalConfigData> _frameTotalConfigDataJsonList = new List<FrameTotalConfigData>();
        public Dictionary<int, FrameConfigData> frameConfigDataDic = new Dictionary<int, FrameConfigData>();
    
        public int currentFrameID;

        private FrameTotalConfigData _currentTotalConfigData;
        public int currentFrameType;
        private string _baseFrameFilePath;
        private string _baseTableFilePath;
        private string _frameFileName = "frame.bytes";
        public FrameConfigData currentSelectConfigData;

        public FrameTotalDetailData currentFrameDetailData;

        public FrameDetailData currentDetailData;

        public void InitializeConfigLogic()
        {
            currentSelectConfigData = null;
            frameTotalConfigDataDic = new Dictionary<int, FrameTotalConfigData>();
            _frameTotalConfigDataJsonList = new List<FrameTotalConfigData>();
            frameConfigDataDic = new Dictionary<int, FrameConfigData>();
            frameTypeList = new List<int>();
            _currentTotalConfigData = null;
            frameTypeList.Add(FRAME_TYPE_MAIN);
            currentFrameType = -1;
            string _directoryFile = Path.GetDirectoryName(Application.dataPath);
            string[] directoryArray = _directoryFile.Split('/');
            _directoryFile = _directoryFile.Replace(directoryArray[directoryArray.Length - 1], "");
            _baseFrameFilePath = _directoryFile+ "FrameConfig/Frame/";
            _baseTableFilePath = _directoryFile + "FrameConfig/FrameDetail/";

            ParseFrameConfigData();
            SelectConfigType(FRAME_TYPE_MAIN);
        }

        public FrameConfigData CreateFrameConfigData()
        {
            if (_currentTotalConfigData == null)
            {
                Debug.LogError("_currentTotalConfigData == null ");
                return null;
            }
                return _currentTotalConfigData.CreateFrameConfigData();
        }
        public void SelectConfigType(int frameType)
        {
            if (!frameTotalConfigDataDic.ContainsKey(frameType))
            {
                Debug.LogError("frame type not contains this type " + frameType);
                return;
            }
            currentFrameType = frameType;
            _currentTotalConfigData = frameTotalConfigDataDic[frameType];
            frameConfigDataDic.Clear();
            foreach(FrameConfigData configData in _currentTotalConfigData.d)
            {
                frameConfigDataDic.Add(configData.tID, configData);
            }


        }
        public bool CheckNameSame(string configName)
        {
            if (_currentTotalConfigData == null)
                return true;
            foreach(FrameConfigData configData in _currentTotalConfigData.d)
            {
                if (configData.name == configName)
                    return true;
            }
            return false;
        }
        public List<FrameConfigData> GetFrameConfigList()
        {
            if (_currentTotalConfigData == null)
                return null;
            return _currentTotalConfigData.d;
        }
        public FrameTotalDetailData SelectConfigData(FrameConfigData configData)
        {
            if (configData == null)
            {
                Debug.LogError(" frame id not contains this id id == ");
                return null;
            }
            currentSelectConfigData = configData;
            currentFrameDetailData = ParseFrameTableData(configData.ID + "_" + configData.tID);
            return currentFrameDetailData;
        }
        public void AddFrameConfigData(FrameConfigData configData)
        {
            if(configData == null)
            {
                Debug.LogError("CreateFrameConfigData configData == null " + currentFrameType);
                return;
            }
            if(!frameTotalConfigDataDic.ContainsKey(currentFrameType))
            {
                Debug.LogError("CreateFrameConfigData error Not Contains frame type "+ currentFrameType);
                return;
            }
            FrameTotalConfigData frameTotalConfigData = frameTotalConfigDataDic[currentFrameType];
            frameTotalConfigData.AddFrameConfigData(configData);
            frameConfigDataDic.Add(configData.tID, configData);
            currentFrameDetailData = new FrameTotalDetailData();
            currentSelectConfigData = configData;
            //add frame detail file
            SaveFrameData();
        }
        public void SaveFrameData()
        {
            //_currentTotalConfigData.AddFrameConfigData(frameConfigData);
            string str = JsonMapper.ToJson(_frameTotalConfigDataJsonList);
            Utility.WriteFileLogic(_baseFrameFilePath, _frameFileName, str);

            SaveFrameDetailData();

            //Debug.Log("table data "+strTable);
            // toJson

        }
        public void SaveFrameDetailData()
        {
            if (currentFrameDetailData != null)
            {
                string strTable = JsonMapper.ToJson(currentFrameDetailData);
                Utility.WriteFileLogic(_baseTableFilePath, currentSelectConfigData.ID + "_" + currentSelectConfigData.tID + ".bytes", strTable);
            }
        }
        public void RemoveConfigData(FrameConfigData configData)
        {
            if (configData == null)
            {
                Debug.LogError("RemoveConfigData configData == null " + currentFrameType);
                return;
            }
            _currentTotalConfigData.RemoveFrameConfigData(configData.ID);
            frameConfigDataDic.Remove(configData.tID);
            if (currentSelectConfigData == configData)
            {
                currentSelectConfigData = null;
                currentFrameDetailData = null;
                currentDetailData = null;
            }
                
            Utility.RemoveFileLogic(_baseTableFilePath, configData.ID + "_" + configData.tID + ".bytes");
        }
        public int GetFrameConfigMaxTableID(int frameType)
        {
            if (!frameTotalConfigDataDic.ContainsKey(frameType))
            {
                Debug.LogError("CreateFrameConfigData error Not Contains frame type " + frameType);
                return -1;
            }
            FrameTotalConfigData frameTotalConfigData = frameTotalConfigDataDic[frameType];
            return (frameTotalConfigData.mID+1);
        }
    
        public FrameDetailData CreateFrameDetailData()
        {
            if (currentFrameDetailData == null)
                return null;
            currentDetailData = currentFrameDetailData.CreateFrameDetailData();
            return currentDetailData;
        }
        public FrameEventData CreateFrameEventData(int eventType)
        {
            if(currentDetailData == null)
                return null;

            return currentDetailData.CreateFrameEventData(eventType) ;
        }
        public FrameConditionData CreateFrameConditionData(int conditionType)
        {
            if (currentDetailData == null)
                return null;
            FrameConditionData c = currentDetailData.CreateConditionData(conditionType);
            return c;
        }
        public FramePathData CreateFramePathData()
        {
            if (currentFrameDetailData == null)
                return null;
            FramePathData p = currentFrameDetailData.CreateFramePathData();
            return p;
        }
        public void RemovePathData(FramePathData pathData)
        {
            if (currentFrameDetailData == null)
                return;
            currentFrameDetailData.RemoveFramePathData(pathData);
        }
        public void Exit()
        {
        }
        #region frame func
        private void ParseFrameConfigData()
        {
            string frameTotalJsonStr = Utility.ReadFileLogic(_baseFrameFilePath, _frameFileName);
            Debug.Log("frame total json str " + frameTotalJsonStr);
            if (frameTotalJsonStr != "")
            {
                _frameTotalConfigDataJsonList = JsonMapper.ToObject<List<FrameTotalConfigData>>(frameTotalJsonStr);
                foreach (FrameTotalConfigData totalConfigData in _frameTotalConfigDataJsonList)
                {
                    frameTotalConfigDataDic[totalConfigData.type] = totalConfigData;
                }
                return;
            }
            FrameTotalConfigData configData;

            configData = new FrameTotalConfigData();
            configData.type = FRAME_TYPE_MAIN;
            configData.mID = GetInitializeTableID(FRAME_TYPE_MAIN);
            frameTotalConfigDataDic.Add(FRAME_TYPE_MAIN, configData);
            _currentTotalConfigData = frameTotalConfigDataDic[FRAME_TYPE_MAIN];
            _frameTotalConfigDataJsonList.Add(configData);

        }

        private FrameTotalDetailData ParseFrameTableData(string tableFileName)
        {
            string tableJsonStr = Utility.ReadFileLogic(_baseTableFilePath, tableFileName + ".bytes");
            if (tableJsonStr == "")
            {
                Debug.LogError("ParseFrameTableData config data not exist ");
                return null;
            }
            FrameTotalDetailData frameTableData = JsonMapper.ToObject<FrameTotalDetailData>(tableJsonStr);
            if (frameTableData == null)
            {
                Debug.LogError("ParseFrameTableData config data not exist ");
            
            }
            return frameTableData;
        }
        private int GetInitializeTableID(int frameType)
        {
            switch(frameType)
            {
                case FRAME_TYPE_MAIN:
                    return 1;
            }
            return -1;
        }
        #endregion

    }
}