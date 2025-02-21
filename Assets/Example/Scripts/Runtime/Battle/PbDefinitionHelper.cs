using System.Collections.Generic;
using System.IO;
using Akari.GfGame;
using Akari.GfUnity;
using Cysharp.Threading.Tasks;
using Google.Protobuf;

namespace GameMain.Runtime
{
    public static class PbDefinitionHelper
    {        
        private static readonly Dictionary<int, PassiveSkillDefinitionMessage> PassiveSkillMessageDict = new Dictionary<int, PassiveSkillDefinitionMessage>();
        private static readonly Dictionary<int, BufferDefinitionMessage> BufferMessageDict = new Dictionary<int, BufferDefinitionMessage>();
        private static readonly Dictionary<int, ShellDefinitionMessage> ShellMessageDict = new Dictionary<int, ShellDefinitionMessage>();
        private static readonly Dictionary<int, AttackDefinitionGroupMessage> AttackDefinitionGroupMessageDict = new Dictionary<int, AttackDefinitionGroupMessage>();
        
        public static async UniTask<PassiveSkillDefinitionMessage> GetPassiveSkillDefinitionMessage(int id)
        {
            if (PassiveSkillMessageDict.TryGetValue(id, out var definitionMessage))
            {
                return definitionMessage;
            }
            
            var binaryObject = await AssetManager.Instance.LoadAsset<BinaryObject>(AssetPathHelper.GetPassiveSkillDefinitionPath(id));
            var stream = new MemoryStream(binaryObject.Bytes);
            if (stream.ReadPbFileHeader().FileType != PbFileTypes.PassiveSkillDefinitionFileType)
            {
                return null;
            }
            var message = new PassiveSkillDefinitionMessage();
            message.MergeDelimitedFrom(stream);
            PassiveSkillMessageDict.Add(id, message);
            
            //预解析 PassiveSkillMessage下的BufferDefinitionMessage
            foreach (var eventMessage in message.Events)
            {
                foreach (var addBuffer in eventMessage.AddBuffers)
                {
                    await GetBufferDefinitionMessage(addBuffer.BuffId);
                }
            }
            
            return message;
        }
        
        public static async UniTask<BufferDefinitionMessage> GetBufferDefinitionMessage(int id)
        {
            if (BufferMessageDict.TryGetValue(id, out var definitionMessage))
            {
                return definitionMessage;
            }
            
            var binaryObject = await AssetManager.Instance.LoadAsset<BinaryObject>(AssetPathHelper.GetBufferDefinitionPath(id));
            var stream = new MemoryStream(binaryObject.Bytes);
            if (stream.ReadPbFileHeader().FileType != PbFileTypes.BufferDefinitionFileType)
            {
                return null;
            }
            var message = new BufferDefinitionMessage();
            message.MergeDelimitedFrom(stream);
            BufferMessageDict.TryAdd(id, message);
            return message;
        }
        
        public static async UniTask<ShellDefinitionMessage> GetShellDefinitionMessage(int id)
        {
            if (ShellMessageDict.TryGetValue(id, out var definitionMessage))
            {
                return definitionMessage;
            }
            
            var binaryObject = await AssetManager.Instance.LoadAsset<BinaryObject>(AssetPathHelper.GetShellDefinitionPath(id));
            var stream = new MemoryStream(binaryObject.Bytes);
            if (stream.ReadPbFileHeader().FileType != PbFileTypes.ShellDefinitionFileType)
            {
                return null;
            }
            var message = new ShellDefinitionMessage();
            message.MergeDelimitedFrom(stream);
            ShellMessageDict.TryAdd(id, message);

            return message;
        }
        
        public static async UniTask<AttackDefinitionGroupMessage> GetAttackDefinitionGroupMessage(int id)
        {
            if (AttackDefinitionGroupMessageDict.TryGetValue(id, out var definitionMessage))
            {
                return definitionMessage;
            }
            
            var binaryObject = await AssetManager.Instance.LoadAsset<BinaryObject>(AssetPathHelper.GetAttackDefinitionGroupPath(id));
            if (binaryObject == null)
            {
                //没有对应配置
                return null;
            }
            var stream = new MemoryStream(binaryObject.Bytes);
            if (stream.ReadPbFileHeader().FileType != PbFileTypes.AttackDefinitionGroupFileType)
            {
                return null;
            }
            var message = new AttackDefinitionGroupMessage();
            message.MergeDelimitedFrom(stream);
            AttackDefinitionGroupMessageDict.TryAdd(id, message);

            return message;
        }

        //Todo: 逻辑定义比较分散独立，等级相关内容如何传入呢?先配置本地固定值
        public static int GetNumericalMessage(BufferDefinitionMessage message, int index,int level = 1)
        { 
            if (index >=  message.NumericalValues.Count)
            {
                //配置错误
                return 0;
            }
            
            var numericalMessage = message.NumericalValues[index];
            if (numericalMessage.Excel <= 0)
            {
                return numericalMessage.Local;
            }
            else
            {
                var numerical = LubanManager.Instance.Tables.TbBufferNumerical.Get(message.Id, level);
                if (numericalMessage.Excel > numerical.Args.Length)
                {
                    //超过上限 没有参数
                    return 0;
                }
                //Excel 从1开始 需要-1
                return numerical.Args[numericalMessage.Excel - 1];
            }
        }
        
        public static int GetNumericalMessage(PassiveSkillDefinitionMessage message, int index,int level = 1)
        {
            if (index >=  message.NumericalValues.Count)
            {
                //配置错误
                return 0;
            }
            
            var numericalMessage = message.NumericalValues[index];
            if (numericalMessage.Excel == 0)
            {
                return numericalMessage.Local;
            }
            else
            {
                //根据id 和 level，以及excelIndex读取配置表
                return 0;
            }
        }

        public static int GetNumericalMessage(AttackDefinitionGroupMessage message, int index,int level = 1)
        {
            if (index >=  message.NumericalValues.Count)
            {
                //配置错误
                return 0;
            }
            
            var numericalMessage = message.NumericalValues[index];
            if (numericalMessage.Excel == 0)
            {
                return numericalMessage.Local;
            }
            else
            {
                //根据id 和 level，以及excelIndex读取配置表
                return 0;
            }
        }
        
        public static int GetNumericalMessage(ShellDefinitionMessage message, int index,int level = 1)
        {
            if (index >=  message.NumericalValues.Count)
            {
                //配置错误
                return 0;
            }
            
            var numericalMessage = message.NumericalValues[index];
            if (numericalMessage.Excel == 0)
            {
                return numericalMessage.Local;
            }
            else
            {
                //根据id 和 level，以及excelIndex读取配置表
                return 0;
            }
        }
    }
}