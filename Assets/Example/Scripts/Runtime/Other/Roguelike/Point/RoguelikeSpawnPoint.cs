using System;
using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfUnity;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 用于生成敌人
    /// </summary>
    public class RoguelikeSpawnPoint : MonoBehaviour
    {
        [SerializeField] private List<RoguelikeSpawnInfo> spawnInfos = new List<RoguelikeSpawnInfo>();
        
        [Serializable]
        public class RoguelikeSpawnInfo
        {
            public int WaveId;
            public int Id;//为0则从候选列表SpawnCandidates中随机选择
        }

        public async UniTask<GfEntity> Spawn(RoguelikeRoomView roomView, int waveId)
        {
            RoguelikeSpawnInfo curRoguelikeSpawnInfo = null;
            for (int i = 0; i < spawnInfos.Count; i++)
            {
                if (spawnInfos[i].WaveId == waveId)
                {
                    curRoguelikeSpawnInfo = spawnInfos[i];
                    break;
                }
            }

            if (curRoguelikeSpawnInfo == null)
            {
                return null;
            }

            int id;
            int level = 10;

            if (curRoguelikeSpawnInfo.Id == 0)
            {
                //从候选列表随机选择
                var spawnCandidate = roomView.GetRandomSpawnCandidate();
                if (spawnCandidate == null)
                {
                    return null;
                }
                
                id = spawnCandidate.Id;
            }
            else
            {
                id = curRoguelikeSpawnInfo.Id;
            }
            
            var entity = await BattleAdmin.Factory.Character.CreateEnemyCharacter(
                new GameCharacterModel(new EnemyData(id,level)), transform.position.ToGfFloat3(), GfQuaternion.Identity, "enemyKey");
            return entity;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var position = transform.position;
            var curX = position.x;
            //var curY = position.y;
            var curZ = position.z;

            var x = 1;
            var y = 1;
            var p0 = new Vector2(0.5f * x, 0.5f * y);
            var p1 = new Vector2(0.5f * x, -0.5f * y);
            var p2 = new Vector2(-0.5f * x, -0.5f * y);
            var p3 = new Vector2(-0.5f * x, 0.5f * y);
            Gizmos.DrawLine(new Vector3(p0.x+curX, 0.1f, p0.y+curZ), new Vector3(p1.x+curX, 0.1f, p1.y+curZ));
            Gizmos.DrawLine(new Vector3(p1.x+curX, 0.1f, p1.y+curZ), new Vector3(p2.x+curX, 0.1f, p2.y+curZ));
            Gizmos.DrawLine(new Vector3(p2.x+curX, 0.1f, p2.y+curZ), new Vector3(p3.x+curX, 0.1f, p3.y+curZ));
            Gizmos.DrawLine(new Vector3(p3.x+curX, 0.1f, p3.y+curZ), new Vector3(p0.x+curX, 0.1f, p0.y+curZ));
        }
    }
}