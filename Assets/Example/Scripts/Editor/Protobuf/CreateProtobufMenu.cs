using System.IO;
using Akari.GfGame;
using GameMain.Runtime;
using Google.Protobuf;
using UnityEditor;

namespace GameMain.Editor
{
    internal static class CreateProtobufMenu
    {
        [MenuItem("Assets/Create/Protobuf/AttackDefinitionGroup")]
        private static void CreateAttackDefinition()
        {
            CreatePb<AttackDefinitionGroupMessage>(PbFileTypes.AttackDefinitionGroupFileType, "AttackDefinitionGroup.pb");
        }
        
        [MenuItem("Assets/Create/Protobuf/PassiveSkillDefinition")]
        private static void CreatePassiveSkillDefinition()
        {
            CreatePb<PassiveSkillDefinitionMessage>(PbFileTypes.PassiveSkillDefinitionFileType, "PassiveSkillDefinition_.pb");
        }
        
        [MenuItem("Assets/Create/Protobuf/ShellDefinition")]
        private static void CreateShellDefinition()
        {
            CreatePb<ShellDefinitionMessage>(PbFileTypes.ShellDefinitionFileType, "ShellDefinition_.pb");
        }
        
        [MenuItem("Assets/Create/Protobuf/BufferDefinition")]
        private static void CreateBufferDefinition()
        {
            CreatePb<BufferDefinitionMessage>(PbFileTypes.BufferDefinitionFileType, "BufferDefinition_.pb");
        }
        
        private static void CreatePb<TMessage>(string pbFileType, string newFileName = "NewProtobufFile.pb") where TMessage : IMessage, new()
        {
            var directoryPath = GetSelectionDirectoryPath();
            var path = Path.Combine(directoryPath, newFileName);

            using (var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
            {
                stream.WritePbFileHeader(pbFileType);

                var message = new TMessage();
                message.WriteDelimitedTo(stream);
            }

            AssetDatabase.Refresh();
        }
        
        private static string GetSelectionDirectoryPath(string defaultPath = "Assets")
        {
            var select = Selection.activeObject;
            if (select == null)
            {
                return defaultPath;
            }

            var path = AssetDatabase.GetAssetPath(select);
            if (string.IsNullOrEmpty(path))
            {
                return defaultPath;
            }

            if (!Directory.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }

            return path;
        }
    }
}