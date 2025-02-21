using Akari.GfCore;
using Akari.GfUnity;
using cfg;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// Roguelike门
    /// 交互后重置状态数据
    /// </summary>
    public class RoguelikeDoor : AClickInteractiveObject
    {
        public override string InteractionTips => "Enter";

        private RoguelikeRoomData roguelikeRoomData;
        private UINameTips _tips;

        protected override void OnInteracting(CharacterInteractive characterInteractive)
        {
            GfLog.Debug("Enter RoguelikeDoor");

            EnterRoguelikeRoom();
            
            characterInteractive.RemoveInteractiveObject(this);
        }

        private void Update()
        {
            if (_tips != null && _tips.gameObject.activeSelf)
            {
                _tips.transform.localPosition = UIHelper.WorldPositionToBattleUI(transform.position, new Vector2(0, 150f));
            }
        }

        private void OnDestroy()
        {
            if (_tips != null)
            {
                Destroy(_tips.gameObject); 
            }
        }

        private void EnterRoguelikeRoom()
        {
            RoguelikeRoomManager.Instance.EnterNextRoom(roguelikeRoomData);
        }

        public async void SetData(RoguelikeRoomData roguelikeRoomData)
        {
            this.roguelikeRoomData = roguelikeRoomData;
            
            if (_tips == null)
            {
                _tips = await UIManager.Instance.Factory.GetUITipsItem<UINameTips>("UINameTips");
            }

            _tips.gameObject.SetActive(true);
            if (roguelikeRoomData.Config.Type == RoomType.Combat)
            {
                _tips.UpdateView($"{roguelikeRoomData.Config.Name}:{roguelikeRoomData.RewardType}");
            }
            else
            {
                _tips.UpdateView(roguelikeRoomData.Config.Name);
            }
        }

        //退出room时 回收
        public void ReturnToPool()
        {
            ResetStateData();
            
            GfPrefabPool.Return(this);
            roguelikeRoomData = null;
            _tips.gameObject.SetActive(false);
        }
    }
}