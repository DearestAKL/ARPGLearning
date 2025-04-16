using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtCharacterChaseNodeData : BtNodeData
    {
        public BtCharacterChaseNodeData()
        {
        }

        public override Node CreateNode()
        {
            return new BtCharacterChaseAction();
        }
    }
    
    public sealed class BtCharacterChaseNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtCharacterNullMessage();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (BtCharacterNullMessage)message;
            return new BtCharacterChaseNodeData();
        }
    }
}