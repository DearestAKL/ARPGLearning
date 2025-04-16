using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterIdleActionData : ABattleCharacterActionData
    {
        public static BattleCharacterIdleActionData Create()
        {
            return Create(() => new BattleCharacterIdleActionData());
        }

        public override int ActionType => BattleCharacterIdleAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            //return currentAction.Accessor.Condition.Frame.CanMove.Current;
            return true;
        }
    }
    
    public sealed class BattleCharacterIdleAction : ABattleCharacterAction
    {
        private BattleCharacterIdleActionData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;
        public const int ActionType = (int)BattleCharacterActionType.Idle;
        
        private static readonly string IdleBattleAnimationName = "Idle";

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);
            _actionData = GetActionData<BattleCharacterIdleActionData>();
        }
        
        public override void OnStart()
        {
            base.OnStart();
            
            AnimationClipIndex = GetAnimationClipIndex();
            
            if (!Animation.IsThatPlaying(AnimationClipIndex))
            {
                Animation.Play(AnimationClipIndex);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            // if (Accessor.Condition.IsMoving &&
            //     GfFloat2.Dot(Accessor.Entity.Transform.Forward.ToXZFloat2(), Accessor.Condition.MoveDirection) > GfMathf.Cos(30f * GfMathf.Deg2Rad)) 
            // {
            //     var turnActionData = BattleCharacterTurnActionData.Create();
            //     EndActionAndRequestForChange(turnActionData);
            // }
            EndActionCheck();
        }

        private int GetAnimationClipIndex()
        {
            return Animation.GetClipIndex(IdleBattleAnimationName);
        }
    }
}