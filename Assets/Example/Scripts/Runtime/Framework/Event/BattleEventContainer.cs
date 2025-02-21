using System;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public class BattleStartEvent : GfEvent
    {
        
    }
    
    public class CharacterDieEvent : GfEvent<GfEntity>
    {
        
    }
    

    public class EnterRoomEvent : GfEvent
    {
        
    }

    public class ExitRoomEvent : GfEvent
    {
        
    }
    
    public class ChangeCharacterEvent : GfEvent<int,int>
    {
        
    }

    public class BattleEventContainer : IDisposable
    {
        public BattleStartEvent OnBattleStartEvent = new BattleStartEvent();
        
        public CharacterDieEvent OnCharacterDieEvent = new CharacterDieEvent();
        
        public EnterRoomEvent OnEnterRoomEvent = new EnterRoomEvent();
        public ExitRoomEvent OnExitRoomEvent = new ExitRoomEvent();
        public ChangeCharacterEvent OnChangeCharacterEvent = new ChangeCharacterEvent();
        public void Dispose()
        {
            
        }
    }
}