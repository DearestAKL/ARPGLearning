using System.Collections.Generic;
using cfg;

namespace GameMain.Runtime
{
    public static class RuntimeDataHelper
    {
        private static readonly Dictionary<int, CharacterData> CharacterDict = new Dictionary<int, CharacterData>();
        
        public static CharacterData CreateCharacterData(int characterId)
        {
            if (CharacterDict.TryGetValue(characterId,out var characterData))
            {
                return characterData;
            }
            else
            {
                characterData = new CharacterData(characterId);
                CharacterDict.Add(characterId, characterData);
                return characterData;
            }
        }

        public static List<AttributeBonusData> GetAllAttributeBonusData(CharacterData character,WeaponData weapon , List<ArmorData> armors)
        {
            var attributeBonusData = new List<AttributeBonusData>();
            if (character != null)
            {
                attributeBonusData.Add(character.AscensionAttributeBonusData);
            }
            if (weapon != null)
            {
                attributeBonusData.Add(weapon.SecondaryAttributeBonusData);
            }

            if (armors != null)
            {
                foreach (var armor in armors)
                {
                    attributeBonusData.Add(armor.SecondaryAttributeBonusData);
                }
            }

            return attributeBonusData;
        }
        
        public static  float GetSumHpValue(CharacterData character, List<ArmorData> armors = null)
        {
            var hpValue = character.Hp;
            if (armors != null)
            {
                foreach (var armor in armors)
                {
                    hpValue += armor.Hp;
                }
            }
            return hpValue;
        }
        
        public static  float GetSumAttackValue(CharacterData character, WeaponData weapon = null, List<ArmorData> armors = null)
        {
            var attackValue = character.Attack;
            
            if (weapon != null)
            {
                attackValue += weapon.Attack; 
            }
            
            if (armors != null)
            {
                foreach (var armor in armors)
                {
                    attackValue += armor.Attack;
                }
            }
            return attackValue;
        }
        
        public static  float GetSumDefenseValue(CharacterData character)
        {
            var defenseValue = character.Defense;
            return defenseValue;
        }
        
        public static List<int> GetAllPassiveSkillIds(CharacterData character, List<ArmorData> armors = null)
        {
            var passiveSkillIds = new List<int>();
            passiveSkillIds.AddRange(character.PassiveSkillIds);
            if (armors != null)
            {
                foreach (var armor in armors)
                {
                    if (armor.Config.PassiveSkill != 0)
                    {
                        passiveSkillIds.Add(armor.Config.PassiveSkill);
                    }
                }
            }
            return passiveSkillIds;
        }
    }
}