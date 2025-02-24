using System;
using TreeDesigner;
using UnityEngine;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("Follow")]
    [NodePath("Action/Follow")]
    public class CharacterFollowNode  : ActionNode
    {
        [SerializeField,ShowInPanel("MinDistance")]
        private int minDistance;
        [SerializeField,ShowInPanel("MaxDistance")]
        private int maxDistance;
        
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeCharacterFollowAction;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtCharacterFollowMessage()
            {
                MinDistance = minDistance,
                MaxDistance = maxDistance
            };
            return message;
        }
    }
}