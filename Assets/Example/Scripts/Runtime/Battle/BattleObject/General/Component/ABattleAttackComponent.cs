using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public abstract class ABattleAttackComponent<TComponent> : AGfGameComponent<TComponent>
    where TComponent : AGfGameComponent<TComponent>
    {
        private GfComponentCache<GfColliderComponent2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>> _colliderCache;

        public abstract IBattleObjectDamageCauserHandler DamageCauserHandler { get; protected set; }

        protected GfColliderComponent2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> Collider => Entity.GetComponent(ref _colliderCache);


        protected abstract GfHandle OwnerHandle { get; }
        public abstract TeamId TeamId { get; }
        
        public override void OnStart()
        {
            Entity.On<BattleRegisterColliderAttackRequest>(OnBattleRegisterColliderAttackRequest);
            Entity.On<BattleDidCauseDamageRequest>(OnBattleDidCauseDamageRequest);
        }

        private void OnBattleDidCauseDamageRequest(in BattleDidCauseDamageRequest request)
        {
            DidCauseDamage(request);
        }
        
        private void OnBattleRegisterColliderAttackRequest(in BattleRegisterColliderAttackRequest request)
        {
            var group = Collider.GetGroup(request.AttackId.ToColliderGroupId());
            if (group == null)
            {
                var attackDefinitions = DamageCauserHandler.AttackDefinitions;
                if (attackDefinitions == null)
                {
                    return;
                }
                for (int i = 0; i < attackDefinitions.Length; i++)
                {
                    var attackDefinition = attackDefinitions[i];
                    if (attackDefinition.AttackId == request.AttackId.GeneralId || attackDefinitions.Length == 1)
                    {
                        var singleAttackModel = new SingleAttackModel(request.AttackId, attackDefinition);
                        group = SetupColliderAttackGroup(singleAttackModel);
                    }
                }
            }

            group.SetEnable(request.IsOn);
        }

        private GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> SetupColliderAttackGroup(SingleAttackModel singleAttackModel) 
        {
            var colliders = new GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>[singleAttackModel.Collisions.Length];
            for (var i = 0; i < singleAttackModel.Collisions.Length; i++)
            {
                var collisionData = singleAttackModel.Collisions[i];
                colliders[i] =
                    new GfCollider2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(collisionData.Offset,
                        collisionData.Extents, false);
            }

            GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> group = null;

            group = new GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(
                colliders, GfAxis.Z, GfAxis.None);
            Collider.Add(singleAttackModel.AttackId.ToColliderGroupId(), group);

            var layerMask = GetBattleLayerMask(singleAttackModel.Info.HitCategory);
            var attackParameter = new BattleColliderAttackParameter(ThisHandle, OwnerHandle, IssueAttackColliderId(singleAttackModel.Info.IgnoreTime != 0), (uint)layerMask, DamageCauserHandler, singleAttackModel);
            group.SetParameter(attackParameter);
            return group;
        }

        private uint AutoGroupUniqueGroupId = 0;
        private GfColliderAttackId2D IssueAttackColliderId(bool enableIgnoreTime)
        {
            //var enableIgnoreTime = singleAttackModel.AttackId.AttackType == AttackType.Shell | singleAttackModel.AttackId.AttackType == AttackType.Gimmick;
            //var uniqueId = 0U;

            //if (singleAttackModel.Info.GroupNumber != 0 && Animation != null)
            //{
            //    GfAssert.ASSERT(singleAttackModel.Info.GroupNumber <= 15, "groupNumberは0〜15までです。");
            //    uniqueId = ((uint)Animation.PlayingClipInfo.ClipIndex) << 4;
            //    uniqueId |= singleAttackModel.Info.GroupNumber;
            //}
            //else
            //{
            //    uniqueId = BattleAdmin.AutoGroupUniqueGroupId++;
            //}

            //return Collider.IssueAttackId(uniqueId, enableIgnoreTime);
            var uniqueId = AutoGroupUniqueGroupId++;

            return Collider.IssueAttackId(uniqueId, enableIgnoreTime);
        }

        private BattleLayerMask GetBattleLayerMask(BattleHitCategory hitCategory) 
        {
            BattleLayerMask layerMask = BattleLayerMask.Invalid;
            if (TeamId == TeamId.TeamA)
            {
                //TeamA

                if ((hitCategory & BattleHitCategory.Other) != 0)
                {
                    layerMask |= BattleLayerMask.TeamB;
                }

                if ((hitCategory & BattleHitCategory.SameIncludingMySelf) != 0 )
                {
                    layerMask |= BattleLayerMask.TeamA;
                }

            }
            else 
            {
                //TeamB

                if (hitCategory == BattleHitCategory.Other)
                {
                    layerMask |= BattleLayerMask.TeamA;
                }

                if ((hitCategory & BattleHitCategory.SameIncludingMySelf) != 0)
                {
                    layerMask |= BattleLayerMask.TeamB;
                }
            }

            return layerMask;
        }
        
        protected abstract void DidCauseDamage(in BattleDidCauseDamageRequest request);
    }
}
