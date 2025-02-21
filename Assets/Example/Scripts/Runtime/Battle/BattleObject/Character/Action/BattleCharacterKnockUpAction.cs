using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterKnockUpActionData : ABattleCharacterActionData
    {
        public GfFloat2 DamageVector;
        public float HorizontalVelocity;
        public float VerticalVelocity;

        public static BattleCharacterKnockUpActionData Create(GfFloat2 damageVector,float horizontalVelocity,float verticalVelocity)
        {
            var data = Create(() => new BattleCharacterKnockUpActionData());
            data.DamageVector = damageVector;
            data.HorizontalVelocity = horizontalVelocity;
            data.VerticalVelocity = verticalVelocity;

            return data;
        }

        public override int ActionType => BattleCharacterKnockUpAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            return true;
        }
    }
    
    public class BattleCharacterKnockUpAction : ABattleCharacterAction
    {
        private enum Status
        {
            KnockUpStart,
            KnockUpLoop,
            Land,
            Lying,
            Stand
        }
        
        private BattleCharacterKnockUpActionData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;
        public const int ActionType = (int)BattleCharacterActionType.KnockUp;

        private static readonly string KnockUpStartAnimationName     = "KnockUpStart";
        private static readonly string KnockUpLoopAnimationName = "KnockUpLoop";
        private static readonly string KnockUpLandAnimationName      = "KnockUpLand";
        private static readonly string LyingAnimationName     = "LyingLoop";
        private static readonly string GetUpAnimationName = "GetUp";
        
        private Status _status;
        
        private float _elapsedTime;

        private float _lyingRemainedTime;
        private const float     LyingDurationTime = 0.5f;
        
        private float _curHorizontalVelocity;
        private float _curVerticalVelocity;
        private bool _hasHorizontalVelocity;
        private bool _hasVerticalVelocity;

        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);
            _actionData = GetActionData<BattleCharacterKnockUpActionData>();

            _status = Status.KnockUpStart;
            Rotate(_actionData.DamageVector * -1);

            _hasHorizontalVelocity = _actionData.HorizontalVelocity > 0f;
            _hasVerticalVelocity = _actionData.VerticalVelocity > 0f;

            _elapsedTime = 0f;
            
            _lyingRemainedTime = LyingDurationTime;
        }

        public override void OnStart()
        {
            base.OnStart();
            AnimationClipIndex = GetAnimationClipIndex();
            Animation.Play(AnimationClipIndex);
        }

        public override void OnUpdate(float deltaTime)
        {
            deltaTime *= Animation.Speed;
            
            switch (_status)
            {
                case Status.KnockUpStart:
                    Fly(deltaTime);
                    if (!Animation.IsThatPlaying(AnimationClipIndex))
                    {
                        if (Accessor.Transform.IsGrounded)
                        {
                            ChangeStatus(Status.Land);
                        }
                        else
                        {
                            ChangeStatus(Status.KnockUpLoop);
                        }
                    }
                    break;
                case Status.KnockUpLoop:
                    Fly(deltaTime);
                    if (Accessor.Transform.IsGrounded)
                    {
                        ChangeStatus(Status.Land);
                    }

                    break;
                case Status.Land:
                    if (!Animation.IsThatPlaying(AnimationClipIndex))
                    {
                        ChangeStatus(Status.Lying);
                    }
                    break;
                case Status.Lying:
                    _lyingRemainedTime -= deltaTime;
                    if (_lyingRemainedTime <= 0)
                    {
                        ChangeStatus(Status.Stand);
                    }
                    break;
                case Status.Stand:
                    if (Animation.IsThatPlaying(AnimationClipIndex))
                    {
                        return;
                    }
                    EndActionCheck();
                    break;
            }

            _elapsedTime += deltaTime;
        }

        private int GetAnimationClipIndex()
        {
            string animationStateName = string.Empty;
            switch (_status)
            {
                case Status.KnockUpStart:
                    animationStateName = KnockUpStartAnimationName;
                    break;
                case Status.KnockUpLoop:
                    animationStateName = KnockUpLoopAnimationName;
                    break;
                case Status.Land:
                    animationStateName = KnockUpLandAnimationName;
                    break;
                case Status.Lying:
                    animationStateName = LyingAnimationName;
                    break;
                case Status.Stand:
                    animationStateName = GetUpAnimationName;
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

        private void Fly(float deltaTime)
        {
            if (_hasHorizontalVelocity)
            {
                //空中
                //如果水平速度数值 > 0，则保持击退效果，水平速度计算如下
                //水平速度 = (水平初速度的1/3次方 - 0.5*时间)的三次方
                var horizontalVelocity = GfMathf.Pow(GfMathf.Pow(_actionData.HorizontalVelocity, 1f / 3f) - 0.5f * _elapsedTime, 3);

                if (horizontalVelocity <= 0f)
                {
                    _hasHorizontalVelocity = false;
                }
                else
                {
                    HorizontalMove(deltaTime, _actionData.DamageVector, horizontalVelocity);
                }
            }

            if (_hasVerticalVelocity)
            {
                //如果竖直速度数值 > 0 (即方向为Y轴向上)，则会无视重力加速度
                //如果竖直速度数值 <= 0 (即方向为Y轴向下)，则需考虑重力加速度
                //竖直速度 > 0:竖直速度=(竖直初速度的1/3次方 - 2.5*时间)的三次方
                //竖直速度 <= 0:竖直速度=(竖直初速度的1/3次方 - 2.5*时间)的三次方 + 实际重力加速度*时间
                //这套减速太慢了
                //var verticalVelocity = GfMathf.Pow(GfMathf.Pow(mActionData.HorizontalVelocity, 1f / 3f) - 2.5f * mElapsedTime * FixedDeltaTime, 3);
                // if (verticalVelocity <= 0F)
                // {
                //     mGravityElapsedTime++;
                //     verticalVelocity = BattleDef.Gravity * mElapsedTime * FixedDeltaTime;
                // }
                
                //暂时不使用上面那套
                var verticalVelocity = GfMathf.Pow(GfMathf.Pow(_actionData.HorizontalVelocity, 1f / 3f) + BattleDef.Gravity * _elapsedTime, 3);

                VerticalMove(deltaTime, verticalVelocity);
            }
        }
    }
}