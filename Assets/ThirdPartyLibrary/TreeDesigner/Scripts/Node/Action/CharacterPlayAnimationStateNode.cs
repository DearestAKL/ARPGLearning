using System;
using UnityEngine;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("PlayAnimationState")]
    [NodePath("Action/Character/PlayAnimationState")]
    public class CharacterPlayAnimationStateNode  : ActionNode
    {
        [SerializeField,ShowInPanel("StateName")]
        string m_StateName;
        
        public override int GetPbTypeId()
        {
            return GameMain.Runtime.RyPbTypes.BtNodeCharacterPlayAnimationStateAction;
        }

        public override Google.Protobuf.IMessage Serialize()
        {
            var message = new GameMain.Runtime.BtCharacterPlayAnimationStateMessage()
            {
                StateName = m_StateName
            };
            return message;
        }
    }
}