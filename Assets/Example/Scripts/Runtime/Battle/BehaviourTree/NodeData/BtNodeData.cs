using System.Collections.Generic;
using Akari.GfCore;
using NPBehave;

///构建NPBehave Node的数据结构
namespace GameMain.Runtime
{
    public abstract class BtNodeData
    { 
        private BtNodeData child;
        private List<BtNodeData> children;
        
        public string PropertyKey;
        public int ChildIndex = -1;
        public int[] ChildrenIndex;
        
        public virtual void Init(List<BtNodeData> children)
        {
            this.children = children;
        }
        
        public virtual void Init(BtNodeData child)
        {
            this.child = child;
        }

        public abstract Node CreateNode();

        protected Node CreateChild()
        {
           return child.CreateNode();
        }

        protected Node[] CreateChildren()
        {
            var nodes = GfArrayUtility.Create<Node>(children.Count);
            for (int i = 0; i < children.Count; i++)
            {
                nodes[i] = children[i].CreateNode();
            }

            return nodes;
        }
    }
}