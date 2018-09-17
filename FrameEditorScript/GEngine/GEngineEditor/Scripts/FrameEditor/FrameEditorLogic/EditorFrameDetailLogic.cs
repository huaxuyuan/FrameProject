using UnityEngine;
using FrameEditor;
using FrameLogicData;
namespace FrameEditorLogic
{
    public class EditorFrameDetailLogic : SingletonNotMono<EditorFrameDetailLogic>
    {
        private ClassObjPoolReflectionWithKey<EditorFrameConditionViewBase> conditionReflectPool;
        private ClassObjPoolReflectionWithKey<EditorFrameEventViewBase> eventReflectPool;
        public void RegisterLogic()
        {
            conditionReflectPool = new ClassObjPoolReflectionWithKey<EditorFrameConditionViewBase>();
            conditionReflectPool.AddReflectionClass(new EditorConditionPressView());

            eventReflectPool = new ClassObjPoolReflectionWithKey<EditorFrameEventViewBase>();
            eventReflectPool.AddReflectionClass(new EditorEventCameraWithGameObjectView());

            conditionReflectPool.GetAllReflectClass();
            eventReflectPool.GetAllReflectClass();
        }

        public void UnRegisterLogic()
        {
            eventReflectPool.Clear();
            conditionReflectPool.Clear();
        }
        public EditorFrameConditionViewBase GetEditorConditionView(int conditionType)
        {
            return conditionReflectPool.GetCloneObj(conditionType);
        }
        public EditorFrameEventViewBase GetEditorEventView(int eventType)
        {
            return eventReflectPool.GetCloneObj(eventType);
        }

        public void RecycleConditionView(EditorFrameConditionViewBase conditionView)
        {
            if (conditionView == null)
                return;
            conditionReflectPool.RecycleCloneObj(conditionView.conditionType, conditionView);
        }
        public void RecycleEventView(EditorFrameEventViewBase eventView)
        {
            eventReflectPool.RecycleCloneObj(eventView.eventType, eventView);
        }

        private VoFrameDetailData _currentFrameDetailData;
        public void SetFrameDetailData(VoFrameDetailData frameDetailData)
        {
            _currentFrameDetailData = frameDetailData;
        }
        public VoFrameConditionData AddCondition(int conditionType)
        {
            if(_currentFrameDetailData == null)
            {
                Debug.LogError("_current frame detail data null");
                return null;
            }
            return _currentFrameDetailData.AddFrameConditionData(conditionType);
        }

        public VoFrameEventData AddEvent(int eventType)
        {
            if (_currentFrameDetailData == null)
            {
                Debug.LogError("_current frame detail data null");
                return null;
            }
            return _currentFrameDetailData.AddFrameEventData(eventType);
        }
        public string GetConditionMenuStr(int conditionType)
        {
            //switch
            switch(conditionType)
            {
                case (int)FrameConstDefine.ConditionType.FRAME_CONDITION_PRESS:
                    return "点击事件";
            }
            return "null";
        }
        public string GetEventMenuStr(int eventType)
        {
            switch(eventType)
            {
                case (int)FrameConstDefine.EventType.FRAME_EVENT_CAMERAMOVE_TARGET:
                    return "相机类型/相机移动目标 ";
            }
            return "null";
        }
        public string GetEventShowStr(int eventType)
        {
            switch (eventType)
            {
                case (int)FrameConstDefine.EventType.FRAME_EVENT_CAMERAMOVE_TARGET:
                    return "相机移动目标 ";
            }
            return "null";
        }

        #region util logic
        public string GetRendererPath(GameObject obj)
        {
            if (obj == null) return "";

            string rendererPath = obj.name;
            Transform parent = obj.transform.parent;
            if (parent == null)
                return rendererPath;
            rendererPath = "/" + parent.name + "/" + rendererPath;
            parent = parent.parent;
            while (parent != null)
            {
                rendererPath = "/" + parent.name + rendererPath;
                parent = parent.parent;
            }
            return rendererPath;
        }
        #endregion

    }
}
