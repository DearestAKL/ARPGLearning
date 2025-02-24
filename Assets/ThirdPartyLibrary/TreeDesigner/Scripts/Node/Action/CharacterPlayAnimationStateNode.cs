using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("PlayAnimationState")]
    [NodePath("Action/PlayAnimationState")]
    public class CharacterPlayAnimationStateNode  : ActionNode
    {
        [SerializeField,ShowInPanel("StateName")]
        string stateName;
        
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeCharacterPlayAnimationStateAction;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtCharacterPlayAnimationStateMessage()
            {
                StateName = stateName
            };
            return message;
        }
    }
}