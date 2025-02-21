using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtCharacterMoveNodeData : BtNodeData
    {
        public BtCharacterMoveNodeData()
        {
        }

        public override Node CreateNode()
        {
            return new BtCharacterMoveAction();
        }
    }
    
    public sealed class BtCharacterMoveNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtCharacterMoveMessage();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (BtCharacterMoveMessage)message;
            return new BtCharacterMoveNodeData();
        }
    }
}