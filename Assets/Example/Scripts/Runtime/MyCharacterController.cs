using System;
using Akari.GfCore;
using Akari.GfUnity;
using KinematicCharacterController;
using UnityEngine;

namespace GameMain.Runtime
{
    [RequireComponent(typeof (KinematicCharacterMotor))]
    public class MyCharacterController : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor motor;

        public Vector3 Gravity = new Vector3(0, -30f, 0);
        
        private GfEntity _gfEntity;
        private void Start()
        {
            if (motor == null)
            {
                motor = GetComponent<KinematicCharacterMotor>();
            }
            // Assign to motor
            motor.CharacterController = this;
        }

        public void SetGfEntity(GfEntity gfEntity)
        {
            _gfEntity = gfEntity;
            _gfEntity.On<SetPositionAndRotationRequest>(SetPositionAndRotation);
        }

        private void SetPositionAndRotation(in SetPositionAndRotationRequest request)
        {
            motor.SetPositionAndRotation(request.Position.ToVector3(),request.Rotation.ToQuaternion());
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_gfEntity != null && _gfEntity.Transform != null)
            {
                currentRotation = _gfEntity.Transform.Rotation.ToQuaternion();
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            Vector3 targetMovementVelocity = Vector3.zero;
            
            if (_gfEntity != null && _gfEntity.Transform != null)
            {
                var gfPosition = _gfEntity.Transform.Position.ToVector3();
                var vector = gfPosition - transform.position;
                float sqrDistance = vector.sqrMagnitude;
                if (sqrDistance > 0.0001f)
                {
                    targetMovementVelocity += vector / deltaTime;
                }
            }

            if (motor.GroundingStatus.IsStableOnGround)
            {
                currentVelocity = targetMovementVelocity;
            }
            else
            {
                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Gravity);
                currentVelocity += velocityDiff * deltaTime;
                
                currentVelocity += Gravity * deltaTime;
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
            if (_gfEntity != null && _gfEntity.Transform != null)
            {
                _gfEntity.Transform.Position = motor.TransientPosition.ToGfFloat3();
                _gfEntity.Transform.Rotation = motor.TransientRotation.ToGfQuaternion();
            }
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
    }
}