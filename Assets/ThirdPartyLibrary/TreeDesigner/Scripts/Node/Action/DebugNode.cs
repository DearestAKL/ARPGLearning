using System;
using System.Collections.Generic;
using UnityEngine;

namespace TreeDesigner 
{
    [Serializable]
    [NodeName("Debug")]
    [NodePath("Action/Debug")]
    public class DebugNode : ActionNode
    {
        [SerializeField, PropertyPort(PortDirection.Input, "Log")]
        StringPropertyPort m_Log = new StringPropertyPort();
        
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeDebugAction;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtDebugMessage();
            if (!HasPropertyEdge())
            {
                message.Message = m_Log?.Value;
            }
            return message;
        }
    }
}