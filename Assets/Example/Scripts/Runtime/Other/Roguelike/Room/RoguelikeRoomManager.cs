using System;
using System.Collections.Generic;
using System.Linq;
using Akari.GfCore;
using cfg;
using Cysharp.Threading.Tasks;

namespace GameMain.Runtime
{
    /// <summary>
    /// Roguelike房间管理系统
    /// </summary>
    public class RoguelikeRoomManager: GfSingleton<RoguelikeRoomManager>
    {
        private readonly int BossRoomNum = 16;
        private readonly int MiniBossRoomNum = 8;
        
        private int _passRoomNum;
        private List<int> _passRooms = new List<int>();
        private Dictionary<int, int> _roomLastAppearedRoomNumMap = new Dictionary<int, int>();

        private ARoguelikeRoom _curRoom;
        private RoguelikeRoomData curRoguelikeRoomData;
        private RoomConfig[] _pendingRooms;//待定的房间
        
        private readonly RoomRewardType[] RoomRewardTypes = new[]
        {
            RoomRewardType.Blessing,
            RoomRewardType.Coin,
            RoomRewardType.Gem,
        };
        
        private readonly Dictionary<RoomType, Type> RoomTypeMap = new Dictionary<RoomType, Type>()
        {
            { RoomType.Init, typeof(NormalRoom) },
            { RoomType.Random, typeof(NormalRoom) },
            { RoomType.Heal, typeof(NormalRoom) },
            { RoomType.Shop, typeof(ShopRoom) },
            { RoomType.Upgrade, typeof(NormalRoom) },
            { RoomType.ProMiniBoss, typeof(NormalRoom) },
            { RoomType.MiniBoss, typeof(BossRoom) },
            { RoomType.ProBoss, typeof(NormalRoom) },
            { RoomType.Boss, typeof(BossRoom) },
            { RoomType.Combat, typeof(CombatRoom) },
        };

        private IGfRandomGenerator Random => BattleAdmin.RandomGenerator;
        
        public void Init(RoomConfig[] configs)
        {
            _pendingRooms = configs;
        }
        
        public void Start()
        {
            for (int i = 0; i < _pendingRooms.Length; i++)
            {
                if (_pendingRooms[i].Type == RoomType.Init)
                {
                    curRoguelikeRoomData = new RoguelikeRoomData(_pendingRooms[i]);
                    break;
                }
            }

            CreateRoom(curRoguelikeRoomData);
        }

        //重新载入时调用
        public void LoadRoom(UserRoguelike userRoguelike)
        {
            _passRoomNum = userRoguelike.PassRoomNum;
            _passRooms = userRoguelike.PassRooms;
            _roomLastAppearedRoomNumMap = userRoguelike.RoomLastAppearedRoomNumMap;
            
            for (int i = 0; i < _pendingRooms.Length; i++)
            {
                if (_pendingRooms[i].Id == userRoguelike.CurRoomId)
                {
                    curRoguelikeRoomData = new RoguelikeRoomData(_pendingRooms[i]);
                    break;
                }
            }
            CreateRoom(curRoguelikeRoomData);
        }

        private async void CreateRoom(RoguelikeRoomData roguelikeRoomData)
        {
            curRoguelikeRoomData = roguelikeRoomData;
            
            await UIHelper.StartLoading();

            var roomView = await AssetManager.Instance.Instantiate<RoguelikeRoomView>(AssetPathHelper.GetRoomPath(curRoguelikeRoomData.Config.AssetName));
            if (RoomTypeMap.TryGetValue(curRoguelikeRoomData.Config.Type, out var type))
            {
                _curRoom = (ARoguelikeRoom)Activator.CreateInstance(type);
                _curRoom.OnInit(curRoguelikeRoomData, roomView);
                
                _curRoom.OnEnter();
            }

            await UniTask.WaitForFixedUpdate();
            
            UIHelper.EndLoading();
        }
        public void CompleteCurRoom()
        {
            _curRoom.OnComplete();
            if (_passRoomNum >= BossRoomNum)
            {
                //正常通关
                UserDataManager.Instance.Battle.ClearBattle();
                Exit();
                return;
            }
            
            //创建next room door
            var nextRoomData = CreateNextRoomData();
            _curRoom.CreateDoors(nextRoomData);
        }
        
