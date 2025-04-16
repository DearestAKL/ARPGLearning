using System.Collections.Generic;
using System.IO;
using Akari.GfGame;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameMain.Runtime
{
    public class BattleCharacterAssets
    {
        public GfAnimationContainerData AnimationContainerData;
        public readonly Dictionary<string,GfAnimationEventResource> AnimationEventResources = new Dictionary<string, GfAnimationEventResource>();
        public readonly Dictionary<string,GfRootMotionResource> RootMotionResources = new Dictionary<string, GfRootMotionResource>();
        public readonly Dictionary<string,AnimationClip> AnimationClips = new Dictionary<string, AnimationClip>();
        public readonly Dictionary<string,AvatarMask> AvatarMasks = new Dictionary<string, AvatarMask>();
        public AttackDefinitionInfoData[] AttackDefinitions;
        public GfBtResource AIResource;


        private static readonly Dictionary<int, BattleCharacterAssets> CharacterAssetsMap = new Dictionary<int, BattleCharacterAssets>();
        
        public static async UniTask<BattleCharacterAssets> Create(GameCharacterModel gameCharacterModel)
        {
            if (CharacterAssetsMap.TryGetValue(gameCharacterModel.Id,out var characterAssets))
            {
                return characterAssets;
            }
            
            characterAssets = new BattleCharacterAssets();
            await characterAssets.PreloadCharacterAssets(gameCharacterModel);
            CharacterAssetsMap.TryAdd(gameCharacterModel.Id, characterAssets);
            return characterAssets;
        }

        private async UniTask PreloadCharacterAssets(GameCharacterModel gameCharacterModel)
        {
            TextAsset animationContainer = await AssetManager.Instance.LoadAsset<TextAsset>(AssetPathHelper.GetAnimationContainerPath(gameCharacterModel.ContainerAssetName));
            var containerStream = new MemoryStream(animationContainer.bytes);
            var containerResource = GfAnimationContainerResourceFactory.Instance.CreateResource(containerStream);
            AnimationContainerData = containerResource.CreateData();

            foreach (var layerInfo in AnimationContainerData.LayerInfos)
            {
                var avatarMaskPath = layerInfo.AvatarMaskPath;
                if (!string.IsNullOrEmpty(avatarMaskPath) && !AvatarMasks.ContainsKey(avatarMaskPath))
                {
                    var avatarMask = await AssetManager.Instance.LoadAsset<AvatarMask>(avatarMaskPath);
                    AvatarMasks.TryAdd(avatarMaskPath, avatarMask);
                }
            }
            
            //AnimationClip,AnimationEvent
            var stateNum = AnimationContainerData.StateInfos.Length;
            for (int i = 0; i < stateNum; i++)
            {
                var characterStateInfo =  AnimationContainerData.StateInfos[i];

                var animationEventPath = characterStateInfo.AnimationEventPath;
                if (!string.IsNullOrEmpty(animationEventPath) && !AnimationEventResources.ContainsKey(animationEventPath))
                {
                    var animationEventAsset = await AssetManager.Instance.LoadAsset<TextAsset>(animationEventPath);
                    var stream = new MemoryStream(animationEventAsset.bytes);
                    var animationEventResource = GfAnimationEventResourceFactory.Instance.CreateResource(stream);
                    AnimationEventResources.TryAdd(animationEventPath,animationEventResource);
                    
                    foreach (var subTrackResource in animationEventResource.SubTrackResources)
                    {
                        if (subTrackResource.Parameter is AnimationEventParameterEffectPlay effectPlay)
                        {
                            //预加载 AnimationEvent中使用的特效
                            await AssetManager.Instance.PreloadAsset<GameObject>(effectPlay.EffectId);
                        }
                        else if(subTrackResource.Parameter is AnimationEventParameterShell shell)
                        {
                            //预加载 AnimationEvent中使用的shell 以及其关联的特效
                            var shellDefinitionMessage = await PbDefinitionHelper.GetShellDefinitionMessage(shell.Id);
                            await AssetManager.Instance.PreloadAsset<GameObject>(shellDefinitionMessage.EffectId);
                        }
                    }
                }

                var rootMotionPath = characterStateInfo.RootMotionPath;
                if (!string.IsNullOrEmpty(rootMotionPath) && !RootMotionResources.ContainsKey(rootMotionPath))
                {
                    var rootMotionAsset = await AssetManager.Instance.LoadAsset<TextAsset>(rootMotionPath);
                    var stream = new MemoryStream(rootMotionAsset.bytes);
                    var rootMotionResource = GfRootMotionResourceFactory.Instance.CreateResource(stream);
                    RootMotionResources.TryAdd(rootMotionPath,rootMotionResource);
                }

                for (int j = 0; j < characterStateInfo.ClipPaths.Length; j++)
                {
                    var clipPath = characterStateInfo.ClipPaths[j];
                    if (!string.IsNullOrEmpty(clipPath) && !AnimationClips.ContainsKey(clipPath))
                    {
                        var animationClip = await AssetManager.Instance.LoadAsset<AnimationClip>(clipPath);
                        AnimationClips.TryAdd(clipPath, animationClip);
                    }
                }
                
            }
            
            //AttackDefinition
            if (gameCharacterModel.AttackDefinitionGroupId != 0)
            {
                var attackDefinitionGroupMessage = await PbDefinitionHelper.GetAttackDefinitionGroupMessage(gameCharacterModel.Id);
                if (attackDefinitionGroupMessage != null)
                {
                    AttackDefinitions = new AttackDefinitionInfoData[attackDefinitionGroupMessage.Infos.Count];
                    for (int i = 0; i < attackDefinitionGroupMessage.Infos.Count; i++)
                    {
                        AttackDefinitions[i] = AttackDefinitionInfoData.CreatData(attackDefinitionGroupMessage,
                            attackDefinitionGroupMessage.Infos[i]);
                    }
                }
            }

            //BehaviourTree AI
            if (!string.IsNullOrEmpty(gameCharacterModel.AiAssetName))
            {
                TextAsset ai = await AssetManager.Instance.LoadAsset<TextAsset>(AssetPathHelper.GetBehaviourTreePath(gameCharacterModel.AiAssetName));
                if (ai != null) 
                {
                    var stream = new MemoryStream(ai.bytes);
                    var gfBtResource = GfBehaviorTreeResourceFactory.Instance.CreateResource(stream);
                    AIResource = gfBtResource;
                }
            }
        }
    }
}