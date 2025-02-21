using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtDebugNodeData : BtNodeData
    {
        public string Message;
        public BtDebugNodeData(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Message = message;
            }
        }

        public override Node CreateNode()
        {
            if (!string.IsNullOrEmpty(PropertyKey))
            {
                return DebugLog.CreatDebugLogByPropertyKey(PropertyKey);
            }
            return DebugLog.CreatDebugLogByMessage(Message);
        }
    }
    
    
    public sealed class BtDebugNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new BtDebugMessage();
        }

        public object CreateInstance(IMessage message)
        {
            var m = (BtDebugMessage)message;
            return new BtDebugNodeData(m.Message);
        }
    }
}