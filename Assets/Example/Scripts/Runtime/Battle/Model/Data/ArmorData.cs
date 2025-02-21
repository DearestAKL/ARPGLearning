using cfg;

namespace GameMain.Runtime
{
    /// <summary>
    /// 防具数据
    /// 基础属性 + 特殊属性
    /// 根据星级和等级读配置表
    /// //todo:套装效果
    /// </summary>
    public class ArmorData
    {
        public ArmorConfig Config{ get; private set; }
        public ArmorLevelAttribute LevelAttribute{ get; private set; }
        public UserWeapon UserData { get; private set; }
        
        public float Hp { get; private set; }
        public float Attack{ get; private set; }
        
        public int Level => UserData.Level;
        
        public AttributeBonusData SecondaryAttributeBonusData{ get; private set; }

        public ArmorData(string armorGuid)
        {
            UserData = UserDataManager.Instance.Weapon.Get(armorGuid);
            Config = LubanManager.Instance.Tables.TbArmor.Get(UserData.Id);

            UpdateAttribute();
        }
        
        /// <summary>
        /// 更新属性信息与
        /// 初始化，等级变化时调用
        /// </summary>
        private void UpdateAttribute()
        {
            LevelAttribute = LubanManager.Instance.Tables.TbArmorLevelAttribute.Get(Config.Quality, UserData.Level);
            
            //TODO：根据防具记录的属性来读取对应的等级属性 血量，攻击，血量百分比，攻击百分比，防御百分比，暴击率，暴击伤害，伤害加成            
            
            //临时
            if (Config.Type == ArmorType.Head || Config.Type == ArmorType.Waist)
            {
                Attack = LevelAttribute.Attack;//主属性固定为Attack
            }
            else
            {
                Hp = LevelAttribute.Hp;//主属性固定为Hp
            }
            
            SecondaryAttributeBonusData = GetAttributeBonusData(Config.SecondaryAttributeBonusType);
        }
        
        private AttributeBonusData GetAttributeBonusData(AttributeBonusType attributeBonusType)
        {
            var value = 0f;
            switch (attributeBonusType)
            {
                case AttributeBonusType.Hp:
                    value = LevelAttribute.HpPercentage;
                    break;
                case AttributeBonusType.Attack:
                    value = LevelAttribute.AttackPercentage;
                    break;
                case AttributeBonusType.Defense:
                    value = LevelAttribute.AttackPercentage;
                    break;
                case AttributeBonusType.CriticalHitRate:
                    value = LevelAttribute.AttackPercentage;
                    break;
                case AttributeBonusType.CriticalHitDamage:
                    value = LevelAttribute.AttackPercentage;
                    break;
                case AttributeBonusType.DamageBonus:
                    value = LevelAttribute.AttackPercentage;
                    break;
            }
            return new AttributeBonusData(attributeBonusType, value);
        }
    }
}