using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed partial class BattleCharacterLightAttackActionData : ABattleCharacterActionData
    {
        public override int ActionType => BattleCharacterLightAttackAction.ActionType;

        public static BattleCharacterLightAttackActionData Create()
        {
            var data = Create(() => new BattleCharacterLightAttackActionData());
            return data;
        }

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return currentAction.Accessor.Condition.Frame.CanAttack.Current;
        }
    }

    public sealed class BattleCharacterLightAttackAction : ABattleCharacterAction
    {
        private BattleCharacterLightAttackActionData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;
        private uint _attackNumber = 0;

        public const int ActionType = (int)BattleCharacterActionType.LightAttack;
        
        private static readonly string AttackAnimationName = "LightAttack";

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);
            
            _actionData = GetActionData<BattleCharacterLightAttackActionData>();

            if (prevAction == this)
            {
                _attackNumber++;
                if (!HasAnimationClipIndex()) 
                {
                    _attackNumber = 1;
                }
            }
            else 
            {
                _attackNumber = 1;
            }

            // 根据敌人位置 自动调整方向
            AttackAutoRotate();
        }
        
        public override void OnStart()
        {
            base.OnStart();
            
            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            EndActionCheck();
        }
        
        private int GetAnimationClipIndex()
        {
            var animationStateName = $"{AttackAnimationName}_{_attackNumber}";
            
            Entity.Request(new ChangeAnimationRequest(animationStateName));
            
            return Animation.GetClipIndex(animationStateName);
        }
        
        private bool HasAnimationClipIndex()
        {
            var animationStateName = $"{AttackAnimationName}_{_attackNumber}";
            return Animation.HasClipIndex(animationStateName);
        }
    }
}