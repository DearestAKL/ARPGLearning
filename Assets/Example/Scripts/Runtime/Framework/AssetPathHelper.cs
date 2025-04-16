using System.Collections.Generic;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public static class AssetPathHelper
    {
        private static readonly string ResPath = "Assets/Example/GameRes/";
        
        private static readonly string AudioPath = "Audio/";
        private static readonly string BgmPath = "Bgm/";
        private static readonly string EffectPath = "Effect/";
        
        private static readonly string OtherPath = "Other/";

        private static readonly string UIPanelPath = "UI/Panel/";
        private static readonly string UIItemPath = "UI/Item/";
        private static readonly string UIBattleItemPath = "UI/Item/Battle/";
        private static readonly string UITipsItemPath = "UI/Item/Tips/";
        
        private static readonly string Config = "Config/";
        
        private static readonly string LubanData = "LubanData/";
        
        private static readonly string BattleCharacterPath = "Character/";
        
        private static readonly string PbDefinitionPath = "PbDefinition/";
        private static readonly string PassiveSkillDefinitionPath = "PassiveSkillDefinition/";
        private static readonly string ShellDefinitionPath = "ShellDefinition/";
        private static readonly string BufferDefinitionPath = "BufferDefinition/";
        private static readonly string AttackDefinitionPath = "AttackDefinition/";
        
        private static readonly string AnimationContainerPath = "AnimationContainer/";
        private static readonly string AnimationEventPath = "AnimationEvent/";
        private static readonly string BehaviourTreePath = "BehaviourTree/";
        
        private static readonly string PrefabsPath = "Prefabs/";
        private static readonly string RoomPath = "Room/";
        private static readonly string InteractiveObjectPath = "InteractiveObject/";
        private static readonly string DamageRangePath = "DamageRange/";
        private static readonly string WorldPath = "World/";

        public static string GetSoundPath(string name)
        {
            return $"{ResPath}{AudioPath}{name}.wav";
        }

        public static string GetBgmSoundPath(string name)
        {
            return $"{ResPath}{AudioPath}{BgmPath}{name}.wav";
        }
        
        public static string GetAudioMixerPath(string name)
        {
            return $"{ResPath}{AudioPath}{name}.mixer";
        }
        

        public static string GetUIPanelPath(string name)
        {
            return $"{ResPath}{UIPanelPath}{name}.prefab";
        }
        
        public static string GetUIItemPath(string name)
        {
            return $"{ResPath}{UIItemPath}{name}.prefab";
        }

        public static string GetUIBattleItemPath(string name)
        {
            return $"{ResPath}{UIBattleItemPath}{name}.prefab";
        }
        
        public static string GetUITipsItemPath(string name)
        {
            return $"{ResPath}{UITipsItemPath}{name}.prefab";
        }

        public static string GetLubanDataPath(string name)
        {
            return $"{ResPath}{LubanData}{name}.bytes";
        }

        public static string GetOtherPath(string name)
        {
            return $"{ResPath}{OtherPath}{name}.prefab";
        }
        
        public static string GetCharacterPath(string name)
        {
            return $"{ResPath}{BattleCharacterPath}{name}/{name}.prefab";
        }
        
        public static string GetNetworkCharacterPath(string name)
        {
            return $"{ResPath}{BattleCharacterPath}{name}/Network/{name}.prefab";
        }
        
        public static string GetCharacterFittingPath(string name)
        {
            return $"{ResPath}{BattleCharacterPath}{name}.prefab";
        }

        public static string GetPassiveSkillDefinitionPath(int id)
        {
            return $"{ResPath}{PbDefinitionPath}{PassiveSkillDefinitionPath}PassiveSkillDefinition_{id}.pb";
        }

        public static string GetBufferDefinitionPath(int id)
        {
            return $"{ResPath}{PbDefinitionPath}{BufferDefinitionPath}BufferDefinition_{id}.pb";
        }

        public static string GetShellDefinitionPath(int id)
        {
            return $"{ResPath}{PbDefinitionPath}{ShellDefinitionPath}ShellDefinition_{id}.pb";
        }
        
        public static string GetAttackDefinitionGroupPath(int id)
        {
            return $"{ResPath}{PbDefinitionPath}{AttackDefinitionPath}AttackDefinitionGroup_{id}.pb";
        }

        // public static string ReplaceBattleEffectPath(string path)
        // {
        //     return path.Replace(GetBattleEffectBasePath(), "");
        // }
        //
        // public static string GetBattleEffectPath(string effectPath)
        // {
        //     //如果是全路径则直接返回，不是全路径则补充
        //     var basePath = GetBattleEffectBasePath();
        //     if (effectPath.StartsWith(basePath))
        //     {
        //         return effectPath;
        //     }
        //     return $"{basePath}{effectPath}";
        // }
        //
        // private static string GetBattleEffectBasePath()
        // {
        //     return $"{ResPath}{EffectPath}Battle/";
        // }
        
        public static string GetBattleCommonEffectPath(string name)
        {
            return $"{ResPath}{EffectPath}Battle/Common/{name}.prefab";
        }
        
        public static string GetAnimationContainerPath(string name)
        {
            return $"{ResPath}{AnimationContainerPath}{name}{GfResourceFileNameSuffix.AnimationContainerFileNameSuffix}.bytes";
        }
        
        public static string GetBehaviourTreePath(string name)
        {
            return $"{ResPath}{BehaviourTreePath}{name}{GfResourceFileNameSuffix.BehaviourTreeFileNameSuffix}.bytes";
        }

        public static string GetRoomPath(string name)
        {
            return $"{ResPath}{PrefabsPath}{RoomPath}{name}.prefab";
        }
        
        public static string GetWorldPath(string name)
        {
            return $"{ResPath}{PrefabsPath}{WorldPath}{name}.prefab";
        }

        public static string GetInteractiveObjectPath(string name)
        {
            return $"{ResPath}{PrefabsPath}{InteractiveObjectPath}{name}.prefab";
        }
        
        public static string GetDamageRangePath(string name)
        {
            return $"{ResPath}{PrefabsPath}{DamageRangePath}{name}.prefab";
        }
    }


    public enum CommonEffectType
    {
        Hit,
        Dash,
        Die,
        Flash,
    }

    public static class BattleCommonEffect
    {
        public static readonly Dictionary<int, string> EffectDictionary = new Dictionary<int, string>()
        {
            {(int)CommonEffectType.Hit, "EF_CMN_Hit"},
            //{(int)CommonEffectType.Dash, "EF_CMN_Dash"},
            {(int)CommonEffectType.Die, "EF_CMN_Die"},
            {(int)CommonEffectType.Flash, "EF_CMN_Flash"},
        };

        public static string GetPath(this CommonEffectType effectType)
        {
            if(EffectDictionary.TryGetValue((int)effectType, out var path))
            {
                return AssetPathHelper.GetBattleCommonEffectPath(path);
            }

            return null;
        }
    }
}