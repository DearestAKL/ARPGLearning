using System;
using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public class BattleGameAdministrator : IDisposable
    {
        public GfEntityComponentSystem EntityComponentSystem { get; private set; }
        public BattleDamageHandler DamageHandler { get; private set; }
        public GfColliderDefendIdManager2D DefendColliderIdManager { get; private set; }
        public RootRandomGenerator RootRandomGenerator { get; set; }
        
        public BattleTime                 Time                    { get; private set; }

        public IBattleFactory Factory { get; private set; }

        public GfVfxManager GfVfxManager { get; private set; }
        
        public IGfRandomGenerator RandomGenerator { get; private set; }
        
        public bool IsInPaused{ get; set; }

        public BattleGameAdministrator(GfEntityComponentSystem entityComponentSystem,
            RootRandomGenerator rootRandomGenerator,
            IBattleFactory battleFactory
        )
        {
            EntityComponentSystem = entityComponentSystem;
            RootRandomGenerator = rootRandomGenerator;

            Time = new BattleTime();

            RandomGenerator = RootRandomGenerator.CreateGenerator();
            DamageHandler = new BattleDamageHandler(RandomGenerator);

            DefendColliderIdManager = new GfColliderDefendIdManager2D();

            Factory = battleFactory;

            GfVfxManager = new GfVfxManager();
        }

        public void Dispose()
        {
            if (EntityComponentSystem != null)
            {
                EntityComponentSystem.Dispose();
                EntityComponentSystem = null;
            }
        }

        public void OnBeginUpdate(float deltaTime)
        {
            if (BattleAdmin.Time.IsPaused != IsInPaused)
            {
                BattleAdmin.Time.IsPaused = IsInPaused;
                if (IsInPaused)
                {
                    PausedEntities();
                }
                else
                {
                    ResumeEntities();
                }
            }
            
            Time.OnUpdate(deltaTime);
            if (!BattleAdmin.Time.IsPaused)
            {
                EntityComponentSystem.OnBeginUpdate(deltaTime);
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (!BattleAdmin.Time.IsPaused)
            {
                EntityComponentSystem.OnUpdate(deltaTime);
            }
        }

        public void OnEndUpdate(float deltaTime)
        {
            if (!BattleAdmin.Time.IsPaused)
            {
                EntityComponentSystem.OnEndUpdate(deltaTime);
            }
        }
        
        private void PausedEntities()
        {
            foreach (var entity in EntityComponentSystem.EntityManager.EntityHandleManager.Buffers)
            {
                if (!entity.IsValid() || entity.GroupId == GfEntityGroupId.Camera)
                {
                    continue;
                }
                
                var characterAccessorComponent = entity.GetComponent<BattleCharacterAccessorComponent>();
                if (characterAccessorComponent != null && !characterAccessorComponent.Condition.HpProperty.IsAlive) 
                {
                    continue;
                }
                
                entity.SetPause(true);
            }
        }
        
        private void ResumeEntities()
        {
            foreach (var entity in EntityComponentSystem.EntityManager.EntityHandleManager.Buffers)
            {
                if (!entity.IsValid() || entity.GroupId == GfEntityGroupId.Camera)
                {
                    continue;
                }
                
                entity.SetPause(false);
            }
        }
    }
}