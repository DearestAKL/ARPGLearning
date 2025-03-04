using cfg;

namespace GameMain.Runtime
{
    public class NpcData
    {
        public NpcConfig Config{ get; private set; }
        
        public float Hp { get; private set; }
        public float Attack{ get; private set; }
        public float Defense{ get; private set; }

        public NpcData(int npcId)
        {
            Config = LubanManager.Instance.Tables.TbNpc.Get(npcId);

            Hp = 100;
            Attack = 10;
            Defense = 10;
        }
    }
}