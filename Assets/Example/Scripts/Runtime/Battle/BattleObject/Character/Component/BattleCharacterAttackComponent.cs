using Akari.GfCore;

namespace GameMain.Runtime
{
    public class BattleCharacterAttackComponent : ABattleAttackComponent<BattleCharacterAttackComponent>
    {
        private GfComponentCache<BattleCharacterAccessorComponent> _accessorCache;

        public override IBattleObjectDamageCauserHandler DamageCauserHandler { get; protected set; }

        private BattleCharacterAccessorComponent Accessor => Entity.GetComponent(ref _accessorCache);

        protected override GfHandle OwnerHandle => ThisHandle;

        public override TeamId TeamId => Accessor.Condition.TeamId;

        public void Initialize(IBattleObjectDamageCauserHandler damageCauserHandler) 
        {
            DamageCauserHandler = damageCauserHandler;
        }

        public override void OnDelete()
        {
            if (DamageCauserHandler != null)
            {
                DamageCauserHandler.Dispose();
                DamageCauserHandler = null;
            }

            base.OnDelete();
        }
        
        protected override void DidCauseDamage(in BattleDidCauseDamageRequest request)
        {
        }
    }
}