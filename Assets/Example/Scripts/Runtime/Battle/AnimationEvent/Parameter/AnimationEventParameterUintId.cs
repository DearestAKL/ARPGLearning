using Akari.GfCore;
using Akari.GfGame;
using Google.Protobuf;

namespace GameMain.Runtime
{
    public sealed class AnimationEventParameterUintId : IGfAnimationEventParameter
    {
        public GfRunTimeTypeId RttId { get; }

        public uint Id;

        public AnimationEventParameterUintId()
        {
            RttId = GfRunTimeTypeOf<AnimationEventParameterUintId>.Id;
        }
    }

    public sealed class AnimationEventParameterUintIdFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new AnimationEventParameterMessageUintId();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (AnimationEventParameterMessageUintId)message;
            var i = new AnimationEventParameterUintId();
            i.Id = m.Id;
            return i;
        }
    }
}