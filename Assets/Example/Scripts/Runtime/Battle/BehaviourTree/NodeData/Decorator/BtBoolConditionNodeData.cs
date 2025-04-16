using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public abstract class AConditionNodeData : BtNodeData
    {
        public Operator Operator;
        public Stops Stops;
        public string Key;

        public AConditionNodeData(int childIndex,int operatorValue,int stopsValue,string key)
        {
            ChildIndex = childIndex;
            Operator = (Operator)operatorValue;
            Stops = (Stops)stopsValue;
            Key = key;
        }
        
        protected virtual Node CreateConditionNode(object value)
        {
            return null;
        }
    }

    public class BtBoolConditionNodeData : AConditionNodeData
    {
        public bool Value;
        
        public BtBoolConditionNodeData(int childIndex, int operatorValue, int stopsValue, string key, bool boolValue)
            : base(childIndex, operatorValue, stopsValue, key)
        {
            Value = boolValue;
        }
        
        public override Node CreateNode()
        {
            return new BlackboardConditionBool(Key, Operator, Value, Stops, CreateChild());
        }
    }
    
    public sealed class BtBoolConditionNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtBoolConditionMessage();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (BtBoolConditionMessage)message;
            return new BtBoolConditionNodeData(m.Base.Child,m.Base.Operator,m.Base.Stops,m.Base.Key,m.BoolValue);
        }
    }
}