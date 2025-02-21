using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMain.Runtime
{
    /// <summary>
    /// 控制所有RoguelikePointView
    /// </summary>
    public class RoguelikeRoomView : MonoBehaviour
    {
        [SerializeField] private SpawnCandidate[] spawnCandidates;

        [SerializeField] private RoguelikeSpawnPoint[] spawnPoints;
        [SerializeField] private RoguelikeDoorPoint[] doorPoints;
        [SerializeField] private RoguelikeShopItemPoint[] shopItemPoints;

        [Serializable]
        public class SpawnCandidate
        {
            public int Id;
            public int Weight;
        }

        public void CreateDoors(List<RoguelikeRoomData> nextRoomDataList)
        {
            if (doorPoints == null)
            {
                return;
            }

            for (int i = 0; i < nextRoomDataList.Count; i++)
            {
                var roomData = nextRoomDataList[i];
                if (doorPoints.Length > i)
                {
                    doorPoints[i].Create(roomData);
                }
            }
        }

        public void CreateShop(List<ShopItemData> shopItemDataList)
        {
            if (shopItemPoints == null)
            {
                return;
            }
            
            for (int i = 0; i < shopItemDataList.Count; i++)
            {
                var shopItemData = shopItemDataList[i];
                if (shopItemPoints.Length > i)
                {
                    shopItemPoints[i].Create(shopItemData);
                }
            }
        }

        public async void Spawn(CombatRoom combatRoom, int waveId)
        {
            if (spawnPoints == null)
            {
                return;
            }
            
            bool isLastWave = true;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                var entity = await spawnPoints[i].Spawn(this,waveId);
                if (entity != null)
                {
                    isLastWave = false;
                    combatRoom.AddEnemy(entity);
                }
            }

            if (isLastWave)
            {
                combatRoom.CompleteRoom();
            }
        }
        
        public SpawnCandidate GetRandomSpawnCandidate()
        {
            if (spawnCandidates == null)
            {
                return null;
            }
            
            // 计算总权重
            int totalWeight = 0;
            foreach (var candidate in spawnCandidates)
            {
                totalWeight += candidate.Weight;
            }

            // 生成随机数
            int randomWeight = Random.Range(0, totalWeight);

            // 遍历列表，累加权重直到超过随机数
            int cumulativeWeight = 0;
            foreach (var candidate in spawnCandidates)
            {
                cumulativeWeight += candidate.Weight;
                if (randomWeight < cumulativeWeight)
                {
                    return candidate;
                }
            }

            // 如果没有选中任何 SpawnCandidate，返回 null 或者抛出异常
            // 取决于您的需求
            return null;
        }
        
#if UNITY_EDITOR
        public void Preparation()
        {
            doorPoints = transform.GetComponentsInChildren<RoguelikeDoorPoint>();
            spawnPoints = transform.GetComponentsInChildren<RoguelikeSpawnPoint>();
            shopItemPoints = transform.GetComponentsInChildren<RoguelikeShopItemPoint>();
        }
#endif
    }
}