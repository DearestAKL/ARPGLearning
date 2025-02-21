using System.Collections.Generic;
using Akari.GfUnityEditor.AnimationEventBundler;
using GameMain.Runtime;
using Google.Protobuf;

namespace GameMain.Editor
{
    public class EditorAnimationEventParameterString : AnimationEventObjectParameterBase
    {
        public string Content;

        public override int GetPbTypeId()
        {
            return RyPbTypes.AnimationEventParameterString;
        }

        public override IMessage Serialize()
        {
            var message = new AnimationEventParameterMessageString();
            message.Content = Content;
            return message;
        }

        private FMOD.Studio.EventInstance _eventInstance;
        private bool test;
        public override void OnEventNotice(AnimationEventTrack track)
        {
            //判断是否是Sound
            if (track.MethodName == "RegisterCommonSoundPlay")
            {
                //没什么用
                PlayFMODEvent();
            }
        }

        private void PlayFMODEvent()
        {
            if (_eventInstance.isValid())
            {
                _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                _eventInstance.release();
            }
            else
            {
                FMODUnity.EditorUtils.LoadPreviewBanks();
            }
                
            FMODUnity.EditorEventRef eventRef = FMODUnity.EventManager.EventFromPath(Content);
            Dictionary<string, float> paramValues = new Dictionary<string, float>();
            // foreach (FMODUnity.EditorParamRef param in eventRef.Parameters)
            // {
            //     paramValues.Add(param.Name, param.Default);
            // }
            _eventInstance = FMODUnity.EditorUtils.PreviewEvent(eventRef, paramValues);
            _eventInstance.start();
        }
    }
}