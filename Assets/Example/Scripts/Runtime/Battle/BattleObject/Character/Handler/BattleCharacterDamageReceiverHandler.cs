using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterDamageReceiverHandler : IBattleObjectDamageReceiverHandler
    {
        private readonly IBattleCharacterAccessorComponent _accessor;

        public TeamId TeamId => _accessor.Condition.TeamId;

        public BattleCharacterDamageReceiverHandler(IBattleCharacterAccessorComponent accessorComponent)
        {
            _accessor = accessorComponent;
        }
        
        public GfFloat3 GetReceiverPosition()
        {
            return _accessor.Transform.Transform.Position;
        }

        public bool CanReceiveKnockUp()
        {
            return _accessor.Condition.CanReceiveKnockUp;
        }

        public float GetDefense()
        {
            return _accessor.Condition.DefenseProperty.TotalValue;
        }
        
        public float GetDamageReduction()
        {
            return _accessor.Condition.DamageReductionProperty.TotalValue;
        }

        public void Dispose()
        {

        }
    }
}
