using Akari.GfCore;

namespace GameMain.Runtime
{
    public sealed class AttackDefinitionCollisionData
    {
        public GfFloat2 Offset { get; }
        public GfFloat2 Extents { get; }

        public AttackDefinitionCollisionData(GfFloat2 offset, GfFloat2 extents)
        {
            Extents = extents;
            Offset = offset;
        }

        public override string ToString()
        {
            return $"{nameof(Offset)}: {Offset}, {nameof(Extents)}: {Extents}";
        }
    }

    //public sealed class AttackDefinitionCollisionDataFactory : IGfPbFactory
    //{
    //    public IMessage CreateMessage()
    //    {
    //        return new AttackDefinitionCollisionMessage();
    //    }

    //    public object CreateInstance(IMessage message)
    //    {
    //        var m = (AttackDefinitionCollisionMessage)message;
    //        var i = new AttackDefinitionCollisionData(m.Group.ToGfString(),
    //                                                  m.Offset.ToGfFloat2(),
    //                                                  m.Extents.ToGfFloat2());
    //        return i;
    //    }
    //}
}
