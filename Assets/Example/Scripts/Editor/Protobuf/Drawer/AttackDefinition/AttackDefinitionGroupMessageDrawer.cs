using Akari.GfUnityEditor.ProtobufExtensions;
using GameMain.Runtime;
using Google.Protobuf;

namespace GameMain.Editor
{
    public class AttackDefinitionGroupMessageDrawer : IMessageDrawer
    {
        private readonly AttackDefinitionGroupMessage _message;
        private readonly IFieldDrawer            _fieldDrawer = new AttackDefinitionGroupFieldDrawer();
        public IMessage Message => _message;
        
        public AttackDefinitionGroupMessageDrawer(AttackDefinitionGroupMessage message)
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
        
        // public static AttackDefinitionGroupMessageDrawer GetDrawer(AttackDefinitionGroupMessage message)
        // {
        //     var drawer = new AttackDefinitionGroupMessageDrawer(message);
        //     return drawer;
        // }
    }
}