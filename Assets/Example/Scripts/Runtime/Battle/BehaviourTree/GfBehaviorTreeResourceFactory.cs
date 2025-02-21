using System.IO;
using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class GfBehaviorTreeResourceFactory : IGfResourceFactory<GfBtResource>
    {
        private static GfBehaviorTreeResourceFactory _instance;
        public static GfBehaviorTreeResourceFactory Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GfBehaviorTreeResourceFactory();
                }
        
                return _instance;
            }
        }
        
        public GfBtResource CreateResource(Stream stream)
        {
            if (stream.ReadPbFileHeader().FileType != GfResourceFileHeaderTypes.BehaviourTreeFileType)
            {
                return null;
            }
            
            var fileHeader = stream.ReadPbMessage<GfBtFileHeaderMessage>();
            var nodeMessages   = GfArrayUtility.Create<Google.Protobuf.IMessage>(fileHeader.NodeTypes.Count);
            var propertyMessages   = GfArrayUtility.Create<Google.Protobuf.IMessage>(fileHeader.PropertyTypes.Count);

            var nodeTypes = fileHeader.NodeTypes.ToArray();
            for (var i = 0; i < nodeTypes.Length; i++)
            {
                nodeMessages[i] = stream.ReadPbMessageByType(nodeTypes[i]);
            }
            
            var propertyTypes = fileHeader.PropertyTypes.ToArray();
            for (var i = 0; i < propertyTypes.Length; i++)
            {
                propertyMessages[i] = stream.ReadPbMessageByType(propertyTypes[i]);
            }

            return new GfBtResource(fileHeader, nodeMessages, propertyMessages);
        }

        public object CreateResourceObject(Stream stream)
        {
            return CreateResource(stream);
        }
    }
}