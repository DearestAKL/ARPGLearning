using Akari.GfCore;

namespace GameMain.Runtime
{

    public class BattleCharacterPlayAnimationStateActionData : ABattleCharacterActionData
    {
        public string AnimationStateName;
        
        public override int ActionType => BattleCharacterPlayAnimationStateAction.ActionType;
        
        public static BattleCharacterPlayAnimationStateActionData Create(string animationStateName)
        {
            var data = Create(() => new BattleCharacterPlayAnimationStateActionData());
            data.AnimationStateName = animationStateName;
            return data;
        }

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return currentAction.Accessor.Condition.Frame.CanAttack.Current;
        }
    }


    public class BattleCharacterPlayAnimationStateAction : ABattleCharacterAction
    {
        private BattleCharacterPlayAnimationStateActionData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;
        
        public const    int                        ActionType = (int)BattleCharacterActionType.PlayAnimationState;
        
        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            _actionData = GetActionData<BattleCharacterPlayAnimationStateActionData>();
        }
        
        public override void OnStart()
        {
            base.OnStart();
            
            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);
            
            AttackAutoRotate();
        }
        
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            EndActionCheck();
        }
        
        private int GetAnimationClipIndex()
        {
            return Animation.GetClipIndex(_actionData.AnimationStateName);
        }
    }
}