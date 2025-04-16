using System;
using GameMain.Runtime;
using UnityEngine;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("ConditionBool")]
    [NodePath("Decorator/ConditionBool")]
    public class BoolConditionNode : DecoratorNode
    {
        [SerializeField,ShowInPanel("Operator")]
        private NPBehave.Operator operatorValue;
        [SerializeField,ShowInPanel("Stops")]
        private NPBehave.Stops stopsValue;
        [SerializeField,ShowInPanel("Key")]
        private CharacterBlackboardKey key;
        [SerializeField,ShowInPanel("BoolValue")]
        private bool boolValue;

        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeBoolCondition;
        }
        
        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtBoolConditionMessage
            {
                Base = new GameMain.Runtime.BtBaseConditionMessage
                {
                    Child = m_Owner.GetSerializerNodeIndex(Child),
                    Operator = (int)operatorValue,
                    Stops = (int)stopsValue,
                    Key = key.GetString(),
                },
                BoolValue = boolValue,
            };
            return message;
        }
    }
}