using Akari.GfUnityEditor.AnimationEventBundler;
using Google.Protobuf;
using GameMain.Runtime;

namespace GameMain.Editor
{
    public class EditorAnimationEventParameterIntValue : AnimationEventObjectParameterBase
    {
        public int Value;

        public override int GetPbTypeId()
        {
            return RyPbTypes.AnimationEventParameterIntValue;
        }

        public override IMessage Serialize()
        {
            var message = new AnimationEventParameterMessageIntValue();
            message.Value = Value;
            return message;
        }
    }
}
