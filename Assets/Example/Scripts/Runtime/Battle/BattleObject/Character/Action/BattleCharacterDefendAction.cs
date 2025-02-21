using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterDefendActionData : ABattleCharacterActionData
    {
        public static BattleCharacterDefendActionData Create()
        {
            return Create(() => new BattleCharacterDefendActionData());
        }

        public override int ActionType => BattleCharacterDefendAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            //return currentAction.Accessor.Condition.Frame.CanDash.Current;
            return true;
        }
    }

    public sealed class BattleCharacterDefendAction : ABattleCharacterAction
    {
        private BattleCharacterDefendActionData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;

        public const int ActionType = (int)BattleCharacterActionType.Defend;
        
        private static readonly string DefendAnimationName = "Defend";
        
        // private const float     DefaultDurationTime = 0.5f;
        //
        // private float _elapsedTime;

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            _actionData = GetActionData<BattleCharacterDefendActionData>();
            
        }
        
        public override void OnStart()
        {
            base.OnStart();
            
            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);
        }

        public override void OnUpdate(float deltaTime)
        {
            // if (EndActionCheck(!Accessor.IsLongFire))
            // {
            //     return;
            // }
            //
            // if (Accessor.IsMoving)
            // {
            //     Rotate(Accessor.Direction);
            // }
        }

        private int GetAnimationClipIndex()
        {
            return Animation.GetClipIndex(DefendAnimationName);
        }
    }
}