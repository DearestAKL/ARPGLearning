// using Akari.GfUnityEditor.ProtobufExtensions;
// using Google.Protobuf;
// using Google.Protobuf.Reflection;
//
// namespace GameMain.Editor
// {
//     public class ShellDefinitionRepeatFieldDrawer : DefaultRepeatedFieldDrawer
//     {
//         protected ShellDefinitionRepeatFieldDrawer(IMessage parent, FieldDescriptor descriptor) : base(parent, descriptor) { }
//         
//         protected override IFieldDrawer UsedFieldDrawer() => new PassiveSkillDefinitionFieldDrawer();
//
//         public static ShellDefinitionRepeatFieldDrawer GetRepeatFieldDrawer(IMessage parent, FieldDescriptor descriptor)
//         {
//             var repeatFieldDrawer = new ShellDefinitionRepeatFieldDrawer(parent, descriptor);
//             repeatFieldDrawer.Init();
//             return repeatFieldDrawer;
//         }
//     }
// }