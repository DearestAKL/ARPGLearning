using System;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("Confront")]
    [NodePath("Action/Confront")]
    public class CharacterConfrontNode : ActionNode
    {
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeCharacterConfrontAction;
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