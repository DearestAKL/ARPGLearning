using System.IO;
using DialogueGraph.Runtime;
using DialogueGraph.Serialize;
using Google.Protobuf;
using UnityEditor;

namespace DialogueGraph
{
    public static class DlogSerializer
    {
        private static DlogObject mCurDlogObject;

        public static void Serialize(DlogObject dlogObject)
        {
            mCurDlogObject = dlogObject;
            
            var dlogMsg = new DlogMessage();
            dlogMsg.StartNode = mCurDlogObject.GetNodeIndexPlusOne(dlogObject.StartNode);
            
            for (int i = 0; i < dlogObject.Nodes.Count; i++)
            {
                dlogMsg.Nodes.Add(CreateNodeMessage(dlogObject.Nodes[i]));
            }
            
            for (int i = 0; i < dlogObject.Properties.Count; i++)
            {
                dlogMsg.Properties.Add(CreatePropertyMessage(dlogObject.Properties[i]));
            }
            
            var stream = new MemoryStream();
            dlogMsg.WriteDelimitedTo(stream);
            
            var path = $"./Assets/Example/GameRes/Dlog/{dlogObject.name}.bytes".Replace(" (Runtime)", "_Dlog");
            var dstStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            dstStream.Write(stream.GetBuffer(), 0, (int)stream.Length);
            dstStream.Flush();
            dstStream.Close();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            dstStream.Dispose();
        }

        public static NodeMessage CreateNodeMessage(Node node)
        {
            var nodeMsg = new NodeMessage();
            nodeMsg.Type = (int)node.Type;

            if (node.ActorGuid != null)
            {
                nodeMsg.CurActor = mCurDlogObject.GetPropertyIndexPlusOne(node.ActorGuid);
            }

            for (int i = 0; i < node.Lines.Count; i++)
            {
                var lineMsg = CreateConversationLineMessage(node.Lines[i]);
                nodeMsg.Lines.Add(lineMsg);
            }
            
            return nodeMsg;
        }

        public static PropertyMessage CreatePropertyMessage(Property property)
        {
            var propertyMsg = new PropertyMessage();
            propertyMsg.Type = (int)property.Type;
            propertyMsg.Reference = property.ReferenceName;

            return propertyMsg;
        }

        public static ConversationLineMessage CreateConversationLineMessage(ConversationLine line)
        {
            var lineMsg = new ConversationLineMessage();
            
            lineMsg.Message = line.Message;

            if (line.Next != null)
            {
                lineMsg.NextNode = mCurDlogObject.GetNodeIndexPlusOne(line.Next);
            }

            if (line.Triggers != null && line.Triggers.Count > 0)
            {
                for (int i = 0; i <  line.Triggers.Count; i++)
                {
                    var propertyIndex = mCurDlogObject.GetPropertyIndexPlusOne(line.Triggers[i]);
                    lineMsg.Triggers.Add(propertyIndex);
                }
            }

            if (line.CheckTrees != null && line.CheckTrees.Count > 0)
            {
                for (int i = 0; i <  line.CheckTrees.Count; i++)
                {
                    var checkTreeMsg = CreateCheckTreeMessage(line.CheckTrees[0]);
                    lineMsg.CheckTrees.Add(checkTreeMsg);
                }
            }
            
            return lineMsg;
        }

        public static CheckTreeMessage CreateCheckTreeMessage(CheckTree checkTree)
        {
            var checkTreeMsg = new CheckTreeMessage();
            checkTreeMsg.BooleanOperation = (int)checkTree.BooleanOperation;

            if (checkTree.PropertyGuid != null)
            {
                checkTreeMsg.CurProperty = mCurDlogObject.GetPropertyIndexPlusOne(checkTree.PropertyGuid);
            }

            if (checkTree.SubtreeA != null)
            {
                checkTreeMsg.SubtreeA = CreateCheckTreeMessage(checkTree.SubtreeA);
            }
            
            if (checkTree.SubtreeB != null)
            {
                checkTreeMsg.SubtreeB = CreateCheckTreeMessage(checkTree.SubtreeB);
            }

            return checkTreeMsg;
        }
    }
}