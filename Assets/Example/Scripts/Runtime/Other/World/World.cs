using System;
using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;
using UnityEngine;

namespace GameMain.Runtime
{
    public class World : MonoBehaviour
    {
        public Collider cinemachineConfinerBoundingVolume;
        public TransitionPoint defaultStartPoint;
        public List<TransitionPoint> transitionPoints;

        private List<GfEntity> _worldGfEntities = new List<GfEntity>();
        
        private float _elapsedTime;
        public bool CheckTime(float deltaTime)
        {
            _elapsedTime += deltaTime;
            if (_elapsedTime > 100)//1000s
            {
                Destroy(this);
                return false;
            }
            return true;
        }

        public void Enter(int transitionPointId = 0)
        {
            BattleUnityAdmin.BattleMainCameraView.SetCinemachineConfinerBoundingVolume(
                cinemachineConfinerBoundingVolume);
            
            gameObject.SetActive(true);
            if (transitionPointId == 0)
            {
                defaultStartPoint.Enter();
            }
            else
            {
                for (int i = 0; i < transitionPoints.Count; i++)
                {
                    if (transitionPoints[i].pointId == transitionPointId)
                    {
                        transitionPoints[i].Enter();
                    }
                }
            }

            foreach (var entity in _worldGfEntities)
            {
                entity.SetActive(true);
            }
        }

        public void Exit()
        {
            gameObject.SetActive(false);
            
            foreach (var entity in _worldGfEntities)
            {
                entity.SetActive(false);
            }
        }

        public void AddGfEntity(GfEntity entity)
        {
            _worldGfEntities.Add(entity);
        }
    }
}