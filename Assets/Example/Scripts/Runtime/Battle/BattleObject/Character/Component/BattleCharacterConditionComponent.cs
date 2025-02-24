using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;
using cfg;
using UnityEngine;

namespace GameMain.Runtime
{
    public interface IBattleCharacterConditionComponent 
    { 
    
    }

    public sealed class BattleCharacterConditionComponent : AGfGameComponent<BattleCharacterConditionComponent>, IBattleCharacterConditionComponent
    {
        public BattleCharacterType BattleCharacterType { get; }
        public TeamId TeamId { get; }

        public PropertyHpData HpProperty { get; }
        public BattleObjectPoiseHandler PoiseHandler { get; }
        
        public PropertyFlatAndPercentageData AttackProperty { get; }
        public PropertyFlatAndPercentageData DefenseProperty { get; }
        
        public PropertyPercentageData DamageBonusProperty { get; }
        public PropertyPercentageData DamageReductionProperty { get; }
        public PropertyPercentageData CriticalHitRateProperty { get; }
        public PropertyPercentageData CriticalHitDamageProperty { get; }
        
        public PropertyFlatAndPercentageData MoveSpeedProperty { get; }

        public CharacterFrameConditionHandler Frame { get; } = new CharacterFrameConditionHandler();
        
        public BattleObjectCoolTimeHandler[] ObjectCoolTimes { get; } 
        
        public IBattleCharacterAccessorComponent Target { get; private set; }
        
        public int Level => CharacterModel.Level;
        public int CharacterId => CharacterModel.Id;
        public bool CanReceiveKnockUp => CharacterModel.CanReceiveKnockUp;

        public bool IsDamageImmunity => Frame.IsDamageImmunity.Current;//伤害免疫，不受伤害
        public bool IsSuperArmor => Frame.IsSuperArmor.Current;//霸体，不会被攻击和控制打断动作
        public bool IsDodge => Frame.IsDodge.Current;//躲闪，不会被攻击命中
        
        public bool IsDashHolding;
        public bool IsFireHolding;
        public bool IsMoving;
        public GfFloat2 MoveDirection;
        public GfFloat2 MouseDirection;

        public GameCharacterModel CharacterModel { get; private set; }

        public BattleCharacterConditionComponent(BattleCharacterType battleCharacterType,GameCharacterModel characterModel)
        {
            CharacterModel = characterModel;
            
            BattleCharacterType = battleCharacterType;
            
            switch (battleCharacterType) 
            {
                case BattleCharacterType.Player:
                    TeamId = TeamId.TeamA;
                    ObjectCoolTimes = new BattleObjectCoolTimeHandler[2];
                    ObjectCoolTimes[0] = new BattleObjectCoolTimeHandler(CharacterModel.NormalActiveSkill.CoolTime, CharacterModel.NormalActiveSkill.MaxSlotNum, CharacterModel.NormalActiveSkill.MaxSlotNum);
                    ObjectCoolTimes[1] = new BattleObjectCoolTimeHandler(CharacterModel.SpActiveSkill.CoolTime, CharacterModel.SpActiveSkill.MaxSlotNum, CharacterModel.SpActiveSkill.MaxSlotNum);
                    break;
                case BattleCharacterType.Enemy:
                    TeamId = TeamId.TeamB;
                    break;
                case BattleCharacterType.Npc:
                case BattleCharacterType.Summoner:
                    TeamId = TeamId.TeamA;
                    break;
                case BattleCharacterType.Invalid:
                    TeamId = TeamId.Invalid;
                    break;
            }

            HpProperty = new PropertyHpData(CharacterModel.Hp, CharacterModel.HpBonus);
            AttackProperty = new PropertyFlatAndPercentageData(CharacterModel.Attack, CharacterModel.AttackBonus);
            DefenseProperty = new PropertyFlatAndPercentageData(CharacterModel.Defense, CharacterModel.DefenseBonus);

            //负数表示没有韧性值
            PoiseHandler = new BattleObjectPoiseHandler(battleCharacterType == BattleCharacterType.Player ? 100 : -1);

            DamageBonusProperty = new PropertyPercentageData(characterModel.DamageBonus);
            DamageReductionProperty = new PropertyPercentageData(0);
            
            CriticalHitRateProperty = new PropertyPercentageData(characterModel.CriticalHitRate);
            CriticalHitDamageProperty = new PropertyPercentageData(characterModel.CriticalHitDamage);

            MoveSpeedProperty = new PropertyFlatAndPercentageData(500);

            HpProperty.OnValueChange.GfSubscribe(OnHpChange);
        }

