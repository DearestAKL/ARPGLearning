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

        private BattleCharacterMoveWalkActionData _actionData;

        public override ABattleCharacterActionData ActionData => _actionData;
        public const int ActionType = (int)BattleCharacterActionType.MoveWalk;
        
        private static readonly string WalkStartAnimationName = "WalkStartF";
        private static readonly string WalkLoopAnimationName = "WalkLoopF";
        private static readonly string WalkEndAnimationName = "WalkEndF";
        
        private Status _status;

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            _actionData = GetActionData<BattleCharacterMoveWalkActionData>();
            
            _status = Status.Start;
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

            switch (_status)
            {
                case Status.Start:
                    if (Accessor.Condition.IsMoving)
                    {
                        UpdateMoveRotate();
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
                        UpdateMoveRotate();
                        if (Accessor.Condition.IsDashHolding)
                        {
                            EndActionAndRequestForChange(BattleCharacterMoveSprintActionData.Create());
                        }
                        else if (!Accessor.Condition.IsWalk)
                        {
                            EndActionAndRequestForChange(BattleCharacterMoveRunActionData.Create());
                        }
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
                HorizontalMove(deltaTime, Accessor.Condition.MoveDirection, 1f);
            }
        }

        private int GetAnimationClipIndex()
        {
            string animationStateName = "";
            switch (_status)
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
            if (_status == status)
            {
                return;
            }
        
            _status = status;
            
            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);
        }
    }
}
