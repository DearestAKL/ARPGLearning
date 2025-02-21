using System;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public class GimmickDamageNotificator : IBattleObjectDamageNotificator
    {
        public GfHandle SelfHandle => _entity.ThisHandle;

        private GfEntity _entity;
        
        public GimmickDamageNotificator(GfEntity entity)
        {
            _entity = entity;
        }

        public void Dispose()
        {
            _entity = null;
        }

        GfHandle IBattleObjectDamageNotificator.SelfHandle => SelfHandle;

        public void ReceiveDamage(BattleDamageResult damageResult, BattleDamageHandler rootHandler)
        {
            GfLog.Debug("GimmickDamageNotificator ReceiveDamage");
            
            _entity.Request(new BattleReceivedDamageRequest(damageResult));
            _entity.RequestToOther(damageResult.AttackerHandle, new BattleDidCauseDamageRequest(damageResult));
        }

        public void ReceiveSimpleDamage(BattleSimpleDamageResult damageResult, BattleDamageHandler rootHandler)
        {
            throw new NotImplementedException();
        }

        void IBattleObjectDamageNotificator.Apply(IGfRandomGenerator randomGenerator, BattleDamageHandler rootHandler)
        {
            Apply(randomGenerator, rootHandler);
        }

        public void Apply(IGfRandomGenerator randomGenerator, BattleDamageHandler rootHandler)
        {
            throw new NotImplementedException();
        }
    }
}