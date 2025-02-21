using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtCharacterIdleNodeData : BtNodeData
    {
        public BtCharacterIdleNodeData()
        {
        }

        public override Node CreateNode()
        {
            return new BtCharacterIdleAction();
        }
    }
    
    public sealed class BtCharacterIdleNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtCharacterIdleMessage();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (BtCharacterIdleMessage)message;
            return new BtCharacterIdleNodeData();
        }
    }
}