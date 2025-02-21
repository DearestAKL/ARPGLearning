namespace GameMain.Runtime
{
    public static class UIIconPathHelper
    {
        private static readonly string UISpritePath = "Assets/Example/GameRes/UI/Sprites/Runtime/";
        
        //private static readonly string AvatarPath = "Avatar/";
        
        private static readonly string CommonPath = "Common/";
        private static readonly string EquipPath = "Equip/";
        private static readonly string RelicPath = "Relic/";
        private static readonly string EquipInfoPath = "EquipInfo/";
        private static readonly string ItemPath = "Item/";

        // public static string GetAvatarIconPath(string name)
        // {
        //     return $"{UISpritePath}{AvatarPath}{name}.png";
        // }

        public static string GetQualityColorBg(int quality)
        {
            string name;
            switch (quality)
            {
                case 1:
                    name = "WHITE";
                    break;
                case 2:
                    name = "GREEN";
                    break;
                case 3:
                    name = "BLUE";
                    break;
                case 4:
                    name = "PURPLE";
                    break;
                case 5:
                    name = "ORANGE";
                    break;
                default: 
                    name = "NONE";
                    break;
            }
            
            return GetCommonIconPath($"QUALITY_{name}");
        }
        
        public static string GetQualityBg(int quality)
        {
            return GetCommonIconPath($"QualityBg_{quality}");
        }
        
        public static string GetQualityBgSmall(int quality)
        {
            return GetCommonIconPath($"QualityBg_{quality}s");
        }

        public static string GetCommonIconPath(string name)
        {
            return $"{UISpritePath}{CommonPath}UI_{name}.png";
        }
        
        public static string GetWeaponPath(string name)
        {
            return $"{UISpritePath}{EquipPath}UI_EquipIcon_{name}.png";
        }

        public static string GetArmorPath(string name)
        {
            return $"{UISpritePath}{RelicPath}UI_RelicIcon_{name}.png";
        }
        
        public static string GetEquipInfoPath(string name)
        {
            return $"{UISpritePath}{EquipInfoPath}UI_EquipIcon_{name} #.png";
        }
        
        public static string GetItemPath(string name)
        {
            return $"{UISpritePath}{ItemPath}UI_ItemIcon_{name}.png";
        }
    }
}