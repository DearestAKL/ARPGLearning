using Akari.GfCore;

namespace GameMain.Runtime
{
    /// <summary>
    /// 非自由跳跃 而是点对点的跳跃 注意了
    /// </summary>
    public sealed class BattleCharacterJumpToActionData : ABattleCharacterActionData
    {
        public GfFloat3 TargetPos;
        
        public static BattleCharacterJumpToActionData Create(GfFloat3 targetPos)
        {
            var data = new BattleCharacterJumpToActionData();
            data.TargetPos = targetPos;
            return data;
        }

        public override int ActionType => BattleCharacterJumpToAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return currentAction.Accessor.Condition.Frame.CanDash.Current;
        }
    }
    
    public class BattleCharacterJumpToAction : ABattleCharacterAction
    {
        private enum Status
        {
            Jump,
            Fall,
            Land
        }
        
        private BattleCharacterJumpToActionData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;
        public const int ActionType = (int)BattleCharacterActionType.JumpTo;
        
        //Jump=>Fall=>Land
        private static readonly string JumpAnimationName = "Jump";//整个Jump动画是起跳=》悬空=》下落这个流程 完整流程刚好y轴归0
        private static readonly string FallAnimationName = "Fall";
        private static readonly string LandAnimationName = "Land";
        
        private float _elapsedTime;
        
        //public float _jumpHeight = 2f;
        
        private Status _status;
        private float _jumpDuration;
        private float _horizontalVelocity;
        private float _verticalVelocity;
        private GfFloat2 _direction;
        private float _jumpDistance;
        private bool _jumpStart = false;

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);

            _actionData = GetActionData<BattleCharacterJumpToActionData>();
            _status = Status.Jump;
            _jumpStart = false;
            _elapsedTime = 0f;
            Entity.Request(new SetJumpToEnableRequest(true));
            
            var vector = (_actionData.TargetPos - Accessor.Entity.Transform.Position).ToXZFloat2();
            _jumpDistance = vector.Magnitude;
            _direction = vector.Normalized;
            
            Rotate(_direction);
        }
        
        public override void OnStart()
        {
            base.OnStart();
            
            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);
        }

        private void JumpStart()
        {
            _jumpDuration = Animation.PlayingClipInfo.Length - _elapsedTime;
            _jumpStart = true;
            
            _horizontalVelocity = _jumpDistance / _jumpDuration;
            _verticalVelocity = -BattleDef.Gravity * (_jumpDuration / 2);
        }

        public override void OnExit(AGfFsmState nextAction)
        {
            base.OnExit(nextAction);
            Entity.Request(new SetJumpToEnableRequest(false));
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            switch (_status)
            {
                case Status.Jump:
                    //起跳
                    if (!_jumpStart && Accessor.Condition.Frame.IsJump.Current)
                    {
                        JumpStart();
                    }

                    if (_jumpStart)
                    {
                        Jump(deltaTime);
                        if (_verticalVelocity <= 0f && Accessor.Entity.Transform.Position.Y - _actionData.TargetPos.Y < 0.1F)
                        {
                            ChangeStatus(Status.Land);
                        }
                    }
                    
                    if (!Animation.IsThatPlaying(AnimationClipIndex))
                    {
                        ChangeStatus(Status.Fall);
                    }
                    break;
                case Status.Fall:
                    //下落
                    Fall(deltaTime);
                    if (Accessor.Entity.Transform.Position.Y - _actionData.TargetPos.Y < 0.1F)
                    {
                        ChangeStatus(Status.Land);
                    }
                    break;
                case Status.Land:
                    if (!Animation.IsThatPlaying(AnimationClipIndex))
                    {
                        var nextActionData = BattleCharacterIdleActionData.Create();
                        EndActionAndRequestForChange(nextActionData);
                    }
                    break;
            }
            
            _elapsedTime += deltaTime;
        }

        private int GetAnimationClipIndex()
        {
            string animationStateName = "";
            switch (_status)
            {
                case Status.Jump:
                    animationStateName = JumpAnimationName;
                    break;
                case Status.Fall:
                    animationStateName = FallAnimationName; 
                    break;
                case Status.Land:
                    animationStateName = LandAnimationName;
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
        
        private void Jump(float deltaTime)
        {
            HorizontalMove(deltaTime, _direction, _horizontalVelocity);
            
            _verticalVelocity += BattleDef.Gravity * deltaTime;
            VerticalMove(deltaTime, _verticalVelocity);
        }

        private void Fall(float deltaTime)
        {
            _verticalVelocity += BattleDef.Gravity * deltaTime;
            VerticalMove(deltaTime, _verticalVelocity);
        }
    }
}