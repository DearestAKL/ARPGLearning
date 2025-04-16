using System;
using Akari.GfCore;
using Akari.GfUnity;
using KinematicCharacterController;
using UnityEngine;

namespace GameMain.Runtime
{
    [RequireComponent(typeof (KinematicCharacterMotor))]
    public class AkariCharacterController : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor motor;

        private Vector3 _gravity = new Vector3(0, BattleDef.Gravity, 0);

        public bool IsStableOnGround => motor.GroundingStatus.IsStableOnGround;
        
        private GfEntity _gfEntity;

        private bool HasActivate => _gfEntity != null && _gfEntity.Transform != null;
        
        private void Start()
        {
            if (motor == null)
            {
                motor = GetComponent<KinematicCharacterMotor>();
            }
            // Assign to motor
            motor.CharacterController = this;
        }

        public void Init(GfEntity gfEntity)
        {
            _gfEntity = gfEntity;
            _gfEntity.On<SetPositionAndRotationRequest>(OnSetPositionAndRotation);
        }

        private void OnSetPositionAndRotation(in SetPositionAndRotationRequest request)
        {
            motor.SetPositionAndRotation(request.Position.ToVector3(),request.Rotation.ToQuaternion());
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (!HasActivate)
            {
                return;
            }
            
            currentRotation = _gfEntity.Transform.Rotation.ToQuaternion();
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!HasActivate)
            {
                return;
            }
            
            Vector3 targetMovementVelocity = Vector3.zero;
            
            var gfPosition = _gfEntity.Transform.Position.ToVector3();
            var vector = gfPosition - transform.position;
            float sqrDistance = vector.sqrMagnitude;
            if (sqrDistance > 0.0001f)
            {
                targetMovementVelocity += vector / deltaTime;
            }

            if (Math.Abs(targetMovementVelocity.y) > 0.1F)
            {
                //如果有y轴方向的变化 着全权由逻辑代码控制
                currentVelocity = targetMovementVelocity;
                
                //使角色在下次更新时跳过地面探测/捕捉。
                //如果不跳过，角色在Y方向位移时仍会被吸附在地面上。试着注释掉这一行。
                motor.ForceUnground();
            }
            else if (motor.GroundingStatus.IsStableOnGround)
            {
                currentVelocity = targetMovementVelocity;
            }
            else
            {
                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, _gravity);
                currentVelocity += velocityDiff * deltaTime;
                currentVelocity += _gravity * deltaTime;
            }
        }
        
        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {

        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            if (!HasActivate)
            {
                return;
            }
            
            _gfEntity.Transform.Position = motor.TransientPosition.ToGfFloat3();
            _gfEntity.Transform.Rotation = motor.TransientRotation.ToGfQuaternion();
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {

        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {

        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {

        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {

        }

        public void UpdateCollidableLayer()
        {
            motor.CollidableLayers = 0;
            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(gameObject.layer, i))
                {
                    motor.CollidableLayers |= (1 << i);
                }
            }
        }

        public void RemoveCollidableLayer(string layerName)
        {
            // 移除层级
            int layerToRemove = LayerMask.NameToLayer(layerName);
            motor.CollidableLayers &= ~(1 << layerToRemove);
        }
        
        public void AddCollidableLayer(string layerName)
        {
            // 添加层级
            int layerToAdd = LayerMask.NameToLayer(layerName);
            motor.CollidableLayers |= (1 << layerToAdd);
        }
    }
}