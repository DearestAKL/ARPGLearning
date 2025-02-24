using System;
using UnityEngine;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("Patrol")]
    [NodePath("Action/Patrol")]
    public class CharacterPatrolNode : ActionNode
    {
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeCharacterPatrolAction;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtCharacterPatrolMessage()
            {
            };
            return message;
        }
    }
}