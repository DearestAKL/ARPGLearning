using UnityEngine;

namespace GameMain.Runtime
{
    public static partial class Constant
    {
        public static class ColorDef
        {
            public static Color White = new Color(255F / 255F, 255F / 255F, 255F / 255F);
            
            public static Color Gray = new Color(166F / 255F, 172F / 255F, 185F / 255F);
            
            public static Color UpYellow = new Color(253F / 255F, 204F / 255F, 50F / 255F);

            public static Color QualityWhite = new Color(114F / 255F, 120F / 255F, 139F / 255F);
            public static Color QualityGreen = new Color(43F / 255F, 143 / 255F, 113F / 255F);
            public static Color QualityBlue = new Color(83F / 255F, 126F / 255F, 202F / 255F);
            public static Color QualityPurple = new Color(160F / 255F, 86F / 255F, 224F / 255F);
            public static Color QualityOrange = new Color(189F / 255F, 105F / 255F, 51F / 255F);

            public static Color GetQualityColor(int quality)
            {
                switch (quality)
                {
                    case 1:
                        return QualityWhite;
                    case 2:
                        return QualityGreen;
                    case 3:
                        return QualityBlue;
                    case 4:
                        return QualityPurple;
                    case 5:
                        return QualityOrange;
                    default: 
                        return QualityWhite;
                }
            }
        }
    }
}