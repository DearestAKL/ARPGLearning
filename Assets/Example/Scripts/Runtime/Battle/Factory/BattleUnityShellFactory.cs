using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class BattleUnityShellFactory : IBattleShellFactory
    {
        public void Dispose()
        {

        }

        public GfEntity CreateShell(uint shellId, ShellDefinitionMessage shellDefinition,BattleShellDamageCauserHandler shellDamageCauserHandler, in GfFloat3 position, in GfFloat3 direction)
        {
            var entity = BattleAdmin.EntityComponentSystem.Create(shellDamageCauserHandler.OwnerEntityTagId, GfEntityGroupId.Shell, $"shell{shellId}");
            
            var gfTransform = new GfTransform();
            gfTransform.Position = position;
            
            entity.AddComponent(new GfActorComponent(gfTransform));
            entity.AddComponent(new GfColliderComponent2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>());
            entity.AddComponent(new GfVfxTrackerComponent(BattleAdmin.VfxManager, GfAxis.X, 0.15f));

            ABattleShellComponent shellComponent;
            if (shellDefinition.ShellType == (int)ShellType.Area)
            {
                shellComponent = new BattleShellAreaComponent(shellDamageCauserHandler,shellDefinition.Duration);
            }
            else
            {
                shellComponent = new BattleShellBulletComponent(shellDamageCauserHandler, shellDefinition.Duration,
                    shellDefinition.Bullet.Speed, (BulletType)shellDefinition.Bullet.BulletType, direction);
                
                UnityBulletShellCollider.Create((BattleShellBulletComponent)shellComponent);
            }
            entity.AddComponent(shellComponent);
            
            shellDamageCauserHandler.SetThisHandle(entity);
            
            return entity;
        }
        
        public async void CreateShellEffect(GfHandle ownerHandle, GfEntity entity,string effectId)
        {
            var effectOffset = GfFloat3.Zero;

            var ownerEntity = BattleAdmin.EntityComponentSystem.GetEntity(ownerHandle);
            
            var vfxComponent = entity.GetComponent<GfVfxTrackerComponent>();

            var vfxHandle = await BattleAdmin.Factory.Effect.CreateEffect(ownerEntity, effectId,
                EffectGroup.Shell, GfFloat3.Zero, GfQuaternion.Identity,
                GfFloat3.One, GfVfxDeleteMode.Delete, GfVfxPriority.High);
            
            vfxComponent.Attach(vfxHandle, effectOffset, GfFloat3.Zero, GfAxis.All, GfAxis.All);
        }
    }
}
