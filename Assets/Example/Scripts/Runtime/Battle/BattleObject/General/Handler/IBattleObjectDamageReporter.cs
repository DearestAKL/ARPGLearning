using System.Collections.Generic;

namespace GameMain.Runtime
{
    public interface IBattleObjectDamageReporter
    {
        void Report(List<BattleDamageResult> battleDamageResults,List<BattleSimpleDamageResult> battleSimpleDamageResults);
    }
}
