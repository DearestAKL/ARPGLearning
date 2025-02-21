using Akari.GfCore;
using Akari.GfGame;
using Google.Protobuf;

namespace GameMain.Runtime
{
    public class AnimationEventParameterString : IGfAnimationEventParameter
    {
        public GfRunTimeTypeId RttId { get; }
        public string Content;

        public AnimationEventParameterString()
        {
            RttId = GfRunTimeTypeOf<AnimationEventParameterString>.Id;
        }
    }
    
    public sealed class AnimationEventParameterStringFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new AnimationEventParameterMessageString();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (AnimationEventParameterMessageString)message;
            var i = new AnimationEventParameterString();
            i.Content = m.Content;
            return i;
        }
    }
}