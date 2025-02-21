using Akari.GfCore;
using UnityEngine;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterGetHitActionData : ABattleCharacterActionData
    {
        public GfFloat2 DamageVector;
        public float HorizontalVelocity;
        public static BattleCharacterGetHitActionData Create(GfFloat2 damageVector,float horizontalVelocity)
        {
            var data = Create(() => new BattleCharacterGetHitActionData());
            data.DamageVector = damageVector;
            data.HorizontalVelocity = horizontalVelocity;

            return data;
        }

        public override int ActionType => BattleCharacterGetHitAction.ActionType;

        public override bool CanTransitionFrom(ABattleCharacterAction currentAction)
        {
            //带打断受击动画的 需要有动画播放cd 不然会相互打断受击动画 这是不合理的
            //强度更强的受击动画 优先级更高 不会被低优先级的受击动画打断（效果不好暂时不要）
            //KnockUp>KnockBack>LightHit
            //在GetHitAction内设置内置播放CD
            // if (currentAction.ActionData.ActionType == (int)BattleCharacterActionType.KnockBack
            //     ||currentAction.ActionData.ActionType == (int)BattleCharacterActionType.KnockUp)
            // {
            //     return false;
            // }
            
            return true;
        }
    }
    
    public class BattleCharacterGetHitAction : ABattleCharacterAction
    {
        private BattleCharacterGetHitActionData _actionData;
        public override ABattleCharacterActionData ActionData => _actionData;
        public const int ActionType = (int)BattleCharacterActionType.GetHit;

        private static readonly string GetHitSmallAnimationName = "GetHitSmall";
        
        //设置内置播放CD
        private static readonly float IntervalTime = 0.1f;
        private float _elapsedTime;

        private bool IsRepeat;
        private bool _hasHorizontalInitVelocity;
        
        public override void OnEnter(AGfFsmState prevAction, bool reenter)
        {
            base.OnEnter(prevAction, reenter);
            _actionData = GetActionData<BattleCharacterGetHitActionData>();
            
            IsRepeat = prevAction == this;
            
            _hasHorizontalInitVelocity = _actionData.HorizontalVelocity > 0f;
            
            if (!IsRepeat || IntervalTime <= _elapsedTime)
            {
                Rotate(_actionData.DamageVector * -1);
            }
        }
        
        public override void OnStart()
        {
            base.OnStart();
            
            if (!IsRepeat || IntervalTime <= _elapsedTime)
            {
                AnimationClipIndex = GetAnimationClipIndex();
                Animation.Play(AnimationClipIndex);
                _elapsedTime = 0;
            }
        }

        public override void OnExit(AGfFsmState nextAction)
        {
            base.OnExit(nextAction);
            _elapsedTime = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
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
            return Animation.GetClipIndex(GetHitSmallAnimationName);
        }
    }
}