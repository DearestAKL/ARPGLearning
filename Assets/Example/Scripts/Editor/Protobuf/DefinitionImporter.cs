using System;
using Akari.GfUnityEditor.ProtobufExtensions;
using Google.Protobuf;

namespace GameMain.Editor
{
    [UnityEditor.AssetImporters.ScriptedImporter(1, "pb")]
    public sealed class DefinitionImporter : ProtobufImporter
    {
    }

    public interface ISupportedTypeFactory
    {
        string FileType    { get; }
        Type   MessageType { get; }
        IMessage GenerateMessage();
        IMessageDrawer GenerateDrawer(IMessage message);
        IMessageValidator GenerateValidator(IMessage message, string path);
    }

    public sealed class SupportedTypeFactory<TMessage, TDrawer, TValidator> : ISupportedTypeFactory
        where TMessage : class, IMessage
        where TDrawer : class, IMessageDrawer
        where TValidator : class, IMessageValidator

    {
        public string FileType    { get; }
        public Type   MessageType => typeof(TMessage);

        public SupportedTypeFactory(string fileType)
        {
            FileType = fileType;
        }

        public IMessage GenerateMessage()
        {
            return Activator.CreateInstance<TMessage>();
        }

        public IMessageDrawer GenerateDrawer(IMessage message)
        {
            return Activator.CreateInstance(typeof(TDrawer), message) as IMessageDrawer;
        }

        public IMessageValidator GenerateValidator(IMessage message, string path)
        {
            return Activator.CreateInstance(typeof(TValidator), message, path) as IMessageValidator;
        }
    }
}