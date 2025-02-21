using Akari.GfCore;

namespace GameMain.Runtime
{
    public interface IBattleSimpleDamageResult
    {
        AttackCategoryType AttackCategoryType { get; }
        DamageViewType DamageViewType { get; }
        int OriginalVariation { get;}
        
        GfHandle AttackerHandle { get; }
        GfHandle DefenderHandle { get; }
    }
    
    public class BattleSimpleDamageResult : IBattleSimpleDamageResult, IGfLocalPoolable
    {
        public GfHandle                       CauserHandle              { get; private set; }
        public IBattleObjectDamageNotificator DefenderDamageNotificator { get; private set; }
        
        public AttackCategoryType AttackCategoryType { get; set; }
        public DamageViewType DamageViewType { get; set;}
        public int OriginalVariation { get; set; }
        
        public GfHandle AttackerHandle { get => CauserHandle; }
        
        public GfHandle DefenderHandle { get => DefenderDamageNotificator.SelfHandle; }

        public void Initialize(GfHandle causerHandle,
            IBattleObjectDamageNotificator defenderDamageNotificator,
            int originalVariation, AttackCategoryType attackCategoryType, DamageViewType damageViewType)
        {
            CauserHandle              = causerHandle;
            DefenderDamageNotificator = defenderDamageNotificator;
            AttackCategoryType        = attackCategoryType;
            OriginalVariation        = originalVariation;
            DamageViewType        = damageViewType;
        }

        public void OnReturnToPool()
        {
            Clear();
        }

        private void Clear()
        {
            CauserHandle = GfHandle.Invalid;
            DefenderDamageNotificator = null;
            AttackCategoryType = AttackCategoryType.Damage;
            DamageViewType = DamageViewType.Normal;
            OriginalVariation = 0;
        }
    }
}