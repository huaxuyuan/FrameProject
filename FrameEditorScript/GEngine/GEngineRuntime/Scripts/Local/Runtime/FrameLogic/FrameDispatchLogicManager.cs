using FrameLogicData;
using System.Collections.Generic;
using UnityEngine;
using Runtime.FrameConditionLogic;
using Runtime.FrameEventLogic;
namespace Runtime.FrameLogic
{
    public class FrameDispatchLogicManager : SingletonNotMono<FrameDispatchLogicManager>
    {
        private VoFrameTotalDetailData _totalDetailData;
        private List<VoFrameDetailData> _currentFrameDetailList;
        private List<VoFrameDetailData> _finishFrameDetailList;
        private Stack<VoFrameDetailData> _tempFrameDetailData;
        public FrameDispatchLogicManager()
        {
            _totalDetailData = null;
            _currentFrameDetailList = new List<VoFrameDetailData>();
            _finishFrameDetailList = new List<VoFrameDetailData>();
            _tempFrameDetailData = new Stack<VoFrameDetailData>();
        }
        public void InitializeFrameLogic()
        {
            FrameConditionManager.Instance.InitializeFrameCondition();
            FrameEventManager.Instance.InitializeFrameEvent();
            _totalDetailData = VoConfigDataManager.Instance.currentFrameTotalDetailData;
            foreach(VoFrameDetailData detailData in _totalDetailData.f)
            {
                _tempFrameDetailData.Push(detailData);
               
            }
            VoFrameDetailData frameDetailData;
            while(_tempFrameDetailData.Count > 0)
            {
                frameDetailData = _tempFrameDetailData.Pop();
                bool success = SetFrameDetailValid(frameDetailData);
                if(success)
                {
                    foreach(VoFrameDetailData detailData in frameDetailData.nextFrameDetailData)
                    {
                        _tempFrameDetailData.Push(detailData);
                    }
                }

            }
        }
        private bool SetFrameDetailValid(VoFrameDetailData frameDetailData)
        {
            if(_currentFrameDetailList.Contains(frameDetailData))
            {
                Debug.LogError("_current frame detail list already contains "+frameDetailData.ID);
                return false;
            }
            if(_finishFrameDetailList.Contains(frameDetailData))
            {
                Debug.LogError("_current frame detail already finish "+frameDetailData.ID);
                return false;
            }
            if(frameDetailData.condition != null)
            {
                BaseConditionLogic conditionLogic = FrameConditionManager.Instance.GetConditionLogic(frameDetailData.condition.ConditionType);
                conditionLogic.InitializeCondition(frameDetailData, ConditionFinishCallBack);
                _currentFrameDetailList.Add(frameDetailData);
                return false;
            }
            else
            {
                FrameDetailFinishLogic(frameDetailData);
                return true;
            }
           
        }
        private void ConditionFinishCallBack(BaseConditionLogic condition)
        {
            Debug.Log("ConditionFinishCallBack "+ condition.currentDetailData);
            FrameDetailFinishLogic(condition.currentDetailData);
            FrameConditionManager.Instance.RecycleConditionLogic(condition);
            
        }
        private void FrameDetailFinishLogic(VoFrameDetailData frameDetailData)
        {
            _currentFrameDetailList.Remove(frameDetailData);
            if(frameDetailData.eventList.Count == 0)
            {
                Debug.LogError(" frame detail not contains event "+frameDetailData.DetailName);
                return;
            }
            foreach (VoFrameEventData frameEventData in frameDetailData.eventList)
            {
                //Debug.Log("FrameDetailFinishLogic " + frameEventData.eventParam);
                BaseEventLogic eventLogic = FrameEventManager.Instance.GetEventLogic(frameEventData.eventType);
                eventLogic.InitializeEventLogic(frameEventData);
                eventLogic.DoEvent();
                //FrameEventManager.Instance.RecycleEventLogic(eventLogic)
            }
        }

    }
}
