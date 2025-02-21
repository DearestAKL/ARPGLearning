using Akari.GfGame;
using Google.Protobuf;
using NPBehave;

namespace GameMain.Runtime
{
    public class BtSequencerNodeData : BtNodeData
    {
        public bool IsRandom;

        public BtSequencerNodeData(int[] childrenIndex,bool isRandom)
        {
            ChildrenIndex = childrenIndex;
            IsRandom = isRandom;
        }

        public override Node CreateNode()
        {
            return IsRandom ? new RandomSequence(CreateChildren()) : new Sequence(CreateChildren());
        }
    }
    
    public sealed class BtSequencerNodeFactory : IGfPbFactory
    {
        public IMessage CreateMessage()
        {
            return new GfBtSequencerMessage();
        }
    
        public object CreateInstance(IMessage message)
        {
            var m = (GfBtSequencerMessage)message;
            return new BtSequencerNodeData(m.Composite.Children.ToArray(), m.Random);
        }
    }
}