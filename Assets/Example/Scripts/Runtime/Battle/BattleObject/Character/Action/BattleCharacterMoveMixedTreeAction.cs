using System;
using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterMoveMixedActionData : ABattleCharacterActionData
    {
        public static BattleCharacterMoveMixedActionData Create()
        {
            return Create(() => new BattleCharacterMoveMixedActionData());
        }

        public override int ActionType => BattleCharacterMoveMixedTreeAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return currentAction.Accessor.Condition.Frame.CanMove.Current;
        }
    }
    
    public class BattleCharacterMoveMixedTreeAction : ABattleCharacterAction
    {
        private enum DirectionStatus
        {
            F,  // 前
            FR, // 前右
            R,  // 右
            BR, // 后右
            B,  // 后
            BL, // 后左
            L,  // 左
            FL  // 前左
        }

        private BattleCharacterMoveMixedActionData _actionData;

        public override ABattleCharacterActionData ActionData => _actionData;
        public const int ActionType = (int)BattleCharacterActionType.MoveMixed;

        private static readonly string[] AnimationNameArray = new []
        {
            "WalkLoopF",
            "WalkLoopFR",
            "WalkLoopR",
            "WalkLoopBR",
            "WalkLoopB",
            "WalkLoopBL",
            "WalkLoopL",
            "WalkLoopFL",
        };

        private DirectionStatus _directionStatus;
        
        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            _actionData = GetActionData<BattleCharacterMoveMixedActionData>();
            _directionStatus = CalcNewStatus();
        }

        public override void OnStart()
        {
            base.OnStart();

            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);
        }

        public override void OnUpdate(float deltaTime)
        {
            if (Accessor.Condition.IsMoving)
            {
                ChangeCurStatus(CalcNewStatus());
                UpdateMoveRotate();
                
                //如果是没有位移的动画 则代码控制位移
                if (!Animation.HasRootMotionMoveAnimation)
                {
                    HorizontalMove(deltaTime, Accessor.Condition.MoveDirection, 1f);
                }
            }
            else
            {
                EndActionCheck();
            }
        }

        public override void OnExit(AGfFsmState nextAction)
        {
            base.OnExit(nextAction);
        }

        private DirectionStatus CalcNewStatus()
        {
            var moveDirection = Accessor.Condition.MoveDirection;
            if (moveDirection.SqrMagnitude < 0.1F)
            {
                return _directionStatus;
            }
            
            var forward = Accessor.Entity.Transform.Forward.ToXZFloat2();
            var angle = GfFloat2.CalcAngle(forward, moveDirection);
            
            // 先调整角度
            float adjustedAngle = angle + 22.5f; // 调整后的角度，提前加上 22.5f

            // 确保角度在 [0, 360) 范围内
            if (adjustedAngle < 0) adjustedAngle += 360;

            // 计算该角度对应的方向索引
            int index = GfMathf.FloorToInt(adjustedAngle / 45f) % 8;

            // 根据计算结果返回对应的方向
            return (DirectionStatus)index;
        }

        private void ChangeCurStatus(DirectionStatus status)
        {
            if (_directionStatus == status)
            {
                return;
            }
            
            _directionStatus = status;
            
            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);
        }
        
        private int GetAnimationClipIndex()
        {
            string animationStateName = AnimationNameArray[(int)_directionStatus];
            return Animation.GetClipIndex(animationStateName);
        }
    }
}