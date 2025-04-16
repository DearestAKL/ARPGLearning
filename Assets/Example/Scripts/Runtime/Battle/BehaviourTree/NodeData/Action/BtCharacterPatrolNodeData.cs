using Akari.GfGame;
using Google.Protobuf;
using NPBehave;


namespace GameMain.Runtime
{
    public class BtCharacterPatrolNodeData : BtNodeData
    {
        public BtCharacterPatrolNodeData()
        {
        }

        public override Node CreateNode()
        {
            return new BtCharacterPatrolAction();
        }
    }

    public sealed class BtCharacterPatrolNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtCharacterNullMessage();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (BtCharacterNullMessage)message;
            return new BtCharacterPatrolNodeData();
        }
    }
}