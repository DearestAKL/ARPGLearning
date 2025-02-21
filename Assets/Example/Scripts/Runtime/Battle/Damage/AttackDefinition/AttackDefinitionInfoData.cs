using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class AttackDefinitionInfoData
    {
        public int AttackId { get; }
        
        /// <summary>
        /// 百分比
        /// </summary>
        public int ApplyingPercent { get; }
        
        /// <summary>
        /// 攻击类型
        /// </summary>
        public AttackCategoryType AttackCategoryType { get;}
        
        /// <summary>
        /// 基础伤害属性来源（攻防血）
        /// </summary>
        public AttributeType SourceType { get;}

        /// <summary>
        /// 削韧值
        /// </summary>
        public int ReducePoiseValue   { get;}

        /// <summary>
        /// 反应等级
        /// </summary>
        public ReactionLevelType ReactionLevelType { get;}
        
        /// <summary>
        /// 水平冲击力
        /// </summary>
        public int HorizontalPower { get;}
        
        /// <summary>
        /// 垂直冲击力
        /// </summary>
        public int VerticalPower { get;}
        
        /// <summary>
        /// 
        /// </summary>
        public BattleHitCategory HitCategory { get; }
        
        /// <summary>
        /// 相机震动参数
        /// </summary>
        public CameraShakeParamData CameraShakeData { get; }

        /// <summary>
        /// 伤害间隔时间
        /// </summary>
        public float IgnoreTime { get;private set;}//目前只有shell会启用IgnoreTime模式
        
        //攻击命中 攻击方 hitstop 等级
        public int AttackerHitStopLevel { get;private set;}
        //攻击命中 受击方 hitstop 等级
        public int DefenderHitStopLevel { get;private set;}
        
        public AttackDefinitionCollisionData[] Collisions { get; }

        public AttackDefinitionInfoData(int attackId,
            int applyingPercent, 
            AttackCategoryType attackCategoryType,
            AttributeType sourceType,
            int reducePoiseValue, 
            ReactionLevelType reactionLevelType,
            int horizontalPower, 
            int verticalPower, 
            BattleHitCategory hitCategory,
            CameraShakeParamData cameraShakeData,
            AttackDefinitionCollisionData[] collisions,
            float ignoreTime,
            int attackerHitStopLevel,
            int defenderHitStopLevel)
        {
            AttackId = attackId;
            ApplyingPercent = applyingPercent;
            AttackCategoryType = attackCategoryType;
            SourceType = sourceType;

            ReducePoiseValue = reducePoiseValue;

            ReactionLevelType = reactionLevelType;
            HorizontalPower = horizontalPower;
            VerticalPower = verticalPower;

            HitCategory = hitCategory;
            CameraShakeData = cameraShakeData;
            Collisions = collisions;

            IgnoreTime = ignoreTime;
            
            AttackerHitStopLevel = attackerHitStopLevel;
            DefenderHitStopLevel = defenderHitStopLevel;
        }

        public static AttackDefinitionInfoData CreatData(ShellDefinitionMessage shellDefinitionMessage, AttackDefinitionInfoMessage attackMessage)
        {
            return CreatData(shellDefinitionMessage.Id,PbDefinitionHelper.GetNumericalMessage(shellDefinitionMessage, attackMessage.PercentIndex), attackMessage);
        }

        public static AttackDefinitionInfoData CreatData(AttackDefinitionGroupMessage attackDefinitionGroupMessage, AttackDefinitionInfoMessage attackMessage)
        {
            return CreatData(attackMessage.AttackId,PbDefinitionHelper.GetNumericalMessage(attackDefinitionGroupMessage, attackMessage.PercentIndex), attackMessage);
        }

        private static AttackDefinitionInfoData CreatData(int attackId,int applyingPercent, AttackDefinitionInfoMessage attackMessage)
        {
            var collisions = new AttackDefinitionCollisionData[attackMessage.Collisions.Count];
            for (int j = 0; j < attackMessage.Collisions.Count; j++)
            {
                var collision = attackMessage.Collisions[j];
                collisions[j] = new AttackDefinitionCollisionData(collision.Offset.ToGfFloat2(), collision.Extents.ToGfFloat2());
            }

            return new AttackDefinitionInfoData(attackId, applyingPercent,
                (AttackCategoryType)attackMessage.CategoryType, (AttributeType)attackMessage.SourceType,
                attackMessage.ReducePoiseValue, (ReactionLevelType)attackMessage.ReactionLevelType,
                attackMessage.HorizontalPower,
                attackMessage.VerticalPower, (BattleHitCategory)attackMessage.HitCategoryType,
                new CameraShakeParamData(attackMessage.CameraShake), collisions, attackMessage.IgnoreTime,
                attackMessage.AttackerHitStopLevel, attackMessage.DefenderHitStopLevel);
        }
    }
}
