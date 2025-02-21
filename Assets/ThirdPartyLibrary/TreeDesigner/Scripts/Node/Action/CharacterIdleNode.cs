using System;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("Idle")]
    [NodePath("Action/Character/Idle")]
    public class CharacterIdleNode  : ActionNode
    {
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeCharacterIdleAction;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtCharacterIdleMessage();
            return message;
        }
    }
}