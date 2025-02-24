using System;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterMoveSprintActionData : ABattleCharacterActionData
    {
        public float DurationTime;
        
        public static BattleCharacterMoveSprintActionData Create(float durationTime = 3F)
        {
            var data = Create(() => new BattleCharacterMoveSprintActionData());
            data.DurationTime = durationTime;
            return data;
        }

        public override int ActionType => BattleCharacterMoveSprintAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return currentAction.Accessor.Condition.Frame.CanMove.Current;
        }
    }

    public sealed class BattleCharacterMoveSprintAction : ABattleCharacterAction
    {
        private enum Status
        {
            Loop,
            End
        }

        private BattleCharacterMoveSprintActionData mActionData;
        
        private float _elapsedTime;
        private float _durationTime;

        public override ABattleCharacterActionData ActionData => mActionData;
        public const int ActionType = (int)BattleCharacterActionType.MoveSprint;

        private static readonly string SprintLoopAnimationName = "SprintLoop";
        private static readonly string SprintEndAnimationName ="SprintEnd";
        
        private Status _status;

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            mActionData = GetActionData<BattleCharacterMoveSprintActionData>();
            
            _status = Status.Loop;

            _elapsedTime = 0;
            _durationTime = Accessor.Condition.IsDashHolding ? int.MaxValue : mActionData.DurationTime;
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
            
            deltaTime *= Animation.Speed;
            
            // if (EndActionCheck(false))
            // {
            //     return;
            // }

            switch (_status)
            {
                case Status.Loop:
                    if (Accessor.Condition.IsMoving)
                    {
                        _elapsedTime += deltaTime;
                        if (_elapsedTime >= _durationTime)
                        {
                            if (Accessor.Condition.IsDashHolding)
                            {
                                _durationTime = int.MaxValue;
                            }
                            else
                            {
                                EndActionAndRequestForChange(BattleCharacterMoveRunActionData.Create(BattleCharacterMoveRunAction.Status.Loop));
                                return;
                            }
                        }
                        
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
                        EndActionAndRequestForChange(BattleCharacterMoveRunActionData.Create());
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
                HorizontalMove(deltaTime, Accessor.Condition.MoveDirection, 4.5f);
            }
        }

        public override void OnExit(AGfFsmState nextAction)
        {
            base.OnExit(nextAction);
        }

        private int GetAnimationClipIndex()
        {
            string animationStateName = "";
            switch (_status)
            {
                case Status.Loop:
                    animationStateName = SprintLoopAnimationName; 
                    break;
                case Status.End:
                    animationStateName = SprintEndAnimationName;
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
