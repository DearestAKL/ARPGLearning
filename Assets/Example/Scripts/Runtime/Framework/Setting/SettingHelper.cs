using UnityEngine;

namespace GameMain.Runtime
{
    public static class SettingHelper
    {
        public static void SetScreenMode(ScreenModeType screenModeType)
        {
            switch (screenModeType)
            {
                case ScreenModeType.FullScreen:
                    Screen.fullScreen = true;
                    break;
                case ScreenModeType.Windowed:
                    Screen.fullScreen = false;
                    break;
            }
        }

        public static void SetResolution(ResolutionType resolutionType)
        {
            //R1920x1080
            var resolution = resolutionType.ToString();
            // 去掉字母 'R' 并以 'x' 为分隔符拆分
            string[] dimensions = resolution.Substring(1).Split('x');
            // 提取宽度和高度，并转换为整数
            int width = int.Parse(dimensions[0]);
            int height = int.Parse(dimensions[1]);
            
            Screen.SetResolution(width, height, Screen.fullScreen);
        }
    }
}