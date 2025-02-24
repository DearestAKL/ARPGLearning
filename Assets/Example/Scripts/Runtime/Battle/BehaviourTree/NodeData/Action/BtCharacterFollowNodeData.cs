using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtCharacterFollowNodeData : BtNodeData
    {
        private float MinDistance;
        private float MaxDistance;
        
        public BtCharacterFollowNodeData(float minDistance,float maxDistance)
        {
            MinDistance = minDistance;
            MaxDistance = maxDistance;
        }
        
        public override Node CreateNode()
        {
            return new BtCharacterFollowAction(MinDistance, MaxDistance);
        }
    }
    
    public sealed class BtCharacterFollowNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtCharacterFollowMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (BtCharacterFollowMessage)message;
            return new BtCharacterFollowNodeData(m.MinDistance, m.MaxDistance);
        }
    }
}