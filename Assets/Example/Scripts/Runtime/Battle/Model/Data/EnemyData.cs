using System.Collections.Generic;
using cfg;

namespace GameMain.Runtime
{
    /// <summary>
    /// 怪物数据
    /// 基础属性
    /// 等级属性 等级成长系数
    /// 生命值和攻击力有两套成长模板
    /// 生命值 一套R方=0.9997 一套模拟角色突破的属性曲线
    /// 攻击力 一套普通曲线 一套普通曲线但在41-84级会多下凹一点 也就是降低了怪物在41-84级间的攻击力
    /// </summary>
    public class EnemyData
    {
        public EnemyConfig Config{ get; private set; }
        public EnemyBaseAttribute BaseAttribute{ get; private set; }
        public EnemyLevelMultiplier LevelMultiplier{ get; private set; }

        public float Hp { get; private set; }
        public float Attack{ get; private set; }
        public float Defense{ get; private set; }
        
        public List<int> PassiveSkillIds = new List<int>();
        
        public int Level { get; private set; }
        
        public EnemyData(int characterId, int level)
        {
            Level = level;
            Config = LubanManager.Instance.Tables.TbEnemy.Get(characterId);
            BaseAttribute = LubanManager.Instance.Tables.TbEnemyBaseAttribute.Get(Config.BaseAttributeId);
            LevelMultiplier = LubanManager.Instance.Tables.TbEnemyLevelMultiplier.Get(level); 
            
            Hp = BaseAttribute.Hp * LevelMultiplier.MultiplierHPA;
            Attack = BaseAttribute.Attack * LevelMultiplier.MultiplierATKA;
            Defense = level * 50 + 500;

            if (Config.PassiveSkill != 0)
            {
                PassiveSkillIds.Add(Config.PassiveSkill);
            }
        }
    }
}