using UnityEngine;

namespace GameMain.Runtime
{
    public class UISettingSoundView : MonoBehaviour
    {
        [SerializeField] private UISettingSlider sliderMain = null;
        [SerializeField] private UISettingSlider sliderMusic = null;
        [SerializeField] private UISettingSlider sliderSound = null;
        [SerializeField] private UISettingSlider sliderVoice = null;

        private void Awake()
        {
            //设置声音
            var mainVolume = SettingManager.Instance.GetInt(Constant.Setting.MainVolume, 10);
            sliderMain.SetValue(mainVolume);
            sliderMain.OnValueChange.AddListener(OnMainVolumeChange);
                
            var musicVolume = SettingManager.Instance.GetInt(Constant.Setting.MusicVolume, 5);
            sliderMusic.SetValue(musicVolume);
            sliderMusic.OnValueChange.AddListener(OnMusicVolumeChange);
            
            var soundVolume = SettingManager.Instance.GetInt(Constant.Setting.SoundVolume, 5);
            sliderSound.SetValue(soundVolume);
            sliderSound.OnValueChange.AddListener(OnSoundVolumeChange);
            
            var voiceVolume = SettingManager.Instance.GetInt(Constant.Setting.VoiceVolume, 5);
            sliderVoice.SetValue(voiceVolume);
            sliderVoice.OnValueChange.AddListener(OnVoiceVolumeChange);
        }

        private void OnMainVolumeChange(int value)
        {
            SettingManager.Instance.SetInt(Constant.Setting.MainVolume, value);
            AudioManager.Instance.SetMainVolume(value / 10f);
        }

        private void OnMusicVolumeChange(int value)
        {
            SettingManager.Instance.SetInt(Constant.Setting.MusicVolume, value);
            AudioManager.Instance.SetBgmVolume(value / 10f);
        }
        
        private void OnSoundVolumeChange(int value)
        {
            SettingManager.Instance.SetInt(Constant.Setting.SoundVolume, value);
            AudioManager.Instance.SetSoundVolume(value / 10f);
        }
        
        private void OnVoiceVolumeChange(int value)
        {
            SettingManager.Instance.SetInt(Constant.Setting.VoiceVolume, value);
            AudioManager.Instance.SetVoiceVolume(value / 10f);
        }
    }
}