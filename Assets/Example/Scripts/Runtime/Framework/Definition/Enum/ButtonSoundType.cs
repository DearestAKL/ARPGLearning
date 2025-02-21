using System.Collections.Generic;

namespace GameMain.Runtime
{
    public enum ButtonSoundType
    {
        None,
        Click,
        Return,
        Select,
        Switch,
    }

    public static class ButtonSoundTypeEx
    {
        private static readonly Dictionary<int, string> SoundTypeMap = new Dictionary<int, string>()
        {
            {(int)ButtonSoundType.Click,Constant.Sound.ButtonSoundClick},
            {(int)ButtonSoundType.Return,Constant.Sound.ButtonSoundReturn},
            {(int)ButtonSoundType.Select,Constant.Sound.ButtonSoundSelect},
            {(int)ButtonSoundType.Switch,Constant.Sound.ButtonSoundSwitch}
        };

        public static string GetSoundAssetName(this ButtonSoundType soundType)
        {
            SoundTypeMap.TryGetValue((int)soundType, out var assetName);
            return assetName;
        }
    }
}