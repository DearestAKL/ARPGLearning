using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed partial class BattleCharacterSpecialAttackActionData : ABattleCharacterActionData
    {
        public override int ActionType => BattleCharacterSpecialAttackAction.ActionType;

        public static BattleCharacterSpecialAttackActionData Create()
        {
            var data = Create(() => new BattleCharacterSpecialAttackActionData());
            return data;
        }

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            //Dash SpecialAttack Ultimate同一优先级，在CanAttack之上 这里使用CanDash的优先级
            return currentAction.Accessor.Condition.Frame.CanDash.Current;
        }
    }
    
    public class BattleCharacterSpecialAttackAction : ABattleCharacterLoopAttackAction
    {
        private BattleCharacterSpecialAttackActionData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;
        public const int ActionType = (int)BattleCharacterActionType.SpecialAttack;

        protected override string AttackStartAnimationName => "SpecialAttackStart";
        protected override string AttackLoopAnimationName => "SpecialAttack";
        protected override string AttackEndAnimationName => "SpecialAttackEnd";
        
        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            _actionData = GetActionData<BattleCharacterSpecialAttackActionData>();
            
            var coolTimeHandler = Accessor.Condition.GetTargetCoolTime(0);
            coolTimeHandler?.SubtractCoolTime(coolTimeHandler.SingleCoolTime);
        }
    }
}