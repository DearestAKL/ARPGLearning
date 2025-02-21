using System;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public interface IBattleObjectDamageNotificator : IDisposable
    {
        GfHandle SelfHandle { get; }

        void ReceiveDamage(BattleDamageResult damageResult, BattleDamageHandler rootHandler);
        void ReceiveSimpleDamage(BattleSimpleDamageResult damageResult, BattleDamageHandler rootHandler);
        void Apply(IGfRandomGenerator randomGenerator, BattleDamageHandler rootHandler);
    }
    
    public abstract class ABattleObjectDamageNotificator : IBattleObjectDamageNotificator
    {
        public abstract GfHandle SelfHandle { get; }

        private bool _hasReceived;

        public abstract void Dispose();

        public void ReceiveDamage(BattleDamageResult damageResult, BattleDamageHandler rootHandler)
        {
            ReceiveDamage(damageResult);
            OnReceived(rootHandler);
        }

        public void ReceiveSimpleDamage(BattleSimpleDamageResult simpleDamageResult, BattleDamageHandler rootHandler)
        {
            ReceiveSimpleDamage(simpleDamageResult);

            OnReceived(rootHandler);
        }

        public void Apply(IGfRandomGenerator randomGenerator, BattleDamageHandler rootHandler)
        {
            Apply(randomGenerator);

            _hasReceived = false;
        }

        protected abstract void ReceiveDamage(BattleDamageResult damageResult);
        
        protected abstract void ReceiveSimpleDamage(BattleSimpleDamageResult simpleDamageResult);
        protected abstract void Apply(IGfRandomGenerator randomGenerator);

        private void OnReceived(BattleDamageHandler rootHandler)
        {
            if (_hasReceived)
            {
                return;
            }

            _hasReceived = true;
            rootHandler.NotifyNotificatorHasReceived(this);
        }

        protected IBattleCharacterAccessorComponent GetCauserAccessor(BattleDamageResult damageResult)
        {
            var causeEntity = BattleAdmin.EntityComponentSystem.EntityManager.Get(damageResult.AttackParameter.OwnerHandle);
            return causeEntity.GetComponent<BattleCharacterAccessorComponent>();
        }
    }
}