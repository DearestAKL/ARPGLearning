using System;
using Akari.GfCore;
using Akari.GfGame;
using Cysharp.Threading.Tasks;

namespace GameMain.Runtime
{
    public interface IBattleEffectFactory : IDisposable
    {
        UniTask<GfHandle> CreateEffect(GfEntity owner,string effectId, EffectGroup effectGroup, GfFloat3 position, GfQuaternion rotation, GfFloat3 scale,
            GfVfxDeleteMode deleteMode, GfVfxPriority priority);
    
        UniTask<GfHandle> CreateEffectByEntity(GfEntity owner, string effectId, EffectGroup effectGroup, GfFloat3 offsetPosition, GfQuaternion offsetRotation, GfFloat3 offsetScale,
            GfVfxDeleteMode deleteMode, GfVfxPriority priority);
    }
}
