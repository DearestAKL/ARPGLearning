using Akari.GfUnityEditor.AnimationEventBundler;
using Google.Protobuf;
using GameMain.Runtime;

namespace GameMain.Editor
{
    public class EditorAnimationEventParameterNull : AnimationEventObjectParameterBase
    {
        public override int GetPbTypeId()
        {
            return RyPbTypes.AnimationEventParameterNull;
        }

        public override IMessage Serialize()
        {
            var message = new AnimationEventParameterMessageNull();
            return message;
        }
    }
}
