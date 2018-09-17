using FrameEditorLogic;
using FrameLogicData;
using UnityEditor;
using UnityEngine;

namespace FrameEditor
{
    [ClassPool((int)FrameConstDefine.ConditionType.FRAME_CONDITION_PRESS)]
    public class EditorConditionPressView : EditorFrameConditionViewBase
    {
        public EditorConditionPressView()
        {
            conditionType = (int)FrameConstDefine.ConditionType.FRAME_CONDITION_PRESS;
        }
        public override void UIDraw()
        {
            base.UIDraw();
            GameObject renderTemp = EditorGUILayout.ObjectField("Renderer", targetObj, typeof(GameObject), true) as GameObject;
            if (renderTemp != targetObj)
                targetObj = renderTemp;
        }
        public override void Recycle()
        {
            base.Recycle();
        }
        public override void Clear()
        {
            base.Clear();
        }
    }
}