        public override void OnAwake()
        {
            base.OnAwake();
            //注册养成属性变更
            Entity.On<UpdatePropertyRequest>(OnUpdatePropertyRequest);
        }

        public override void OnUpdate(float deltaTime)
        {
            Frame.OnUpdate();

            if (ObjectCoolTimes != null)
            {
                foreach (var coolTime in ObjectCoolTimes)
                {
                    coolTime.OnUpdate(deltaTime);
                }
            }

            PoiseHandler.OnUpdate(deltaTime);
            
            //GfLog.Debug($"伤害免疫:{IsDamageImmunity}=====霸体：{IsSuperArmor}");
        }

        public void SetTargetAccessor(IBattleCharacterAccessorComponent accessor)
        {
            Target = accessor;
        }
        
        public BattleObjectCoolTimeHandler GetTargetCoolTime(int index)
        {
            if (index < 0 || index >= ObjectCoolTimes.Length)
            {
                return null;
            }

            return ObjectCoolTimes[index];
        }
        
        public IPropertyData GetConditionPropertyData(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.Hp:
                    return HpProperty;
                case AttributeType.Attack:
                    return AttackProperty;
                case AttributeType.Defense:
                    return DefenseProperty;
                case AttributeType.DamageBonus:
                    return DamageBonusProperty;
                case AttributeType.DamageReduction:
                    return DamageReductionProperty;
                case AttributeType.CriticalHitRate:
                    return CriticalHitRateProperty;
                case AttributeType.CriticalHitDamage:
                    return CriticalHitDamageProperty;
                case AttributeType.MoveSpeed:
                    return MoveSpeedProperty;
                default:
                    return null;
            }
        }

        private void OnHpChange(bool isAdd)
        {
            Entity.Request(new BattleCurHpChangeRequest(isAdd));

            if (HpProperty.CurValue <= 0)
            {
                Entity.Request(new CurHpMakeZeroRequest());
            }
        }
        
        public float GetSourceValue(AttributeType type,int percentage)
        {
            float sourceValue = 0f;
            if (type == AttributeType.Hp)
            {
                sourceValue = HpProperty.TotalMaxValue * percentage / 100;
            }
            else if (type == AttributeType.Attack)
            {
                sourceValue = AttackProperty.TotalValue * percentage / 100;
            }
            else if (type == AttributeType.Defense)
            {
                sourceValue = DefenseProperty.TotalValue * percentage / 100;
            }
            return sourceValue;
        }
        
        public void UpdateInput(
            bool isMoving,
            GfFloat2 moveDirection,
            GfFloat2 mouseDirection,
            bool isDashHolding,
            bool isFireHolding)
        {
            IsMoving = isMoving;
            MoveDirection = moveDirection;
            MouseDirection = mouseDirection;
            
            IsDashHolding = isDashHolding;
            IsFireHolding = isFireHolding;
        }

        /// <summary>
        /// 角色属性变化后需要更新填入的基础属性
        /// </summary>
        private void OnUpdatePropertyRequest(in UpdatePropertyRequest request)
        {
            CharacterModel.UpdateAttribute();
            
            HpProperty.UpdateBaseData(CharacterModel.Hp, CharacterModel.HpBonus);
            AttackProperty.UpdateBaseData(CharacterModel.Attack, CharacterModel.AttackBonus);
            DefenseProperty.UpdateBaseData(CharacterModel.Defense, CharacterModel.DefenseBonus);
            
            //PoiseHandler

            DamageBonusProperty.UpdateBaseData(CharacterModel.DamageBonus);
            //DamageReductionProperty.UpdateBaseData(0);
            
            CriticalHitRateProperty.UpdateBaseData(CharacterModel.CriticalHitRate);
            CriticalHitDamageProperty.UpdateBaseData(CharacterModel.CriticalHitDamage);

            //MoveSpeedProperty
        }
    }
}
