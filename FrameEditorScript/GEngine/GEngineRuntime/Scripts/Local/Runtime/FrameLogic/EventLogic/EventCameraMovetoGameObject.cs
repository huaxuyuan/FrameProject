using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Runtime.FrameEventLogic
{
    [ClassPool((int)FrameConstDefine.EventType.FRAME_EVENT_CAMERAMOVE_TARGET)]
    public class EventCameraMovetoGameObject : BaseEventLogic
    {
        public override void DoEvent()
        {
            base.DoEvent();
            Debug.Log("camera move target " + frameEventData.eventParam.targetObjectPath);
        }
    }
}
