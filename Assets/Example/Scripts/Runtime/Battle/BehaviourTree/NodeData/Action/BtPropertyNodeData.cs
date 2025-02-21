using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtPropertyNodeData : BtNodeData
    {
        public object Value;

        public BtPropertyNodeData(object value)
        {
            Value = value;
        }

        public override Node CreateNode()
        {
            return new Property(PropertyKey, Value);
        }
    }
    
    public sealed class BtSetIntPropertyNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtPropertyIntMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (BtPropertyIntMessage)message;
            var value = new BtIntPropertyData(m.Key,m.Value);
            return new BtPropertyNodeData(value);
        }
    }
    
    public sealed class BtSetStringPropertyNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtPropertyStringMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (BtPropertyStringMessage)message;
            var value = new BtStringPropertyData(m.Key, m.Content);
            return new BtPropertyNodeData(value);
        }
    }
}