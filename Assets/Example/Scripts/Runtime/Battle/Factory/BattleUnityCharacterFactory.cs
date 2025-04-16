using System;
using System.Collections.Generic;
using System.Linq;
using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using Cysharp.Threading.Tasks;
using GfAnimation;
using UnityEngine;

namespace GameMain.Runtime
{
    public class BattleUnityCharacterFactory : IBattleCharacterFactory
    {
        public void Dispose()
        {
        }
        
        public async UniTask<GfEntity> CreateUserCharacter(GameCharacterModel gameCharacterModel,GfFloat3 position, GfQuaternion rotation)
        {
            var entity = await CreateCharacterInternal(gameCharacterModel, BattleCharacterType.Player, position, rotation);

            var actionComponent = entity.GetComponent<BattleObjectActionComponent>();
            actionComponent.Add(BattleCharacterDashAction.ActionType, new BattleCharacterDashAction());
            actionComponent.Add(BattleCharacterLightAttackAction.ActionType, new BattleCharacterLightAttackAction());
            actionComponent.Add(BattleCharacterChargeAttackAction.ActionType, new BattleCharacterChargeAttackAction());
            actionComponent.Add(BattleCharacterSpecialAttackAction.ActionType, new BattleCharacterSpecialAttackAction());
            actionComponent.Add(BattleCharacterUltimateAction.ActionType, new BattleCharacterUltimateAction());
            actionComponent.Add(BattleCharacterJumpToAction.ActionType, new BattleCharacterJumpToAction());
            return entity;
        }

        public async UniTask<GfEntity> CreateEnemyCharacter(GameCharacterModel gameCharacterModel, GfFloat3 position, GfQuaternion rotation, string enemyKey)
        {
            var entity = await CreateCharacterInternal(gameCharacterModel, BattleCharacterType.Enemy,position,rotation);
            
            var actionComponent = entity.GetComponent<BattleObjectActionComponent>();
            actionComponent.Add(BattleCharacterMoveMixedTreeAction.ActionType, new BattleCharacterMoveMixedTreeAction());
            return entity;
        }
        
        public async UniTask<GfEntity> CreateSummonerCharacter(GameCharacterModel gameCharacterModel,GfFloat3 position, GfQuaternion rotation,string summonerKey)
        {
            var entity = await CreateCharacterInternal(gameCharacterModel, BattleCharacterType.Summoner,position,rotation);
            return entity;
        }
        
