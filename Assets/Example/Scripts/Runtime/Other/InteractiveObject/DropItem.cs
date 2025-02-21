using Akari.GfCore;
using Akari.GfUnity;
using cfg;

namespace GameMain.Runtime
{
    /// <summary>
    /// 掉落物
    /// 捡起后回收
    /// </summary>
    public class DropItem : AClickInteractiveObject
    {
        private RewardType _rewardType;
        private int _itemNum;
        private int _id;
        public override string InteractionTips  => "DropItem";
        
        protected override void OnInteracting(CharacterInteractive characterInteractive)
        {
            GfLog.Debug("GetItem");
            
            //TODO: 根据type num _id 获取道具
            
            characterInteractive.RemoveInteractiveObject(this);
            ReturnToPool();
        }

        public void SetData(RewardType rewardType, int itemNum,int id = 0)
        {
            _rewardType = rewardType;
            _id = id;
            _itemNum = itemNum;
        }

        public void ReturnToPool()
        {
            ResetStateData();
            GfPrefabPool.Return(this);
        }
    }
}