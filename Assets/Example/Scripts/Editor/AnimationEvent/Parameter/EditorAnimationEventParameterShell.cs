using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnityEditor.AnimationEventBundler;
using GameMain.Runtime;
using Google.Protobuf;

namespace GameMain.Editor
{
    public class EditorAnimationEventParameterShell : AnimationEventObjectParameterBase
    {
        public int          Id;
        public GfFloat3Data OffsetPosition;
        public GfFloat3Data OffsetRotation;
        public bool IsLockTarget;

        public override int GetPbTypeId()
        {
            return RyPbTypes.AnimationEventParameterShell;
        }

        public override IMessage Serialize()
        {
            var message = new AnimationEventParameterMessageShell();
            message.Id          = Id;
            message.OffsetPosition      = OffsetPosition.ToGfFloat3().ToGfFloat3Message();
            message.OffsetRotation      = OffsetRotation.ToGfFloat3().ToGfFloat3Message();
            message.IsLockTarget      = IsLockTarget;
            return message;
        }
    }
}