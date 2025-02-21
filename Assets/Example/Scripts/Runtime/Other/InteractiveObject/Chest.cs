using System.Collections.Generic;
using Akari;
using Akari.GfCore;
using Akari.GfUnity;
using cfg;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 宝箱
    /// 一般使用chest point创建,开启后回收
    /// </summary>
    public class Chest : AClickInteractiveObject
    {
        [SerializeField] private Animation animation;
        
        private bool _isShowing = false;
        private ChestConfig _chestConfig;
        public override string InteractionTips => "打开";
        
        protected override async void OnInteracting(CharacterInteractive characterInteractive)
        {
            GfLog.Debug("Open Chest");
            
            // if (!UserDataManager.Instance.Player.OpenChest(_chestConfig.Id))
            // {
            //     //todo: show tips
            //     //已经获得
            //     ResetStateData();
            //     return;
            // }
            
            //获得奖励
            //var items = GetRewards(_chestConfig.Rewards);
            //await UIManager.Instance.OpenUIPanel(UIType.UIRewardDialog, new UIRewardDialog.Params(items));

            characterInteractive.RemoveInteractiveObject(this);
            
            if (animation != null)
            {
                //播放开启动画
                animation.Stop();
                //animation.Play(clip.name);
                animation.Play();
                _isShowing = true;
            }
            else
            {
                //ReturnToPool();
            }
        }

        private void Update()
        {
            if (_isShowing)
            {
                if (!animation.isPlaying)
                {
                    _isShowing = false;
                    
                    //ReturnToPool();
                }
            }
        }

        public void SetData(ChestConfig chestConfig)
        {
            _chestConfig = chestConfig;
        }
        
        public void ReturnToPool()
        {
            ResetStateData();
            GfPrefabPool.Return(this);
        }

        //todo:改完通用接口
        // private List<FancyItemData> GetRewards(RewardConfig[] rewards)
        // {
        //     var items = new List<FancyItemData>();
        //     foreach (var reward in rewards)
        //     {
        //         if (reward.Type == RewardType.Item)
        //         {
        //             UserDataManager.Instance.Item.AddItem(reward.Id, reward.Value);
        //             items.Add(new UIItemData(reward.Id, reward.Value));
        //         }
        //         else if (reward.Type == RewardType.Weapon)
        //         {
        //             var userWeapon = UserDataManager.Instance.Weapon.AddWeapon(reward.Id);
        //             items.Add(new UIWeaponData(userWeapon));
        //         }
        //         else if (reward.Type == RewardType.Armor)
        //         {
        //             var userArmor = UserDataManager.Instance.Armor.AddArmor(reward.Id);
        //             items.Add(new UIArmorData(userArmor));
        //         }
        //     }
        //     return items;
        // }
    }
}