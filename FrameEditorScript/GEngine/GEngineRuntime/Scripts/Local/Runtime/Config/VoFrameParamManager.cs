using System;

namespace FrameLogicData
{
    #region paramBase
    public class EventParamBase : IClassObjWithoutKey
    {
        public int eventType;
        public string targetObjectPath = "";
        public virtual void Clear()
        {

        }

        public virtual void Recycle()
        {

        }
    }
    public class ConditionParamBase : IClassObjWithoutKey
    {
        public int conditionType;
        public string targetObjectPath;
        public virtual void Clear()
        {

        }

        public virtual void Recycle()
        {

        }
    }
    #endregion
    #region eventParam
    [ClassPool((int)FrameConstDefine.EventType.FRAME_EVENT_CAMERAMOVE_TARGET)]
    public class EventCameraTargetParam : EventParamBase
    {
        public int moveSpeed;//speed *1000
        public EventCameraTargetParam()
        {
            moveSpeed = 0;
            eventType =(int)(FrameConstDefine.EventType.FRAME_EVENT_CAMERAMOVE_TARGET);
        }
        public override void Clear()
        {
            targetObjectPath = null;
            moveSpeed = 0;
        }

        public override void Recycle()
        {
            targetObjectPath = "";
            moveSpeed = 0;
        }
    }
    #endregion

    #region conditionParam
    [ClassPool((int)FrameConstDefine.ConditionType.FRAME_CONDITION_PRESS)]
    public class ConditionPressParam : ConditionParamBase
    {
        public ConditionPressParam()
        {
            targetObjectPath = "";
            conditionType = (int)FrameConstDefine.ConditionType.FRAME_CONDITION_PRESS;
        }
        public override void Clear()
        {
            targetObjectPath = "";
        }

        public override void Recycle()
        {
            targetObjectPath = "";
        }
    }
    #endregion
    public class VoFrameParamManager :SingletonNotMono<VoFrameParamManager>
    {
        private ClassObjPoolReflectionWithKey<EventParamBase> _eventParamPool ;
        private ClassObjPoolReflectionWithKey<ConditionParamBase> _conditionParamPool ;

        public void RegisterFrameParam()
        {
            _eventParamPool = new ClassObjPoolReflectionWithKey<EventParamBase>();
            _conditionParamPool = new ClassObjPoolReflectionWithKey<ConditionParamBase>();
            _eventParamPool.AddReflectionClass(new EventCameraTargetParam());
            _eventParamPool.GetAllReflectClass();

            _conditionParamPool.AddReflectionClass(new ConditionPressParam());
            _conditionParamPool.GetAllReflectClass();
        }
        public void UnRegisterFrameParam()
        {
            _eventParamPool.Clear();
            _conditionParamPool.Clear();
        }
        public EventParamBase GetEventParam(int eventType)
        {
            return _eventParamPool.GetCloneObj(eventType);
        }
        public Type GetEventClassType(int eventType)
        {
            return _eventParamPool.GetReflectionType(eventType);
        }
        public void RecycleEventParam(EventParamBase eventParam)
        {
            if(eventParam != null)
            {
                _eventParamPool.RecycleCloneObj(eventParam.eventType, eventParam);
            }
        }
        public ConditionParamBase GetConditionParam(int conditionType)
        {
            return _conditionParamPool.GetCloneObj(conditionType);
        }
        public void RecycleConditionParam(ConditionParamBase conditionParam)
        {
            if(conditionParam != null)
            {
                _conditionParamPool.RecycleCloneObj(conditionParam.conditionType, conditionParam);
            }
        }
        public Type GetConditionClassType(int conditionType)
        {
            return _conditionParamPool.GetReflectionType(conditionType);
        }

    }
}
