using Akari.GfUnityEditor.ProtobufExtensions;
using GameMain.Runtime;
using Google.Protobuf;

namespace GameMain.Editor
{
    public class BufferDefinitionMessageDrawer : IMessageDrawer
    {
        private readonly BufferDefinitionMessage _message;
        private readonly IFieldDrawer            _fieldDrawer = new BufferDefinitionFieldDrawer();
        public IMessage Message => _message;


        public BufferDefinitionMessageDrawer(BufferDefinitionMessage message)
        {
            _message = message;
        }

        public bool Draw()
        {
            if (_message == null)
            {
                return false;
            }
            return _fieldDrawer.DrawMessage(Message);
        }
    }
}