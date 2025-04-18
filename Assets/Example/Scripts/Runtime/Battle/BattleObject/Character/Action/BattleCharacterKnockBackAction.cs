using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterKnockBackActionData : ABattleCharacterActionData
    {
        public GfFloat2 DamageVector;
        public float HorizontalVelocity;
        public static BattleCharacterKnockBackActionData Create(GfFloat2 damageVector,float horizontalVelocity)
        {
            var data = Create(() => new BattleCharacterKnockBackActionData());
            data.DamageVector = damageVector;
            data.HorizontalVelocity = horizontalVelocity;

            return data;
        }

        public override int ActionType => BattleCharacterKnockBackAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            // if (currentAction.ActionData.ActionType == (int)BattleCharacterActionType.KnockUp)
            // {
            //     return false;
            // }
            return true;
        }
    }
    
    public class BattleCharacterKnockBackAction : ABattleCharacterAction
    {
        private BattleCharacterKnockBackActionData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;
        public const int ActionType = (int)BattleCharacterActionType.KnockBack;
        
        private static readonly string GetHitLargeAnimationName = "GetHitLarge";

        private float _elapsedTime;
        private bool _hasHorizontalInitVelocity;
        private bool _isInvalid = false;
        
        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);
            _actionData = GetActionData<BattleCharacterKnockBackActionData>();
            
            _hasHorizontalInitVelocity = _actionData.HorizontalVelocity > 0f;
            _elapsedTime = 0f;
            
            Rotate(_actionData.DamageVector * -1);
        }
        
        public override void OnStart()
        {
            base.OnStart();
            AnimationClipIndex = GetAnimationClipIndex();
            
            if (AnimationClipIndex < 0)
            {
                _isInvalid = true;
                //没有配套的击退动画 则改为受击动画GitHit
                EndActionAndRequestForChange(
                    BattleCharacterGetHitActionData.Create(_actionData.DamageVector,
                        _actionData.HorizontalVelocity));
            }
            else
            {
                Animation.Play(AnimationClipIndex);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_isInvalid)
            {
                return;
            }
            
            deltaTime *= Animation.Speed;
            
            if (_hasHorizontalInitVelocity)
            {
                //在地面
                //如果水平速度数值 > 0，则保持击退效果，水平速度计算如下
                //水平速度 = (水平初速度的1/3次方 - 3*时间)的三次方
                var horizontalVelocity = GfMathf.Pow(GfMathf.Pow(_actionData.HorizontalVelocity, 1f / 3f) - 3 * _elapsedTime, 3);
                
                if (horizontalVelocity <= 0f)
                {
                    _hasHorizontalInitVelocity = false;
                }
                else
                {
                    HorizontalMove(deltaTime, _actionData.DamageVector, horizontalVelocity);
                }
            }

            _elapsedTime += deltaTime;

            EndActionCheck();
        }

        private int GetAnimationClipIndex()
        {
            return Animation.GetClipIndex(GetHitLargeAnimationName);
        }
    }
}