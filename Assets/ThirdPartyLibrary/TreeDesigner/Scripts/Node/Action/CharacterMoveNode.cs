using System;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("Move")]
    [NodePath("Action/Move")]
    public class CharacterMoveNode  : ActionNode
    {
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeCharacterMoveAction;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtCharacterMoveMessage();
            return message;
        }
    }
}