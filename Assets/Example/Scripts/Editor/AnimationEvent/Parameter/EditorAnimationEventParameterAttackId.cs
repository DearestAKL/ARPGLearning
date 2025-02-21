using System.IO;
using Akari.GfGame;
using Akari.GfUnity;
using Akari.GfUnityEditor.AnimationEventBundler;
using Google.Protobuf;
using GameMain.Runtime;

namespace GameMain.Editor
{
    public class EditorAnimationEventParameterAttackId : AnimationEventObjectParameterBase
    {
        //public BinaryObject AttackDefinitionGroup;
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

        // private AttackDefinitionGroupMessage GetAttackDefinitionGroupMessage()
        // {
        //     var stream = new MemoryStream(AttackDefinitionGroup.Bytes);
        //     
        //     if (stream.ReadPbFileHeader().FileType != PbFileTypes.AttackDefinitionGroupFileType)
        //     {
        //         return null;
        //     }
        //
        //     var message = new AttackDefinitionGroupMessage();
        //     message.MergeDelimitedFrom(stream);
        //
        //     return message;
        // }
    }
}