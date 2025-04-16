using System;
using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain.Runtime
{
    [Serializable]
    public class GizmosData
    {
        public enum ShapeType
        {
            Line,
            Box,
            Circle,
            Lines,
        }

        public ShapeType Type;
        public Transform Transform;
        public Color Color = Color.yellow;
        
        public float X;
        public float Y;
        
        public float Angle;
        public float Radius;
        
        public Vector3[] Points;
        
        private Vector3 targetPos;

        public Transform TargetTransform;
        public bool HasTargetTransform;

        private bool _isValid = true;
        public bool IsValid
        {
            get
            {
                if (!_isValid)
                {
                    return _isValid;
                }
                
                if (Type == ShapeType.Lines)
                {
                    return Transform != null && Transform.gameObject.activeSelf && !Points.IsEmpty();
                }
                else if (Type == ShapeType.Line)
                {
                    return Transform != null && Transform.gameObject.activeSelf && (!HasTargetTransform || TargetTransform != null);
                }
                else
                {
                    return Transform != null && Transform.gameObject.activeSelf;
                }
            }
        }

        public void SetValid(bool isValid)
        {
            _isValid = isValid;
        }

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
            data.Type = ShapeType.Line;
            return data;
        }
        
        public static GizmosData CreateLineGizmosData(Transform transform, Transform target)
        {
            var data = new GizmosData();
            data.Transform = transform;
            data.TargetTransform = target;
            data.HasTargetTransform = true;
            data.Type = ShapeType.Line;
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

        public static GizmosData CreateLinesGizmosData(GfTransform transform,Vector3[] points)
        {
            var data = new GizmosData();
            data.Transform = GetUnityTransform(transform);
            data.Points = points;
            data.Type = ShapeType.Lines;
            return data;
        }
        
        public static GizmosData CreateLineGizmosData(GfTransform transform, GfFloat3 targetPos)
        {
            var data = new GizmosData();
            data.Transform = GetUnityTransform(transform);
            data.TargetPos = targetPos.ToVector3();
            data.Type = ShapeType.Line;
            return data;
        }

        public static GizmosData CreateLineGizmosData(GfTransform transform, GfTransform target)
        {
            var data = new GizmosData();
            data.Transform = GetUnityTransform(transform);
            data.TargetTransform = GetUnityTransform(target);
            data.HasTargetTransform = true;
            data.Type = ShapeType.Line;
            return data;
        }
        
        public static GizmosData CreateCircleGizmosData(GfTransform transform, float radius, float angle)
        {
            var data = new GizmosData();
            data.Transform = GetUnityTransform(transform);
            data.Radius = radius;
            data.Angle = angle;
            data.Type = ShapeType.Circle;
            return data;
        }

        private static Transform GetUnityTransform(GfTransform transform)
        {
            if (transform is GfUnityTransform unityTransform)
            {
                return unityTransform.GetUnityTransform();
            }
            else if(transform is GfKinematicTransform kinematicTransform)
            {
                return kinematicTransform.GetUnityTransform();
            }
            else
            {
                return null;
            }
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
                if (!gizmosData.IsValid)
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
                if (gizmosData.Transform == null && gizmosData.Points.IsEmpty())
                {
                    return;
                }
                gizmosData.DrawGizmosData();
            }
        }
    }
}