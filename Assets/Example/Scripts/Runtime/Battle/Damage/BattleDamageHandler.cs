using System;
using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class BattleDamageHandler : IDisposable
    {
        private List<IBattleObjectDamageNotificator> _notificatorList;

        private readonly IGfRandomGenerator _randomGenerator;

        private readonly GfManagedLocalInstancePool<BattleDamageResult> _damageResultPool;
        private readonly GfManagedLocalInstancePool<BattleSimpleDamageResult> _simpleDamageResultPool;

        public BattleDamageHandler(IGfRandomGenerator randomGenerator)
        {
            _notificatorList = new List<IBattleObjectDamageNotificator>();

            _randomGenerator = randomGenerator;
            _damageResultPool = new GfManagedLocalInstancePool<BattleDamageResult>(32, () => new BattleDamageResult());
            _simpleDamageResultPool = new GfManagedLocalInstancePool<BattleSimpleDamageResult>(32, () => new BattleSimpleDamageResult());
        }

        public void Dispose()
        {
            if (_notificatorList != null)
            {
                _notificatorList.Clear();
                _notificatorList = null;
            }
        }

        /// <summary>
        /// 将攻击结果反映在该帧的所有攻击接收者上
        /// NOTE: 命中检测过程完成后每帧调用一次
        /// </summary>
        public void PostProcess()
        {
            for (var i = 0; i < _notificatorList.Count; i++)
            {
                _notificatorList[i].Apply(_randomGenerator, this);
            }

            _notificatorList.Clear();
        }

        public void SetWarningFlag(BattleColliderDefendParameter defendParameter)
        {
            defendParameter.DamageReceiverHandler.SetWarningFlag(true);
        }

        public bool IsCollisionTarget(BattleColliderAttackParameter attackParameter, BattleColliderDefendParameter defendParameter) 
        {
            var causerHandler = attackParameter.DamageCauserHandler;
            var receiverHandler = defendParameter.DamageReceiverHandler;
            var singleAttackModel = attackParameter.SingleAttackModel;

            var canHit = singleAttackModel.CanHitByHitCategory(causerHandler.TeamId, receiverHandler.TeamId, attackParameter.OwnerHandle, defendParameter.OwnerHandle);
            if (!canHit) 
            {
                return false;
            }
            
            return true;
        }
        
        //通常伤害 会计算防御，暴击与增减伤
        public bool HandleDamage(BattleColliderAttackParameter attackParameter, BattleColliderDefendParameter defendParameter) 
        {
            var result = _damageResultPool.Issue();

            var receiverHandler = defendParameter.DamageReceiverHandler;
            var damageNotificator = defendParameter.DamageNotificator;
            
            result.AttackParameter        = attackParameter;
            result.DefendParameter        = defendParameter;
            
            if (damageNotificator is GimmickDamageNotificator)
            {
                //Gimmick
                result.AttackParameter        = attackParameter;
                result.DefendParameter        = defendParameter;
                damageNotificator.ReceiveDamage(result, this);
                return true;
            }
            
            var causerHandler = attackParameter.DamageCauserHandler;
            var singleAttackModel = attackParameter.SingleAttackModel;
            if (singleAttackModel.Info.AttackCategoryType == AttackCategoryType.Heal)
            {
                CalculateHealValue(result, causerHandler, receiverHandler, singleAttackModel);
            }
            else
            {
                CalculateDamageValue(result, causerHandler, receiverHandler, singleAttackModel);
            }
            
            damageNotificator.ReceiveDamage(result, this);
            return true;
        }

        //直伤
        public bool HandleSimpleDamage(GfHandle causerHandle,IBattleObjectDamageNotificator defenderDamageNotificator, int originalVariation,
            AttackCategoryType attackCategoryType,DamageViewType damageViewType = DamageViewType.Normal)
        {
            var simpleDamageResult = _simpleDamageResultPool.Issue();
            simpleDamageResult.Initialize(causerHandle, defenderDamageNotificator, originalVariation,
                attackCategoryType, damageViewType);
            
            defenderDamageNotificator.ReceiveSimpleDamage(simpleDamageResult, this);
            return true;
        }

        //持续Dot伤害 会计算增减伤
        public bool HandleDotDamage(IBattleObjectDamageCauserHandler causerHandler,
            IBattleObjectDamageReceiverHandler receiverHandler,
            IBattleObjectDamageNotificator damageNotificator,
            float damageValue)
        {
            var result = _damageResultPool.Issue();
            
            CalculateDotDamageValue(result, causerHandler, receiverHandler, damageValue);
            
            damageNotificator.ReceiveDamage(result, this);
            
            return true;
        }


        private bool CalculateDamageValue(
            BattleDamageResult result,
            IBattleObjectDamageCauserHandler causerHandler,
            IBattleObjectDamageReceiverHandler receiverHandler,
            SingleAttackModel singleAttackModel)
        {
            var infoData = singleAttackModel.Info;

            //计算基础伤害
            float attackValue;
            if (infoData.SourceType == AttributeType.Hp)
            {
                attackValue = causerHandler.GetMaxHp();
            }
            else if (infoData.SourceType == AttributeType.Defense)
            {
                attackValue = causerHandler.GetDefense();
            }
            else
            {
                attackValue = causerHandler.GetAttack();
            }
            
            float damageMultiplier = infoData.ApplyingPercent / 100f;
            var damageValue = attackValue * damageMultiplier;
            
            //计算暴击相关
            var criticalHitRate = causerHandler.GetCriticalHitRate();
            if (_randomGenerator.Range(0, 1f) < criticalHitRate)
            {
                //暴击生效
                result.IsCritical = true;
                var criticalHitDamage = causerHandler.CriticalHitDamage();
                damageValue *= criticalHitDamage;
            }

            //计算防御减伤
            var defenseDamageReduction = BattleDamageCalculatorHelper.CalcDefenseDamageReduction(causerHandler, receiverHandler);
            damageValue *= 1 - defenseDamageReduction;

            //计算伤害加成相关
            float damageBonus = causerHandler.GetDamageBonus();
            float damageReduction = receiverHandler.GetDamageReduction();
            damageValue *= 1 + damageBonus - damageReduction;
            
            //最终伤害
            result.AttackCategoryType = infoData.AttackCategoryType;
            result.OriginalVariation = (int)damageValue;
            
            result.DamageVector = causerHandler.CalculateDamageVector(receiverHandler.GetReceiverPosition());

            result.ReducePoiseValue = infoData.ReducePoiseValue;
            result.ReactionLevelType = infoData.ReactionLevelType;
            result.HorizontalPower = infoData.HorizontalPower;
            result.VerticalPower = infoData.VerticalPower;
            result.CameraShakeData = infoData.CameraShakeData;

            return true;
        }

        private bool CalculateHealValue(BattleDamageResult result,
            IBattleObjectDamageCauserHandler causerHandler,
            IBattleObjectDamageReceiverHandler receiverHandler,
            SingleAttackModel singleAttackModel)
        {
            var infoData = singleAttackModel.Info;

            //计算基础恢复
            float attackValue = causerHandler.GetAttack();
            float damageMultiplier = infoData.ApplyingPercent / 100f;
            var damageValue = attackValue * damageMultiplier;
            
            //最终恢复
            result.AttackCategoryType = infoData.AttackCategoryType;
            result.OriginalVariation = (int)damageValue;
            return true;
        }

        private bool CalculateDotDamageValue(            
            BattleDamageResult result,
            IBattleObjectDamageCauserHandler causerHandler,
            IBattleObjectDamageReceiverHandler receiverHandler,
            float damageValue)
        {
            //Dot伤害忽略暴击防御等
            
            //计算伤害加成相关
            float damageBonus = causerHandler.GetDamageBonus();
            float damageReduction = receiverHandler.GetDamageReduction();
            damageValue *= 1 + damageBonus - damageReduction;
            
            result.OriginalVariation = (int)damageValue;
            
            return true;
        }

        public void NotifyNotificatorHasReceived(IBattleObjectDamageNotificator damageNotificator)
        {
            _notificatorList.Add(damageNotificator);
        }
    }
}