        public async UniTask<GfEntity> CreateNpcCharacter(GameCharacterModel gameCharacterModel,GfFloat3 position, GfQuaternion rotation,string npcKey)
        {
            //var entity = await CreateCharacterInternal(gameCharacterModel, BattleCharacterType.Npc,position,rotation);

            BattleCharacterType battleCharacterType = BattleCharacterType.Npc;
            
            var characterAssets = await BattleCharacterAssets.Create(gameCharacterModel);
            var path = AssetPathHelper.GetCharacterPath(gameCharacterModel.CharacterAssetName);

            BattleCharacterUnityView entityUnityView = await AssetManager.Instance.Instantiate<BattleCharacterUnityView>(path);

            var entity = BattleAdmin.EntityComponentSystem.Create(GetGfEntityTagId(battleCharacterType), GfEntityGroupId.Character, gameCharacterModel.Id.ToString());
            entityUnityView.Init(battleCharacterType, entity);
            GfTransform entityTransform = new GfKinematicTransform(entityUnityView.transform);

            //基础组件 Actor
            entity.AddComponent(new GfActorComponent(entityTransform));

            //基础组件 Transform
            var transformComponent = entity.AddComponent(new BattleCharacterTransformComponent());
            transformComponent.SetTransform(position, rotation);
            
            //动画组件 Animation 依赖于Action
            var animationComponent = entity.AddComponent(new GfAnimationComponent());
            animationComponent.HasRootMotionMoveAnimation = entityUnityView.Animation.HasRootMotionMoveAnimation;
            
            //行为组件Action 用来驱动Animation
            var actionComponent = entity.AddComponent(new BattleObjectActionComponent(new BattleObjectActionContext(entity)));
            
            //状态组件 Condition 记录角色数据
            entity.AddComponent(new BattleCharacterConditionComponent(battleCharacterType, gameCharacterModel));
            
            //访问器组件Accessor 组件引用的集合
            var battleCharacterAccessorComponent = entity.AddComponent(new BattleCharacterAccessorComponent(entity));

            //角色表现组件 CharacterView 
            var viewComponent = entity.AddComponent(new BattleCharacterViewComponent(entityUnityView));
            
            //初始化动画组件
            animationComponent.AddTrack(new GfAnimationEventTrack());
            animationComponent.AddTrack(new GfRootMotionTrack());
            animationComponent.AddTrack(new GfMultilayerAnimationTrack(entityUnityView.Animation, entityUnityView.Root));
            
#if UNITY_EDITOR
            entityUnityView.gameObject.GetOrAddComponent<CharacterAnimationContainerView>().SetEntity(entity);
#endif
            
            //为Action组件添加Action
            actionComponent.Add(BattleCharacterIdleAction.ActionType, new BattleCharacterIdleAction());
            
            actionComponent.Add(BattleCharacterMoveWalkAction.ActionType, new BattleCharacterMoveWalkAction());
            actionComponent.Add(BattleCharacterMoveRunAction.ActionType, new BattleCharacterMoveRunAction());
            actionComponent.Add(BattleCharacterMoveSprintAction.ActionType, new BattleCharacterMoveSprintAction());

            //Build AnimationContainer 传入动画数据
            BuildAnimationContainer(entity, characterAssets);
            
            //设置攻击定义
            if (characterAssets.AttackDefinitions != null)
            {
                entity.GetComponent<BattleCharacterAttackComponent>().DamageCauserHandler.AttackDefinitions = characterAssets.AttackDefinitions;
            }
            
            //设置AI
            if (characterAssets.AIResource != null) 
            {
                //添加AI组件 Director
                entity.AddComponent(new BattleCharacterDirectorComponent(characterAssets.AIResource.CreateData()));
                actionComponent.Add(BattleCharacterPlayAnimationStateAction.ActionType, new BattleCharacterPlayAnimationStateAction());
                //添加寻路组件NavMesh
                entity.AddComponent(new BattleCharacterNavMeshComponent());
            }
            
            //执行初始Action
            actionComponent.ForceTransition(new GfFsmStateTransitionRequest(BattleCharacterIdleAction.ActionType, new BattleCharacterIdleActionData()));
            
            return entity;
        }

