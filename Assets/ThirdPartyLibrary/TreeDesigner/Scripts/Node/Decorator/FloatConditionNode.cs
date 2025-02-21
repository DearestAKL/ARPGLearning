using System;
using GameMain.Runtime;
using UnityEngine;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("ConditionFloat")]
    [NodePath("Decorator/ConditionFloat")]
    public class FloatConditionNode : DecoratorNode
    {
        [SerializeField,ShowInPanel("Operator")]
        private NPBehave.Operator operatorValue;
        [SerializeField,ShowInPanel("Stops")]
        private NPBehave.Stops stopsValue;
        [SerializeField,ShowInPanel("Key")]
        private CharacterBlackboardKey key;
        [SerializeField,ShowInPanel("FloatValue")]
        private float floatValue;
        
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeFloatCondition;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtFloatConditionMessage
            {
                Base = new GameMain.Runtime.BtBaseConditionMessage
                {
                    Child = m_Owner.GetSerializerNodeIndex(Child),
                    Operator = (int)operatorValue,
                    Stops = (int)stopsValue,
                    Key = key.GetString(),
                },
                FloatValue = floatValue,
            };
            return message;
        }
    }
}