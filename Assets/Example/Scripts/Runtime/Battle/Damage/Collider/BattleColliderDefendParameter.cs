using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class BattleColliderDefendParameter : IGfColliderDefendParameter2D
    {
        public GfColliderDefendId2D DefendColliderId { get; }

        public uint Layer { get; }

        public uint Level { get; }

        public GfHandle ThisHandle { get;}
        public GfHandle OwnerHandle { get;}

        public IBattleObjectDamageReceiverHandler DamageReceiverHandler { get; }
        public IBattleObjectDamageNotificator DamageNotificator { get; }

        public BattleColliderDefendParameter(
            GfHandle thisHandle,
            GfHandle ownerHandle,
            GfColliderDefendId2D defendColliderId, 
            IBattleObjectDamageReceiverHandler damageReceiverHandler,
            IBattleObjectDamageNotificator damageNotificator)
        {
            DefendColliderId = defendColliderId;
            DamageReceiverHandler = damageReceiverHandler;
            DamageNotificator = damageNotificator;
            Layer = (uint)GetBattleLayerMask();
        }

        private BattleLayerMask GetBattleLayerMask()
        {
            BattleLayerMask layerMask;
            if (DamageReceiverHandler.TeamId == TeamId.TeamA)
            {
                layerMask = BattleLayerMask.TeamA;
            }
            else if (DamageReceiverHandler.TeamId == TeamId.TeamB)
            {
                layerMask = BattleLayerMask.TeamB;
            }
            else 
            {
                layerMask = BattleLayerMask.Invalid;
            }

            return layerMask;
        }
    }
}
