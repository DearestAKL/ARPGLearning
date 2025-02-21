using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtCharacterServiceNodeData : BtNodeData
    {
        public float Interval;
        public BtCharacterServiceNodeData(int childIndex,float interval)
        {
            ChildIndex = childIndex;
            Interval = interval;
        }
        
        public override Node CreateNode()
        {
            return new BtCharacterService(Interval, CreateChild());
        }
    }
    
    public sealed class BtServiceNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtCharacterServiceMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (BtCharacterServiceMessage)message;
            return new BtCharacterServiceNodeData(m.Child,m.Interval);
        }
    }
}