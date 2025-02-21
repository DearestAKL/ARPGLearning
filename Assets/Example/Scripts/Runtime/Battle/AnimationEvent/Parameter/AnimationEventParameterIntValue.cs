using Akari.GfCore;
using Akari.GfGame;
using Google.Protobuf;

namespace GameMain.Runtime
{
    public sealed class AnimationEventParameterIntValue : IGfAnimationEventParameter
    {
        public GfRunTimeTypeId RttId { get; }

        public int Value;

        public AnimationEventParameterIntValue()
        {
            RttId = GfRunTimeTypeOf<AnimationEventParameterIntValue>.Id;
        }
    }

    public sealed class AnimationEventParameterIntValueFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new AnimationEventParameterMessageIntValue();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (AnimationEventParameterMessageIntValue)message;
            var i = new AnimationEventParameterIntValue();
            i.Value = m.Value;
            return i;
        }
    }
}
