using System;
using System.ComponentModel;

namespace GameMain.Runtime
{
    public static class BattleDef
    {
        public const float Gravity = -9.8f * 2;

        public const int BaseCriticalHitRate = 5;
        public const int BaseCriticalHitDamage = 50;
        
        public const float TOLERANCE = 0.01f;
    }

    public static class PbFileTypes
    {
        public const string AttackDefinitionGroupFileType = "AttackDefinitionGroupResource";
        public const string PassiveSkillDefinitionFileType = "PassiveSkillDefinitionResource";
        public const string ShellDefinitionFileType = "ShellDefinitionResource";
        public const string BufferDefinitionFileType = "BufferDefinitionResource";
    }

    public static class BattleComponentSystemSortingOrder 
    {
        //越小优先级越高 默认优先级是10000
        
        
        public const int ActionComponent = 20040;
        
        public const int GfAnimationComponent = 30010;
        
        public const int ConditionComponent = 40010;
        
        public const int GfColliderComponent = 40022;
    }
    
    public static class BattleCameraShakeParam
    {
        public enum ShakePower
        {
            NONE,
            XXS,//Extra Extra Small
            XS,//Extra Small
            S,//Small
            M,//Medium
            L,//Large
            XL,//Extra Large
            XXL,//Extra Extra Large
        };
    
        public enum ShakeDirection
        {
            RANDOM,
            VERTICAL,
            HORIZON,
        };
    
        public static float Magnitude(this ShakePower power)
        {
            switch (power)
            {
                case ShakePower.NONE:
                    return 0f;
                case ShakePower.XXS:
                    return 0.05f;
                case ShakePower.XS:
                    return 0.2f;
                case ShakePower.S:
                    return 0.5f;
                case ShakePower.M:
                    return 0.8f;
                case ShakePower.L:
                    return 1.2f;
                case ShakePower.XL:
                    return 2;
                case ShakePower.XXL:
                    return 3f;
            }
    
            return 0f;
        }
    }

    public enum EffectGroup : byte
    {
        OneShot = 1, // 一次性特效
        Action,      // 与Action相关的特效
        Shell,       // shell特效
        Environment, // 环境特效
    }

    public enum TeamId
    {
        Invalid,
        TeamA,
        TeamB,
    }

    public enum BattleCharacterType
    {
        Invalid = 0,
        Player = 1,
        Enemy = 2,
        Summoner = 3,
        Npc = 4,
    }

    [Flags]
    public enum BattleHitCategory
    {
        Invalid = 0,                          
        Other = 1 << 0, // 敌方                
        SameWithoutMySelf = 1 << 1, // 除自己之外的己方                    
        Myself = 1 << 2, // 自己
                         
        SameIncludingMySelf = SameWithoutMySelf | Myself, //包括自己在内的己方
        OtherAndSameWithoutMySelf = Other | SameWithoutMySelf, //  除自己之外的己方和敌方
        OtherAndSameIncludingMySelf = Other | SameWithoutMySelf | Myself,// 己方和敌方
    }

    [Flags]
    public enum BattleLayerMask 
    {
        Invalid = 0,
        TeamA = 1 << 0,
        TeamB = 1 << 1,
    }

    public enum ReactionLevelType
    {
        None = 0,
        /// <summary>
        /// 微颤,轻微震动,不打断动作
        /// </summary>
        Tremor = 1,
        /// <summary>
        /// 轻击,播放受击动画，小段位移
        /// </summary>
        LightHit = 2,
        /// <summary>
        /// 击退，播放受击动画，可能被迫水平位移
        /// </summary>
        KnockBack  = 3,
        /// <summary>
        /// 击飞，播放击飞动画
        /// </summary>
        KnockUp    = 4,
    }

    public enum ShellType
    {
        Bullet,
        Area
    }

    public enum BulletType
    {
        Normal,//普通
        Penetrate,//穿透
    }

    public enum AttackCategoryType
    {
        Damage,
        Heal,
    }

    public enum DamageViewType
    {
        Normal,
        Hide,
    }

    public enum BattleActiveSkillType
    {
        NormalAttack = 0,//普通攻击
        NormalSkill = 1,//普通技能
        SpecialSkill = 2,//特殊技能
    }
    
    public enum AttackType
    {
        CharacterAttack = 1,
        Shell = 2,
        Warning = 3,
    }

    public enum PassiveSkillEventType
    {
        [Description("当角色出生")]
        OnCreate = 0,
        [Description("当角色死亡")]
        OnDie = 1,
        [Description("当命中目标")]
        OnHit = 2,
        [Description("当被命中")]
        OnBeHit = 3,
        
        [Description("当替换进场")]
        OnChangeIn = 4,
        [Description("当替换出场")]
        OnChangeOut = 5,
        [Description("当击杀敌人")]
        OnKillEnemy = 6,
        
