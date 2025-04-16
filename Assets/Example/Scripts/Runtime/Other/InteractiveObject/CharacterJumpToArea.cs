using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public class CharacterJumpToArea  : AClickInteractiveObject
    {
        [SerializeField] private Transform targetPos;
        
        public override string InputName => Constant.InputDef.Jump;
        public override string InteractionTips  => "跳跃";

        protected override void OnInteracting(CharacterInteractive characterInteractive)
        {
            var nextActionData = BattleCharacterJumpToActionData.Create(targetPos.position.ToGfFloat3());
            characterInteractive.Entity.Request(new GfChangeActionRequest<BattleObjectActionComponent>(nextActionData.ActionType, nextActionData, nextActionData.Priority));

            if (characterInteractive.Accessor.Condition.Frame.CanDash.Current)
            {
                characterInteractive.RemoveInteractiveObject(this);
                ResetStateData();
            }
            else
            {
                //跳跃失败 重新设置为未交换
                IsInteracting = false;
            }
        }
    }
}