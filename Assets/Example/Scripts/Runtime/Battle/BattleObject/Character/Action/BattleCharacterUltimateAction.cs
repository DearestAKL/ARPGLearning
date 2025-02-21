using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed partial class BattleCharacterUltimateActionData : ABattleCharacterActionData
    {
        public override int ActionType => BattleCharacterUltimateAction.ActionType;

        public static BattleCharacterUltimateActionData Create()
        {
            var data = Create(() => new BattleCharacterUltimateActionData());
            return data;
        }

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return currentAction.Accessor.Condition.Frame.CanAttack.Current;
        }
    }
    
    public class BattleCharacterUltimateAction : ABattleCharacterLoopAttackAction
    {
        private BattleCharacterUltimateActionData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;
        public const int ActionType = (int)BattleCharacterActionType.Ultimate;
        protected override string AttackStartAnimationName => "UltimateStart";
        protected override string AttackLoopAnimationName => "Ultimate";
        protected override string AttackEndAnimationName => "UltimateEnd";
        
        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            _actionData = GetActionData<BattleCharacterUltimateActionData>();

            var coolTimeHandler = Accessor.Condition.GetTargetCoolTime(1);
            coolTimeHandler?.SubtractCoolTime(coolTimeHandler.SingleCoolTime);
        }
    }
}