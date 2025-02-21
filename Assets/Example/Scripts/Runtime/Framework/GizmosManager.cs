using System;
using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    [Serializable]
    public class GizmosData
    {
        public enum ShapeType
        {
            Line,
            Box,
            Circle
        }

        public ShapeType Type;
        public Transform Transform;
        public Color Color = Color.yellow;
        
        public float X;
        public float Y;
        
        public float Angle;
        public float Radius;
        
        private Vector3 targetPos;
        
        public Transform TargetTransform;
        public bool HasTargetTransform;
        public Vector3 TargetPos
        {
            get
            {
                if (TargetTransform != null)
                {
                    return TargetTransform.position;
                }

                return targetPos;
            }
            private set => targetPos = value;
        }

        public static GizmosData CreateLineGizmosData(Transform transform, Vector3 targetPos)
        {
            var data = new GizmosData();
            data.Transform = transform;
            data.TargetPos = targetPos;
            return data;
        }
        
        public static GizmosData CreateLineGizmosData(Transform transform, Transform target)
        {
            var data = new GizmosData();
            data.Transform = transform;
            data.TargetTransform = target;
            data.HasTargetTransform = true;
            return data;
        }
        
        public static GizmosData CreateBoxGizmosData(Transform transform, float x, float y)
        {
            var data = new GizmosData();
            data.Transform = transform;
            data.X = x;
            data.Y = y;
            data.Type = ShapeType.Box;
            return data;
        }
        
        public static GizmosData CreateCircleGizmosData(Transform transform, float radius, float angle)
        {
            var data = new GizmosData();
            data.Transform = transform;
            data.Radius = radius;
            data.Angle = angle;
            data.Type = ShapeType.Circle;
            return data;
        }

        public static GizmosData CreateLineGizmosData(GfTransform transform, GfFloat3 targetPos)
        {
            return CreateLineGizmosData((transform as GfUnityTransform)?.GetUnityTransform(), targetPos.ToVector3());
        }

        public static GizmosData CreateLineGizmosData(GfTransform transform, GfTransform target)
        {
            return CreateLineGizmosData((transform as GfUnityTransform)?.GetUnityTransform(),(target as GfUnityTransform)?.GetUnityTransform());
        }
        
        public static GizmosData CreateCircleGizmosData(GfTransform transform, float radius, float angle)
        {
            return CreateCircleGizmosData((transform as GfUnityTransform)?.GetUnityTransform(),radius,angle);
        }
    }

    public class GizmosManager : MonoBehaviour
    {
        public List<GizmosData> gizmosDataList = new List<GizmosData>();

        private float _curTime;
        private float _removeTime = 1f;
        private List<GizmosData> _removes = new List<GizmosData>();
        
        private static GizmosManager _instance;
        public static GizmosManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("GizmosManager");
                    _instance = go.AddComponent<GizmosManager>();
                }
                return _instance;
            }
        }

        public void AddGizmosData(GizmosData gizmosData)
        {
            if (!gizmosDataList.Contains(gizmosData))
            {
                gizmosDataList.Add(gizmosData);
            }
        }
        
        public void RemoveGizmosData(GizmosData gizmosData)
        {
            if (gizmosDataList.Contains(gizmosData))
            {
                gizmosDataList.Remove(gizmosData);
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void Update()
        {
            _curTime += Time.deltaTime;
            if (_curTime < _removeTime)
            {
                return;
            }
            _curTime = 0;
            
            //定期回收
            foreach (var gizmosData in gizmosDataList)
            {
                if (gizmosData.Transform == null || 
                    !gizmosData.Transform.gameObject.activeSelf  || 
                    (gizmosData.HasTargetTransform && gizmosData.TargetTransform == null)
                    )
                {
                    _removes.Add(gizmosData);
                }
            }

            foreach (var remove in _removes)
            {
                gizmosDataList.Remove(remove);
            }
            _removes.Clear();
        }

        private void OnDrawGizmos()
        {
            foreach (var gizmosData in gizmosDataList)
            {
                if (gizmosData.Transform == null)
                {
                    return;
                }
                gizmosData.DrawGizmosData();
            }
        }
    }
}