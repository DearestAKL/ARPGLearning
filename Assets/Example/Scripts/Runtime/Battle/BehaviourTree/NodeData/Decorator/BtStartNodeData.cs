using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtStartNodeData : BtNodeData
    {
        public BtStartNodeData(int childIndex)
        {
            ChildIndex = childIndex;
        }

        public override Node CreateNode()
        {
            return null;
        }
        
        public Root CreateRoot(Blackboard blackboard, Clock clock)
        {
            return new Root(blackboard, clock, CreateChild());
        }
    }
    
    public sealed class BtStartNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new GfBtStartMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (GfBtStartMessage)message;
            return new BtStartNodeData(m.Child);
        }
    }
}