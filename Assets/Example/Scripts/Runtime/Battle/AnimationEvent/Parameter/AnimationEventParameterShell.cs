using Akari.GfCore;
using Akari.GfGame;
using Google.Protobuf;

namespace GameMain.Runtime
{
    public sealed class AnimationEventParameterShell : IGfAnimationEventParameter
    {
        public GfRunTimeTypeId RttId { get; }

        public int      Id;
        public GfFloat3 OffsetPosition;
        public GfFloat3 OffsetRotation;
        public bool IsLockTarget;

        public AnimationEventParameterShell()
        {
            RttId = GfRunTimeTypeOf<AnimationEventParameterShell>.Id;
        }
    }

    public sealed class AnimationEventParameterShellFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new AnimationEventParameterMessageShell();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (AnimationEventParameterMessageShell)message;
            var i = new AnimationEventParameterShell();
            i.Id                      = m.Id;
            i.OffsetPosition          = m.OffsetPosition.ToGfFloat3();
            i.OffsetRotation          = m.OffsetRotation.ToGfFloat3();
            i.IsLockTarget          = m.IsLockTarget;
            return i;
        }
    }
}