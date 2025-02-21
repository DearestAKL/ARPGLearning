using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtCharacterPlayAnimationStateNodeData : BtNodeData
    {
        public string StateName;
        
        public BtCharacterPlayAnimationStateNodeData(string stateName)
        {
            StateName = stateName;
        }

        public override Node CreateNode()
        {
            return new BtCharacterPlayAnimationStateAction(StateName);
        }
    }
    
    public sealed class BtCharacterPlayAnimationStateNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtCharacterPlayAnimationStateMessage();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (BtCharacterPlayAnimationStateMessage)message;
            return new BtCharacterPlayAnimationStateNodeData(m.StateName);
        }
    }
}