using Akari.GfCore;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace GameMain.Runtime
{
    public class AudioManager : GfSingleton<AudioManager>
    {
        private EventInstance _bgmEventInstance;

        private float _mainVolume = 1f;
        private float _bgmVolume = 1f;

        private AudioGroup _soundAudioGroup;
        private AudioGroup _voiceAudioGroup;

        public async UniTask Init()
        {
            var bankAssets = await AssetManager.Instance.LoadAllAssetsAsync<TextAsset>("Assets/Example/GameRes/Audio/Desktop/Master.bytes");
            foreach (var bankAsset in bankAssets)
            {
                RuntimeManager.LoadBank(bankAsset);
            }

            _soundAudioGroup = new AudioGroup(10);
            _voiceAudioGroup = new AudioGroup(10);
        }
        
        public void PlayBgm(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            if (_bgmEventInstance.isValid())
            {
                StopEventInstance(_bgmEventInstance);
            }
            
            _bgmEventInstance = RuntimeManager.CreateInstance(name);
            _bgmEventInstance.setVolume(_bgmVolume * _mainVolume);
            _bgmEventInstance.start();
        }

        public void PauseBgm(bool pause)
        {
            if (_bgmEventInstance.isValid())
            {
                _bgmEventInstance.setPaused(pause);
            }
        }

        public void StopBgm()
        {
            if (_bgmEventInstance.isValid())
            {
                _bgmEventInstance.stop(STOP_MODE.IMMEDIATE);
                _bgmEventInstance.release();
            }
        }

        public void SetMainVolume(float value)
        {
            _mainVolume = value;

            //Update
            SetBgmVolume(_bgmVolume);
            SetSoundVolume(_soundAudioGroup.Volume);
            SetVoiceVolume(_voiceAudioGroup.Volume);
        }

        public void SetBgmVolume(float value)
        {
            _bgmVolume = value;
            if (_bgmEventInstance.isValid())
            {
                _bgmEventInstance.setVolume(value);
            }
        }

        public void PlaySound(string name, bool is3D = false, Vector3 pos3D = default)
        {
            _soundAudioGroup.Play(name, is3D, pos3D);
        }

        public void SetSoundVolume(float value)
        {
            _soundAudioGroup.SetVolume(value * _mainVolume);
        }
        
        public void PlayVoice(string name,bool is3D = false,Vector3 pos3D = default)
        {
            _voiceAudioGroup.Play(name, is3D, pos3D);
        }

        public void SetVoiceVolume(float value)
        {
            _voiceAudioGroup.SetVolume(value * _mainVolume);
        }
        
        public void StopVoice()
        {

        }

        private void StopEventInstance(EventInstance eventInstance)
        {
            eventInstance.stop(STOP_MODE.IMMEDIATE);
            eventInstance.release();
            eventInstance.clearHandle();
        }

        public void SetListenerLocation(GameObject gameObject)
        {
            RuntimeManager.SetListenerLocation(gameObject);
        }
    }
}