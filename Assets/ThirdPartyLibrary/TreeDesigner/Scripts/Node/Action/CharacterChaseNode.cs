using System;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("Chase")]
    [NodePath("Action/Chase")]
    public class CharacterChaseNode : ActionNode
    {
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeCharacterChaseAction;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtCharacterNullMessage()
            {
            };
            return message;
        }
    }
}