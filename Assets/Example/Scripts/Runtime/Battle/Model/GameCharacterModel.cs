using System.Collections.Generic;
using cfg;

namespace GameMain.Runtime
{
    public class GameCharacterModel
    {
        public int Id { get; private set; }
        public bool CanReceiveKnockUp { get; private set; }
        
        public string CharacterAssetName { get; private set; }
        public string ContainerAssetName { get; private set; }
        public string AiAssetName { get; private set; }
        public cfg.ActiveSkill NormalActiveSkill { get; private set; }
        public cfg.ActiveSkill SpActiveSkill { get; private set; }

        public float Hp { get; private set; }
        public float Attack{ get; private set; }
        public float Defense{ get; private set; }
        
        public float CriticalHitRate{ get; private set; }
        public float CriticalHitDamage{ get; private set; }
        public float DamageBonus{ get; private set; }
        public float HpBonus{ get; private set; }
        public float AttackBonus{ get; private set; }
        public float DefenseBonus{ get; private set; }

        public List<int> PassiveSkillIds = new List<int>();
        
        public int Level { get; private set; }
        public BattleCharacterType BattleCharacterType { get; private set; }
        
        public CharacterData CharacterData { private set; get; }
        public WeaponData WeaponData { private set; get; }
        public List<ArmorData> ArmorsList { private set; get; }

        public GameCharacterModel(float hp, float attack = 0, float defense = 0, int level = 1)
        {
            Hp = hp;
            Attack = attack;
            Defense = defense;
            Level = level;
        }

        public GameCharacterModel(CharacterData character,WeaponData weapon = null, List<ArmorData> armors = null)
        {
            InitData(character, weapon, armors);
        }
        
        public GameCharacterModel(EnemyData enemyData)
        {
            InitData(enemyData);
        }

        #region Character
        private void InitData(CharacterData character,WeaponData weapon = null, List<ArmorData> armors = null)
        {
            BattleCharacterType = BattleCharacterType.Player;
            
            Id = character.Config.Id;
            CanReceiveKnockUp = character.Config.CanReceiveKnockUp;
            CharacterAssetName = character.Config.CharacterAssetName;
            ContainerAssetName = character.Config.ContainerAssetName;
            SpActiveSkill = character.SpActiveSkill;
            NormalActiveSkill = character.NormalActiveSkill;

            CharacterData = character;
            WeaponData = weapon;
            ArmorsList = armors;
            
            UpdateAttribute();
        }

        public void UpdateAttribute()
        {
            var attributeBonusData = RuntimeDataHelper.GetAllAttributeBonusData(CharacterData, WeaponData, ArmorsList);
            
            Hp = RuntimeDataHelper.GetSumHpValue(CharacterData, ArmorsList);
            Attack = RuntimeDataHelper.GetSumAttackValue(CharacterData, WeaponData, ArmorsList);
            Defense = RuntimeDataHelper.GetSumDefenseValue(CharacterData);
            
            HpBonus = attributeBonusData.GetSumBonusValue(AttributeBonusType.Hp);
            AttackBonus = attributeBonusData.GetSumBonusValue(AttributeBonusType.Attack);
            DefenseBonus = attributeBonusData.GetSumBonusValue(AttributeBonusType.Defense);
            CriticalHitRate = BattleDef.BaseCriticalHitRate + attributeBonusData.GetSumBonusValue(AttributeBonusType.CriticalHitRate);
            CriticalHitDamage = BattleDef.BaseCriticalHitDamage + attributeBonusData.GetSumBonusValue(AttributeBonusType.CriticalHitDamage);
            DamageBonus = attributeBonusData.GetSumBonusValue(AttributeBonusType.DamageBonus);

            PassiveSkillIds = RuntimeDataHelper.GetAllPassiveSkillIds(CharacterData, ArmorsList);
        }
        #endregion

        #region Enemy

        private void InitData(EnemyData enemyData)
        {
            BattleCharacterType = BattleCharacterType.Enemy;
            
            Id = enemyData.Config.Id;
            CanReceiveKnockUp = enemyData.Config.CanReceiveKnockUp;
            CharacterAssetName = enemyData.Config.CharacterAssetName;
            ContainerAssetName = enemyData.Config.ContainerAssetName;
            
            Hp = enemyData.Hp;
            Attack = enemyData.Attack;
            Defense = enemyData.Defense;

            PassiveSkillIds = enemyData.PassiveSkillIds;
            
            AiAssetName = enemyData.Config.AiAssetName;
        }

        #endregion
    }
}