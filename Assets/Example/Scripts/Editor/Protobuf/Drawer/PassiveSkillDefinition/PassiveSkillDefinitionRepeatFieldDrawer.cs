using Akari.GfUnityEditor.ProtobufExtensions;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace GameMain.Editor
{
    public class PassiveSkillDefinitionRepeatFieldDrawer : DefaultRepeatedFieldDrawer
    {
        protected PassiveSkillDefinitionRepeatFieldDrawer(IMessage parent, FieldDescriptor descriptor) : base(parent, descriptor) { }
        
        protected override IFieldDrawer UsedFieldDrawer() => new PassiveSkillDefinitionFieldDrawer();

        public static PassiveSkillDefinitionRepeatFieldDrawer GetRepeatFieldDrawer(IMessage parent, FieldDescriptor descriptor)
        {
            var repeatFieldDrawer = new PassiveSkillDefinitionRepeatFieldDrawer(parent, descriptor);
            repeatFieldDrawer.Init();
            return repeatFieldDrawer;
        }
    }
}