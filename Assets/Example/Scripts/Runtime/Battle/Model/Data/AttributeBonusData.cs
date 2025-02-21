using System.Collections.Generic;
using cfg;

namespace GameMain.Runtime
{
    public struct AttributeBonusData
    {
        public AttributeBonusType BonusType;
        public float BonusValue;

        public AttributeBonusData(AttributeBonusType bonusType,float bonusValue)
        {
            BonusType = bonusType;
            BonusValue = bonusValue;
        }
    }

    public static class AttributeBonusDataExtension
    {
        public static float GetBonusValue(this AttributeBonusData bonusData,AttributeBonusType bonusType)
        {
            return bonusData.BonusType == bonusType ? bonusData.BonusValue : 0;
        }
        
        public static float GetSumBonusValue(this List<AttributeBonusData> bonusData,AttributeBonusType bonusType)
        {
            float sumValue = 0;
            if (bonusData != null) 
            {
                foreach (var data in bonusData)
                {
                    sumValue += data.GetBonusValue(bonusType);
                }
            }
            return sumValue;
        }
    }
}