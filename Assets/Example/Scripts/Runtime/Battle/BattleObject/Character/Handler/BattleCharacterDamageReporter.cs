using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public class BattleCharacterDamageReporter : IBattleObjectDamageReporter
    {
        private readonly GfEntity _entity;

        public BattleCharacterDamageReporter(GfEntity entity) => _entity = entity;

        public void Report(List<BattleDamageResult> battleDamageResults,List<BattleSimpleDamageResult> battleSimpleDamageResults)
        {
            _entity.Request(new BattlePlayDamageEffectAndUIRequest(battleDamageResults.ToArray(),battleSimpleDamageResults.ToArray()));
        }
    }
}
