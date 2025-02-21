using cfg;

namespace GameMain.Runtime
{
    public class RoguelikeRoomData
    {
        public RoomConfig Config;
        
        //Combat 类型需要有奖励类型
        public RoomRewardType RewardType;
        public int BossId;

        public RoguelikeRoomData(RoomConfig config)
        {
            Config = config;
        }
    }

    public class ShopItemData
    {
        public int ItemId;
        public RewardType RewardType;
        public int ItemNum;
        public int Cost;

        public ShopItemData(int itemId,RewardType rewardType,int itemNum,int cost)
        {
            ItemId = itemId;
            RewardType = rewardType;
            ItemNum = itemNum;
            Cost = cost;
        }
    }
}