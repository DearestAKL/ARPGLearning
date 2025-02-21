using Akari.GfUnityEditor.ProtobufExtensions;
using GameMain.Runtime;
using Google.Protobuf;

namespace GameMain.Editor
{
    public class ShellDefinitionMessageDrawer : IMessageDrawer
    {
        private readonly ShellDefinitionMessage _message;
        private readonly IFieldDrawer            _fieldDrawer = new ShellDefinitionFieldDrawer();
        public IMessage Message => _message;


        public ShellDefinitionMessageDrawer(ShellDefinitionMessage message)
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