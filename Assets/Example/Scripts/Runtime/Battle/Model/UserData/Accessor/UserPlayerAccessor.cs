
using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public class UserPlayerAccessor
    {
        private readonly UserPlayer _userPlayer;
        public UserPlayer UserPlayer => _userPlayer;
        public int Gem => _userPlayer.Gem;
       
        public UserPlayerAccessor(UserData userData)
        {
            _userPlayer = userData.Player ?? new UserPlayer();
        }

        public void AddGem(int num)
        {
            _userPlayer.Gem += num;
            EventManager.Instance.UIEvent.OnPlayerCharacterGemChangeEvent.Invoke(_userPlayer.Gem);
        }
        
        public void SubtractGem(int num)
        {
            _userPlayer.Gem -= num;
            EventManager.Instance.UIEvent.OnPlayerCharacterGemChangeEvent.Invoke(_userPlayer.Gem);
        }

        public bool OpenChest(int chestId)
        {
            if (!HasGotChest(chestId))
            {
                _userPlayer.GetChestIds.Add(chestId);
                return true;
            }
            
            GfLog.Warn($"Has Got Chest {chestId}");
            return false;
        }

        public bool HasGotChest(int chestId)
        {
            return _userPlayer.GetChestIds.Contains(chestId);
        }
    }
    
    public class UserPlayer
    {
        public int Gem;
        
        public List<int> GetChestIds = new List<int>();//获得的宝箱id
    }
    //当前战斗数据,通关,退出或者重置副本清空.
}