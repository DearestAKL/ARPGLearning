using Akari.GfUnity;
using Cysharp.Threading.Tasks;

namespace GameMain.Runtime
{
    /// <summary>
    /// 统一管理一些通用Manager
    /// </summary>
    public static class ManagerHelper
    {
        public static bool IsInitYooAssets = false;
        
        private static bool _isInitCommonManager = false;

        public static async UniTask InitCommonManager(bool hasLoading = false)
        {
            if (_isInitCommonManager)
            {
                return;
            }

            //初始化UI管理器
            if (UIManager.CreateInstance())
            {
                await UIManager.Instance.Init();
                if (hasLoading)
                {
                    UIManager.Instance.MainCamera.enabled = false;
                    await UIHelper.StartLoading();
                    UIManager.Instance.MainCamera.enabled = true;
                }
            }
            
            //初始化声音管理器
            if (AudioManager.CreateInstance())
            {
                await AudioManager.Instance.Init();
            }

            //初始化设置管理器
            SettingManager.CreateInstance();
            InitSettings();

            //初始化用户数据管理器
            UserDataManager.CreateInstance();
            
            //初始化Luban管理器
            if (LubanManager.CreateInstance())
            {
                await LubanManager.Instance.Init();
            }
            
            GfPrefabPool.Initialize();

            _isInitCommonManager = true;
        }

        private static void InitSettings()
        {
            AudioManager.Instance.SetMainVolume(SettingManager.Instance.GetInt(Constant.Setting.MainVolume, 10) / 10f);
            AudioManager.Instance.SetBgmVolume(SettingManager.Instance.GetInt(Constant.Setting.MusicVolume, 10) / 10f);
            AudioManager.Instance.SetSoundVolume(SettingManager.Instance.GetInt(Constant.Setting.SoundVolume, 10) / 10f);
            AudioManager.Instance.SetVoiceVolume(SettingManager.Instance.GetInt(Constant.Setting.VoiceVolume, 10) / 10f);

            SettingHelper.SetScreenMode((ScreenModeType)SettingManager.Instance.GetInt(Constant.Setting.ScreenMode));
            SettingHelper.SetResolution((ResolutionType)SettingManager.Instance.GetInt(Constant.Setting.Resolution));
        }
    }
}