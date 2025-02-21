using Akari.GfGame;

namespace GameMain.Runtime
{
    public sealed class BattleTime
    {
        public float   ElapsedTimeNeverStop              { get; private set; }//没有暂停的经过时间
        public float   PausedTime                        { get; private set; }//暂停时间
        public float   ElapsedTimeExcludingPause { get; private set; }//出开暂停时间的经过时间
        public bool IsPaused = true;                       
        
        public void OnUpdate(float deltaTime)
        {
            ElapsedTimeNeverStop += deltaTime;
            
            if (IsPaused)
            {
                PausedTime += deltaTime;
            }

            ElapsedTimeExcludingPause = ElapsedTimeNeverStop - PausedTime;
        }
    }
}