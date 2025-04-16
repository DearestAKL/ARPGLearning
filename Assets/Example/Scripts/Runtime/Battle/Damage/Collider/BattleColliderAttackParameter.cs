using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class BattleColliderAttackParameter : IGfColliderAttackParameter2D
    {
        public GfColliderAttackId2D AttackColliderId { get; }

        public uint Layer { get; }

        public uint Level { get; }

        public GfHandle ThisHandle { get; }
        public GfHandle OwnerHandle { get; }

        public IBattleObjectDamageCauserHandler DamageCauserHandler { get; }

        public SingleAttackModel SingleAttackModel { get; }
        

        public BattleColliderAttackParameter(
            GfHandle thisHandle,
            GfHandle ownerHandle, 
            GfColliderAttackId2D attackColliderId,
            uint layer,
            IBattleObjectDamageCauserHandler damageCauserHandler, 
            SingleAttackModel singleAttackModel)
        {
            ThisHandle = thisHandle;
            OwnerHandle = ownerHandle;
            AttackColliderId = attackColliderId;
            Layer = layer;
            DamageCauserHandler = damageCauserHandler;
            SingleAttackModel = singleAttackModel;
        }
    }
}
