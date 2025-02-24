using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterMoveWalkActionData : ABattleCharacterActionData
    {
        public static BattleCharacterMoveWalkActionData Create()
        {
            return Create(() => new BattleCharacterMoveWalkActionData());
        }

        public override int ActionType => BattleCharacterMoveWalkAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return currentAction.Accessor.Condition.Frame.CanMove.Current;
        }
    }

    public sealed class BattleCharacterMoveWalkAction : ABattleCharacterAction
    {
        private enum Status
        {
            Start,
            Loop,
            End
        }

        private BattleCharacterMoveWalkActionData mActionData;

        public override ABattleCharacterActionData ActionData => mActionData;
        public const int ActionType = (int)BattleCharacterActionType.MoveWalk;
        
        private static readonly string WalkStartAnimationName = "WalkStart";
        private static readonly string WalkLoopAnimationName = "WalkLoop";
        private static readonly string WalkEndAnimationName = "WalkEnd";
        
        private Status mStatus;

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            mActionData = GetActionData<BattleCharacterMoveWalkActionData>();
            
            mStatus = Status.Start;
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

            switch (mStatus)
            {
                case Status.Start:
                    if (Accessor.Condition.IsMoving)
                    {
                        Rotate(Accessor.Condition.MoveDirection);
                        if (!Animation.IsThatPlaying(AnimationClipIndex))
                        {
                            ChangeStatus(Status.Loop);
                        }
                    }
                    else
                    {
                        EndActionAndRequestForChange(BattleCharacterIdleActionData.Create());
                    }
                    break;
                case Status.Loop:
                    if (Accessor.Condition.IsMoving)
                    {
                        Rotate(Accessor.Condition.MoveDirection);
                    }
                    else
                    {
                        ChangeStatus(Status.End);  
                    }
                    break;
                case Status.End:
                    if (Accessor.Condition.IsMoving)
                    {
                        ChangeStatus(Status.Start);
                    }
                    else
                    {
                        if (!Animation.IsThatPlaying(AnimationClipIndex))
                        {
                            var nextActionData = BattleCharacterIdleActionData.Create();
                            EndActionAndRequestForChange(nextActionData);
                        }
                    }
                    break;
            }
            
            //如果是没有位移的动画 则代码控制位移
            if (!Animation.HasRootMotionMoveAnimation)
            {
                HorizontalMove(deltaTime, Accessor.Condition.MoveDirection, 1.5f);
            }
        }

        private int GetAnimationClipIndex()
        {
            string animationStateName = "";
            switch (mStatus)
            {
                case Status.Start:
                    animationStateName = WalkStartAnimationName;
                    break;
                case Status.Loop:
                    animationStateName = WalkLoopAnimationName; 
                    break;
                case Status.End:
                    animationStateName = WalkEndAnimationName;
                    break;
            }
            
            return Animation.GetClipIndex(animationStateName);
        }

        private void ChangeStatus(Status status)
        {
            if (mStatus == status)
            {
                return;
            }
        
            mStatus = status;
            
            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);
        }
    }
}
