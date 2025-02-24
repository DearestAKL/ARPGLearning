using cfg;

namespace GameMain.Runtime
{
    public class SummonerData
    {
        public SummonerConfig Config{ get; private set; }
        
        public float Hp { get; private set; }
        public float Attack{ get; private set; }
        public float Defense{ get; private set; }
        
        public int Level { get; private set; }

        public SummonerData(int summonerId, int level)
        {
            Level = level;
            Config = LubanManager.Instance.Tables.TbSummoner.Get(summonerId);

            Hp = 100;
            Attack = 10;
            Defense = 10;
        }
    }
}