using System;

namespace GameMain.Runtime
{
    public interface IBattleFactory : IDisposable
    {
        IBattleEffectFactory            Effect            { get; }
        IBattleCharacterFactory         Character         { get; }
        IBattleShellFactory         Shell         { get; }
    }
    
}
