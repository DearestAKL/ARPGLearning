// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-18 18:01:08.818
//------------------------------------------------------------

namespace GameMain.Runtime
{
    public class UISideFilterDialogSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btnConfirm = null;
		protected UnityEngine.UI.Button btnReset = null;
		protected UnityEngine.UI.Button btnClose = null;
		protected UnityEngine.GameObject goWeaponFilter = null;
		protected GameMain.Runtime.UIFilterToggleGroup weaponType = null;
		protected GameMain.Runtime.UIFilterToggleGroup weaponQuality = null;
		protected UnityEngine.GameObject goArmorFilter = null;
		protected GameMain.Runtime.UIFilterToggleGroup armorType = null;
		protected GameMain.Runtime.UIFilterToggleGroup armorQuality = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            btnConfirm = rc.Get<UnityEngine.UI.Button>("btnConfirm");
			btnReset = rc.Get<UnityEngine.UI.Button>("btnReset");
			btnClose = rc.Get<UnityEngine.UI.Button>("btnClose");
			goWeaponFilter = rc.Get<UnityEngine.GameObject>("goWeaponFilter");
			weaponType = rc.Get<GameMain.Runtime.UIFilterToggleGroup>("weaponType");
			weaponQuality = rc.Get<GameMain.Runtime.UIFilterToggleGroup>("weaponQuality");
			goArmorFilter = rc.Get<UnityEngine.GameObject>("goArmorFilter");
			armorType = rc.Get<GameMain.Runtime.UIFilterToggleGroup>("armorType");
			armorQuality = rc.Get<GameMain.Runtime.UIFilterToggleGroup>("armorQuality");
			
        }
    }
}
