using System;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public class PlayerCharacterHpChangeEvent : GfEvent<int,int>
    {
    }

    public class PlayerCharacterExpChangeEvent : GfEvent<int, int>
    {
    }
    
    public class PlayerCharacterLevelChangeEvent : GfEvent<int,int>
    {
    }
    
    public class PlayerCharacterCoinChangeEvent : GfEvent<int>
    {
    }

    public class PlayerCharacterGemChangeEvent : GfEvent<int>
    {
    }
    
    public class ExitButtonEvent : GfEvent
    {
        
    }
    
    public class UIEventContainer: IDisposable
    {
        public PlayerCharacterHpChangeEvent    OnPlayerCharacterHpChangeEvent    { get; } = new PlayerCharacterHpChangeEvent();
        public PlayerCharacterExpChangeEvent    OnPlayerCharacterExpChangeEvent    { get; } = new PlayerCharacterExpChangeEvent();
        public PlayerCharacterLevelChangeEvent    OnPlayerCharacterLevelChangeEvent    { get; } = new PlayerCharacterLevelChangeEvent();
        public PlayerCharacterCoinChangeEvent    OnPlayerCharacterCoinChangeEvent    { get; } = new PlayerCharacterCoinChangeEvent();
        public PlayerCharacterGemChangeEvent    OnPlayerCharacterGemChangeEvent    { get; } = new PlayerCharacterGemChangeEvent();

        public void Dispose()
        {
            
        }
    }
}