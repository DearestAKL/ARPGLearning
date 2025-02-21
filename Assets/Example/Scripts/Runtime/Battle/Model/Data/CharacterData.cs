using System.Collections.Generic;
using cfg;

namespace GameMain.Runtime
{
    /// <summary>
    /// 角色数据
    /// 基础属性
    /// 等级属性 等级成长系数
    /// 突破属性 突破基础属性加成 突破属性增益(Bonus Attribute)*（突破阶段数-1)
    /// BaseAttributeValue = BaseValue*LevelMultiplier + AscensionValue(突破总属性*突破占比系数，1-6突破=>38:27:36:27:27:27)
    /// </summary>
    public class CharacterData
    {
        public CharacterConfig Config{ get; private set; }
        public CharacterBaseAttribute BaseAttribute{ get; private set; }//角色的基础属性就拆出来吧 也可以不拆
        public CharacterAscensionAttribute AscensionAttribute{ get; private set; }//角色的突破总属性
        public CharacterLevelExp CharacterLevelExp{ get; private set; }//角色当前等级经验
        public CharacterLevelExp CharacterMaxLevelExp{ get; private set; }//角色当前阶段最大等级经验
        public ActiveSkill NormalActiveSkill { get; private set; }
        public ActiveSkill SpActiveSkill { get; private set; }
        
        public UserCharacter UserData { get; private set; }
        
        public float Hp { get; private set; }
        public float Attack{ get; private set; }
        public float Defense{ get; private set; }
        
        public List<int> PassiveSkillIds = new List<int>();

        public int Id => Config.Id;
        public int Level => UserData.Level;
        public int Exp => UserData.Exp;
        public int AscensionLevel => UserData.AscensionLevel;
        
        public int CurMaxLevel;
        public bool IsCurMaxLevel => CurMaxLevel == Level;
        public bool IsMaxLevel => CharacterLevelExp.UpExp == 0;
        public int CurLevelUpExp => CharacterLevelExp.UpExp;
        public int CurAscensionMaxExp => CharacterMaxLevelExp.TotalExp;
        public float CurExpRatio => IsCurMaxLevel ? 0f : CurLevelExp / (float)CurLevelUpExp;
        public int CurLevelExp => Exp - CharacterLevelExp.TotalExp;
        
        public AttributeBonusData AscensionAttributeBonusData{ get; private set; }

        public CharacterData(int characterId)
        {
            UserData = UserDataManager.Instance.Character.Get(characterId);
            
            Config = LubanManager.Instance.Tables.TbCharacter.Get(characterId);
            BaseAttribute = LubanManager.Instance.Tables.TbCharacterBaseAttribute.Get(characterId);
            AscensionAttribute = LubanManager.Instance.Tables.TbCharacterAscensionAttribute.Get(characterId);
            
            NormalActiveSkill = LubanManager.Instance.Tables.TbActiveSkill.GetOrDefault(Config.NormalSkill);//可空
            SpActiveSkill = LubanManager.Instance.Tables.TbActiveSkill.GetOrDefault(Config.SpSkill);//可空

            UpdateData();
            
            InitPassiveSkills();
        }
        
        private void InitPassiveSkills()
        {
            if (Config.PassiveSkill1 != 0)
            {
                PassiveSkillIds.Add(Config.PassiveSkill1);
            }
            
            if (Config.PassiveSkill2 != 0)
            {
                PassiveSkillIds.Add(Config.PassiveSkill2);
            }
            
            if (NormalActiveSkill!=null && NormalActiveSkill.PassiveSkill != 0)
            {
                PassiveSkillIds.Add(NormalActiveSkill.PassiveSkill);
            }
            
            if (SpActiveSkill!=null && SpActiveSkill.PassiveSkill != 0)
            {
                PassiveSkillIds.Add(SpActiveSkill.PassiveSkill);
            }
        }

        /// <summary>
        /// 更新属性信息与其它和等级，突破等级相关内容
        /// 初始化，或者等级和突破等级变化时调用
        /// </summary>
        public void UpdateData()
        {
            var levelMultiplier = CharacterDataHelper.GetLevelMultiplier(Config.Quality, UserData.Level);
            var ascensionMultiplier = CharacterDataHelper.GetAscensionMultiplier(UserData.AscensionLevel);
            Hp = BaseAttribute.Hp * levelMultiplier + AscensionAttribute.Hp * ascensionMultiplier;
            Attack = BaseAttribute.Attack * levelMultiplier + AscensionAttribute.Attack * ascensionMultiplier;
            Defense = BaseAttribute.Defense * levelMultiplier + AscensionAttribute.Defense * ascensionMultiplier;
            
            CharacterLevelExp = LubanManager.Instance.Tables.TbCharacterLevelExp.Get(UserData.Level);
            CurMaxLevel = 20;
            for (int i = 0; i < UserData.AscensionLevel; i++)
            {
                CurMaxLevel += i > 0 ? 10 : 20;
            }
            
            CharacterMaxLevelExp = LubanManager.Instance.Tables.TbCharacterLevelExp.Get(CurMaxLevel);
            
            AscensionAttributeBonusData =
                CharacterDataHelper.GetAscensionAttributeBonusData(Config.AscensionAttributeBonusType, UserData.AscensionLevel,
                    Config.Quality);
        }
    }
}