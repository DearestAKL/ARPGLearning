using System.IO;
using System.Linq;
using Akari.GfGame;
using Akari.GfUnity;
using Akari.GfUnityEditor.ProtobufExtensions;
using GameMain.Runtime;
using Google.Protobuf;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    [CustomEditor(typeof(DefinitionImporter))]
    public sealed class DefinitionImporterEditor : ProtobufImporterEditor
    {
        private static readonly ISupportedTypeFactory[] SupportedTypeFactories =
        {
            new SupportedTypeFactory<AttackDefinitionGroupMessage, AttackDefinitionGroupMessageDrawer, DefaultMessageValidator>(PbFileTypes.AttackDefinitionGroupFileType),
            new SupportedTypeFactory<PassiveSkillDefinitionMessage,PassiveSkillDefinitionMessageDrawer, DefaultMessageValidator>(PbFileTypes.PassiveSkillDefinitionFileType),
            new SupportedTypeFactory<ShellDefinitionMessage,ShellDefinitionMessageDrawer, DefaultMessageValidator>(PbFileTypes.ShellDefinitionFileType),
            new SupportedTypeFactory<BufferDefinitionMessage,BufferDefinitionMessageDrawer, DefaultMessageValidator>(PbFileTypes.BufferDefinitionFileType),
        };
        
        private bool _enabled;
        
        public override void OnEnable()
        {
            base.OnEnable();
            _enabled = true;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _enabled = false;
        }

        public override void OnInspectorGUI()
        {
            if (!_enabled)
            {
                return;
            }
            base.OnInspectorGUI();
        }

        protected override IMessage DecodeMessage(BinaryObject binaryObject)
        {
            using (var stream = new MemoryStream(binaryObject.Bytes))
            {
                var header  = stream.ReadPbFileHeader();
                var factory = SupportedTypeFactories.FirstOrDefault(f => f.FileType == header.FileType);
                if (factory == null)
                {
                    Debug.LogWarning($"unsupported pb file, path={binaryObject.Path}");
                    return null;
                }

                var message = factory.GenerateMessage();
                message.MergeDelimitedFrom(stream);
                return message;
            }
        }

        protected override byte[] EncodeMessage(IMessage message)
        {
            var factory = SupportedTypeFactories.FirstOrDefault(f => f.MessageType == message.GetType());
            if (factory == null)
            {
                Debug.LogError($"invalid message, {message}");
                return null;
            }

            using (var stream = new MemoryStream())
            {
                stream.WritePbFileHeader(factory.FileType);
                message.WriteDelimitedTo(stream);

                return stream.ToArray();
            }
        }

        protected override IMessageDrawer GenerateDrawer(IMessage message)
        {
            var factory = SupportedTypeFactories.FirstOrDefault(f => f.MessageType == message.GetType());
            if (factory == null)
            {
                return base.GenerateDrawer(message);
            }

            return factory.GenerateDrawer(message);
        }

        protected override IMessageValidator GenerateValidator(IMessage message, string path)
        {
            var factory = SupportedTypeFactories.FirstOrDefault(f => f.MessageType == message.GetType());
            if (factory == null)
            {
                return base.GenerateValidator(message, path);
            }

            return factory.GenerateValidator(message, path);
        }
    }
}
