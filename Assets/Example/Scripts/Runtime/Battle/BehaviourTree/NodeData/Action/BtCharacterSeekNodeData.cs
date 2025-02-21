using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtCharacterSeekNodeData : BtNodeData
    {
        public float Radius;
        public int Angle;
        
        public BtCharacterSeekNodeData(float radius,int angle)
        {
            Radius = radius;
            Angle = angle;
        }

        public override Node CreateNode()
        {
            return new BtCharacterSeekAction(Radius,Angle);
        }
    }
    
    public sealed class BtCharacterSeekNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtCharacterSeekMessage();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (BtCharacterSeekMessage)message;
            return new BtCharacterSeekNodeData(m.Radius,m.Angle);
        }
    }
}