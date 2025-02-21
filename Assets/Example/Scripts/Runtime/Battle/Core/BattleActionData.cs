using System;
using System.Threading;
using Akari.GfCore;
using Akari.GfGame;
namespace GameMain.Runtime
{
    public class BattleActionData : IGfActionData
    {
        private GfPoolAbleInternal poolAbleInternal;
        
        public ref GfPoolAbleInternal PoolAbleInternal => ref poolAbleInternal;
        
        public GfRunTimeTypeId RttId { get;  private set;}

        protected static TActionData Create<TActionData>(Func<TActionData> factory)
            where TActionData : BattleActionData
        {
            var data = GfGlobalInstancePool<TActionData>.Create(factory);
            data.RttId = GfRunTimeTypeOf<TActionData>.Id;
            
            return data;
        }

        public virtual void Dispose()
        {
            this.ReturnToPool();
        }
        
        public virtual void OnReturnToPool()
        {

        }
    }
}
