using UnityEngine;

namespace GameMain.Runtime
{
    public static partial class Constant
    {
        public static class Sound
        {
            public const string BgmAlis = "event:/2D/Dima Koltsov — Alis";
            public const string BgmFolia = "event:/2D/Dima Koltsov — Folia";
            public const string BgmFondness = "event:/2D/Dima Koltsov — Fondness";
            
            public const string AmbienceBeach = "event:/2D/ambience-beach";
            public const string AmbienceCity = "event:/2D/ambience-city";
            public const string AmbienceForest = "event:/2D/ambience-forest";
            // public const string AmbienceGarden = "event:/ambience-garden";
            // public const string AmbienceJungle = "event:/ambience-jungle";
            // public const string AmbienceNone = "event:/ambience-none";
            // public const string AmbiencePark = "event:/ambience-park";
            // public const string AmbienceRain = "event:/ambience-rain";
            // public const string AmbienceSnow = "event:/ambience-snow";
            
            public const string ButtonSoundClick = "event:/2D/menu-click";
            public const string ButtonSoundReturn = "event:/2D/menu-return";
            public const string ButtonSoundSelect = "event:/2D/menu-select";
            public const string ButtonSoundSwitch = "event:/2D/menu-switch";
            
            public const string Camera = "event:/2D/camera";
            public const string CameraFocus = "event:/2D/camera-focus";
            public const string Intro = "event:/2Dintro";
            
            public const string HitSound = "event:/3D/intro";
            public const string Dash_1 = "event:/3D/dash_1";
            public const string Dash_2 = "event:/3D/dash_2";

            public static string GetRandomDashSound()
            {
                return (Random.Range(0, 2) > 0) ? Dash_1 : Dash_2;
            }
        }
    }
}