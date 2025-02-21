using Akari.GfUnityEditor.AnimationEventBundler;
using Google.Protobuf;
using GameMain.Runtime;

namespace GameMain.Editor
{
    public class EditorAnimationEventParameterUintId : AnimationEventObjectParameterBase
    {
        public uint Id;

        public override int GetPbTypeId()
        {
            return RyPbTypes.AnimationEventParameterUintId;
        }

        public override IMessage Serialize()
        {
            var message = new AnimationEventParameterMessageUintId();
            message.Id = Id;
            return message;
        }
    }
}
