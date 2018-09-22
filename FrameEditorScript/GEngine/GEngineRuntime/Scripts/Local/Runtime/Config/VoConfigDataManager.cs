using System;
using System.Collections.Generic;
using ConfigData;
using LitJson;
using UnityEngine;
namespace FrameLogicData
{
    public class VoFrameConfigData
    {
        public int ID;// frame id used sort
        public int tID;//frame id used to logic default ID == tID
        public string name// frame name
        {
            get
            { return frameConfigData.name; }
            set
            { frameConfigData.name = value; }
        }
        public string describe
        {
            get
            { return frameConfigData.describe; }
            set
            { frameConfigData.describe = value; }
        }
       
        public FrameConfigData frameConfigData;
        public VoFrameConfigData(FrameConfigData configData)
        {
            frameConfigData = configData;
        }
    }
    public class VoFrameTotalDetailData
    {
        public List<VoFrameDetailData> f = new List<VoFrameDetailData>();
        public List<VoFrameDetailData> d = new List<VoFrameDetailData>();
        public List<VoPathData> pathList;

        public Dictionary<int, VoFrameDetailData> detailDic = new Dictionary<int, VoFrameDetailData>();
        public FrameTotalDetailData frameTotalDetail;
        public VoFrameDetailData currentDetailData;
        public VoFrameTotalDetailData(FrameTotalDetailData frameTotalData)
        {
            pathList = new List<VoPathData>();
            frameTotalDetail = frameTotalData;
            VoFrameDetailData detailData;
            foreach(FrameDetailData frameDetailData in frameTotalData.d)
            {
                detailData = new VoFrameDetailData(frameDetailData);
                d.Add(detailData);
                detailDic.Add(detailData.ID, detailData);
            }
            foreach(int fID in frameTotalData.f)
            {
                f.Add(detailDic[fID]);
            }
            foreach (FramePathData pathData in frameTotalData.p)
            {
                VoPathData voPathData = new VoPathData(pathData);
                pathList.Add(voPathData);
            }
        }
        public VoFrameDetailData CreateVoFrameDetailData()
        {
            VoFrameDetailData voFrameDetailData = new VoFrameDetailData(ConfigDataManager.Instance.CreateFrameDetailData());
            currentDetailData = voFrameDetailData;
            return voFrameDetailData;
          
        }
        public void SelectFrameDetailData(VoFrameDetailData frameDetail)
        {
            if (frameDetail == null)
                return;
            currentDetailData = frameDetail;
            ConfigDataManager.Instance.currentDetailData = currentDetailData.frameDetailData;
        }
        public void AddVoFrameDetailData(VoFrameDetailData detailData)
        {
            if (detailData == null)
                return;
            detailData.SaveFrameDetailData();
            d.Add(detailData);
            detailDic.Add(detailData.ID, detailData);
            ConfigDataManager.Instance.currentFrameDetailData.AddFrameDetailData(detailData.frameDetailData);
        }
        public void RemoveVoFrameDetailData(VoFrameDetailData detailData)
        {
            if (detailData == null)
                return;
            if (d.Contains(detailData))
                d.Remove(detailData);
            if (f.Contains(detailData))
                f.Remove(detailData);
            detailDic.Remove(detailData.ID);
            ConfigDataManager.Instance.currentFrameDetailData.RemoveFrameDetailData(detailData.frameDetailData);
        }
        public void SaveTotalDetailData()
        {
            frameTotalDetail.ClearFrameDetailData();
            foreach(VoFrameDetailData detailData in d)
            {
                frameTotalDetail.AddFrameDetailData(detailData.frameDetailData);
            }
            foreach(VoFrameDetailData detailData in f)
            {
                frameTotalDetail.AddFrameLauchDetailData(detailData.frameDetailData);
            }
            foreach(VoPathData pathData in pathList)
            {
                frameTotalDetail.AddFramePathData(pathData.framePathData);
            }
        }
        public bool CheckFrameDetailNameSame(string name)
        {
            foreach(VoFrameDetailData detailData in d)
            {
                if (detailData.DetailName == name)
                    return true;
            }
            return false;
        }
        public VoPathData CreatePathData()
        {
            VoPathData pathData = new VoPathData(ConfigDataManager.Instance.CreateFramePathData());
            return pathData;
        }
        public void AddPathData(VoPathData pathData)
        {
            pathList.Add(pathData);
        }
        public void RemovePathData(VoPathData pathData)
        {
            if (!pathList.Contains(pathData))
                return;
            ConfigDataManager.Instance.RemovePathData(pathData.framePathData);
            pathList.Remove(pathData);
        }

    }

    public class VoFrameDetailData
    {
        public int ID
        {
            get
            { return frameDetailData.ID; }
            set
            { frameDetailData.ID = value; }
        }
        public string DetailName
        {
            get
            { return frameDetailData.detailName; }
            set
            { frameDetailData.detailName = value; }
        }
        public int maxEventSortID;
        public VoFrameConditionData condition;
        public List<VoFrameEventData> eventList;
        public List<VoFrameDetailData> nextFrameDetailData;//下一个有效的细节数据
        public FrameDetailData frameDetailData;

