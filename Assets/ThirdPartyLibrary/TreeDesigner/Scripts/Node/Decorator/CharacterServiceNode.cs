using System;
using UnityEngine;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("CharacterService")]
    [NodePath("Decorator/CharacterService")]
    public class CharacterServiceNode : DecoratorNode
    {
        [SerializeField, ShowInPanel("Interval")]
        private float interval;
        
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeCharacterService;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtCharacterServiceMessage
            {
                Child = m_Owner.GetSerializerNodeIndex(Child),
                Interval = interval
            };
            return message;
        }
    }
}