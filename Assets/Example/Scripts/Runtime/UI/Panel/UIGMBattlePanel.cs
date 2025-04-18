using Akari.GfCore;
using Akari.GfGame;
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

            btnMod.SetActive(false);
            toggleMod.SetActive(false);
            
            btnClose.onClick.AddListener(Close);
            
            CreatButton(SpawnEnemy,"生成Enemy");
            CreatButton(SpawnNpc,"生成Npc");
            CreatButton(WitchTime,"WitchTime");

            CreatToggle(GMConfig.SetIgnoreSkillCd, GMConfig.IgnoreSkillCd, "技能无Cd");
            CreatToggle(GMConfig.SetApplyRootMotion, GMConfig.ApplyRootMotion, "开启RootMotion");
        }

        private void CreatButton(UnityAction action,string btnText)
        {
            var btn = Object.Instantiate(btnMod, contentBtns).GetComponent<Button>();
            btn.gameObject.SetActive(true);
            btn.onClick.AddListener(action);
            
            var txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = btnText;
        }

        private void CreatToggle(UnityAction<bool> action, bool isOn, string toggleText)
        {
            var toggle = Object.Instantiate(toggleMod, contentSingleToggles).GetComponent<Toggle>();
            toggle.gameObject.SetActive(true);
            toggle.isOn = isOn;
            toggle.onValueChanged.AddListener(action);

            var txt = toggle.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = toggleText;
        }

        private void SpawnEnemy()
        {
            var pos = BattleAdmin.Player.Entity.Transform.Position + GfFloat3.Forward;
            
            BattleAdmin.Factory.Character.CreateEnemyCharacter(
                new GameCharacterModel(new EnemyData(2001,10)), pos, GfQuaternion.Identity, "enemyKey").ToCoroutine();
        }

        private void SpawnNpc()
        {
            var pos = BattleAdmin.Player.Entity.Transform.Position + GfFloat3.Forward;
            BattleAdmin.Factory.Character.CreateNpcCharacter(
                new GameCharacterModel(new NpcData(4001)), pos, GfQuaternion.Identity, "npcKey").ToCoroutine();
        }

        private void WitchTime()
        {
            foreach (var entity in BattleAdmin.EntityComponentSystem.EntityManager.EntityHandleManager.Buffers)
            {
                if (!entity.IsValid() || entity.Tag == GfEntityTagId.TeamA)
                {
                    continue;
                }
                
                entity.Request(new GfChangeSpeedMagnificationRequest(9001,
                    0.1F,
                    3f,
                    true));
            }
        }
    }
}