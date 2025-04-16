using Akari.GfCore;

namespace GameMain.Runtime
{
    public class BtCharacterPlayAnimationStateAction : ABtCharacterAction
    {
        private readonly string _animationStateName;
        
        public BtCharacterPlayAnimationStateAction(string animationStateName) : base("BtCharacterPlayAnimationStateAction")
        {
            _animationStateName = animationStateName;
        }

        protected override void DoStart()
        {
            var actionData = BattleCharacterPlayAnimationStateActionData.Create(_animationStateName);
            SendRequest(actionData);

            Clock.AddTimer(0.1f, 0f, -1, OnUpdateTimer);
            
            GfLog.Debug("PlayAnimationState开始");
        }

        protected override void DoStop()
        {
            Clock.RemoveTimer(OnUpdateTimer);
            
            Stopped(true);
            
            GfLog.Debug("PlayAnimationState结束");
        }
        
        private void OnUpdateTimer()
        {
            //if (Accessor.Action.GetNowActionId() != (int)BattleCharacterActionType.PlayAnimationState)
            if (Accessor.Condition.Frame.CanAttack.Current)
            {
                Stop();
            }
        }
    }
}