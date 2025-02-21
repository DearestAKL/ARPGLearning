using System;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public class BattleColliderDamageHandler : IGfDamageHandler2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>
    {
        private BattleDamageHandler _damageHandler;

        public BattleColliderDamageHandler(BattleDamageHandler damageHandler)
        {
            _damageHandler = damageHandler ?? throw new ArgumentNullException(nameof(damageHandler));
        }


        public void Dispose()
        {
            if (_damageHandler != null)
            {
                _damageHandler.Dispose();
                _damageHandler = null;
            }
        }

        public bool IsCollisionTarget(GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> attackColliderGroup, GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter> defendColliderGroup)
        {
            return _damageHandler.IsCollisionTarget(attackColliderGroup.AttackParameter, defendColliderGroup.DefendParameter);
        }

        public bool Handling(in GfDamageClaim2D<BattleColliderAttackParameter, BattleColliderDefendParameter> damageClaim)
        {
            return _damageHandler.HandleDamage(damageClaim.AttackParameter, damageClaim.DefendParameter);
        }

        public void PostProcess()
        {
            _damageHandler.PostProcess();
        }

        public float CalcIgnoreTime(in GfDamageClaim2D<BattleColliderAttackParameter, BattleColliderDefendParameter> damageClaim)
        {
            //damageClaim.AttackParameter.DamageCauserHandler.IgnoreTime
            return damageClaim.AttackParameter.SingleAttackModel.Info.IgnoreTime;
        }
    }
}
