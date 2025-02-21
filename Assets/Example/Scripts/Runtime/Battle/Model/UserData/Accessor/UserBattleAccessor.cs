using System.Collections.Generic;
using Akari.GfCore;

namespace GameMain.Runtime
{
    //副本战斗中的金币，祝福，遗物
    //只在副本中存在 无法带出
    //这里记录的数据是为了还原战斗
    public class UserBattleAccessor
    {
        private UserRoguelike userRoguelike;
        public UserRoguelike UserRoguelike => userRoguelike;
        public int Coin => userRoguelike.Coin;
        public bool HasBattle => userRoguelike.CurRoomId != 0;
        
        public UserBattleAccessor(UserData userData)
        {
            userRoguelike = userData.Roguelike ?? new UserRoguelike();
        }

        public bool IsEnoughCoin(int num)
        {
            return userRoguelike.Coin >= num;
        }

        public void AddCoin(int num)
        {
            userRoguelike.Coin += num;
            EventManager.Instance.UIEvent.OnPlayerCharacterCoinChangeEvent.Invoke(userRoguelike.Coin);
        }
        
        public void SubtractCoin(int num)
        {
            userRoguelike.Coin -= num;
            EventManager.Instance.UIEvent.OnPlayerCharacterCoinChangeEvent.Invoke(userRoguelike.Coin);
        }
        
        public void ClearBattle()
        {
            userRoguelike = new UserRoguelike();
        }

        public void SaveRoomInfo(int curRoomId, int passRoomNum,List<int> passRooms,Dictionary<int, int> roomLastAppearedRoomNumMap)
        {
            userRoguelike.CurRoomId = curRoomId;
            userRoguelike.PassRoomNum = passRoomNum;
            userRoguelike.PassRooms = passRooms;
            userRoguelike.RoomLastAppearedRoomNumMap = roomLastAppearedRoomNumMap;
        }

        public void AddBlessing(int id)
        {
            if (userRoguelike.Blessings.Contains(id))
            {
                GfLog.Error($"添加了重复的Blessings{id}");
            }
            else
            {
                userRoguelike.Blessings.Add(id);
            }
        }
    }

    public class UserRoguelike
    {
        public int Coin;
        public List<int> Blessings = new List<int>();
        public List<int> Artifacts = new List<int>();
        
        //room
        public int CurRoomId;
        public int PassRoomNum;
        public List<int> PassRooms;
        public Dictionary<int, int> RoomLastAppearedRoomNumMap;
    }
}