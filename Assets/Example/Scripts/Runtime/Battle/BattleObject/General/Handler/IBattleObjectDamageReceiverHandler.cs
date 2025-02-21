using System;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public interface IBattleObjectDamageReceiverHandler : IDisposable 
    {
        TeamId TeamId { get; }

        GfFloat3 GetReceiverPosition();
        bool CanReceiveKnockUp();
        float GetDefense();
        float GetDamageReduction();
    }
}
