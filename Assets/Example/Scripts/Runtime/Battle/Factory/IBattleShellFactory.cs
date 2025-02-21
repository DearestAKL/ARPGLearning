using System;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public interface IBattleShellFactory : IDisposable
    {
        GfEntity CreateShell(uint shellId, ShellDefinitionMessage shellDefinition,BattleShellDamageCauserHandler shellDamageCauserHandler, in GfFloat3 basePosition, in GfFloat3 offset, in GfFloat3 direction);

        void CreateShellEffect(GfHandle ownerHandle, GfEntity entity, string effectId);
    }
}
