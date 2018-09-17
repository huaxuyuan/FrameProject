using Runtime.FrameLogic;
using GameLogic.State;
using FrameLogicData;

namespace Runtime.FrameLogic.State
{
    class FrameInitializeState : BaseState<FrameLogicManager>
    {
        public override void Enter(FrameLogicManager type)
        {
            VoConfigDataManager.Instance.InitializeConfigData();
            type.SetGuiEnable(true);
        }
        public override void Exit(FrameLogicManager type)
        {

        }
    }
}
