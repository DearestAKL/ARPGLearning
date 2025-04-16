using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using UnityEngine;
using UnityEngine.AI;

namespace GameMain.Runtime
{
        
    /// <summary>
    /// 依赖于Unity NavMesh的寻路组件
    /// 根据目标点 调整的移动方向
    /// </summary>
    public class BattleCharacterNavMeshComponent : AGfGameComponent<BattleCharacterNavMeshComponent>
    {
        private GfComponentCache<BattleCharacterAccessorComponent> _accessorCache;
        private BattleCharacterAccessorComponent Accessor => Entity.GetComponent(ref _accessorCache);
        
        private Vector3 CurPos => Entity.Transform.Position.ToVector3();
        
        private Vector3 _targetPos;
        private Vector3[] _pathCorners;
        private int _currentPathIndex;

        public bool IsFinish { get; private set; }

#if UNITY_EDITOR
        private GizmosData _gizmosData;
#endif

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (!Accessor.Condition.IsMoving || _pathCorners.IsEmpty() || _currentPathIndex >= _pathCorners.Length)
            {
#if UNITY_EDITOR
                if (_gizmosData != null)
                {
                    _gizmosData.SetValid(false);
                    _gizmosData = null;
                }
#endif
                return;
            }

            if (_currentPathIndex >= _pathCorners.Length)
            {
                //已经完成路径
                IsFinish = true;
                return;
            }
            // 获取当前目标点
            Vector3 targetPosition = _pathCorners[_currentPathIndex];

            // 如果到达当前目标点，移动到下一个点
            if (Vector3.Distance(CurPos, targetPosition) < 0.1f)
            {
                _currentPathIndex++;
                Accessor.Condition.MoveDirection = GfFloat2.Zero;
            }
            else
            {
                //设置当前移动方向
                Accessor.Condition.MoveDirection = (targetPosition - CurPos).ToGfFloat3().ToXZFloat2().Normalized;
            }
        }
        
        private Vector3[] GetPath(Vector3 startPos, Vector3 targetPos,int areaMask = -1)
        {
            if (!NavMesh.SamplePosition(startPos, out var ownNavMeshHit, 5f, areaMask))
            {
                GfLog.Warn($"[导航] 角色自身不在导航网格内");
                return null;
            }

            if (!NavMesh.SamplePosition(targetPos, out var targetNavMeshHit, 5f, areaMask))
            {
                GfLog.Warn($"[导航] 目标位置不在导航网格内");
                return null;
            }
            var currentDestination = targetNavMeshHit.position;

            NavMeshPath navMeshPath = new NavMeshPath();
            NavMesh.CalculatePath(ownNavMeshHit.position, currentDestination, areaMask, navMeshPath);
            if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
            {
                GfLog.Warn("[导航] 导航路径状态 Invalid");
                return null;
            }

            return navMeshPath.corners;
        }
        
        public GfFloat3 GetRandomPoint(float range,int areaMask = -1)
        {
            Vector3 randomDirection = Random.insideUnitSphere * range;  // 以主角为中心的随机点
            randomDirection += CurPos; // 偏移到主角附近

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, range, areaMask))
            {
                return hit.position.ToGfFloat3(); // 找到可行走点
            }

            return CurPos.ToGfFloat3(); // 若找不到有效点，则返回主角当前位置
        }

        public void SetTargetPos(GfFloat3 targetPos)
        {
            _targetPos = targetPos.ToVector3();
            //计算最新的路径点信息
            _pathCorners = GetPath(CurPos, _targetPos);
            _currentPathIndex = 0;
            IsFinish = false;
#if UNITY_EDITOR
            _gizmosData?.SetValid(false);
            _gizmosData = GizmosData.CreateLinesGizmosData(Accessor.Entity.Transform, _pathCorners);
            GizmosManager.Instance.AddGizmosData(_gizmosData);
#endif
        }
    }
}