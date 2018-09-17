using FrameLogicData;
using UnityEngine;
using FrameEditorLogic;
namespace FrameEditor
{
    public class EditorFrameEventViewBase : IClassObjWithoutKey
    {
        public int eventType;

        public VoFrameEventData frameEventData;
        public GameObject targetObj;
        public void OnEnter(VoFrameEventData eventData)
        {
            frameEventData = eventData;
            string targetPath = eventData.eventParam.targetObjectPath;
            if (targetPath == null || targetPath == "")
            {
                targetObj = null;
            }
            else
            {
                targetObj = GameObject.Find(targetPath);
            }
        }
        public virtual void Recycle()
        {
            
        }
        public virtual void Clear()
        {

        }
        public virtual void UIDraw()
        {

        }
        public virtual void SaveData()
        {
            if(targetObj != null)
            {
                Debug.Log("targetObj != null");
                frameEventData.eventParam.targetObjectPath = EditorFrameDetailLogic.Instance.GetRendererPath(targetObj);
            }
            else
            {
                
                frameEventData.eventParam.targetObjectPath = null;
            }
        }
    }
}
