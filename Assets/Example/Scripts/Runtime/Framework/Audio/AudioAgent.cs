using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public class AudioAgent
    {
        private readonly AudioGroup _audioGroup;
        private string _name;
        private const int Capacity = 2;
        private List<EventInstance> _eventInstances = new List<EventInstance>();

        public string Name => _name;

        public AudioAgent(AudioGroup audioGroup,string name)
        {
            _audioGroup = audioGroup;
            _name = name;
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public void Play(bool is3D,Vector3 pos3D)
        {
            EventInstance candidateEvent = default;

            for (int i = 0; i < _eventInstances.Count; i++)
            {
                var eventInstance = _eventInstances[i];
                eventInstance.getPlaybackState(out var state);
                if (state == PLAYBACK_STATE.STOPPED)
                {
                    candidateEvent = eventInstance;
                    break;
                }
            }

            if (!candidateEvent.isValid())
            {
                if (_eventInstances.Count > Capacity)
                {
                    // 停止当前正在播放的音效
                    _eventInstances[0].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    candidateEvent = _eventInstances[0];
                }
                else
                {
                    var newEventInstance = RuntimeManager.CreateInstance(_name);
                    newEventInstance.setVolume(_audioGroup.Volume);
                    _eventInstances.Add(newEventInstance);
                    candidateEvent = newEventInstance;
                }
            }

            if (is3D)
            {
                candidateEvent.set3DAttributes(pos3D.To3DAttributes());
            }

            candidateEvent.start();
        }

        public void ResetVolume()
        {
            foreach (var item in _eventInstances)
            {
                item.setVolume(_audioGroup.Volume);
            }
        }

        public bool IsAllStop()
        {
            foreach (var item in _eventInstances)
            {
                item.getPlaybackState(out var state);
                if (state != PLAYBACK_STATE.STOPPED)
                {
                    return false;
                }
            }

            return true;
        }

        public void Release()
        {
            foreach (var item in _eventInstances)
            {
                item.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                item.release();
            }
        }
    }
}