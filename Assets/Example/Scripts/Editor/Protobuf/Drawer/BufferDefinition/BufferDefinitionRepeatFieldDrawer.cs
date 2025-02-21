using Akari.GfUnityEditor.ProtobufExtensions;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace GameMain.Editor
{
    public class BufferDefinitionRepeatFieldDrawer: DefaultRepeatedFieldDrawer
    {
        protected BufferDefinitionRepeatFieldDrawer(IMessage parent, FieldDescriptor descriptor) : base(parent, descriptor) { }
        
        protected override IFieldDrawer UsedFieldDrawer() => new BufferDefinitionFieldDrawer();

        public static BufferDefinitionRepeatFieldDrawer GetRepeatFieldDrawer(IMessage parent, FieldDescriptor descriptor)
        {
            var repeatFieldDrawer = new BufferDefinitionRepeatFieldDrawer(parent, descriptor);
            repeatFieldDrawer.Init();
            return repeatFieldDrawer;
        }
    }
}