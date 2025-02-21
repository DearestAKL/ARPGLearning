using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;
using Google.Protobuf;

namespace GameMain.Runtime
{
    public class GfBtResource : IGfResource<GfBtData>
    {
        public GfBtFileHeaderMessage Header { get; }
        public IMessage[] NodeMessages { get; }
        public IMessage[] PropertyMessages { get; }

        public GfBtResource(GfBtFileHeaderMessage header, IMessage[] nodeMessages, IMessage[] propertyMessages)
        {
            Header = header;
            NodeMessages = nodeMessages;
            PropertyMessages = propertyMessages;
        }

        public GfBtData CreateData()
        {
            //property
            var propertyDatas =  GfArrayUtility.Create<BtPropertyData>(Header.PropertyTypes.Count);
            for (var i = 0; i < Header.PropertyTypes.Count; i++)
            {
                var propertyData = PropertyMessages[i].CreatePbInstanceByType<BtPropertyData>(Header.PropertyTypes[i]);
                propertyDatas[i] = propertyData;
            }
            
            //Node
            var nodeDatas = GfArrayUtility.Create<BtNodeData>(Header.NodeTypes.Count);
            for (var i = 0; i < Header.NodeTypes.Count; i++)
            {
                var nodeData = NodeMessages[i].CreatePbInstanceByType<BtNodeData>(Header.NodeTypes[i]);
                nodeDatas[i] = nodeData;
            }
            
            for (int i = 0; i < nodeDatas.Length; i++)
            {
                var nodeData = nodeDatas[i];
                List<BtNodeData> children = new List<BtNodeData>();

                if (nodeData.ChildrenIndex != null)
                {
                    foreach (var childIndex in nodeData.ChildrenIndex)
                    {
                        children.Add(nodeDatas[childIndex]);
                    }
                    nodeData.Init(children);
                }

                if(nodeData.ChildIndex >= 0)
                {
                    nodeData.Init(nodeDatas[nodeData.ChildIndex]);
                }
                nodeDatas[i].Init(children);
            }

            for (int i = 0; i < Header.PropertyEdges.Count; i++)
            {
                var propertyEdge = Header.PropertyEdges[i];
                nodeDatas[propertyEdge.NodeIndex].PropertyKey = propertyDatas[propertyEdge.PropertyIndex].Key;
            }

            return new GfBtData(Header.StartNodeIndex, nodeDatas, propertyDatas);
        }
    }
}