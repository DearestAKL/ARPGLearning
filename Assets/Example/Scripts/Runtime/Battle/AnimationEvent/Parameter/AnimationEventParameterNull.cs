using Akari.GfCore;
using Akari.GfGame;
using Google.Protobuf;

namespace GameMain.Runtime
{
    public sealed class AnimationEventParameterNull : IGfAnimationEventParameter
    {
        public GfRunTimeTypeId RttId { get; }

        public AnimationEventParameterNull()
        {
            RttId = GfRunTimeTypeOf<AnimationEventParameterNull>.Id;
        }
    }

    public sealed class AnimationEventParameterNullFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new AnimationEventParameterMessageNull();
        }

        public object CreateInstance(IMessage message)
        {
            var i = new AnimationEventParameterNull();
            return i;
        }
    }
}
