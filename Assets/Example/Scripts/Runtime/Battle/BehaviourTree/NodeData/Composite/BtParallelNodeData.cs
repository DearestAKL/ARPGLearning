using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtParallelNodeData : BtNodeData
    {
        public Parallel.Policy SuccessPolicy;
        public Parallel.Policy FailurePolicy;
        
        public BtParallelNodeData(int[] childrenIndex,int successPolicy,int failurePolicy)
        {
            ChildrenIndex = childrenIndex;
            SuccessPolicy = (Parallel.Policy)successPolicy;
            FailurePolicy = (Parallel.Policy)failurePolicy;
        }

        public override Node CreateNode()
        {
            return new Parallel(SuccessPolicy, FailurePolicy, CreateChildren());
        }
    }
    
    public sealed class BtParallelNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new GfBtParallelMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (GfBtParallelMessage)message;
            return new BtParallelNodeData(m.Composite.Children.ToArray(),m.SuccessPolicy, m.FailurePolicy);
        }
    }
}