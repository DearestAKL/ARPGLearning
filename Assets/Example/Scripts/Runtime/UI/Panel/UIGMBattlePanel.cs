using Akari.GfCore;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    /// <summary>
    /// 战斗GM功能集合
    /// </summary>
    public class UIGMBattlePanel : UIGMBattlePanelSign
    {
        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);

            btnClose.onClick.AddListener(Close);
            
            CreatButton(SpawnEnemy,"生成Enemy");
        }

        private void CreatButton(UnityAction action,string btnText)
        {
            var btn = Object.Instantiate(btnMod, contentBtns).GetComponent<Button>();
            btn.gameObject.SetActive(true);
            btn.onClick.AddListener(action);
            
            var txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = btnText;
        }

        private void SpawnEnemy()
        {
            BattleAdmin.Factory.Character.CreateEnemyCharacter(
                new GameCharacterModel(new EnemyData(2001,10)), GfFloat3.Zero, GfQuaternion.Identity, "enemyKey").ToCoroutine();
        }
    }
}