using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameEditorLogic;
using FrameLogicData;
using UnityEngine;

namespace FrameEditor
{
    public class EditorFrameConditionViewBase : IClassObjWithoutKey
    {
        public int conditionType;
        public VoFrameConditionData frameConditionData;
        public GameObject targetObj;
        public EditorFrameConditionViewBase()
        {

        }
        public void OnEnter(VoFrameConditionData conditionData)
        {
            frameConditionData = conditionData;
            string targetPath = frameConditionData.conditionParam.targetObjectPath;
            if (targetPath == null || targetPath == "")
            {
                targetObj = null;
            }
            else
            {
                targetObj = GameObject.Find(targetPath);
            }
        }
        public virtual void UIDraw()
        {
            if (frameConditionData == null)
                return;
        }
        public virtual void SaveData()
        {
            if (targetObj != null)
            {
                Debug.Log("targetObj != null");
                frameConditionData.conditionParam.targetObjectPath = EditorFrameDetailLogic.Instance.GetRendererPath(targetObj);
            }
            else
            {

                frameConditionData.conditionParam.targetObjectPath = null;
            }
        }
        public virtual void Recycle()
        {

        }
        public virtual void Clear()
        {

        }
    }
}
