using System.Collections.Generic;
using System.IO;
using Akari.GfGame;
using Akari.GfUnityEditor;
using GameMain.Runtime;
using UnityEditor;
using UnityEngine;

namespace TreeDesigner
{
    public static class GfEBtSerializer
    {
        public static void Serialize(BaseTree behaviourTree)
        {
            behaviourTree.InitTree();

            var container = GfEBtContainer.Creat(behaviourTree);
            var path = $"{GfEditorSettings.Instance.BehaviourTreeOutputDirectory}/{behaviourTree.name}{GfResourceFileNameSuffix.BehaviourTreeFileNameSuffix}.bytes";
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                //写入头文件
                stream.WritePbFileHeader(GfResourceFileHeaderTypes.BehaviourTreeFileType);
                var header = new GfBtFileHeaderMessage();
                header.StartNodeIndex = container.StartNodeIndex;
                foreach (var node in container.Nodes)
                {
                    header.NodeTypes.Add(node.GetPbTypeId());
                }
                foreach (var property in container.Properties)
                {
                    header.PropertyTypes.Add(property.GetPbTypeId());
                }

                foreach (var propertyEdge in container.PropertyEdges)
                {
                    header.PropertyEdges.Add(new GfBtPropertyEdgeMessage()
                        { PropertyIndex = propertyEdge.PropertyIndex, NodeIndex = propertyEdge.NodeIndex });
                }

                stream.WritePbMessage(header);
                
                //写入Nodes
                for (var i = 0; i < container.Nodes.Count; i++)
                {
                    stream.WritePbMessage(container.Nodes[i].Serialize());
                }
                
                //写入Properties
                for (var i = 0; i < container.Properties.Count; i++)
                {
                    stream.WritePbMessage(container.Properties[i].Serialize());
                }
                
                stream.Flush();
            }
            
            behaviourTree.SerializerTemporaryNodes = null;
            behaviourTree.SerializerTemporaryProperties = null;
            behaviourTree.DisposeTree();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            Debug.Log($"serialize tree {behaviourTree.name} complete");
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }


    public class GfEBtContainer
    {
        public class PropertyEdge
        {
            public int PropertyIndex;
            public int NodeIndex;
        }

        public int StartNodeIndex;
        
        public List<PropertyEdge> PropertyEdges = new List<PropertyEdge>();
        
        public List<BaseNode> Nodes = new List<BaseNode>();
        public List<BaseExposedProperty> Properties = new List<BaseExposedProperty>();

        public static GfEBtContainer Creat(BaseTree behaviourTree)
        {
            var container = new GfEBtContainer();
            
            behaviourTree.SerializerTemporaryNodes = container.Nodes;
            behaviourTree.SerializerTemporaryProperties = container.Properties;
            
            //记录黑板数据
            for (int i = 0; i < behaviourTree.ExposedProperties.Count; i++)
            {
                var property = behaviourTree.ExposedProperties[i];
                container.Properties.Add(property);
            }
            
            //记录node数据
            for (int i = 0; i < behaviourTree.Nodes.Count; i++)
            {
                var node = behaviourTree.Nodes[i];
                if (node is ExposedPropertyNode exposedPropertyNode) 
                {
                    if (exposedPropertyNode.NodeType == ExposedPropertyNodeType.Get)
                    {
                        continue;
                    }
                    if (exposedPropertyNode.NodeType == ExposedPropertyNodeType.Set)
                    {
                        //记录setProperty 的 PropertyIndex;
                        var propertyEdge = new PropertyEdge();
                        propertyEdge.PropertyIndex = container.Properties.IndexOf(exposedPropertyNode.ExposedProperty);
                        propertyEdge.NodeIndex = container.Nodes.Count;
                        container.PropertyEdges.Add(propertyEdge);
                    }
                }
                container.Nodes.Add(node);
            }

            for (int i = 0; i < behaviourTree.PropertyEdges.Count; i++)
            {
                var propertyEdge = new PropertyEdge();
                if (behaviourTree.PropertyEdges[i].StartNode is ExposedPropertyNode exposedPropertyNode)
                {
                    var exposedProperty = exposedPropertyNode.ExposedProperty;
                    propertyEdge.PropertyIndex = container.Properties.IndexOf(exposedProperty);
                }

                var node = behaviourTree.PropertyEdges[i].EndNode;
                propertyEdge.NodeIndex = container.Nodes.IndexOf(node);
                
                container.PropertyEdges.Add(propertyEdge);
            }
            
            container.StartNodeIndex =  container.Nodes.IndexOf(behaviourTree.Root);
            return container;
        }
    }
}