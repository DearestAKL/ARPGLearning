using Akari.GfCore;

namespace GameMain.Runtime
{
    public abstract class ABattleShellComponent : ABattleAttackComponent<ABattleShellComponent>
    {
        protected BattleShellDamageCauserHandler ShellDamageCauserHandler { get; private set; }
        public sealed override IBattleObjectDamageCauserHandler DamageCauserHandler { get; protected set; }
        protected sealed override GfHandle                         OwnerHandle         => ShellDamageCauserHandler.OwnerHandle;
        public sealed override    TeamId                           TeamId              => ShellDamageCauserHandler.TeamId;
        
        public ABattleShellComponent(BattleShellDamageCauserHandler shellDamageCauserHandler)
        {
            ShellDamageCauserHandler = shellDamageCauserHandler;
            DamageCauserHandler = ShellDamageCauserHandler;
        }
        
        public sealed override void OnAwake()
        {
            base.OnAwake();
            OnAwakeInternal();
        }
        
        public sealed override void OnEnd()
        {
            base.OnEnd();
        }
        
        public sealed override void OnStart()
        {
            base.OnStart();
            OnStartInternal();
        }
        
        public sealed override void OnBeginUpdate(float deltaTime)
        {
            base.OnBeginUpdate(deltaTime);
        }

        public sealed override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            OnUpdateInternal(deltaTime);
        }

        public sealed override void OnEndUpdate(float deltaTime)
        {
            base.OnEndUpdate(deltaTime);
        }
        
        public sealed override void OnDelete()
        {
            OnDeleteInternal();
            if (DamageCauserHandler != null)
            {
                DamageCauserHandler.Dispose();
                DamageCauserHandler = null;
            }

            base.OnDelete();
        }
        
        protected abstract void OnAwakeInternal();
        protected abstract void OnStartInternal();
        protected abstract void OnUpdateInternal(float deltaTime);
        protected abstract void OnDeleteInternal();

        protected void Delete()
        {
            BattleAdmin.EntityComponentSystem.Delete(ThisHandle);
        }
        
        protected override void DidCauseDamage(in BattleDidCauseDamageRequest request)
        {
            BattleAdmin.EntityComponentSystem.RequestToEntity(OwnerHandle, request);
        }
    }
}