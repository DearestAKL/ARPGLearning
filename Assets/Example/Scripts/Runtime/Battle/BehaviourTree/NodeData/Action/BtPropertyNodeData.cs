using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtSetIntPropertyNodeData : BtNodeData
    {
        public int Value;

        public BtSetIntPropertyNodeData(int value)
        {
            Value = value;
        }

        public override Node CreateNode()
        {
            return new SetIntProperty(PropertyKey, Value);
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
            return new BtSetIntPropertyNodeData(m.Value);
        }
    }
    
    public class BtSetBoolPropertyNodeData : BtNodeData
    {
        public bool Value;

        public BtSetBoolPropertyNodeData(bool value)
        {
            Value = value;
        }

        public override Node CreateNode()
        {
            return new SetBoolProperty(PropertyKey, Value);
        }
    }
    
    public sealed class BtSetBoolPropertyNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtPropertyBoolMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (BtPropertyBoolMessage)message;
            return new BtSetBoolPropertyNodeData(m.Value);
        }
    }
    
    public class BtSetFloatPropertyNodeData : BtNodeData
    {
        public float Value;

        public BtSetFloatPropertyNodeData(float value)
        {
            Value = value;
        }

        public override Node CreateNode()
        {
            return new SetFloatProperty(PropertyKey, Value);
        }
    }
    
    public sealed class BtSetFloatPropertyNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtPropertyFloatMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (BtPropertyFloatMessage)message;
            return new BtSetFloatPropertyNodeData(m.Value);
        }
    }
    
    // public sealed class BtSetStringPropertyNodeFactory : IGfPbFactory
    // {
    //     public IMessage CreateMessage()
    //     {
    //         return new BtPropertyStringMessage();
    //     }
    //
    //     public object CreateInstance(IMessage message)
    //     {
    //         var m = (BtPropertyStringMessage)message;
    //         var value = new BtStringPropertyData(m.Key, m.Content);
    //         return new BtPropertyNodeData(value);
    //     }
    // }
}