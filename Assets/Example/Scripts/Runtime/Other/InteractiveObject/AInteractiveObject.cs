using UnityEngine;

namespace GameMain.Runtime
{
    //TODO:待完善 一边添加 一边更新优化
    public abstract class AInteractiveObject : MonoBehaviour
    {
        public abstract string InteractionTips { get; }
        
        protected bool IsInteracting;
        private bool _canInteract = true;

        protected abstract void OnInteracting(CharacterInteractive characterInteractive);

        public abstract bool Check(CharacterInteractive characterInteractive);

        private void OnTriggerEnter(Collider other)
        {
            if (!_canInteract) { return; }
            
            var characterInteractive = other.GetComponent<CharacterInteractive>();
            if (characterInteractive != null)
            {
                characterInteractive.AddInteractiveObject(this);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!_canInteract) { return; }
            
            var characterInteractive = other.GetComponent<CharacterInteractive>();
            if (characterInteractive != null)
            {
                characterInteractive.RemoveInteractiveObject(this);
            }
        }
        
        public void SetCanInteract(bool canInteract)
        {
            _canInteract = canInteract;
        }
        
        /// <summary>
        /// 重置状态数据
        /// 交互中，可交互
        /// </summary>
        protected virtual void ResetStateData()
        {
            IsInteracting = false;
            _canInteract = true;
        }
    }
    
    /// <summary>
    /// 点击交互
    /// </summary>
    public abstract class AClickInteractiveObject : AInteractiveObject
    {
        public override bool Check(CharacterInteractive characterInteractive)
        {
            if (!IsInteracting)
            {
                IsInteracting = true;
                OnInteracting(characterInteractive);
                return true;
            }
            return false;
        }
    }
    
    // /// <summary>
    // /// 长按交互
    // /// </summary>
    // public abstract class AHoldInteractiveObject : AInteractiveObject
    // {
    //     protected bool InteractiveCompleted = false;
    //     
    //     public override bool Check(CharacterInteractive characterInteractive)
    //     {
    //         IsInteracting = BattleUnityAdmin.BattleInput.Interacting;
    //         if (IsInteracting)
    //         {
    //             if (!InteractiveCompleted)
    //             {
    //                 OnInteracting(characterInteractive);
    //             }
    //         }
    //
    //         return IsInteracting;
    //     }
    //     
    //     /// <summary>
    //     /// 重置状态数据
    //     /// 交互中，可交互，交互完成
    //     /// </summary>
    //     protected override void ResetStateData()
    //     {
    //         base.ResetStateData();
    //         InteractiveCompleted = false;
    //     }
    // }
}