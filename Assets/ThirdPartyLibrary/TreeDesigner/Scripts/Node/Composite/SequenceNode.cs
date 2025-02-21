using System;
using UnityEngine;

namespace TreeDesigner 
{
    [NodeName("Sequence")]
    [NodePath("Composite/Sequence")]
    public class SequenceNode : CompositeNode
    {
        [SerializeField,ShowInPanel("IsRandom")]
        bool m_IsRandom;
        
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeSequencer;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.GfBtSequencerMessage
            {
                Composite = GetCompositeMessage(),
                Random = m_IsRandom
            };
            return message;
        }
    }
}