using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class BattleObjectActionContext : IGfActionContext
    {
        public GfEntity Entity { get; set; }

        public BattleObjectActionContext(GfEntity entity)
        {
            Entity = entity;
        }
    }

    public sealed class BattleObjectActionComponent : GfActionComponent<BattleObjectActionComponent, BattleObjectActionContext>
    {
        public BattleObjectActionComponent(BattleObjectActionContext context, bool enablePrioritySort = true) : base(context, enablePrioritySort)
        {
            
            
        }
    }
}