using System;
using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    //public interface IBattleObjectDamageBaseCalcCauserHandler : IDisposable
    //{

    //}

    //public interface IBattleObjectDamageCalcCauserHandler : IBattleObjectDamageBaseCalcCauserHandler
    //{

    //}

    //public interface IBattleObjectDamageCauserHandler: IBattleObjectDamageCalcCauserHandler
    //{

    //}

    public interface IBattleObjectDamageCauserHandler : IDisposable
    {
        TeamId TeamId { get; }

        GfFloat3 GetCauserPosition();

        AttackDefinitionInfoData[] AttackDefinitions{ get; set; }
        
        int GetLevel();
        float GetMaxHp();
        float GetAttack();
        float GetDefense();
        float GetDamageBonus();
        float GetCriticalHitRate();
        float CriticalHitDamage();

    }

}
