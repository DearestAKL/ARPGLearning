using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public static class BattleHelper
    {
        public static void InitAllGfPbFactory()
        {
            //========================AnimationEvent========================
            GfPbFactory.SetFactory(RyPbTypes.AnimationEventParameterEffectPlay, new AnimationEventParameterEffectPlayFactory());
            GfPbFactory.SetFactory(RyPbTypes.AnimationEventParameterUintId, new AnimationEventParameterUintIdFactory());
            GfPbFactory.SetFactory(RyPbTypes.AnimationEventParameterNull, new AnimationEventParameterNullFactory());
            GfPbFactory.SetFactory(RyPbTypes.AnimationEventParameterString, new AnimationEventParameterStringFactory());
            GfPbFactory.SetFactory(RyPbTypes.AnimationEventParameterShell, new AnimationEventParameterShellFactory());
            GfPbFactory.SetFactory(RyPbTypes.AnimationEventParameterIntValue, new AnimationEventParameterIntValueFactory());
            
            //========================Bt========================
            GfPbFactory.SetFactory(RyPbTypes.BtPropertyInt, new BtIntPropertyFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtPropertyString, new BtStringPropertyFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtPropertyFloat, new BtFloatPropertyFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtPropertyBool, new BtBoolPropertyFactory());
            
            GfPbFactory.SetFactory(RyPbTypes.BtNodeSetPropertyInt, new BtSetIntPropertyNodeFactory());
            //GfPbFactory.SetFactory(RyPbTypes.BtNodeSetPropertyString, new BtSetStringPropertyNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeSetPropertyFloat, new BtSetFloatPropertyNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeSetPropertyBool, new BtSetBoolPropertyNodeFactory());

            GfPbFactory.SetFactory(RyPbTypes.BtNodeStart, new BtStartNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeSelector, new BtSelectorNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeSequencer, new BtSequencerNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeParallel, new BtParallelNodeFactory());
            
            GfPbFactory.SetFactory(RyPbTypes.BtNodeWaitAction, new BtWaitNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeDebugAction, new BtDebugNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeCharacterPlayAnimationStateAction, new BtCharacterPlayAnimationStateNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeCharacterPatrolAction, new BtCharacterPatrolNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeCharacterFollowAction, new BtCharacterFollowNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeCharacterChaseAction, new BtCharacterChaseNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeCharacterConfrontAction, new BtCharacterConfrontNodeFactory());
            
            GfPbFactory.SetFactory(RyPbTypes.BtNodeFloatCondition, new BtFloatConditionNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeCharacterService, new BtServiceNodeFactory());
            GfPbFactory.SetFactory(RyPbTypes.BtNodeBoolCondition, new BtBoolConditionNodeFactory());
        }

        public static void InitAllComponentSystem()
        {
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<GfAnimationComponent>(BattleComponentSystemSortingOrder.GfAnimationComponent));
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfComponentSystem<GfActorComponent>());
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<BattleCharacterAttackComponent>());

            BattleAdmin.EntityComponentSystem.AddComponentSystem(
                new GfColliderComponentSystem2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(
                    new GfDamageMediator2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(
                        new BattleColliderDamageHandler(BattleAdmin.DamageHandler)),
                    BattleComponentSystemSortingOrder.GfColliderComponent));

            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<BattleObjectActionComponent>(BattleComponentSystemSortingOrder.ActionComponent));
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<BattleCharacterConditionComponent>(BattleComponentSystemSortingOrder.ConditionComponent));
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<BattleCharacterAccessorComponent>());
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<BattleCharacterTransformComponent>());
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<BattleCharacterPassiveSkillComponent>());
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<BattleCharacterBufferComponent>());
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<BattleDamageRangeComponent>());

            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<GfBoneComponent>());

            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfVfxComponentSystem(BattleAdmin.VfxManager));
            
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<GfVfxSpeedAffectComponent>());
            
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<BattleCharacterDirectorComponent>());
            
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<GfVfxTrackerComponent>());
            
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<ABattleShellComponent>());
            
            BattleAdmin.EntityComponentSystem.AddComponentSystem(new GfGameComponentSystem<BattleCharacterWarningComponent>());
            
            //UnityComponent
            BattleAdmin.EntityComponentSystem.AddComponentSystem(
                new GfGameComponentSystem<BattleMainCameraAccessorComponent>());
            BattleAdmin.EntityComponentSystem.AddComponentSystem(
                new GfGameComponentSystem<BattleMainCameraActionComponent>());
            BattleAdmin.EntityComponentSystem.AddComponentSystem(
                new GfGameComponentSystem<BattleCharacterViewComponent>());
            BattleAdmin.EntityComponentSystem.AddComponentSystem(
                new GfGameComponentSystem<SubsidiaryComponent>());
            
            BattleAdmin.EntityComponentSystem.AddComponentSystem(
                new GfGameComponentSystem<BattleCharacterNavMeshComponent>());
            
            BattleAdmin.EntityComponentSystem.AddComponentSystem(
                new GfGameComponentSystem<ShellLauncherComponent>());
        }
    }
}