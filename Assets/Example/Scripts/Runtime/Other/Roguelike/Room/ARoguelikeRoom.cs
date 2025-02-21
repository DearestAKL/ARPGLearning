using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfUnity;
using cfg;
using UnityEngine;

namespace GameMain.Runtime
{
    public abstract class ARoguelikeRoom
    {
        //创建=>进入=>房间事件完成=>创建Door=>退出=>销毁回收
        protected RoguelikeRoomData RoomData;
        protected RoguelikeRoomView RoomView;
        protected readonly GfCompositeDisposable Subscriptions = new();


        public virtual void OnInit(RoguelikeRoomData roguelikeRoomData,RoguelikeRoomView roomView)
        {
            RoomData = roguelikeRoomData;
            RoomView = roomView;
        }

        public virtual void OnEnter()
        {
            BattleAdmin.Player.Transform.SetTransform(GfFloat3.Zero, GfQuaternion.Identity);
        }

        public virtual void OnExit()
        {
            Subscriptions.Dispose();
            //卸载当前房间资源
            Object.Destroy(RoomView.gameObject);
            
            BattleAdmin.Instance.ClearBattle();
            
            RoomView = null;
            RoomData = null;
        }

        public virtual void OnComplete()
        {
            
        }

        // public virtual void OnUpdate()
        // {
        //     
        // }

        public void CreateDoors(List<RoguelikeRoomData> nextRoomDataList)
        {
            RoomView.CreateDoors(nextRoomDataList);
        }

        public void CompleteRoom()
        {
            RoguelikeRoomManager.Instance.CompleteCurRoom();
        }
    }

    public class NormalRoom : ARoguelikeRoom
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CompleteRoom();
        }
    }

    /// <summary>
    /// 商店房
    /// </summary>
    public class ShopRoom : ARoguelikeRoom
    {
        public override void OnEnter()
        {
            base.OnEnter();
            GenerateShop();
        }
        
        private void GenerateShop()
        {
            List<ShopItemData> shopItemDataList = new List<ShopItemData>()
            {
                new ShopItemData(1,RewardType.Coin,1,100),
                new ShopItemData(2,RewardType.Coin,200,100),
                new ShopItemData(3,RewardType.Coin,5,100),
            };
            
            RoomView.CreateShop(shopItemDataList);
        }
    }

    /// <summary>
    /// 战斗房
    /// </summary>
    public class CombatRoom : ARoguelikeRoom
    {
        private List<GfEntity> _curActiveEnemies = new List<GfEntity>();
        private int _curWave;
        private int _curEnemyNum;
        private DropItem _dropItem;

        public override void OnEnter()
        {
            base.OnEnter();
            EventManager.Instance.BattleEvent.OnCharacterDieEvent.GfSubscribe(OnCharacterDieEvent).AddTo(Subscriptions);
            NextWave();
        }

        public override async void OnComplete()
        {
            base.OnComplete();
            
            _dropItem = await AssetManager.Instance.InstantiateFormPool<DropItem>(AssetPathHelper.GetInteractiveObjectPath("Ammo"));

            if (RoomData.RewardType == RoomRewardType.Gem)
            {
                //_dropItem.SetData(RewardType.Gem,100);
                _dropItem.SetData(RewardType.Coin,100);
            }
            else if(RoomData.RewardType == RoomRewardType.Coin)
            {
                _dropItem.SetData(RewardType.Coin,100);
            }
            else if (RoomData.RewardType == RoomRewardType.Blessing)
            {
                //_dropItem.SetData(RewardType.Blessing, 1);
                _dropItem.SetData(RewardType.Coin, 1);
            }
            _dropItem.transform.position = Vector3.zero;
        }

        public override void OnExit()
        {
            //可能是强制退出 敌人并为全部死亡 这里需要主动调用delete
            foreach (var activeEnemy in _curActiveEnemies)
            {
                activeEnemy.Delete();
            }
            
            _curActiveEnemies.Clear();
            _curWave = 0;
            _curEnemyNum = 0;

            if (_dropItem != null && _dropItem.gameObject.activeSelf)
            {
                //没被捡取回收 则需要在退出房间时回收
                GfPrefabPool.Return(_dropItem);
            }
            _dropItem = null;
            
            base.OnExit();
        }

        public void AddEnemy(GfEntity gfEntity)
        {
            _curEnemyNum++;
            _curActiveEnemies.Add(gfEntity);
        }
        
        private void NextWave()
        {
            RoomView.Spawn(this,_curWave);
        }

        private void OnCharacterDieEvent(GfEntity entity)
        {
            bool removeEnemy = _curActiveEnemies.Remove(entity);
            if (!removeEnemy)
            {
                return;
            }
            _curEnemyNum--;

            if (_curEnemyNum == 0)
            {
                //上一波怪全部死亡，应该则出现下一波
                _curWave++;
                NextWave();
            }
        }
        
        //刷怪依赖的数据
        
        //Runtime数据
        //天气
        //时间
        //玩家数量
        //难度
        
        //Config数据
        
        //房间数据
        //隶属于房间的敌人列表 以及生成权重
        
        //地图上配置SpawnPoint
        
        //SpawnInfo
        //刷怪条件,例如概率
        //怪物等级
        //生成点位
        //怪物id 有则优先使用 为0则从房间敌人列表中随机生成
        
    }

    /// <summary>
    /// Boss房
    /// </summary>
    public class BossRoom : ARoguelikeRoom
    {
        private GfEntity _curBoss;
        private DropItem _dropItem;

        public override void OnEnter()
        {
            base.OnEnter();
            EventManager.Instance.BattleEvent.OnCharacterDieEvent.GfSubscribe(OnCharacterDieEvent).AddTo(Subscriptions);
            GenerateBoss();
        }

        public override async void OnComplete()
        {
            base.OnComplete();
            
            _dropItem = await AssetManager.Instance.InstantiateFormPool<DropItem>(AssetPathHelper.GetInteractiveObjectPath("Ammo"));
            //dropItem.SetData(RewardType.Blessing, 1);
            _dropItem.SetData(RewardType.Coin, 1);
            _dropItem.transform.position = new Vector3(0, 0, -5);
        }

        public override void OnExit()
        {
            //可能是强制退出 这里需要主动调用delete
            if (_curBoss != null)
            {
                _curBoss.Delete();
                _curBoss = null;
            }
            
            if (_dropItem != null && _dropItem.gameObject.activeSelf)
            {
                //没被捡取回收 则需要在退出房间时回收
                GfPrefabPool.Return(_dropItem);
            }
            _dropItem = null;
            
            base.OnExit();
        }

        private async void GenerateBoss()
        {
            var id = RoomData.BossId;
            float x = Random.Range(-10f, 10f);
            float y = Random.Range(-10f, 10f);

            _curBoss = await BattleAdmin.Factory.Character.CreateEnemyCharacter(
                new GameCharacterModel(new EnemyData(id,1)), new GfFloat3(x, 0, y), GfQuaternion.Identity, "enemyKey");
        }
        
        private void OnCharacterDieEvent(GfEntity entity)
        {
            if (_curBoss == entity)
            {
                _curBoss = null;
                CompleteRoom();
            }
        }
    }
}