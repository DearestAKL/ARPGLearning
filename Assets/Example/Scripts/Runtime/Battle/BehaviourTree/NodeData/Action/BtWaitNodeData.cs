using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtWaitNodeData : BtNodeData
    {
        public float Duration;
        public BtWaitNodeData(float duration)
        {
            Duration = duration;
        }

        public override Node CreateNode()
        {
            return new Wait(Duration);
        }
    }
    
    public sealed class BtWaitNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtWaitMessage();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (BtWaitMessage)message;
            return new BtWaitNodeData(m.Duration);
        }
    }
}