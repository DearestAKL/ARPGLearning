using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public class BattleCharacterWarningComponent : AGfGameComponent<BattleCharacterWarningComponent>
    {
        private GfComponentCache<GfColliderComponent2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>> _colliderCache;
        private GfComponentCache<BattleCharacterAccessorComponent> _accessorCache;

        private IBattleObjectDamageCauserHandler _damageCauserHandler;

        private GfColliderComponent2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> Collider => Entity.GetComponent(ref _colliderCache);
        
        private BattleCharacterAccessorComponent Accessor => Entity.GetComponent(ref _accessorCache);

        public void SetDamageCauserHandler(IBattleObjectDamageCauserHandler damageCauserHandler)
        {
            _damageCauserHandler = damageCauserHandler;
        }

        public override void OnStart()
        {
            Entity.On<BattleRegisterColliderWarningRequest>(OnBattleRegisterColliderWarningRequest);
        }

        private void OnBattleRegisterColliderWarningRequest(in BattleRegisterColliderWarningRequest request)
        {
            var group = Collider.GetGroup(request.AttackId.ToColliderGroupId());
            if (group == null)
            {
                if (_damageCauserHandler != null)
                {
                    var attackDefinitions = _damageCauserHandler.AttackDefinitions;
                    if (attackDefinitions == null)
                    {
                        return;
                    }
                    for (int i = 0; i < attackDefinitions.Length; i++)
                    {
                        var attackDefinition = attackDefinitions[i];
                        if (attackDefinition.AttackId == request.AttackId.GeneralId || attackDefinitions.Length == 1)
                        {
                            var collisions = attackDefinition.Collisions;
                            group = SetupColliderWarningGroup(request.AttackId, collisions);
                        }
                    }
                }
                else
                {
                    return;
                }
            }

            group.SetEnable(request.IsOn);
        }
        
        private GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> SetupColliderWarningGroup(AttackId attackId,AttackDefinitionCollisionData[] collisions) 
        {
            var colliders = new GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>[collisions.Length];
            for (var i = 0; i < collisions.Length; i++)
            {
                var collisionData = collisions[i];
                colliders[i] =
                    new GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(collisionData.Offset,
                        collisionData.Extents, false);
            }

            GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> group = null;

            group = new GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(
                colliders, GfAxis.Z, GfAxis.None);
            Collider.Add(attackId.ToColliderGroupId(), group);

            BattleLayerMask layerMask = Accessor.Condition.TeamId == TeamId.TeamA
                ? BattleLayerMask.TeamB
                : BattleLayerMask.TeamA;

            var warningParameter = new GfColliderWarningParameter2D((uint)layerMask);
            group.SetParameter(warningParameter);
            
            return group;
        }
        
        private GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> SetupColliderWarningGroup(AttackId attackId,GfFloat2 offset,GfFloat2 extents)
        {
            var colliders =
                new GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>[]
                {
                    new GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter,
                        BattleColliderDefendParameter>(offset,
                        extents, false)
                };

            GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> group = null;

            group = new GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(
                colliders, GfAxis.Z, GfAxis.None);
            Collider.Add(attackId.ToColliderGroupId(), group);

            BattleLayerMask layerMask = Accessor.Condition.TeamId == TeamId.TeamA
                ? BattleLayerMask.TeamB
                : BattleLayerMask.TeamA;

            var warningParameter = new GfColliderWarningParameter2D((uint)layerMask);
            group.SetParameter(warningParameter);
            
            return group;
        }
    }
}