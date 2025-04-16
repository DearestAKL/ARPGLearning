using DG.Tweening;
using KinematicCharacterController;
using UnityEngine;

namespace GameMain.Runtime
{
    public enum ElevatorStatus
    {
        Start,
        StartToEnd,
        End,
        EndToStart
    }
    
    [RequireComponent(typeof (PhysicsMover))]
    public class ElevatorMoverController : AClickInteractiveObject, IMoverController
    {
        public PhysicsMover mover;
        
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform fellowPoint;
        [SerializeField] private GameObject runningEdgeCollider;//可空
        [SerializeField] private GameObject startEdgeCollider;//可空
        [SerializeField] private GameObject endEdgeCollider;//可空
        [SerializeField] private float time = 3F;

        private ElevatorStatus _curStatus = ElevatorStatus.Start;
        
        public override string InteractionTips  => "运行";

        private void Start()
        {
            if (mover == null)
            {
                mover = GetComponent<PhysicsMover>();
            }
            mover.MoverController = this;

            ChangeStatus(_curStatus, true);
            mover.SetPosition(fellowPoint.position);
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = fellowPoint.position;
            goalRotation = fellowPoint.rotation;
        }

        private void ChangeStatus(ElevatorStatus status,bool isInit = false)
        {
            if (_curStatus == status && !isInit)
            {
                return;
            }
            _curStatus = status;

            bool isRunning = status == ElevatorStatus.StartToEnd || status == ElevatorStatus.EndToStart;
            SetCanInteract(!isRunning);
            if (runningEdgeCollider != null)
            {
                runningEdgeCollider.gameObject.SetActive(isRunning);
            }
            if (startEdgeCollider != null)
            {
                startEdgeCollider.SetActive(status == ElevatorStatus.Start);
            }
            if (endEdgeCollider != null)
            {
                endEdgeCollider.SetActive(status == ElevatorStatus.End);
            }

            if (_curStatus == ElevatorStatus.StartToEnd)
            {
                //开始移动StartToEnd
                fellowPoint.DOMove(endPoint.position, time).SetEase(Ease.InOutQuad).OnComplete(() => ChangeStatus(ElevatorStatus.End));
            }
            else if(_curStatus == ElevatorStatus.EndToStart)
            {
                //开始移动EndToStart
                fellowPoint.DOMove(startPoint.position, time).SetEase(Ease.InOutQuad).OnComplete(() => ChangeStatus(ElevatorStatus.Start));
            }
            else if(_curStatus == ElevatorStatus.Start)
            {
                //移动结束
                fellowPoint.position = startPoint.position;
            }
            else if(_curStatus == ElevatorStatus.End)
            {
                //移动结束
                fellowPoint.position = endPoint.position;
            }
        }

        protected override void OnInteracting(CharacterInteractive characterInteractive)
        {
            if (_curStatus == ElevatorStatus.Start)
            {
                ChangeStatus(ElevatorStatus.StartToEnd);
            }
            else if (_curStatus == ElevatorStatus.End)
            {
                ChangeStatus(ElevatorStatus.EndToStart);
            }
            else
            {
                //运行中无法交互
                IsInteracting = false;
                return;
            }
            
            characterInteractive.RemoveInteractiveObject(this);
            ResetStateData();
        }

        public void Call(ElevatorStatus status)
        {
            if (_curStatus == status)
            {
                //已经在对应位置了
                //TIPS
            }
            else if (_curStatus == ElevatorStatus.EndToStart || _curStatus == ElevatorStatus.StartToEnd)
            {
                //运行中 呼叫无效
                //TIPS
            }
            else
            {
                //呼叫生效 电梯开始运行
                ChangeStatus(status == ElevatorStatus.Start ? ElevatorStatus.EndToStart : ElevatorStatus.StartToEnd);
            }
        }
    }
}