        private async UniTask<GfEntity> CreateCharacterInternal(GameCharacterModel gameCharacterModel,BattleCharacterType battleCharacterType,GfFloat3 position, GfQuaternion rotation)
        {
            var characterAssets = await BattleCharacterAssets.Create(gameCharacterModel);
            
            var path = AssetPathHelper.GetCharacterPath(gameCharacterModel.CharacterAssetName);

            bool isPooled = battleCharacterType == BattleCharacterType.Enemy;
            BattleCharacterUnityView entityUnityView = isPooled
                ? await AssetManager.Instance.InstantiateFormPool<BattleCharacterUnityView>(path)
                : await AssetManager.Instance.Instantiate<BattleCharacterUnityView>(path);
            entityUnityView.transform.position = position.ToVector3();
            entityUnityView.transform.rotation = rotation.ToQuaternion();
            
            
            var entity = BattleAdmin.EntityComponentSystem.Create(GetGfEntityTagId(battleCharacterType), GfEntityGroupId.Character, gameCharacterModel.Id.ToString());
            entityUnityView.Init(battleCharacterType, entity);
            GfTransform entityTransform = new GfKinematicTransform(entityUnityView.transform, isPooled);

            //基础组件 Actor
            entity.AddComponent(new GfActorComponent(entityTransform));

            //基础组件 Transform
            var transformComponent = entity.AddComponent(new BattleCharacterTransformComponent());
            transformComponent.SetTransform(position, rotation);
            
            //攻击组件 Attack
            var attackComponent = entity.AddComponent(new BattleCharacterAttackComponent());
            //受击碰撞组件 Collider
            var colliderComponent = entity.AddComponent(new GfColliderComponent2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>());

            //动画组件 Animation 依赖于Action
            var animationComponent = entity.AddComponent(new GfAnimationComponent());
            animationComponent.HasRootMotionMoveAnimation = entityUnityView.Animation.HasRootMotionMoveAnimation;
            
            //行为组件Action 用来驱动Animation
            var actionComponent = entity.AddComponent(new BattleObjectActionComponent(new BattleObjectActionContext(entity)));
            
            //状态组件 Condition 记录角色数据
            entity.AddComponent(new BattleCharacterConditionComponent(battleCharacterType, gameCharacterModel));

            //被动技能组件 PassiveSkill
            var passiveSkillComponent = entity.AddComponent(new BattleCharacterPassiveSkillComponent());
            //Buff组件 Buffer
            entity.AddComponent(new BattleCharacterBufferComponent());
            
            //访问器组件Accessor 组件引用的集合
            var battleCharacterAccessorComponent = entity.AddComponent(new BattleCharacterAccessorComponent(entity));

            //角色表现组件 CharacterView 
            var viewComponent = entity.AddComponent(new BattleCharacterViewComponent(entityUnityView));
            
            //角色表现组件 伤害预警 DamageWarning
            entity.AddComponent(new BattleDamageRangeComponent());
            //角色表现组件 特效速度控制 VfxSpeedAffect
            entity.AddComponent(new GfVfxSpeedAffectComponent(BattleAdmin.VfxManager));
            //角色表现组件 骨骼组件 应用于特效Attach，也就是跟随相关逻辑
            entity.AddComponent(new GfBoneComponent(entityUnityView.Bone.Bones, entityUnityView.Bone.BoneIndexMap));
            
            //创建角色附属对象
            if (entityUnityView.SubsidiaryAnimationTrackViews != null && entityUnityView.SubsidiaryAnimationTrackViews.Count > 0)
            {
                for (int i = 0; i < entityUnityView.SubsidiaryAnimationTrackViews.Count; i++)
                {
                    var subsidiaryEntity = CreateCharacterSubsidiary(entityUnityView.SubsidiaryAnimationTrackViews[i]);
                    viewComponent.AddSubsidiary(subsidiaryEntity);
                }
            }
            
            //初始化Attack组件
            var damageCauserHandler = new BattleCharacterDamageCauserHandler(battleCharacterAccessorComponent);
            attackComponent.Initialize(damageCauserHandler);
                
            if (battleCharacterType == BattleCharacterType.Enemy)
            {
                //攻击预警组件 Warning
                var warningComponent = entity.AddComponent(new BattleCharacterWarningComponent());
                warningComponent.SetDamageCauserHandler(damageCauserHandler);
            }
            
            //初始化Collider组件
            colliderComponent.Add(BattleColliderGroupName.Defend,
                new GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(
                    new[] { RyColliderHelper.CreateColliderForDefenderSize() }, GfAxis.None, GfAxis.Z));
            var receiver = new BattleCharacterDamageReceiverHandler(battleCharacterAccessorComponent);
            var defendColliderId = BattleAdmin.DefendColliderIdManager.Issue(entity.ThisHandle);
            var damageNotificator = new BattleCharacterDamageNotificator();
            damageNotificator.Initialize(battleCharacterAccessorComponent,  new BattleCharacterDamageReporter(entity));
            var defendParameter = new BattleColliderDefendParameter(entity.ThisHandle, entity.ThisHandle, defendColliderId, receiver, damageNotificator);
            colliderComponent.SetParameter(BattleColliderGroupName.Defend, defendParameter);
            colliderComponent.SetEnable(BattleColliderGroupName.Defend, true);
            //设置DamageReceiver与DamageNotificator
            battleCharacterAccessorComponent.SetDamageReceiver(receiver);
            battleCharacterAccessorComponent.SetDamageNotificator(damageNotificator);
            
            //初始化动画组件
            animationComponent.AddTrack(new GfAnimationEventTrack());
            animationComponent.AddTrack(new GfRootMotionTrack());
            animationComponent.AddTrack(new GfMultilayerAnimationTrack(entityUnityView.Animation, entityUnityView.Root));
            
#if UNITY_EDITOR
            entityUnityView.gameObject.GetOrAddComponent<CharacterAnimationContainerView>().SetEntity(entity);
#endif
            
            //为Action组件添加Action
            actionComponent.Add(BattleCharacterIdleAction.ActionType, new BattleCharacterIdleAction());
            
            actionComponent.Add(BattleCharacterMoveWalkAction.ActionType, new BattleCharacterMoveWalkAction());
            actionComponent.Add(BattleCharacterMoveRunAction.ActionType, new BattleCharacterMoveRunAction());
            actionComponent.Add(BattleCharacterMoveSprintAction.ActionType, new BattleCharacterMoveSprintAction());
            
            actionComponent.Add(BattleCharacterGetHitAction.ActionType, new BattleCharacterGetHitAction());
            actionComponent.Add(BattleCharacterKnockBackAction.ActionType, new BattleCharacterKnockBackAction());
            actionComponent.Add(BattleCharacterKnockUpAction.ActionType, new BattleCharacterKnockUpAction());
            
            actionComponent.Add(BattleCharacterDieAction.ActionType, new BattleCharacterDieAction());
            
            //Build AnimationContainer 传入动画数据
            BuildAnimationContainer(entity, characterAssets);
            
            //设置攻击定义
            if (characterAssets.AttackDefinitions != null)
            {
                entity.GetComponent<BattleCharacterAttackComponent>().DamageCauserHandler.AttackDefinitions = characterAssets.AttackDefinitions;
            }
            
            //设置AI
            if (characterAssets.AIResource != null) 
            {
                //添加AI组件 Director
                entity.AddComponent(new BattleCharacterDirectorComponent(characterAssets.AIResource.CreateData()));
                actionComponent.Add(BattleCharacterPlayAnimationStateAction.ActionType, new BattleCharacterPlayAnimationStateAction());
                //添加寻路组件NavMesh
                entity.AddComponent(new BattleCharacterNavMeshComponent());
            }
            
            //添加被动技能
            passiveSkillComponent.AddPassiveSkills(gameCharacterModel.PassiveSkillIds);
            
            //执行初始Action
            actionComponent.ForceTransition(new GfFsmStateTransitionRequest(BattleCharacterIdleAction.ActionType, new BattleCharacterIdleActionData()));
            return entity;
        }