        [Description("当血量变化")]
        OnHpChange = 7,
        [Description("动画事件触发")]
        OnAnimationEvent = 8,
    }

    public enum PassiveSkillProConditionType
    {
        [Description("时间间隔")]
        TimeInterval = 0,
        [Description("属性")]
        Attribute = 1,
        [Description("拥有buffer")]
        HasBuffer = 2,
    }

    public enum SelectTargetType
    {
        [Description("自己")]
        Self = 0,
        [Description("队伍")]
        Team = 1,
    }

    public enum SelectTargetFilterType
    {
        Attribute = 0,
    }
    
    public enum BufferEndType
    {
        [Description("永远满足(瞬间生效效果)")]
        AlwaysFulfilled = 0,
        [Description("时间(S)")]
        TimeOver = 1,
        
        [Description("攻击命中")]
        Hit = 11,//这种某种条件达成才生效的条件如何处理
        
        [Description("没有目标buffer")]
        NoTargetBuffer = 21,
    }
    
    [Flags]
    public enum BufferOverlayType
    {
        Invalid = 0,    
        [Description("可叠加")]
        Overlay = 1 << 0, // 叠加          
        [Description("可刷新")]
        Refresh = 1 << 1, // 刷新         
        [Description("可消耗")]
        Cost = 1 << 2, // 刷新               
        
        [Description("可叠加，可刷新")]
        OverlayRefresh = Overlay|Refresh,
        
        [Description("可叠加，可消耗")]
        OverlayCost = Overlay|Cost,
        
        [Description("可叠加，可刷新,可消耗")]
        OverlayRefreshCost = Overlay|Refresh|Cost,
    }

    public enum BufferEffectType
    {
        [Description("改变属性-根据基础数值与来源控制")]
        Attribute = 0,
        [Description("改变当前生命值(瞬间)")]
        ChangeCurHp = 1,
        [Description("改变属性-根据已损失生命值百分比控制")]
        AttributeByHealthLost = 2,
    }
    
    //生效类型
    public enum BufferEffectValidType
    {
        TimeInterval = 0,
        Attribute = 1,
        HasBuffer = 2,
    }
        
    //触发类型
    public enum BufferEffectTriggerType
    {
        OnUpdate,//持续类型的bufferEffect
        OnBegin,
        OnEnd,
        OnHit
    }

    public enum AttributeType
    {
        Hp   = 0,//生命值
        
        Attack  = 1,//攻击力
        Defense = 2,//防御力
        
        DamageBonus = 3,//伤害加成
        DamageReduction = 4,//伤害减免
        
        CriticalHitRate = 5,//暴击率
        CriticalHitDamage = 6,//暴击伤害
        
        MoveSpeed = 11,//移动速度
    }

    public enum DotType
    {
        Poison,//中毒,施加者攻击力百分比伤害
        Burn,//燃烧,施加者攻击力百分比伤害
        Bleed,//流血,施加者攻击力百分比伤害
    }

    public static class BattleColliderGroupName
    {
        public static readonly BattleColliderGroupId Defend = new BattleColliderGroupId(new AttackId(), true);
    }
    
    //todo:
    
    // RPG 箱庭？织梦岛 箱庭世界冒险加地牢探险
    // act没有优势 轻度动作
    // 还是NRP 虽然很难但还是得搞 角色，场景(草，树，水，建筑)，这套流程3d游戏怎么样都要接触到的，没办法
    // 异世界/机械/卡通/DND
    // 异世界参考最多 更轻松休闲
    // 机械 参考尼尔 末日废土机械 难以驾驭 虽然很感兴趣但不得不放弃
    // q版卡通 参考织梦岛 卡比
    // DND 同机械
    // 最后还得异世界
    
    //1.角色与怪物的设计
    //2.顿帧表现的配置和逻辑
    //3.远程敌人设计 与 近战敌人设计
    
    //还是说循序渐进 先整肉鸽战斗 再整rpg
    //或者说直接俯视视角元神 驾驭不住
    
    // 一定要有对标游戏 有了对标游戏才能有对应的场景设计玩法设计 目前只支持轻度的动作
    // 游戏参考 织梦岛 神之天平 八方旅人
    // 探索解密 角色养成
    // 多角色 可切换控制
    // 城镇村落(养成休息) 野外(战斗探险宝箱收集) 洞穴神殿(解密战斗)
    // 在一个初始村落醒来
    
    // rpg吧 心头好 多角色切换控制权 是否可以辅助战斗看情况
    //角色 点按轻攻击 长按蓄力攻击 小技能e 大技能q 点按闪避 长按奔跑
    //剑设计 轻攻击快速挥剑轻度向前位移或者无位移 蓄力攻击长按进入蓄力状态 松开释放或者蓄力时间满足释放
    //需要道具键位
    //钓鱼
    
    //先把肉鸽战斗保留下来当一个玩法
    
    //建模 小房子 栅栏 宝箱 
}
