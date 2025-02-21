using Akari.GfUnityEditor.ProtobufExtensions;
using GameMain.Runtime;
using Google.Protobuf;

namespace GameMain.Editor
{
    public class PassiveSkillDefinitionMessageDrawer : IMessageDrawer
    {
        private readonly PassiveSkillDefinitionMessage _message;
        private readonly IFieldDrawer            _fieldDrawer = new PassiveSkillDefinitionFieldDrawer();
        public IMessage Message => _message;


        public PassiveSkillDefinitionMessageDrawer(PassiveSkillDefinitionMessage message)
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