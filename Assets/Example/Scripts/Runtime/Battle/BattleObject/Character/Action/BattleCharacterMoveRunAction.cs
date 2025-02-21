using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterMoveRunActionData : ABattleCharacterActionData
    {
        public BattleCharacterMoveRunAction.Status Status;
        
        public static BattleCharacterMoveRunActionData Create(BattleCharacterMoveRunAction.Status status = BattleCharacterMoveRunAction.Status.Start)
        {
            var data = Create(() => new BattleCharacterMoveRunActionData());
            data.Status = status;
            return data;
        }

        public override int ActionType => BattleCharacterMoveRunAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return currentAction.Accessor.Condition.Frame.CanMove.Current;
        }
    }

    public sealed class BattleCharacterMoveRunAction : ABattleCharacterAction
    {
        public enum Status
        {
            Start,
            Loop,
            End
        }

        private BattleCharacterMoveRunActionData _actionData;

        public override ABattleCharacterActionData ActionData => _actionData;
        public const int ActionType = (int)BattleCharacterActionType.MoveRun;
        
        private static readonly string RunStartAnimationName = "RunStart";
        private static readonly string RunLoopAnimationName = "RunLoop";
        private static readonly string RunEndAnimationName = "RunEnd";

        private Status _status;

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            _actionData = GetActionData<BattleCharacterMoveRunActionData>();
            
            _status = _actionData.Status;
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
                        Rotate(Accessor.Condition.MoveDirection);
                        if (!Animation.IsThatPlaying(AnimationClipIndex))
                        {
                            ChangeStatus(Status.Loop);
                        }
                    }
                    else
                    {
                        EndActionAndRequestForChange(new BattleCharacterIdleActionData());
                    }
                    break;
                case Status.Loop:
                    if (Accessor.Condition.IsMoving)
                    {
                        Rotate(Accessor.Condition.MoveDirection);
                        if (Accessor.Condition.IsDashHolding)
                        {
                            EndActionAndRequestForChange(new BattleCharacterMoveSprintActionData());
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
        }

        private int GetAnimationClipIndex()
        {
            string animationStateName = "";
            switch (_status)
            {
                case Status.Start:
                    animationStateName = RunStartAnimationName;
                    break;
                case Status.Loop:
                    animationStateName = RunLoopAnimationName; 
                    break;
                case Status.End:
                    animationStateName = RunEndAnimationName;
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
