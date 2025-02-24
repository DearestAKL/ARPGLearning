// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2025-02-24 16:53:49.223
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UIBattlePanelSign : UIPanel
    {
		//---UI---
		protected GameMain.Runtime.UIBattleAttackInfo ultimate = null;
		protected GameMain.Runtime.UIBattleAttackInfo specialAttack = null;
		protected GameMain.Runtime.UIBattleAttackInfo basicAttack = null;
		protected TMPro.TextMeshProUGUI txtExp = null;
		protected TMPro.TextMeshProUGUI txtLevel = null;
		protected UnityEngine.UI.Image imgExp = null;
		protected UnityEngine.UI.Image imgHp = null;
		protected TMPro.TextMeshProUGUI txtHp = null;
		protected TMPro.TextMeshProUGUI txtTime = null;
		protected TMPro.TextMeshProUGUI txtGemNum = null;
		protected TMPro.TextMeshProUGUI txtCoinNum = null;
		protected GameMain.Runtime.UIBattleAttackInfo dash = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            ultimate = rc.Get<GameMain.Runtime.UIBattleAttackInfo>("ultimate");
			specialAttack = rc.Get<GameMain.Runtime.UIBattleAttackInfo>("specialAttack");
			basicAttack = rc.Get<GameMain.Runtime.UIBattleAttackInfo>("basicAttack");
			txtExp = rc.Get<TMPro.TextMeshProUGUI>("txtExp");
			txtLevel = rc.Get<TMPro.TextMeshProUGUI>("txtLevel");
			imgExp = rc.Get<UnityEngine.UI.Image>("imgExp");
			imgHp = rc.Get<UnityEngine.UI.Image>("imgHp");
			txtHp = rc.Get<TMPro.TextMeshProUGUI>("txtHp");
			txtTime = rc.Get<TMPro.TextMeshProUGUI>("txtTime");
			txtGemNum = rc.Get<TMPro.TextMeshProUGUI>("txtGemNum");
			txtCoinNum = rc.Get<TMPro.TextMeshProUGUI>("txtCoinNum");
			dash = rc.Get<GameMain.Runtime.UIBattleAttackInfo>("dash");
			
        }
    }
}
