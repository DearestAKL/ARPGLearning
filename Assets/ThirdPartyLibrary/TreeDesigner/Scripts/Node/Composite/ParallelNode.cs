using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreeDesigner
{
    [NodeName("Parallel")]
    [NodePath("Composite/Parallel")]
    public class ParallelNode : CompositeNode
    {
        [SerializeField,ShowInPanel("SuccessPolicy")]
        NPBehave.Parallel.Policy m_SuccessPolicy;
        [SerializeField,ShowInPanel("FailurePolicy")]
        NPBehave.Parallel.Policy m_FailurePolicy;
        
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeParallel;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.GfBtParallelMessage
            {
                Composite = GetCompositeMessage(),
                SuccessPolicy = (int)m_SuccessPolicy,
                FailurePolicy = (int)m_FailurePolicy,
            };
            return message;
        }
    }
}