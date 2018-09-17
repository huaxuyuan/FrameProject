using FrameLogicData;
using UnityEngine;
namespace Runtime.FrameConditionLogic
{
    [ClassPool((int)FrameConstDefine.ConditionType.FRAME_CONDITION_PRESS)]
    public class ConditionPressLogic : BaseConditionLogic
    {
        public override void InitializeCondition(VoFrameDetailData conditionParam, CallbackDelegate<BaseConditionLogic> conditionCallBack)
        {
            base.InitializeCondition(conditionParam, conditionCallBack);
            Debug.Log("condition press initialize");
            ConditionFinish();
            
        }
    }
}
