using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtCharacterConfrontNodeData : BtNodeData
    {
        public BtCharacterConfrontNodeData()
        {
        }

        public override Node CreateNode()
        {
            return new BtCharacterConfrontAction();
        }
    }
    
    public sealed class BtCharacterConfrontNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtCharacterNullMessage();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (BtCharacterNullMessage)message;
            return new BtCharacterConfrontNodeData();
        }
    }
}