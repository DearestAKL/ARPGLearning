using System;
using UnityEngine;

namespace GameMain.Runtime
{
    [Serializable]
    public class CharacterStateView
    {
        public string Name;
        public AnimationClip Clip;
        public TextAsset AnimationEvent;
        public bool IsRepeat;

        public string StateName => Name;
        public float Length => Clip.length;
        public float FrameRate => Clip.frameRate;
    }
}
