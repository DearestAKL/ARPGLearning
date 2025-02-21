using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("Wait")]
    [NodePath("Action/Wait")]
    public class WaitNode : ActionNode
    {
        [SerializeField, PropertyPort(PortDirection.Input, "Time")]
        FloatPropertyPort m_Time = new FloatPropertyPort();

        float m_CurrentTime;

        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeWaitAction;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtWaitMessage();
            if (!HasPropertyEdge())
            {
                message.Duration = m_Time.Value;
            }
            return message;
        }
    }
}