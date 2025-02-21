using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtSelectorNodeData : BtNodeData
    {
        public bool IsRandom;

        public BtSelectorNodeData(int[] childrenIndex,bool isRandom)
        {
            ChildrenIndex = childrenIndex;
            IsRandom = isRandom;
        }

        public override Node CreateNode()
        {
            return IsRandom ? new RandomSelector(CreateChildren()) : new Selector(CreateChildren());
        }
    }
    
    public sealed class BtSelectorNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new GfBtSelectorMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (GfBtSelectorMessage)message;
            return new BtSelectorNodeData(m.Composite.Children.ToArray(), m.Random);
        }
    }
}