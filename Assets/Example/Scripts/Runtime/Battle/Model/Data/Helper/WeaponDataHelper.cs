using Akari.GfCore;
using cfg;

namespace GameMain.Runtime
{
    public static class WeaponDataHelper
    {
        public static float GetLevelMultiplier(int rarity, int level)
        {
            var levelMultiplier = LubanManager.Instance.Tables.TbWeaponLevelMultiplier.Get(level);
            
            if (rarity >= 5)
            {
                //5星 系数
                return levelMultiplier.MultiplierC1;
            }
            else if (rarity >= 4)
            {
                //4星 系数
                return levelMultiplier.MultiplierB1;
            }
            else
            {
                //1-3星 系数
                return levelMultiplier.MultiplierA1;
            }
        }
        
        /// <summary>
        /// 获取武器突破攻击力加成
        /// 1-2星 最大突破等级为4
        /// 3-5星 最大突破等级为6
        /// </summary>
        /// <param name="rarity">品质</param>
        /// <param name="ascensionLevel">突破等级</param>
        /// <returns></returns>
        public static float GetAscensionBonus(int rarity, int ascensionLevel)
        {
            if (ascensionLevel == 0)
            {
                return 0;
            }
            
            if (rarity <= 2 && ascensionLevel > 4)
            {
                GfLog.Error("武器品质与其突破等级不匹配");
            }
            
            var weaponAscensionAttribute = LubanManager.Instance.Tables.TbWeaponAscensionAttribute.Get(rarity);
            if (ascensionLevel == 1)
            {
                return weaponAscensionAttribute.Ascension1;
            }
            else if (ascensionLevel == 2)
            {
                return weaponAscensionAttribute.Ascension2;
            }
            else if (ascensionLevel == 3)
            {
                return weaponAscensionAttribute.Ascension3;
            }
            else if (ascensionLevel == 4)
            {
                return weaponAscensionAttribute.Ascension4;
            }
            else if (ascensionLevel == 5)
            {
                return weaponAscensionAttribute.Ascension5;
            }
            else if (ascensionLevel == 6)
            {
                return weaponAscensionAttribute.Ascension6;
            }
            else
            {
                GfLog.Error("武器突破等级错误");
                return 0f;
            }
        }
        
        
        /// <summary>
        /// 武器副属性
        /// 1-5-10-15-----90，每五级提升
        /// </summary>
        /// <param name="attributeBonusType"></param>
        /// <param name="baseValue"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static AttributeBonusData GetSecondaryAttributeBonusData(AttributeBonusType attributeBonusType, float baseValue, int level)
        {
            var keyLevel = level < 5 ? 1 : (level / 5) * 5;
            
            var levelMultiplier = LubanManager.Instance.Tables.TbWeaponSecondaryAttributeLevelMultiplier.Get(keyLevel);

            return new AttributeBonusData(attributeBonusType, baseValue * levelMultiplier.Multiplier);
        }
    }
}