        public GameObject targetObj;

        public VoFrameDetailData(FrameDetailData detailData)
        {
            frameDetailData = detailData;
            condition = null;
            eventList = new List<VoFrameEventData>();
            nextFrameDetailData = new List<VoFrameDetailData>();
            eventList = new List<VoFrameEventData>();
            foreach (FrameEventData eventData in frameDetailData.e)
            {
                VoFrameEventData voFrameEventData = new VoFrameEventData(eventData);
                voFrameEventData.baseNameString = DetailName;
                eventList.Add(voFrameEventData);
            }

            if(frameDetailData.c != null)
            {
                condition = new VoFrameConditionData(frameDetailData.c);
                condition.baseNameString = DetailName;
            }
        }
        public void SaveFrameDetailData()
        {
            if (condition != null)
                condition.SaveConditionData();
            foreach (VoFrameEventData frameEventData in eventList)
                frameEventData.SaveEventData();
        }

        public VoFrameEventData AddFrameEventData(int eventType)
        {
            VoFrameEventData frameEventData = new VoFrameEventData(ConfigDataManager.Instance.CreateFrameEventData(eventType));
            frameEventData.eventType = eventType;
            frameEventData.baseNameString = DetailName;
            eventList.Add(frameEventData);
            eventList.Sort();
            return frameEventData;
        }
        public void RemoveEventData(VoFrameEventData frameEventData)
        {
            if (eventList.Contains(frameEventData))
            {
                eventList.Remove(frameEventData);
                frameDetailData.RemoveFrameEventData(frameEventData.frameEventData);
            }

            eventList.Sort();
        }
        public void SaveEventData(VoFrameEventData frameEventData)
        {
            frameEventData.SaveEventData();
        }
        public bool HasFrameConditionData()
        {
            return (condition != null);
        }
        public VoFrameConditionData AddFrameConditionData(int conditionType)
        {
            if (condition != null)
                return condition;
            condition = new VoFrameConditionData(ConfigDataManager.Instance.CreateFrameConditionData(conditionType));
            condition.baseNameString = DetailName;
            return condition;
        }
        public void RemoveConditionData()
        {
            ConfigDataManager.Instance.currentDetailData.RemoveConditionData();
            condition = null;
        }
        public void SaveConditionData()
        {
            if (condition != null)
                condition.SaveConditionData();
        }


    }

    public class VoFrameConditionData
    {
        public FrameConditionData frameConditionData;
        public string baseNameString;

        public int ConditionType
        {
            get
            { return frameConditionData.conditionType; }
            set
            { frameConditionData.conditionType = value; }
        }
        public ConditionParamBase conditionParam;
        public VoFrameConditionData(FrameConditionData conditionData)
        {
            frameConditionData = conditionData;

            if (!string.IsNullOrEmpty(conditionData.conditionParam))
            {
                Debug.Log("condition param "+conditionData.conditionParam);
                Type conditionTypeClass = VoFrameParamManager.Instance.GetConditionClassType(conditionData.conditionType);
                if (conditionTypeClass != null)
                    conditionParam = (ConditionParamBase)JsonMapper.ToObject(conditionData.conditionParam, conditionTypeClass);
                else
                    Debug.LogError("event class type null " + conditionData.conditionType);
            }
            else
                conditionParam = VoFrameParamManager.Instance.GetConditionParam(conditionData.conditionType);
        }
        private string GetConditionParamStr()
        {
            string strTable = JsonMapper.ToJson(conditionParam);
            return strTable;
        }
        public void SaveConditionData()
        {
            frameConditionData.conditionParam = GetConditionParamStr();
        }
    }
    public class VoFrameEventData
    {
        public int eventType;
        public EventParamBase eventParam;
        public FrameEventData frameEventData;
        public string baseNameString;
        public VoFrameEventData()
        {
            frameEventData = null;
        }
        public VoFrameEventData(FrameEventData eventData)
        {
            frameEventData = eventData;
            eventType = eventData.eventType;
            if(eventData.eventParam != "")
            {
                Type eventTypeClass = VoFrameParamManager.Instance.GetEventClassType(eventData.eventType);
                if (eventTypeClass != null)
                    eventParam = (EventParamBase)JsonMapper.ToObject(eventData.eventParam, eventTypeClass);
                else
                    Debug.LogError("event class type null "+eventData.eventType);
            }
            else
                eventParam = VoFrameParamManager.Instance.GetEventParam(eventData.eventType);
        }
        private string GetEventParamStr()
        {
            Debug.Log("event param " + eventParam);
            string strTable = JsonMapper.ToJson(eventParam);
            return strTable;
        }
        public void SaveEventData()
        {
            if (frameEventData == null)
            {
                Debug.Log("frame evetn data null");
                frameEventData = ConfigDataManager.Instance.CreateFrameEventData(eventType);   
            }
            frameEventData.eventType = eventType;
            frameEventData.eventParam = GetEventParamStr();
        }
    }
    public class VoPathData
    {
        public int pathID
        {
            get
            { return framePathData.pathID; }
        }
        public string PathName
        {
            get
            { return framePathData.pathName; }
            set
            { framePathData.pathName = value; }
        }
        public string PathTarget
        {
            get
            { return framePathData.targetObjPath; }
            set
            { framePathData.targetObjPath = value; }
        }
        public bool LookTarget
        {
            get
            { return framePathData.lookTarget; }
            set
            { framePathData.lookTarget = value; }
        }
        public FramePathData framePathData;
        public List<VoPathNode> pathNodeList;
        public VoPathData(FramePathData pathData)
        {
            pathNodeList = new List<VoPathNode>();
            framePathData = pathData;
        }
        public void ClearPathVariable()
        {
            pathNodeList.Clear();
            framePathData.ClearPathNode();
        }
        public VoPathNode CreatePathNode()
        {
            VoPathNode pathNode = new VoPathNode(framePathData.CreatPathNode());
            pathNodeList.Add(pathNode);
            return pathNode;
        }
        public void SavePathNode()
        {
            foreach (VoPathNode pathNode in pathNodeList)
                pathNode.SavePathNode();
        }
    }
    public class VoPathNode
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 handleprev;
        public Vector3 handlenext;
        public int curveTypeRotation;
        public int curveTypePosition;
        public bool chained;