        public void EnterNextRoom(RoguelikeRoomData nextRoguelikeRoom)
        {
            _passRoomNum++;
            _passRooms.Add(curRoguelikeRoomData.Config.Id);
            
            _curRoom.OnExit();
            
            CreateRoom(nextRoguelikeRoom);
        }
        
        private List<RoguelikeRoomData> CreateNextRoomData()
        {
            int nextRoomNum = (_passRoomNum == MiniBossRoomNum - 2 || _passRoomNum == MiniBossRoomNum - 1 || _passRoomNum == BossRoomNum - 1) ? 1 : Random.Range(2, 4);
            List<RoguelikeRoomData> nextRooms = new List<RoguelikeRoomData>(nextRoomNum);

            //筛选备选房间
            List<int> alternativeRoomIndexList = new List<int>();//备选房间
            int probabilitySum = 0;
            for (int i = 0; i < _pendingRooms.Length; i++)
            {
                var pendingRoom = _pendingRooms[i];
                if (_passRoomNum < pendingRoom.StartNum)
                {
                    continue;
                }

                if (pendingRoom.IntervalNum > 0)
                {
                    var curIntervalNum = GetAppearedIntervalNum(pendingRoom.Id);
                    if (curIntervalNum > 0 && curIntervalNum <= pendingRoom.IntervalNum)
                    {
                        continue;
                    }
                }

                if (pendingRoom.IsInevitable)
                {
                    nextRooms.Add(new RoguelikeRoomData(pendingRoom));
                    if (nextRooms.Count >= nextRoomNum)
                    {
                        break;
                    }
                    continue;
                }
                
                probabilitySum += pendingRoom.Probability;
                alternativeRoomIndexList.Add(i);
            }

            while (nextRoomNum > nextRooms.Count)
            {
                int randomRoomIndex = 0;
                var randomValue = Random.Range(1, probabilitySum);
                for (int i = 0; i < alternativeRoomIndexList.Count; i++)
                {
                    var alternativeRoom = _pendingRooms[alternativeRoomIndexList[i]];
                    if (alternativeRoom.Probability >= randomValue)
                    {
                        randomRoomIndex = alternativeRoomIndexList[i];
                        break;
                    }
                    randomValue -= alternativeRoom.Probability;
                }

                var nextRoom = _pendingRooms[randomRoomIndex];
                nextRooms.Add(new RoguelikeRoomData(nextRoom));
                
                if (nextRoom.IntervalNum != 0)
                {
                    probabilitySum -= nextRoom.Probability;
                    alternativeRoomIndexList.Remove(randomRoomIndex);
                }
            }

            
            var rewardTypeIndexList = Enumerable.Range(0, RoomRewardTypes.Length).ToList();
            foreach (var nextRoom in nextRooms)
            {
                _roomLastAppearedRoomNumMap[nextRoom.Config.Id] = _passRoomNum;
                
                if (nextRoom.Config.Type == RoomType.Combat)
                {
                    //Combat类型随机奖励
                    var rewardTypeIndex = rewardTypeIndexList[Random.Range(0, rewardTypeIndexList.Count)];
                    var rewardType = RoomRewardTypes[rewardTypeIndex];
                    
                    nextRoom.RewardType = rewardType;
                    //奖励是否提升为sp奖励

                    rewardTypeIndexList.Remove(rewardTypeIndex);
                }
                else if (nextRoom.Config.Type == RoomType.Boss || nextRoom.Config.Type == RoomType.MiniBoss)
                {
                    nextRoom.BossId = 2001;
                }
            }

            return nextRooms;
        }

        private int GetAppearedIntervalNum(int id)
        {
            if (_roomLastAppearedRoomNumMap.TryGetValue(id, out var lastAppearedRoomNum))
            {
                return _passRoomNum - lastAppearedRoomNum;
            }
            return -1;
        }

        public void Exit()
        {
            //退出Room 
            _curRoom.OnExit();
            //返回大厅
            EventManager.Instance.BattleEvent.OnExitRoomEvent.Invoke();
            
            Dispose();
        }
        public void Save()
        {
            UserDataManager.Instance.Battle.SaveRoomInfo(curRoguelikeRoomData.Config.Id,_passRoomNum,_passRooms,_roomLastAppearedRoomNumMap);
        }
    }
}