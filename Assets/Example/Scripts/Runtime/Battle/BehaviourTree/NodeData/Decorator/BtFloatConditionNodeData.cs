using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtFloatConditionNodeData : AConditionNodeData
    {
        public float FloatValue;

        public BtFloatConditionNodeData(int childIndex, int operatorValue, int stopsValue, string key, float floatValue)
            : base(childIndex, operatorValue, stopsValue, key)
        {
            FloatValue = floatValue;
        }

        public override Node CreateNode()
        {
            return CreateConditionNode(FloatValue);
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