using System.Collections.Generic;
using System.Linq;
using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using Cysharp.Threading.Tasks;
using GfAnimation;
using Ryu.InGame.Unity;
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
            
            return entity;
        }

        public async UniTask<GfEntity> CreateEnemyCharacter(GameCharacterModel gameCharacterModel, GfFloat3 position, GfQuaternion rotation, string enemyKey)
        {
            var entity = await CreateCharacterInternal(gameCharacterModel, BattleCharacterType.Enemy,position,rotation);

            var actionComponent = entity.GetComponent<BattleObjectActionComponent>();
            actionComponent.Add(BattleCharacterPlayAnimationStateAction.ActionType, new BattleCharacterPlayAnimationStateAction());
            
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

            var entity = BattleAdmin.EntityComponentSystem.Create(0, GfEntityGroupId.Character, gameCharacterModel.Id.ToString());
            var entityTransform = entityUnityView.transform.CreateGfUnityTransform(isPooled);
            entityUnityView.Init(battleCharacterType, entity);
            
            entity.AddComponent(new GfActorComponent(entityTransform));
            var attackComponent = entity.AddComponent(new BattleCharacterAttackComponent());
            var colliderComponent = entity.AddComponent(new GfColliderComponent2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>());
            
            var animationComponent = entity.AddComponent(new GfAnimationComponent());

            entity.AddComponent(new BattleCharacterConditionComponent(battleCharacterType, gameCharacterModel));
            
            var actionComponent = entity.AddComponent(new BattleObjectActionComponent(new BattleObjectActionContext(entity)));

            var transformComponent = entity.AddComponent(new BattleCharacterTransformComponent(entityTransform));
            transformComponent.SetTransform(position,rotation);
            
            var passiveSkillComponent = entity.AddComponent(new BattleCharacterPassiveSkillComponent());
            entity.AddComponent(new BattleCharacterBufferComponent());
            
            entity.AddComponent(new BattleDamageWarningComponent());

            entity.AddComponent(new GfVfxSpeedAffectComponent(BattleAdmin.VfxManager));
            
            var battleCharacterAccessorComponent = entity.AddComponent(new BattleCharacterAccessorComponent(entity));

            var viewComponent = entity.AddComponent(new BattleCharacterViewComponent(entityUnityView));
            
            entity.AddComponent(new GfBoneComponent(entityUnityView.Bone.Bones, entityUnityView.Bone.BoneIndexMap));
            
            //Subsidiary
            if (entityUnityView.SubsidiaryAnimationTrackViews != null && entityUnityView.SubsidiaryAnimationTrackViews.Count > 0)
            {
                for (int i = 0; i < entityUnityView.SubsidiaryAnimationTrackViews.Count; i++)
                {
                    var subsidiaryEntity = CreateCharacterSubsidiary(entityUnityView.SubsidiaryAnimationTrackViews[i]);
                    viewComponent.AddSubsidiary(subsidiaryEntity);
                }
            }
            
            attackComponent.Initialize(new BattleCharacterDamageCauserHandler(battleCharacterAccessorComponent));
            
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
            
            battleCharacterAccessorComponent.SetDamageReceiver(receiver);
            battleCharacterAccessorComponent.SetDamageNotificator(damageNotificator);
            
            animationComponent.AddTrack(new GfAnimationEventTrack());
            animationComponent.AddTrack(new GfMultilayerAnimationTrack(entityUnityView.Animation,
                entityUnityView.Root));
            
#if UNITY_EDITOR
            entityUnityView.gameObject.GetOrAddComponent<CharacterAnimationContainerView>().SetEntity(entity);
#endif
            
            actionComponent.Add(BattleCharacterIdleAction.ActionType, new BattleCharacterIdleAction());
            actionComponent.Add(BattleCharacterMoveWalkAction.ActionType, new BattleCharacterMoveWalkAction());
            actionComponent.Add(BattleCharacterMoveRunAction.ActionType, new BattleCharacterMoveRunAction());
            actionComponent.Add(BattleCharacterMoveSprintAction.ActionType, new BattleCharacterMoveSprintAction());
            
            actionComponent.Add(BattleCharacterGetHitAction.ActionType, new BattleCharacterGetHitAction());
            actionComponent.Add(BattleCharacterKnockBackAction.ActionType, new BattleCharacterKnockBackAction());
            actionComponent.Add(BattleCharacterKnockUpAction.ActionType, new BattleCharacterKnockUpAction());
            
            actionComponent.Add(BattleCharacterDieAction.ActionType, new BattleCharacterDieAction());
            
            BuildAnimationContainer(entity, characterAssets);
            if (characterAssets.AttackDefinitions != null)
            {
                entity.GetComponent<BattleCharacterAttackComponent>().DamageCauserHandler.AttackDefinitions = characterAssets.AttackDefinitions;
            }
            if (characterAssets.AIResource != null) 
            {
                entity.AddComponent(new BattleCharacterDirectorComponent(characterAssets.AIResource.CreateData()));
            }
            
            passiveSkillComponent.AddPassiveSkills(gameCharacterModel.PassiveSkillIds);
            
            actionComponent.ForceTransition(new GfFsmStateTransitionRequest(BattleCharacterIdleAction.ActionType, new BattleCharacterIdleActionData()));
            
            return entity;
        }

        private GfEntity CreateCharacterSubsidiary(GfSimpleAnimationTrackView simpleAnimationView)
        {
            var entity = BattleAdmin.EntityComponentSystem.Create(0, GfEntityGroupId.Character, "Subsidiary");

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
            

            // animationEventTrack 处理
            var animationEventTrack = animationComponent.GetTrack<GfAnimationEventTrack>();
            animationEventTrack.PrepareMemory(stateNum);
            for (int i = 0; i < stateNum; i++)
            {
                var characterStateInfo = stateInfos[i];
                if (string.IsNullOrEmpty(characterStateInfo.AnimationEventPath))
                {
                    continue;
                }

                battleCharacterAssets.AnimationEventResources.TryGetValue(characterStateInfo.AnimationEventPath, out var animationEventResource);
                if (animationEventResource != null)
                {
                    var animationEventData = animationEventResource.CreateData();
                    animationEventTrack.Set(i, animationEventData, characterStateInfo.AnimationEventPath);
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
    }
}