        private GfEntity CreateCharacterSubsidiary(GfSimpleAnimationTrackView simpleAnimationView)
        {
            var entity = BattleAdmin.EntityComponentSystem.Create(GfEntityTagId.None, GfEntityGroupId.Character, "Subsidiary");

            entity.AddComponent(new GfActorComponent(simpleAnimationView.transform.CreateGfUnityTransform()));
            
            var animationComponent = entity.AddComponent(new GfAnimationComponent());
            animationComponent.AddTrack(new GfSimpleAnimationTrack(simpleAnimationView));
            
            var states = simpleAnimationView.SimpleAnimation.GetAllEditorState();
            for (int i = 0; i < states.Length; i++)
            {
                var state = states[i];
                var index = i;
                animationComponent.AddClip(state.name, index, state.clip.length, state.clip.frameRate, state.defaultState, default);
            }
            
            entity.AddComponent(new SubsidiaryComponent());
            
            return entity;
        }

        private void BuildAnimationContainer(GfEntity entity, BattleCharacterAssets battleCharacterAssets)
        {
            var containerData = battleCharacterAssets.AnimationContainerData;
            var layerInfos = containerData.LayerInfos;
            var stateInfos = containerData.StateInfos;
            var atoBInfos = containerData.AtoBInfos;
            var animationComponent = entity.GetComponent<GfAnimationComponent>();

            var stateNum = stateInfos.Length;

            // clipMap
            for (int i = 0; i < stateNum; i++)
            {
                var characterStateInfo = stateInfos[i];
                animationComponent.AddClip(characterStateInfo.StateName, i, characterStateInfo.Length,
                    characterStateInfo.FrameRate, characterStateInfo.IsRepeat, characterStateInfo.AnimationEventPath);
            }
            

            // animationEventTrack 和 rootMotionTrack处理
            var animationEventTrack = animationComponent.GetTrack<GfAnimationEventTrack>();
            var rootMotionTrack = animationComponent.GetTrack<GfRootMotionTrack>();
            animationEventTrack.PrepareMemory(stateNum);
            rootMotionTrack.PrepareMemory(stateNum);
            for (int i = 0; i < stateNum; i++)
            {
                var characterStateInfo = stateInfos[i];
                if (!string.IsNullOrEmpty(characterStateInfo.AnimationEventPath))
                {
                    battleCharacterAssets.AnimationEventResources.TryGetValue(characterStateInfo.AnimationEventPath, out var animationEventResource);
                    if (animationEventResource != null)
                    {
                        var animationEventData = animationEventResource.CreateData();
                        animationEventTrack.Set(i, animationEventData, characterStateInfo.AnimationEventPath);
                    }
                }
                
                if (characterStateInfo.RootMotionPath != null)
                {
                    battleCharacterAssets.RootMotionResources.TryGetValue(characterStateInfo.RootMotionPath, out var rootMotionResource);
                    if (rootMotionResource != null)
                    {
                        var rootMotionData = rootMotionResource.CreateData();
                        rootMotionTrack.Set(i, rootMotionData);
                    }
                }
            }

            animationEventTrack.AddListener(new CharacterEffectAnimationEventListener(entity));

            animationEventTrack.AddListener(new CharacterDamageAnimationEventListener(entity));

            animationEventTrack.AddListener(new CharacterActionCancelAnimationEventListener(entity));

            animationEventTrack.AddListener(new CharacterSoundAnimationAnimationEventListener(entity));

            animationEventTrack.AddListener(new CreateShellAnimationEventListener(entity));
            
            animationEventTrack.AddListener(new CharacterTriggerPassiveSkillAnimationEventListener(entity));


            // multilayerAnimationTrack 处理
            var multilayerAnimationTrack = animationComponent.GetTrack<GfMultilayerAnimationTrack>();
            if (multilayerAnimationTrack == null)
            {
                return;
            }
            
            
            var maxLayerNum = layerInfos.Length;
            multilayerAnimationTrack.Initialize(maxLayerNum, stateNum, 0);
            animationComponent.InitializeBaseLayerWeight(maxLayerNum);

            for (int i = 0; i < layerInfos.Length; i++)
            {
                var layerInfo = layerInfos[i];
                
                var layerMaxStateNum = 0;
                for (int j = 0; j < stateNum; j++)
                {
                    if (stateInfos[j].LayerNo == layerInfo.LayerNo)
                    {
                        layerMaxStateNum++;
                    }
                }

                if (layerMaxStateNum <= 0)
                {
                    continue;
                }
                
                battleCharacterAssets.AvatarMasks.TryGetValue(layerInfo.AvatarMaskPath, out var avatarMask);
                multilayerAnimationTrack.AddLayer(layerInfo.LayerNo, layerMaxStateNum, layerInfo.IsAdditive, layerInfo.StartState, avatarMask);
                multilayerAnimationTrack.SetLayerWeight(layerInfo.LayerNo, layerInfo.LayerWeight);
                animationComponent.SetBaseLayerWeight(layerInfo.LayerNo, layerInfo.LayerWeight);   
            }

            for (int i = 0; i < stateNum; i++)
            {
                var characterStateInfo = stateInfos[i];
                
                List<AnimationClip> animationClips = new List<AnimationClip>();
                for (int j = 0; j < characterStateInfo.ClipPaths.Length; j++)
                {
                    battleCharacterAssets.AnimationClips.TryGetValue(characterStateInfo.ClipPaths[j], out var animationClip);
                    if (animationClip != null)
                    {
                        animationClips.Add(animationClip);
                    }
                }

                if (animationClips.Count == 1)
                {
                    multilayerAnimationTrack.AddState(characterStateInfo.LayerNo, characterStateInfo.StateName, animationClips.FirstOrDefault(), false); 
                }
                else if(animationClips.Count >= 2)
                {
                    if (characterStateInfo.IsSyncBlend)
                    {
                        multilayerAnimationTrack.AddSyncBlendState(characterStateInfo.LayerNo, characterStateInfo.StateName, animationClips.ToArray(),characterStateInfo.ParameterName, false);
                    }
                    else
                    {
                        multilayerAnimationTrack.AddBlendState(characterStateInfo.LayerNo, characterStateInfo.StateName, animationClips.ToArray(),characterStateInfo.ParameterName, false);
                    }
                }
                else
                {
                    //无动画轨道
                }
            }

            for (int i = 0; i < atoBInfos.Length; i++)
            {
                var atoBInfo = atoBInfos[i];
                multilayerAnimationTrack.AddParamAtoB(atoBInfo.NameA, atoBInfo.NameB, (MultilayerAnimationFadeType)atoBInfo.FadeType,
                    atoBInfo.FadeTime);
            }
            
            animationComponent.Play("Idle");

            //multilayerAnimationTrack.SetParameter("testParameter", 1);
        }

        private int GetGfEntityTagId(BattleCharacterType characterType)
        {
            switch (characterType)
            {
                case BattleCharacterType.Player:
                case BattleCharacterType.Summoner:
                    return GfEntityTagId.TeamA;
                case BattleCharacterType.Enemy:
                    return GfEntityTagId.TeamB;
                default:
                    return GfEntityTagId.None;
            }
        }
    }
}