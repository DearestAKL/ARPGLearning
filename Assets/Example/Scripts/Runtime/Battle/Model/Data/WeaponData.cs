using cfg;

namespace GameMain.Runtime
{
    /// <summary>
    /// 武器数据
    /// 基础攻击
    /// 等级属性 攻击力等级成长系数
    /// 突破属性 突破攻击力
    /// Attack = BaseAttack * LevelMultiplier + AscensionBonus
    /// 副属性 每五级提升一次
    /// </summary>
    public class WeaponData
    {
        public WeaponConfig Config{ get; private set; }
        public WeaponLevelMultiplier LevelMultiplier{ get; private set; }
        public UserWeapon UserData { get; private set; }
        public float Attack{ get; private set; }

        public int Level => UserData.Level;
        
        public int AscensionLevel => UserData.AscensionLevel;
        
        public AttributeBonusData SecondaryAttributeBonusData{ get; private set; }
        
        public WeaponData(string weaponGuid)
        {
            UserData = UserDataManager.Instance.Weapon.Get(weaponGuid);
            Config = LubanManager.Instance.Tables.TbWeapon.Get(UserData.Id);

            UpdateData();
        }
        
        /// <summary>
        /// 更新属性信息与其它和等级，突破等级相关内容
        /// 初始化，或者等级和突破等级变化时调用
        /// </summary>
        private void UpdateData()
        {
            var levelMultiplier = WeaponDataHelper.GetLevelMultiplier(Config.Quality, UserData.Level);
            var ascensionBonus = WeaponDataHelper.GetAscensionBonus(Config.Quality, AscensionLevel);
            Attack = Config.Attack * levelMultiplier + ascensionBonus;
            
            SecondaryAttributeBonusData =
                WeaponDataHelper.GetSecondaryAttributeBonusData(Config.SecondaryAttributeBonusType,
                    Config.SecondaryAttributeBonusValue, UserData.Level);
        }
        
    }
}