        private FramePathNode _pathNode;
        public VoPathNode( FramePathNode pathNode)
        {
            _pathNode = pathNode;
        }
        public void SavePathNode()
        {
            _pathNode.chained = chained;
            _pathNode.position = new JsonVector3Float(position);
            _pathNode.positionCurveType = curveTypePosition;
            _pathNode.rotationCurveType = curveTypeRotation;
            _pathNode.rotation = new JsonVector3Float(rotation);
            _pathNode.prePosition = new JsonVector3Float(handleprev);
            _pathNode.preRotation = new JsonVector3Float(handlenext);

        }
    }
    public class VoConfigDataManager :SingletonNotMono<VoConfigDataManager>
    {
        public VoFrameConfigData currentFrameConfigData;
        public VoFrameTotalDetailData currentFrameTotalDetailData;
        public List<VoFrameConfigData> totalFrameConfigList;
        public void InitializeConfigData()
        {
            ConfigDataManager.Instance.InitializeConfigLogic();
            VoFrameParamManager.Instance.RegisterFrameParam();
            totalFrameConfigList = new List<VoFrameConfigData>();
        }
        public void ExitConfigLogic()
        {
            VoFrameParamManager.Instance.UnRegisterFrameParam();
        }
        #region frame config logic
        public VoFrameConfigData CreateFrameConfigData()
        {
            FrameConfigData configData = ConfigDataManager.Instance.CreateFrameConfigData();
            if (configData == null)
                return null;
            return new VoFrameConfigData(configData);
                

        }
        public void AddFrameConfigData(VoFrameConfigData configData)
        {
            currentFrameConfigData = configData;
            ConfigDataManager.Instance.AddFrameConfigData(configData.frameConfigData);
            currentFrameTotalDetailData = new VoFrameTotalDetailData(ConfigDataManager.Instance.currentFrameDetailData);
        }
        public void SaveFrameDetailData()
        {
            ConfigDataManager.Instance.SaveFrameDetailData();
        }
        public void SelectFrameConfigType(int type)
        {
            ConfigDataManager.Instance.SelectConfigType(type);
        }
        public List<FrameConfigData> GetFrameConfigDataList()
        {
            return ConfigDataManager.Instance.GetFrameConfigList();
        }

        public bool CheckConfigHasSameName(string configName)
        {
            return ConfigDataManager.Instance.CheckNameSame(configName);
        }

        public void SelectFrameConfigData(FrameConfigData configData)
        {
            FrameTotalDetailData frameTotalData = ConfigDataManager.Instance.SelectConfigData(configData);
            currentFrameConfigData = new VoFrameConfigData(configData);
            currentFrameTotalDetailData = new VoFrameTotalDetailData(frameTotalData);
        }
        public void RemoveFrameConfigData(FrameConfigData configData)
        {
            ConfigDataManager.Instance.RemoveConfigData(configData);
            if(currentFrameConfigData.frameConfigData == configData)
            {
                currentFrameConfigData = null;
                currentFrameTotalDetailData = null;
            }

        }
        public void RemoveCurrentConfigData()
        {
            if (currentFrameConfigData == null)
                return;
            ConfigDataManager.Instance.RemoveConfigData(currentFrameConfigData.frameConfigData);
            currentFrameConfigData = null;
            currentFrameTotalDetailData = null;
        }
        public void SaveFrameConfigData()
        {
            if(currentFrameTotalDetailData != null)
                currentFrameTotalDetailData.SaveTotalDetailData();
            ConfigDataManager.Instance.SaveFrameData();
        }
        #endregion
        #region detail logic
        public bool CheckFrameDetailNameSame(string name)
        {
            if (currentFrameTotalDetailData == null)
                return true;
            if (currentFrameTotalDetailData.currentDetailData == null)
                return true;
            return currentFrameTotalDetailData.CheckFrameDetailNameSame(name);
            
        }
        #endregion
        #region frame event and condition logic

        #endregion


    }
}

