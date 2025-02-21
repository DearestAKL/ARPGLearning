using System;
using System.Diagnostics;
using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public class BattleAdmin : IDisposable
    {
        private static BattleAdmin _instance;
        private BattleGameAdministrator _currentAdministrator;
        public BattleGameAdministrator CurrentAdministrator => _currentAdministrator;

        public static GfEntityComponentSystem EntityComponentSystem => Instance._currentAdministrator.EntityComponentSystem;

        public static BattleDamageHandler DamageHandler => Instance._currentAdministrator.DamageHandler;

        public static GfColliderDefendIdManager2D DefendColliderIdManager => Instance._currentAdministrator.DefendColliderIdManager;

        public static GfVfxManager VfxManager => Instance._currentAdministrator.GfVfxManager;
        public static BattleTime Time => Instance._currentAdministrator.Time;

        public static IBattleFactory Factory => Instance._currentAdministrator.Factory;
        public static IGfRandomGenerator RandomGenerator => Instance._currentAdministrator.RandomGenerator;

        public static IBattleCharacterAccessorComponent Player => PlayerEntity is { IsActive: true }
            ? PlayerEntity.GetComponent<BattleCharacterAccessorComponent>()
            : null;

        public static GfEntity PlayerEntity { private get;set; }

        public static bool HasInstance => _instance != null;
        
        public static BattleAdmin Instance
        {
            get
            {
                Debug.Assert(_instance != null);
                return _instance;
            }
        }

        public static void Init(BattleGameAdministrator battleGameAdministrator)
        {
            Debug.Assert(_instance == null);
            Debug.Assert(battleGameAdministrator != null);

            _instance = new BattleAdmin();
            _instance._currentAdministrator = battleGameAdministrator;
        }
        
        public void Dispose()
        {
            _currentAdministrator = null;
            _instance = null;
        }

        //全局暂停
        public static void SetPaused(bool paused)
        {
            Instance._currentAdministrator.IsInPaused = paused;
            VfxManager.SetPauseTargetGroup(0xffffffff, paused, true);
        }

        public void ClearBattle()
        {
            ClearBattleShells();
            ClearBattleEffects();
        }

        private void ClearBattleShells()
        {
            EntityComponentSystem.DeleteTargetGroup(GfEntityGroupId.Shell);
        }

        private void ClearBattleEffects()
        {
            ulong effectGroupIds = 0UL;
            effectGroupIds |= 1UL << (int)EffectGroup.OneShot;
            effectGroupIds |= 1UL << (int)EffectGroup.Action;
            effectGroupIds |= 1UL << (int)EffectGroup.Shell;
            VfxManager.ForceRemoveTargetGroup(effectGroupIds);
        }
    }
}
