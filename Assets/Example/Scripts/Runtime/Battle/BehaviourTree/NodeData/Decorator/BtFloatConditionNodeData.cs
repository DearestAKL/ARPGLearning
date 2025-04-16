using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtFloatConditionNodeData : AConditionNodeData
    {
        public float Value;

        public BtFloatConditionNodeData(int childIndex, int operatorValue, int stopsValue, string key, float floatValue)
            : base(childIndex, operatorValue, stopsValue, key)
        {
            Value = floatValue;
        }

        public override Node CreateNode()
        {
            return new BlackboardConditionFloat(Key, Operator, Value, Stops, CreateChild());
        }
    }
    
    public sealed class BtFloatConditionNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtFloatConditionMessage();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (BtFloatConditionMessage)message;
            return new BtFloatConditionNodeData(m.Base.Child,m.Base.Operator,m.Base.Stops,m.Base.Key,m.FloatValue);
        }
    }
}