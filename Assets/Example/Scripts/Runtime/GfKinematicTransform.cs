using System.Runtime.CompilerServices;
using Akari.GfCore;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    //相对比GfUnityTransform 不进行Position和Rotation的同步 由KCC控制
    public class GfKinematicTransform : GfTransform
    {
        private Transform _transform;
        private bool      _isPooled;

        // ------------------------------
        public GfKinematicTransform(Transform transform, bool isPooled = false)
        {
            _transform = transform;
            _isPooled  = isPooled;
            if (transform != null)
            {
                Position = transform.localPosition.ToGfFloat3();
                Rotation = transform.localRotation.ToGfQuaternion();
                Scale    = transform.localScale.ToGfFloat3();
            }
        }

        // ------------------------------
        public Transform GetUnityTransform()
        {
            return _transform;
        }

        // ------------------------------
        public void SetUnityTransform(Transform transform)
        {
            _transform = transform;
        }
        
        // ------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void OnChangePosition(in GfFloat3 position)
        {
        }

        // ------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void OnChangeRotation(in GfQuaternion rotation)
        {
        }

        // ------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void OnChangeLocalScale(in GfFloat3 scale)
        {
            if (_transform != null)
            {
                _transform.localScale = scale.ToVector3();
            }
        }

        // ------------------------------
        public override void SetActive(bool active)
        {
            _transform.gameObject.SetActive(active);
        }

        // ------------------------------
        public override void Dispose()
        {
            if (_transform != null)
            {
                if (_isPooled)
                {
                    GfPrefabPool.Return(_transform);
                }
                else
                {
                    Object.Destroy(_transform.gameObject);
                }

                _transform = null;
            }
            base.Dispose();
        }
    }
}