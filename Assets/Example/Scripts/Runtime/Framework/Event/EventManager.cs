using Akari.GfCore;

namespace GameMain.Runtime
{
    public class EventManager : GfSingleton<EventManager>
    {
        //跨业务系统 再使用事件
        
        public BattleEventContainer BattleEvent             { get; private set; }
        public UIEventContainer UIEvent             { get; private set; }
        public PatchEventContainer  PatchEvent             { get; private set; }
        
        protected override void OnCreated()
        {
            BattleEvent = new BattleEventContainer();
            UIEvent = new UIEventContainer();
            PatchEvent = new PatchEventContainer();
        }
        
        protected override void OnDisposed()
        {
            if (BattleEvent != null)
            {
                BattleEvent.Dispose();
                BattleEvent = null;
            }
            
            if (UIEvent != null)
            {
                UIEvent.Dispose();
                UIEvent = null;
            }
            
            if (PatchEvent != null)
            {
                PatchEvent.Dispose();
                PatchEvent = null;
            }
        }
    }
}
