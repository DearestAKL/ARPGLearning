using KinematicCharacterController;
using UnityEngine;

namespace GameMain.Runtime
{
    public class MyMovingPlatform : MonoBehaviour, IMoverController
    {
        public PhysicsMover mover;
        public Transform fellowTarget;

        // private Vector3 _lastPosition;
        // private Quaternion _lastRotation;
        
        private void Start()
        {
            if (mover == null)
            {
                mover = GetComponent<PhysicsMover>();
            }
            mover.MoverController = this;
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            // Vector3 _positionBeforeAnim = transform.position;
            // Quaternion _rotationBeforeAnim = transform.rotation;
            
            goalPosition = fellowTarget.position;
            goalRotation = fellowTarget.rotation;
        }
    }
}