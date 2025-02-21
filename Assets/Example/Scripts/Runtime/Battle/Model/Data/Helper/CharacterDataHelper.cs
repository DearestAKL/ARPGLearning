using cfg;

namespace GameMain.Runtime
{
    public static class CharacterDataHelper
    {
        private static readonly int[] AscensionRatio = new int[] { 38, 27, 36, 27, 27, 27 };
        private const int AscensionSumValue = 38 + 27 + 36 + 27 + 27 + 27;
        
        public static float GetLevelMultiplier(int rarity,int level)
        {
            var levelMultiplier = LubanManager.Instance.Tables.TbCharacterLevelMultiplier.Get(level);
            //4星用MultiplierA 5星用MultiplierB
            return rarity > 4 ? levelMultiplier.MultiplierB : levelMultiplier.MultiplierA;
        }
        
        public static float GetAscensionMultiplier(int ascensionLevel)
        {
            //突破阶段数-1） 1-6突破=>38:27:36:27:27:27

            int curSumValue = 0;
            for (int i = 0; i < ascensionLevel; i++)
            {
                if (i > AscensionRatio.Length) { break; }
                curSumValue += AscensionRatio[i];
            }

            return curSumValue / (float)AscensionSumValue;
        }

        public static AttributeBonusData GetAscensionAttributeBonusData(AttributeBonusType attributeBonusType, int ascensionLevel, int rarity)
        {
            //Enum 当key已经优化过了，已经不会进行装拆箱了
            var ascensionBonus = LubanManager.Instance.Tables.TbCharacterAscensionBonus.Get(attributeBonusType);
            //4星用BonusA 5星用BonusB
            var bonusValue = rarity > 4 ? ascensionBonus.BonusB : ascensionBonus.BonusA;
            //突破属性增益(Bonus Attribute)*（突破阶段数-1)
            if (ascensionLevel > 1)
            {
                bonusValue *= ascensionLevel - 1;
            }
            else
            {
                bonusValue = 0;
            }
            
            //4星用BonusA 5星用BonusB
            return new AttributeBonusData(attributeBonusType, bonusValue);
        }
        
        public static int GetLevelUp(int level, int exp)
        {
            var characterLevelExp = LubanManager.Instance.Tables.TbCharacterLevelExp.Get(level);
            if (exp - characterLevelExp.TotalExp >= characterLevelExp.UpExp && characterLevelExp.UpExp > 0)
            {
                //未满级
                //可升级
                level++;
                return GetLevelUp(level, exp);
            }
            return level;
        }

        /// <summary>
        /// 获取提升后的角色属性（升级/突破）
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="characterData"></param>
        /// <param name="newLevel"></param>
        /// <param name="newAscensionLevel"></param>
        /// <returns></returns>
        public static float GetAfterAscensionAttribute(this CharacterData characterData, AttributeType attributeType, int newLevel, int newAscensionLevel)
        {
            
            var levelMultiplier = GetLevelMultiplier(characterData.Config.Quality, newLevel);
            var ascensionMultiplier = GetAscensionMultiplier(newAscensionLevel);

            switch (attributeType)
            {
                case AttributeType.Hp:
                    return characterData.BaseAttribute.Hp * levelMultiplier +
                           characterData.AscensionAttribute.Hp * ascensionMultiplier;
                case AttributeType.Attack:
                    return characterData.BaseAttribute.Attack * levelMultiplier +
                           characterData.AscensionAttribute.Attack * ascensionMultiplier;
                case AttributeType.Defense:
                    return characterData.BaseAttribute.Defense * levelMultiplier +
                           characterData.AscensionAttribute.Defense * ascensionMultiplier;
                default:
                    return 0;
            }
        }
    }
}