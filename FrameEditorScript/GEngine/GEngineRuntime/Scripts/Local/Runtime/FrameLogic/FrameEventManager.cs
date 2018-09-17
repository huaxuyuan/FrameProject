using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameLogicData;
using Runtime.FrameEventLogic;
namespace Runtime.FrameLogic
{
    public class FrameEventManager:SingletonNotMono<FrameEventManager>
    {
        private ClassObjPoolReflectionWithKey<BaseEventLogic> _eventPool;
        public void InitializeFrameEvent()
        {
            _eventPool = new ClassObjPoolReflectionWithKey<BaseEventLogic>();
            _eventPool.AddReflectionClass(new EventCameraMovetoGameObject());
            _eventPool.GetAllReflectClass();
        }
        public BaseEventLogic GetEventLogic(int eventType)
        {
            return _eventPool.GetCloneObj(eventType);
        }
        public void RecycleEventLogic(BaseEventLogic eventLogic)
        {
            if (eventLogic == null)
                return;
            _eventPool.RecycleCloneObj(eventLogic.eventType, eventLogic);
        }
    }
}
namespace Runtime.FrameEventLogic
{
    public class BaseEventLogic : IClassObjWithoutKey
    {
        public int eventType;
        public VoFrameEventData frameEventData;
        public void InitializeEventLogic(VoFrameEventData param)
        {
            frameEventData = param;
        }
        public virtual void DoEvent()
        {

        }
        public virtual void ExitEvent()
        {

        }
        public virtual void Clear()
        {
            
        }

        public virtual void Recycle()
        {
            
        }
    }

}
