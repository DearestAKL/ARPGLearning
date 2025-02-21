using System;
using Akari.GfCore;
using Akari.GfGame;
using GameMain.Runtime;

namespace GameMain.Runtime
{
    public abstract class ACharacterAnimationEventListener : IDisposable
    {
        private GfEntity _entity;

        protected GfComponentCache<BattleCharacterAccessorComponent> AccessorCache;

        protected BattleCharacterAccessorComponent  Accessor => _entity.GetComponent(ref AccessorCache);

        protected ACharacterAnimationEventListener(GfEntity entity)
        {
            _entity  = entity;
        }

        protected GfEntity GetSelfEntity()
        {
            return _entity;
        }

        public virtual void Dispose()
        {
            _entity = null;
        }
    }
}