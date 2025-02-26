using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterDamageCauserHandler : IBattleObjectDamageCauserHandler
    {
        private readonly IBattleCharacterAccessorComponent _accessor;

        public TeamId TeamId => _accessor.Condition.TeamId;

        public BattleCharacterDamageCauserHandler(IBattleCharacterAccessorComponent accessorComponent)
        {
            _accessor = accessorComponent;
        }

        public GfFloat3 GetCauserPosition()
        {
            return _accessor.Entity.Transform.Position;
        }

        public GfFloat2 CalculateDamageVector(GfFloat3 receiverPosition)
        {
            return (receiverPosition - _accessor.Entity.Transform.Position).ToXZFloat2().Normalized;
        }

        public AttackDefinitionInfoData[] AttackDefinitions { get; set; }
        public int GetLevel()
        {
            return _accessor.Condition.Level;
        }

        public float GetMaxHp()
        {
            return _accessor.Condition.HpProperty.TotalMaxValue;
        }

        public float GetAttack()
        {
            return _accessor.Condition.AttackProperty.TotalValue;
        }

        public float GetDefense()
        {
            return _accessor.Condition.DefenseProperty.TotalValue;
        }

        public float GetDamageBonus()
        {
            return _accessor.Condition.DamageBonusProperty.TotalValue;
        }

        public float GetCriticalHitRate()
        {
            return _accessor.Condition.CriticalHitRateProperty.TotalValue;
        }

        public float CriticalHitDamage()
        {
            return _accessor.Condition.CriticalHitDamageProperty.TotalValue;
        }

        public void Dispose()
        {

        }
    }
}
