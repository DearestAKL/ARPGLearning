using Akari.GfCore;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 泉水，恢复生命值
    /// 使用后，改变表现，关闭交互
    /// </summary>
    public class SpringWater : AClickInteractiveObject
    {
        [SerializeField] private int value = 250;
        
        public override string InteractionTips => "恢复";

        protected override void OnInteracting(CharacterInteractive characterInteractive)
        {
            GfLog.Debug("Heal HP");
            
            var hpProperty = characterInteractive.Entity.GetComponent<BattleCharacterConditionComponent>()?.HpProperty;

            if (hpProperty != null)
            {
                hpProperty.AddCurValue(value,true);
                //关闭交互
                SetCanInteract(false);
                //改变表现状态
                
                characterInteractive.RemoveInteractiveObject(this);
            }
            else
            {
                //无效
                ResetStateData();
            }
        }
    }
}