using Akari.GfCore;
using Akari.GfGame;
using Google.Protobuf;

namespace GameMain.Runtime
{
    public class AnimationEventParameterEffectPlay : IGfAnimationEventParameter
    {
        public GfRunTimeTypeId RttId { get; protected set; }

        public string EffectId;
        public string TargetBoneName;
        public GfFloat3 OffsetPosition;
        public GfFloat3 OffsetRotation;
        public GfFloat3 EffectSize;
        public float CameraOffset;
        public bool CancelRemove;
        public bool ChangeAttribute;

        public AnimationEventParameterEffectPlay()
        {
            RttId = GfRunTimeTypeOf<AnimationEventParameterEffectPlay>.Id;
        }
    }

    //public class AnimationEventParameterLoopEffectPlay : AnimationEventParameterEffectPlay
    //{
    //    public AnimationEventParameterLoopEffectPlay()
    //    {
    //        RttId = GfRunTimeTypeOf<AnimationEventParameterLoopEffectPlay>.Id;
    //    }
    //}

    //public class AnimationEventParameterEffectAttach : IGfAnimationEventParameter
    //{
    //    public GfRunTimeTypeId RttId { get; }

    //    public GfString EffectId;
    //    public GfString TargetBoneName;
    //    public GfFloat3 OffsetPosition;
    //    public GfFloat3 OffsetRotation;
    //    public GfFloat3 EffectSize;
    //    public float CameraOffset;
    //    public bool DisposeIfDetached;
    //    public bool DisableFollowXPosition;
    //    public bool DisableFollowYPosition;
    //    public bool DisableFollowZPosition;
    //    public bool DisableFollowXRotation;
    //    public bool DisableFollowYRotation;
    //    public bool DisableFollowZRotation;
    //    public EffectDetachMode DetachMode;
    //    public bool CancelRemove;
    //    public bool ChangeAttribute;

    //    public AnimationEventParameterEffectAttach()
    //    {
    //        RttId = GfRunTimeTypeOf<AnimationEventParameterEffectAttach>.Id;
    //    }
    //}

    //public class AnimationEventParameterEffectDetach : IGfAnimationEventParameter
    //{
    //    public GfRunTimeTypeId RttId { get; }

    //    public GfString TargetBoneName;

    //    public AnimationEventParameterEffectDetach()
    //    {
    //        RttId = GfRunTimeTypeOf<AnimationEventParameterEffectDetach>.Id;
    //    }
    //}

    //public class AnimationEventParameterSceneDarkness : IGfAnimationEventParameter
    //{
    //    public GfRunTimeTypeId RttId { get; }

    //    public int Level;

    //    public int Frame;

    //    public AnimationEventParameterSceneDarkness()
    //    {
    //        RttId = GfRunTimeTypeOf<AnimationEventParameterSceneDarkness>.Id;
    //    }
    //}

    public sealed class AnimationEventParameterEffectPlayFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new AnimationEventParameterMessageEffectPlay();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (AnimationEventParameterMessageEffectPlay)message;
            var i = new AnimationEventParameterEffectPlay();
            i.EffectId = m.EffectId;
            i.TargetBoneName = m.TargetBoneName;
            i.OffsetPosition = m.OffsetPosition.ToGfFloat3();
            i.OffsetRotation = m.OffsetRotation.ToGfFloat3();
            i.EffectSize = m.EffectSize.ToGfFloat3();
            i.CameraOffset = m.CameraOffset;
            i.CancelRemove = m.CancelRemove;
            i.ChangeAttribute = m.ChangeAttribute;
            return i;
        }
    }

    //public sealed class AnimationEventParameterLoopEffectPlayFactory : IGfPbFactory
    //{
    //    public IMessage CreateMessage()
    //    {
    //        return new AnimationEventParameterMessageLoopEffectPlay();
    //    }

    //    public object CreateInstance(IMessage message)
    //    {
    //        var m = (AnimationEventParameterMessageLoopEffectPlay)message;
    //        var i = new AnimationEventParameterLoopEffectPlay();
    //        i.EffectId = m.EffectId.ToGfString();
    //        i.TargetBoneName = m.TargetBoneName.ToGfString();
    //        i.OffsetPosition = m.OffsetPosition.ToGfFloat3();
    //        i.OffsetRotation = m.OffsetRotation.ToGfFloat3();
    //        i.EffectSize = m.EffectSize.ToGfFloat3();
    //        i.CameraOffset = m.CameraOffset;
    //        i.CancelRemove = m.CancelRemove;
    //        i.ChangeAttribute = m.ChangeAttribute;
    //        return i;
    //    }
    //}

    //public sealed class AnimationEventParameterEffectAttachFactory : IGfPbFactory
    //{
    //    public IMessage CreateMessage()
    //    {
    //        return new AnimationEventParameterMessageEffectAttach();
    //    }

    //    public object CreateInstance(IMessage message)
    //    {
    //        var m = (AnimationEventParameterMessageEffectAttach)message;
    //        var i = new AnimationEventParameterEffectAttach();
    //        i.EffectId = m.EffectId.ToGfString();
    //        i.TargetBoneName = m.TargetBoneName.ToGfString();
    //        i.OffsetPosition = m.OffsetPosition.ToGfFloat3();
    //        i.OffsetRotation = m.OffsetRotation.ToGfFloat3();
    //        i.EffectSize = m.EffectSize.ToGfFloat3();
    //        i.CameraOffset = m.CameraOffset;
    //        i.DisposeIfDetached = m.DisposeIfDetached;
    //        i.DisableFollowXPosition = m.DisableFollowXPosition;
    //        i.DisableFollowYPosition = m.DisableFollowYPosition;
    //        i.DisableFollowZPosition = m.DisableFollowZPosition;
    //        i.DisableFollowXRotation = m.DisableFollowXRotation;
    //        i.DisableFollowYRotation = m.DisableFollowYRotation;
    //        i.DisableFollowZRotation = m.DisableFollowZRotation;
    //        i.DetachMode = m.DetachMode;
    //        i.CancelRemove = m.CancelRemove;
    //        i.ChangeAttribute = m.ChangeAttribute;
    //        return i;
    //    }
    //}

    //public sealed class AnimationEventParameterEffectDetachFactory : IGfPbFactory
    //{
    //    public IMessage CreateMessage()
    //    {
    //        return new AnimationEventParameterMessageEffectDetach();
    //    }

    //    public object CreateInstance(IMessage message)
    //    {
    //        var m = (AnimationEventParameterMessageEffectDetach)message;
    //        var i = new AnimationEventParameterEffectDetach();
    //        i.TargetBoneName = m.TargetBoneName.ToGfString();
    //        return i;
    //    }
    //}

    //public sealed class AnimationEventParameterSceneDarknessFactory : IGfPbFactory
    //{
    //    public IMessage CreateMessage()
    //    {
    //        return new AnimationEventParameterMessageSceneDarkness();
    //    }

    //    public object CreateInstance(IMessage message)
    //    {
    //        var m = (AnimationEventParameterMessageSceneDarkness)message;
    //        var i = new AnimationEventParameterSceneDarkness();
    //        i.Level = m.Level;
    //        i.Frame = m.Frame;
    //        return i;
    //    }
    //}
}
