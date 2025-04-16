using Akari.GfGame;
using Google.Protobuf;

namespace GameMain.Runtime
{
    public abstract class BtPropertyData
    {
        public string Key;
        protected BtPropertyData(string key)
        {
            Key = key;
        }
    }
    
    public class BtIntPropertyData : BtPropertyData
    {
        public int Value;

        public BtIntPropertyData(string key,int value) : base(key)
        {
            Value = value;
        }
    }
    
    public sealed class BtIntPropertyFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtPropertyIntMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (BtPropertyIntMessage)message;
            return new BtIntPropertyData(m.Key,m.Value);
        }
    }
    
    public class BtBoolPropertyData : BtPropertyData
    {
        public bool Value;

        public BtBoolPropertyData(string key,bool value) : base(key)
        {
            Value = value;
        }
    }
    
    public sealed class BtBoolPropertyFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtPropertyBoolMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (BtPropertyBoolMessage)message;
            return new BtBoolPropertyData(m.Key,m.Value);
        }
    }
    
    public class BtFloatPropertyData : BtPropertyData
    {
        public float Value;

        public BtFloatPropertyData(string key,float value) : base(key)
        {
            Value = value;
        }
    }
    
    public sealed class BtFloatPropertyFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtPropertyFloatMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (BtPropertyFloatMessage)message;
            return new BtFloatPropertyData(m.Key,m.Value);
        }
    }

    public class BtStringPropertyData : BtPropertyData
    {
        public string Content;

        public BtStringPropertyData(string key,string content) : base(key)
        {
            Content = content;
        }
    }
    
    public sealed class BtStringPropertyFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtPropertyStringMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (BtPropertyStringMessage)message;
            return new BtStringPropertyData(m.Key,m.Content);
        }
    }
}