using System.Collections.Generic;
using cfg;

namespace GameMain.Runtime
{
    //md 这些数据的更新处理为什么这么折磨 ui端与战斗端都需要更新属性
    public class UICharacterModel
    {
        private BattleCharacterConditionComponent _condition;//如果为空 需要构建属性
        public CharacterData CharacterData { private set; get; }
        public WeaponData WeaponData { private set; get; }
        public List<ArmorData> ArmorsList { private set; get; }

        public int Hp { private set; get; }
        public int Attack  { private set; get; }
        public int Defense  { private set; get; }
        
        public int AddHp { private set; get; }
        public int AddAttack  { private set; get; }
        public int AddDefense  { private set; get; }
        
        public float CriticalHitRate  { private set; get; }
        public float CriticalHitDamage  { private set; get; }
        
        public float DamageBonus  { private set; get; }
        public float DamageReduction  { private set; get; }


        /// <summary>
        /// 上场角色
        /// </summary>
        /// <param name="condition"></param>
        public UICharacterModel(BattleCharacterConditionComponent condition)
        {
            _condition = condition;
            CharacterData = condition.CharacterModel.CharacterData;
            WeaponData = condition.CharacterModel.WeaponData;
            ArmorsList = condition.CharacterModel.ArmorsList;
            
            //暂时 直接从_condition取实时数据
            Hp = _condition.HpProperty.TotalMaxShowValue;
            Attack = _condition.AttackProperty.TotalShowValue;
            Defense = _condition.DefenseProperty.TotalShowValue;
            
            AddHp = _condition.HpProperty.AdditionMaxShowValue;
            AddAttack = _condition.AttackProperty.AdditionShowValue;
            AddDefense = _condition.DefenseProperty.AdditionShowValue;
            
            CriticalHitRate = _condition.CriticalHitRateProperty.TotalPercent;
            CriticalHitDamage = _condition.CriticalHitDamageProperty.TotalPercent;
            DamageBonus = _condition.DamageBonusProperty.TotalPercent;
            DamageReduction = _condition.DamageReductionProperty.TotalPercent;
        }

        /// <summary>
        /// 未上场角色
        /// </summary>
        /// <param name="character"></param>
        /// <param name="weapon"></param>
        /// <param name="armors"></param>
        public UICharacterModel(CharacterData character,WeaponData weapon = null, List<ArmorData> armors = null)
        {
            CharacterData = character;
            WeaponData = weapon;
            ArmorsList = armors;
            
            var attributeBonusData = RuntimeDataHelper.GetAllAttributeBonusData(character, weapon, armors);
            
            var baseHp = RuntimeDataHelper.GetSumHpValue(character, armors);
            var baseAttack = RuntimeDataHelper.GetSumAttackValue(character, weapon, armors);
            var baseDefense = RuntimeDataHelper.GetSumDefenseValue(character);
            
            var hpBonus = attributeBonusData.GetSumBonusValue(AttributeBonusType.Hp);
            var attackBonus = attributeBonusData.GetSumBonusValue(AttributeBonusType.Attack);
            var defenseBonus = attributeBonusData.GetSumBonusValue(AttributeBonusType.Defense);
            
            var hpProperty = new PropertyHpData(baseHp, hpBonus);
            var attackProperty = new PropertyFlatAndPercentageData(baseAttack, attackBonus);
            var defenseProperty = new PropertyFlatAndPercentageData(baseDefense, defenseBonus);
            Hp = hpProperty.TotalMaxShowValue;
            Attack = attackProperty.TotalShowValue;
            Defense = defenseProperty.TotalShowValue;
            AddHp = hpProperty.AdditionMaxShowValue;
            AddAttack = attackProperty.AdditionShowValue;
            AddDefense = defenseProperty.AdditionShowValue;
            
            CriticalHitRate = BattleDef.BaseCriticalHitRate + attributeBonusData.GetSumBonusValue(AttributeBonusType.CriticalHitRate);
            CriticalHitDamage = BattleDef.BaseCriticalHitDamage + attributeBonusData.GetSumBonusValue(AttributeBonusType.CriticalHitDamage);
            
            DamageBonus = 0;
            DamageReduction = 0;
        }
    }
}