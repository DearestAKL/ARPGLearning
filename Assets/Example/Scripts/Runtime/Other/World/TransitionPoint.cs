using System;
using Akari.GfCore;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public class TransitionPoint : MonoBehaviour
    {
        public int pointId;
        public Transform point;
        public int nextWorldId;
        
        public GfFloat3 PointPosition => (point ? point : transform).position.ToGfFloat3();
        public GfQuaternion PointRotation => (point ? point : transform).rotation.ToGfQuaternion();

        private int _playerLayerId;

        public void Awake()
        {
            _playerLayerId = LayerMask.NameToLayer(Constant.Layer.Player);
        }
        
        public void Enter()
        {
            //移动角色到point点位
            BattleAdmin.Player.Transform.SetTransform(PointPosition, PointRotation);
        }

        private async void Exit()
        {
            await UIHelper.StartLoading();
            //根据nextWorldId加载world，并找到world中相同pointId的TransitionPoint 调用其enter方法
            await WorldManager.Instance.ChangeWorld(nextWorldId, pointId);
            UIHelper.EndLoading();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _playerLayerId)
            {
                return;
            }
            Exit();
        }
    }
}