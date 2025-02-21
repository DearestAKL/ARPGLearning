using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public class BattleShellDamageCauserHandler : IBattleObjectDamageCauserHandler
    {
        public GfHandle                     OwnerHandle            { get; }
        public TeamId                       TeamId                 { get; }
        public GfEntity                     ThisEntity             { get; private set; }

        private readonly BattleCharacterAccessorComponent _ownerAccessor;
        
        public AttackDefinitionInfoData[] AttackDefinitions { get; set; }
        public AttackDefinitionInfoData AttackDefinition 
        {
            get
            {
                if (AttackDefinitions == null || AttackDefinitions.Length <= 0)
                {
                    return null;
                }
                return AttackDefinitions[0];
            }
        }

        public BattleShellDamageCauserHandler(GfEntity owner, TeamId teamId,AttackDefinitionInfoData[] attackDefinitions)
        {
            OwnerHandle = owner.ThisHandle;
            TeamId = teamId;
            AttackDefinitions = attackDefinitions;
            _ownerAccessor = owner.GetComponent<BattleCharacterAccessorComponent>();
        }
        
        public BattleShellDamageCauserHandler(GfEntity owner, TeamId teamId,AttackDefinitionInfoData attackDefinition)
        {
            OwnerHandle = owner.ThisHandle;
            TeamId = teamId;
            AttackDefinitions = new AttackDefinitionInfoData[]{attackDefinition};
            _ownerAccessor = owner.GetComponent<BattleCharacterAccessorComponent>();
        }

        public void SetThisHandle(GfEntity thisEntity)
        {
            ThisEntity = thisEntity;
        }


        public void Dispose()
        {
        }
        
        public GfFloat3 GetCauserPosition()
        {
            return ThisEntity.Transform.Position;
        }
        
        public int GetLevel()
        {
            return _ownerAccessor.Condition.Level;
        }

        public float GetMaxHp()
        {
            return _ownerAccessor.Condition.HpProperty.TotalMaxValue;
        }

        public float GetAttack()
        {
            return _ownerAccessor.Condition.AttackProperty.TotalValue;
        }

        public float GetDefense()
        {
            return _ownerAccessor.Condition.DefenseProperty.TotalValue;
        }

        public float GetDamageBonus()
        {
            return _ownerAccessor.Condition.DamageBonusProperty.TotalValue;
        }

        public float GetCriticalHitRate()
        {
            return _ownerAccessor.Condition.CriticalHitRateProperty.TotalValue;
        }

        public float CriticalHitDamage()
        {
            return _ownerAccessor.Condition.CriticalHitDamageProperty.TotalValue;
        }
    }
}