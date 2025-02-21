using Akari.GfCore;
using cfg;

namespace GameMain.Runtime
{
    /// <summary>
    /// Roguelike传送门
    /// 交互后重置状态数据
    /// </summary>
    public class RoguelikePortal : AClickInteractiveObject
    {
        public override string InteractionTips => "Enter Roguelike Portal";

        protected override async void OnInteracting(CharacterInteractive characterInteractive)
        {
            GfLog.Debug("Enter RoguelikePortal");
            if (UserDataManager.Instance.Battle.HasBattle)
            {
                await UIHelper.OpenCommonMessageDialog("继续上次冒险", () =>
                {
                    //继续
                    characterInteractive.RemoveInteractiveObject(this);
                    EnterBattleMap();
                },
                () =>
                {
                    //重新开始 需要ClearBattle
                    characterInteractive.RemoveInteractiveObject(this);
                    UserDataManager.Instance.Battle.ClearBattle();
                    EnterBattleMap();
                },"继续","重新开始");
            }
            else
            {
                await UIHelper.OpenCommonMessageDialog("确认开始冒险", () =>
                {
                    characterInteractive.RemoveInteractiveObject(this);
                    EnterBattleMap();
                });
            }

            ResetStateData();
        }

        
        private void EnterBattleMap()
        {
            BattleAdmin.Player.Transform.RecordWorldPosition();
            
            //创建战斗房间
            RoomConfig[] pendingRooms = LubanManager.Instance.Tables.TbRoom.DataList.ToArray();
            pendingRooms.Sort((a, b) => b.Priority.CompareTo(a.Priority));

            RoguelikeRoomManager.CreateInstance();
            RoguelikeRoomManager.Instance.Init(pendingRooms);
            
            if (UserDataManager.Instance.Battle.HasBattle)
            {
                RoguelikeRoomManager.Instance.LoadRoom(UserDataManager.Instance.Battle.UserRoguelike);
            }
            else
            {
                RoguelikeRoomManager.Instance.Start();
            }
            
            EventManager.Instance.BattleEvent.OnEnterRoomEvent.Invoke();
        }
    }
}