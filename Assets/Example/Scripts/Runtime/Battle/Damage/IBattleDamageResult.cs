using Akari.GfCore;

namespace GameMain.Runtime
{
    public interface IBattleDamageResult : IBattleSimpleDamageResult
    {
        CameraShakeParamData CameraShakeData { get;}
        
        bool IsCritical { get;}
    }
    
    public class BattleDamageResult : IBattleDamageResult, IGfLocalPoolable
    {
        public AttackCategoryType AttackCategoryType { get;set; }
        public DamageViewType DamageViewType { get;set; }

        /// <summary>
        /// 伤害值
        /// </summary>
        public int OriginalVariation { get; set; }

        /// <summary>
        /// 伤害方向
        /// </summary>
        public GfFloat2 DamageVector;
        
        /// <summary>
        /// 削韧值
        /// </summary>
        public int ReducePoiseValue   { get; set; }

        /// <summary>
        /// 反应等级
        /// </summary>
        public ReactionLevelType ReactionLevelType { get; set; }
        
        /// <summary>
        /// 水平冲击力
        /// </summary>
        public int HorizontalPower { get; set; }

        /// <summary>
        /// 垂直冲击力
        /// </summary>
        public int VerticalPower { get; set; }
        
        public bool IsCritical { get; set; }

        public CameraShakeParamData CameraShakeData { get; set; }
        
        public GfHandle AttackerHandle => AttackParameter.ThisHandle;
        public GfHandle DefenderHandle => DefendParameter.OwnerHandle;

        public BattleColliderAttackParameter AttackParameter;
        public BattleColliderDefendParameter DefendParameter;
        
        public float AttackerHitStopSpan;
        public float DefenderHitStopSpan;

        public void OnReturnToPool()
        {
            Clear();
        }

        private void Clear() 
        {
            OriginalVariation = 0;
            DamageVector = GfFloat2.Zero;
            ReducePoiseValue = 0;
            ReactionLevelType = ReactionLevelType.None;
            HorizontalPower = 0;
            VerticalPower = 0;
            DamageViewType = DamageViewType.Normal;
            CameraShakeData = null;
            IsCritical = false;
        }
    }
}
