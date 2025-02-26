using System;
using Akari.GfCore;
using Akari.GfUnity;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMain.Runtime
{
    public sealed class BattleMainCameraUnityView : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        private Camera _mainCamera;
        public Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }

        public GfHandle SelfHandle { get; private set; }

        public void SetSelfHandle(GfHandle handle)
        {
            if (SelfHandle != GfHandle.Invalid)
            {
                throw new InvalidOperationException("BattleCameraUnityView的Handle以及设定过了");
            }

            SelfHandle = handle;
            EventManager.Instance.BattleEvent.OnChangeCharacterEvent.GfSubscribe(OnChangeCharacterEvent);
        }

        /// <summary>
        /// 设置 CM FreeLook 的 跟随目标
        /// </summary>
        /// <param name="follow">跟随目标</param>
        public void SetFreeLookFollow(Transform follow)
        {
            virtualCamera.Follow = follow;
        }

        /// <summary>
        /// 设置 CM FreeLook 的 观察目标
        /// </summary>
        /// <param name="lookAt">观察目标</param>
        public void SetFreeLookLookAt(Transform lookAt)
        {
            virtualCamera.LookAt = lookAt;
        }

        public void ShakeCamera(BattleCameraShakeParam.ShakePower power, BattleCameraShakeParam.ShakeDirection direction)
        {
            if (power == BattleCameraShakeParam.ShakePower.NONE)
            {
                return;
            }
            
            var shakeMagnitude = power.Magnitude();
            var shakeDirection = direction;

            Vector3 force;
            if (shakeDirection == BattleCameraShakeParam.ShakeDirection.VERTICAL)
            {
                force = Vector3.up;
            }
            else if(shakeDirection == BattleCameraShakeParam.ShakeDirection.HORIZON)
            {
                force = Vector3.left;
            }
            else
            {
                force = Random.insideUnitCircle;
            }

            impulseSource.GenerateImpulse(force * shakeMagnitude);
        }

        private void OnChangeCharacterEvent(int lastCharacterId, int curCharacterId)
        {
            var lookTransform = (BattleAdmin.Player.Entity.Transform as GfUnityTransform)?.GetUnityTransform();
            SetFreeLookFollow(lookTransform);
            SetFreeLookLookAt(lookTransform);
        }
    }
}
