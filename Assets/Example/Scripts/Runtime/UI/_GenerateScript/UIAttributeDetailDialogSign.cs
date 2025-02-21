// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-03 10:50:19.255
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UIAttributeDetailDialogSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btnClose = null;
		protected GameMain.Runtime.UIAttributeItem attributeMaxHp = null;
		protected GameMain.Runtime.UIAttributeItem attributeAttack = null;
		protected GameMain.Runtime.UIAttributeItem attributeDefense = null;
		protected GameMain.Runtime.UIAttributeItem attributeCriticalHitRate = null;
		protected GameMain.Runtime.UIAttributeItem attributeCriticalHitDamage = null;
		protected GameMain.Runtime.UIAttributeItem attributeDamageBonus = null;
		protected GameMain.Runtime.UIAttributeItem attributeDamageReduction = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            btnClose = rc.Get<UnityEngine.UI.Button>("btnClose");
			attributeMaxHp = rc.Get<GameMain.Runtime.UIAttributeItem>("attributeMaxHp");
			attributeAttack = rc.Get<GameMain.Runtime.UIAttributeItem>("attributeAttack");
			attributeDefense = rc.Get<GameMain.Runtime.UIAttributeItem>("attributeDefense");
			attributeCriticalHitRate = rc.Get<GameMain.Runtime.UIAttributeItem>("attributeCriticalHitRate");
			attributeCriticalHitDamage = rc.Get<GameMain.Runtime.UIAttributeItem>("attributeCriticalHitDamage");
			attributeDamageBonus = rc.Get<GameMain.Runtime.UIAttributeItem>("attributeDamageBonus");
			attributeDamageReduction = rc.Get<GameMain.Runtime.UIAttributeItem>("attributeDamageReduction");
			
        }
    }
}
