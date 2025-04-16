using UnityEngine;

namespace GameMain.Runtime
{
    public class ElevatorCall : AClickInteractiveObject
    {
        [SerializeField] private ElevatorMoverController elevator;
        [SerializeField] private ElevatorStatus status = ElevatorStatus.Start;
        
        public override string InteractionTips  => "呼叫电梯";
        protected override void OnInteracting(CharacterInteractive characterInteractive)
        {
            elevator.Call(status);
            
            characterInteractive.RemoveInteractiveObject(this);
            ResetStateData();
        }
    }
}