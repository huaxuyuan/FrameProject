
using FrameEditorLogic;
using FrameLogicData;
using UnityEditor;
using UnityEngine;

namespace FrameEditor
{
    [ClassPool((int)FrameConstDefine.EventType.FRAME_EVENT_CAMERAMOVE_TARGET)]
    public class EditorEventCameraWithGameObjectView : EditorFrameEventViewBase
    {
        
        public EditorEventCameraWithGameObjectView()
        {
            eventType = (int)FrameConstDefine.EventType.FRAME_EVENT_CAMERAMOVE_TARGET;
        }
        public override void UIDraw()
        {
            base.UIDraw();
            GameObject renderTemp = EditorGUILayout.ObjectField("Renderer", targetObj, typeof(GameObject), true) as GameObject;
            if (renderTemp != targetObj)
                targetObj = renderTemp;
        }
        public override void Clear()
        {
            base.Clear();
        }
        public override void Recycle()
        {
            base.Recycle();
        }
    }
}
