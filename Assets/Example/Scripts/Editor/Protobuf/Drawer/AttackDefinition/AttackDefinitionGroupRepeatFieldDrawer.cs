using Akari.GfUnityEditor.ProtobufExtensions;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace GameMain.Editor
{
    public class AttackDefinitionGroupRepeatFieldDrawer : DefaultRepeatedFieldDrawer
    {
        protected AttackDefinitionGroupRepeatFieldDrawer(IMessage parent, FieldDescriptor descriptor) : base(parent, descriptor) { }
        
        protected override IFieldDrawer UsedFieldDrawer() => new AttackDefinitionGroupFieldDrawer();

        public static AttackDefinitionGroupRepeatFieldDrawer GetRepeatFieldDrawer(IMessage parent, FieldDescriptor descriptor)
        {
            var attackDefinitionGroupRepeatFieldDrawer = new AttackDefinitionGroupRepeatFieldDrawer(parent, descriptor);
            attackDefinitionGroupRepeatFieldDrawer.Init();
            return attackDefinitionGroupRepeatFieldDrawer;
        }
    }
}