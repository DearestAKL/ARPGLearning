using System;
using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterDamageNotificator : ABattleObjectDamageNotificator
    {
        private sealed class ReactionParameter
        {
            public BattleCharacterActionType DamageReactionActionType { get; set; }
            public GfFloat2       Vector                   { get; set; }
            public ReactionLevelType       ReactionLevelType                    { get; set; }
            public float       HorizontalVelocity                    { get; set; }
            public float       VerticalVelocity                   { get; set; }

            public bool IsDie  { get; set; }

            public ReactionParameter()
            {
                Reset();
            }

            public void Reset()
            {
                DamageReactionActionType = BattleCharacterActionType.None;
                Vector = GfFloat2.Zero;
                ReactionLevelType = ReactionLevelType.None;
                VerticalVelocity = 0f;
                HorizontalVelocity = 0f;
                IsDie = false;
            }
        }

        private readonly List<BattleDamageResult> _receivedDamageResults;
        private readonly List<BattleSimpleDamageResult> _receivedSimpleDamageResults;

        private BattleCharacterAccessorComponent _accessor;
        private IBattleObjectDamageReporter _reporter;

        private ReactionParameter _reactionParameter;

        private GfEntity Entity => _accessor.Entity;

        public override GfHandle SelfHandle => Entity.ThisHandle;

        public BattleCharacterDamageNotificator()
        {
            _receivedDamageResults = new List<BattleDamageResult>();
            _receivedSimpleDamageResults = new List<BattleSimpleDamageResult>();
            //mApplyingParameter = new ApplyingParameter();
            _reactionParameter = new ReactionParameter();
        }

        public override void Dispose()
        {
            _reporter = null;
            _accessor = null;
        }

        public void Initialize(BattleCharacterAccessorComponent accessorComponent, IBattleObjectDamageReporter damageReporter)
        {
            _accessor = accessorComponent;
            _reporter = damageReporter;
        }

        protected override void Apply(IGfRandomGenerator randomGenerator)
        {
            if (_receivedDamageResults.Count == 0 && _receivedSimpleDamageResults.Count == 0) 
            {
                return;
            }
            
            if (!_accessor.IsAlive)
            {
                _receivedDamageResults.Clear();
                _receivedSimpleDamageResults.Clear();
                return;
            }

            ApplyDamageResults(randomGenerator);
            
            ApplySimpleDamageResults(randomGenerator);

            _reporter.Report(_receivedDamageResults, _receivedSimpleDamageResults);

            _receivedDamageResults.Clear();
            _receivedSimpleDamageResults.Clear();
        }

        protected override void ReceiveDamage(BattleDamageResult damageResult)
        {
            if (_accessor.Condition.IsDodge && damageResult.AttackCategoryType == AttackCategoryType.Damage)
            {
                //躲闪
                return;
            }
            
            _receivedDamageResults.Add(damageResult);
        }

        protected override void ReceiveSimpleDamage(BattleSimpleDamageResult simpleDamageResult)
        {
            _receivedSimpleDamageResults.Add(simpleDamageResult);
        }
        
        private void ApplyDamageResults(IGfRandomGenerator randomGenerator)
        {
            for (var i = 0; i < _receivedDamageResults.Count; i++)
            {
                ApplyDamageResult(_receivedDamageResults[i], randomGenerator);
            }
            
            ApplyDamageReaction();
        }

        private void ApplyDamageResult(BattleDamageResult damageResult, IGfRandomGenerator randomGenerator) 
        {
            if (_reactionParameter.IsDie)
            {
                //已经死亡，就不需要计算反应效果了
                return;
            }
            
            if (damageResult.AttackCategoryType == AttackCategoryType.Heal)
            {
                _accessor.Condition.HpProperty.AddCurValue(damageResult.OriginalVariation);
            }
            else
            {
                if (!_accessor.Condition.IsDamageImmunity)
                {
                    //非伤害免疫
                    _accessor.Condition.HpProperty.SubtractCurValue(damageResult.OriginalVariation);
                }
                else
                {
                    damageResult.OriginalVariation = 0;
                }

                if (!_accessor.Condition.IsSuperArmor)
                {
                    //非霸体状态
                    _accessor.Condition.PoiseHandler.AddValue(damageResult.ReducePoiseValue,out var isBreak);
                }
                
                if (!_accessor.IsAlive)
                {
                    _reactionParameter.IsDie = true;
                }
            
                //伤害方向
                _reactionParameter.Vector = damageResult.DamageVector;

                //检测抗打断是否失效,检测是否处于霸体状态
                if (_accessor.Condition.PoiseHandler.IsFailure && !_accessor.Condition.IsSuperArmor)
                {
                    //反应效果
                    CalculateReactionEffect(damageResult);  
                }

                var causeAccessorComponent = GetCauserAccessor(damageResult);
                if (causeAccessorComponent.Condition.BattleCharacterType == BattleCharacterType.Player)
                {
                    BattleCharacterHitStopHelper.CalcHitStopSpan(damageResult);
                    BattleCharacterHitStopHelper.RequestHitStop(damageResult, _accessor, causeAccessorComponent);
                }
            }
            Entity.Request(new BattleReceivedDamageRequest(damageResult));
            Entity.RequestToOther(damageResult.AttackerHandle, new BattleDidCauseDamageRequest(damageResult));
        }

        private void ApplyDamageReaction()
        {
            if (_reactionParameter.IsDie)
            {
                //死亡
                RequestForChangeAction(
                    BattleCharacterActionDieData.Create(_reactionParameter.Vector));
            }
            else
            {
                switch (_reactionParameter.ReactionLevelType)
                {
                    case ReactionLevelType.None:
                        break;
                    case ReactionLevelType.Tremor:
                        //TODO:Tremor
                        break;
                    case ReactionLevelType.LightHit:
                        //轻击
                        RequestForChangeAction(
                            BattleCharacterGetHitActionData.Create(_reactionParameter.Vector,
                                _reactionParameter.HorizontalVelocity));
                        break;
                    case ReactionLevelType.KnockBack:
                        //击退
                        RequestForChangeAction(
                            BattleCharacterKnockBackActionData.Create(_reactionParameter.Vector,
                                _reactionParameter.HorizontalVelocity));
                        break;
                    case ReactionLevelType.KnockUp:
                        //击飞
                        RequestForChangeAction(
                            BattleCharacterKnockUpActionData.Create(_reactionParameter.Vector,
                                _reactionParameter.HorizontalVelocity,_reactionParameter.VerticalVelocity));
                        break;
                }
            }

            _reactionParameter.Reset();
        }
        
        private void ApplySimpleDamageResults(IGfRandomGenerator randomGenerator)
        {
            for (var i = 0; i < _receivedSimpleDamageResults.Count; i++)
            {
                ApplySimpleDamageResult(_receivedSimpleDamageResults[i], randomGenerator);
            }
        }

        private void ApplySimpleDamageResult(BattleSimpleDamageResult simpleDamageResult, IGfRandomGenerator randomGenerator)
        {
            if (simpleDamageResult.AttackCategoryType == AttackCategoryType.Heal)
            {
                _accessor.Condition.HpProperty.AddCurValue(simpleDamageResult.OriginalVariation);
            }
            else
            {
                if (!_accessor.Condition.IsDamageImmunity)
                {
                    //非伤害免疫
                    _accessor.Condition.HpProperty.SubtractCurValue(simpleDamageResult.OriginalVariation);
                }
            }
        }

        private void RequestForChangeAction(ABattleCharacterActionData actionData)
        {
            Entity.Request(new GfChangeActionRequest<BattleObjectActionComponent>(actionData.ActionType, actionData, actionData.Priority));
        }

        
        /// <summary>
        /// 计算反应效果
        /// 若攻击的反应等级为击飞时，满足 "攻击的竖直力 >= 5.5 * 敌人的重量" 才能触发击飞；否则，改成触发击退
        /// 若攻击的反应等级为击退或轻击时，满足 "攻击的水平力 >= 2 * 敌人的重量" 才能触发击退或轻击；否则，改成触发微颤
        /// </summary>
        /// <param name="damageResult"></param>
        private void CalculateReactionEffect(BattleDamageResult damageResult)
        {
            if (_reactionParameter.ReactionLevelType >= damageResult.ReactionLevelType)
            {
                //TODO:选取相同反应等级中，power更大的。
                return;
            }
            
            //TODO:mAccessor的重量
            int weight = 100;


            switch (damageResult.ReactionLevelType)
            {
                case ReactionLevelType.None:
                    break;
                case ReactionLevelType.Tremor:
                    //TODO:Tremor
                    break;
                case ReactionLevelType.LightHit:
                    if (damageResult.HorizontalPower >= 2 * weight)
                    {
                        //TODO:LightHit
                    }
                    else
                    {
                        damageResult.ReactionLevelType = ReactionLevelType.Tremor;
                        CalculateReactionEffect(damageResult);
                        return;
                    }
                    break;
                case ReactionLevelType.KnockBack:
                    if (damageResult.HorizontalPower >= 2 * weight)
                    {
                        //TODO:Knockback
                    }
                    else
                    {
                        damageResult.ReactionLevelType = ReactionLevelType.Tremor;
                        CalculateReactionEffect(damageResult);
                        return;
                    }
                    break;
                case ReactionLevelType.KnockUp:
                    if (damageResult.DefendParameter.DamageReceiverHandler.CanReceiveKnockUp() && damageResult.VerticalPower >= 5.5f * weight)
                    {
                        //TODO:Knockup
                    }
                    else
                    {
                        damageResult.ReactionLevelType = ReactionLevelType.KnockBack;
                        CalculateReactionEffect(damageResult);
                        return;
                    }
                    break;
            }
            
            
            _reactionParameter.ReactionLevelType = damageResult.ReactionLevelType;

            if (damageResult.ReactionLevelType >= ReactionLevelType.LightHit)
            {
                //计算水平初速度
                //水平方向初速度大小 = 实际水平作用力大小 / 实际重量
                //实际水平方向初速度大小 = Clamp{水平方向初速度大小，1，1000}
                
                var velocity = damageResult.HorizontalPower / (float)weight;
                _reactionParameter.HorizontalVelocity = Math.Clamp(velocity, 1f, 1000f);
            }

            if (damageResult.ReactionLevelType >= ReactionLevelType.KnockUp)
            {
                //计算垂直初速度
                //竖直方向初速度大小 = 实际竖直作用力大小 / 实际重量
                //实际竖直方向初速度大小 = Clamp{竖直方向初速度大小 ，8，1000}
                //TODO:如果目标的 无视最小受击竖直速度 为 真，实际竖直方向初速度大小 = 竖直方向初速度大小
                
                var velocity = damageResult.VerticalPower / (float)weight;
                _reactionParameter.VerticalVelocity = Math.Clamp(velocity, 8f, 1000f);
            }
        }
    }
}
