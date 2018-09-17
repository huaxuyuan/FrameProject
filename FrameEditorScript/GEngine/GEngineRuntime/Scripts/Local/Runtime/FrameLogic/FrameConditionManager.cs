using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameLogicData;
using Runtime.FrameConditionLogic;
namespace Runtime.FrameLogic
{
    public class FrameConditionManager : SingletonNotMono<FrameConditionManager>
    {
        private ClassObjPoolReflectionWithKey<BaseConditionLogic> _conditionPool;
        public void InitializeFrameCondition()
        {
            _conditionPool = new ClassObjPoolReflectionWithKey<BaseConditionLogic>();
            _conditionPool.AddReflectionClass(new ConditionPressLogic());
            _conditionPool.GetAllReflectClass();
        }
        public BaseConditionLogic GetConditionLogic(int conditionType)
        {
            return _conditionPool.GetCloneObj(conditionType);
        }
        public void RecycleConditionLogic(BaseConditionLogic conditionLogic)
        {
            if (conditionLogic == null)
                return;
            _conditionPool.RecycleCloneObj(conditionLogic.conditionType, conditionLogic);
        }
    }
}
namespace Runtime.FrameConditionLogic
{
    public class BaseConditionLogic : IClassObjWithoutKey
    {
        public int conditionType;
        private CallbackDelegate<BaseConditionLogic> _conditionCallBack;
        public VoFrameDetailData currentDetailData;
        public virtual void InitializeCondition(VoFrameDetailData conditionParam,
                                                CallbackDelegate<BaseConditionLogic> conditionCallBack)
        {
            currentDetailData = conditionParam;
            _conditionCallBack = conditionCallBack;
        }

        public virtual void ConditionValid()
        {

        }
        public virtual void ConditionNotValid()
        {

        }
        public virtual void ConditionFinish()
        {
            _conditionCallBack(this);
        }
        public virtual void Clear()
        {
            
        }

        public virtual void Recycle()
        {
            
        }


    }
}
