using System;
using System.Collections.Generic;
using System.Linq;
using Akari.GfCore;
using cfg;

namespace GameMain.Runtime
{
    public class UIBlessingChoicesDialog : UIBlessingChoicesDialogSign
    {
        private readonly UIBlessingItem[] _blessingItems = new UIBlessingItem[3];

        private readonly List<int> _commonWeights = new List<int>(){1,1,0};
        private readonly List<int> _rareWeights = new List<int>(){1,2,1};
        private readonly List<int> _epicWeights = new List<int>(){0,0,1};
        
        private readonly List<int> _commonBlessingList = new List<int>();
        private readonly List<int> _rareBlessingList = new List<int>();
        private readonly List<int> _epicBlessingList = new List<int>();
        
        

        private IGfRandomGenerator RandomGenerator => BattleAdmin.RandomGenerator;
         
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent,
            object userData)
        {
            base.OnInit(name, go, parent, userData);
            
            _blessingItems[0] = blessingItem_1;
            _blessingItems[1] = blessingItem_2;
            _blessingItems[2] = blessingItem_3;

            for (int i = 0; i < _blessingItems.Length; i++)
            {
                _blessingItems[i].OnClickEvent.AddListener(OnClickBlessingItem);
            }
            
            //先取出所有Blessing，按照品质分为三个list
            var dataList = LubanManager.Instance.Tables.TbBlessing.DataList;
            foreach (var data in dataList)
            {
                if (data.Quality == 1)
                {
                    _commonBlessingList.Add(data.Id);
                }
                else if(data.Quality == 2)
                {
                    _rareBlessingList.Add(data.Id);
                }
                else
                {
                    _epicBlessingList.Add(data.Id);
                }
            }
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            int rewardQuality = 1;
            
            List<int> userBlessings = UserDataManager.Instance.Battle.UserRoguelike.Blessings;
            List<int> choicesBlessings = new List<int>();
            LotteryBlessing(userBlessings, choicesBlessings, rewardQuality,_blessingItems.Length);
            
            for (int i = 0; i < choicesBlessings.Count; i++)
            {
                var blessing = LubanManager.Instance.Tables.TbBlessing.Get(choicesBlessings[i]);
                _blessingItems[i].gameObject.SetActive(true);
                _blessingItems[i].UpdateView(blessing);
                
            }

            if (choicesBlessings.Count < _blessingItems.Length)
            {
                for (int i = choicesBlessings.Count; i < _blessingItems.Length; i++)
                {
                    _blessingItems[i].gameObject.SetActive(false);
                }
            }
        }
        
        public override void OnClose()
        {
            base.OnClose();
        }

        private void LotteryBlessing(List<int> userBlessings, List<int> choicesBlessings, int rewardQuality,int lotteryTimes)
        {
            //根据奖励品级 权重 筛选出目标祝福 普通(普通1：稀有：1)-高级（普通1：稀有：2 史诗：1）-特级(史诗：1)
            
            //随机出品质
            List<int> targetWeights = rewardQuality switch
            {
                1 => _commonWeights,
                2 => _rareWeights,
                _ => _epicWeights
            };
            List<int> weights = targetWeights.ToList();

            int reLotteryTimes = weights.Count - 1;//重新抽取次数，当目标品质祝福列表没有符合要求祝福时，则消耗抽取次数重新抽取
            while (lotteryTimes > 0)
            {
                //返回的是满足的权重下标，0-普通,1-高级,2-特级
                var lotteryIndex = weights.Lottery(RandomGenerator);
            
                List<int> blessings = lotteryIndex switch
                {
                    0 => _commonBlessingList,
                    1 => _rareBlessingList,
                    _ => _epicBlessingList
                };
            
                //计算差集,得出userBlessings列表和choicesBlessings列表中都未拥有的所有祝福Id,同时
                // 使用 HashSet 进行差集计算
                HashSet<int> difference = new HashSet<int>(blessings);
                difference.ExceptWith(userBlessings);
                difference.ExceptWith(choicesBlessings);
    
                if (difference.Count > 0)
                {
                    // 再从差集列表中随机选择
                    int targetId = difference.ElementAt(RandomGenerator.Range(0, difference.Count));
                    choicesBlessings.Add(targetId);
                    lotteryTimes--;
                }
                else
                {
                    if (reLotteryTimes <= 0)
                    {
                        //没有重新抽取次数了 说明三个品质的祝福池全部抽完了
                        break;
                    }
                    // 没有目标祝福了，将改品质权重降为0
                    weights[lotteryIndex] = 0;
                    // 如果所有权重都为0，根据原始权重,为初始化权重为0的品级设置权重
                    if (weights.All(w => w == 0))
                    {
                        for (int i = 0; i < targetWeights.Count; i++)
                        {
                            if (targetWeights[i] == 0)
                            {
                                weights[i] = 1;
                            }
                        }
                    }

                    reLotteryTimes--;
                }
            }
        }

        private async void OnClickBlessingItem(BlessingConfig blessing)
        {
            UserDataManager.Instance.Battle.AddBlessing(blessing.Id);
            
            //TODO:祝福对战斗角色的影响
            
            // var passiveSkillDefinitionMessage = await PbDefinitionHelper.GetPassiveSkillDefinitionMessage(blessing.PassiveSkillId);
            // BattleAdmin.Player.PassiveSkill.AddPassiveSkill(passiveSkillDefinitionMessage);
            
            Close();
        }
    }
}