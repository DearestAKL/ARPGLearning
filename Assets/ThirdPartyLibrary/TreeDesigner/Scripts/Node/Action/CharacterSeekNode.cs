using System;
using UnityEngine;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("Seek")]
    [NodePath("Action/Character/Seek")]
    public class CharacterSeekNode : ActionNode
    {
        [SerializeField,ShowInPanel("Radius")]
        private float radius;
        [SerializeField,ShowInPanel("Angle")]
        private int angle;
        
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeCharacterSeekAction;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtCharacterSeekMessage()
            {
                Radius = radius,
                Angle = angle,
            };
            return message;
        }
    }
}