using System;
using UnityEngine;

namespace TreeDesigner
{
    [NodeName("Selector")]
    [NodePath("Composite/Selector")]
    public class SelectorNode : CompositeNode
    {
        [SerializeField,ShowInPanel("IsRandom")]
        bool m_IsRandom;
        
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeSelector;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.GfBtSelectorMessage
            {
                Composite = GetCompositeMessage(),
                Random = m_IsRandom
            };
            return message;
        }
    }
}