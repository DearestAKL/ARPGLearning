using Akari.GfCore;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameMain.Runtime
{
    public class UIBattlePanel : UIBattlePanelSign
    {
        private int _actionId;
        private float _elapsedTime;

        private int _curCharacterId;
        private int _curHp;
        private int _curMaxHp;
        private int _curLevel;
        private int _curExp;//总经验
        private int _curCoin;
        private int _curGem;
        
        private int _curLevelUpExp;//当前等级升级所需经验
        private int _curLevelTotalExp;//当前等级总经验

        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent,
            object userData)
        {
            base.OnInit(name, go, parent, userData);
            
            EventManager.Instance.UIEvent.OnPlayerCharacterHpChangeEvent.GfSubscribe(OnPlayerCharacterHpChangeEvent).AddTo(InitSubscriptions);
            EventManager.Instance.UIEvent.OnPlayerCharacterExpChangeEvent.GfSubscribe(OnPlayerCharacterExpChangeEvent).AddTo(InitSubscriptions);
            EventManager.Instance.UIEvent.OnPlayerCharacterLevelChangeEvent.GfSubscribe(OnPlayerCharacterLevelChangeEvent).AddTo(InitSubscriptions);
            EventManager.Instance.UIEvent.OnPlayerCharacterCoinChangeEvent.GfSubscribe(OnPlayerCharacterCoinChangeEvent).AddTo(InitSubscriptions);
            EventManager.Instance.UIEvent.OnPlayerCharacterGemChangeEvent.GfSubscribe(OnPlayerCharacterGemChangeEvent).AddTo(InitSubscriptions);
            
            EventManager.Instance.BattleEvent.OnChangeCharacterEvent.GfSubscribe(OnChangeCharacterEvent).AddTo(InitSubscriptions);

            InitInputAction();
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
            var condition = BattleAdmin.Player.Condition;
            _curCharacterId = condition.CharacterId;
            _curHp = condition.HpProperty.CurShowValue;
            _curMaxHp = condition.HpProperty.TotalMaxShowValue;
            
            var userCharacterData = UserDataManager.Instance.Character.Get(_curCharacterId);
            _curExp = userCharacterData.Exp;
            _curLevel = userCharacterData.Level;
            
            _curCoin = UserDataManager.Instance.Battle.Coin;
            _curGem = UserDataManager.Instance.Player.Gem;
            
            UpdateCharacterView();
            UpdateTime(0);
            
            UpdateCoin();
            UpdateGem();
            
            BattleAdmin.SetPaused(false);
        }

        public override void OnClose()
        {
            base.OnClose();
            BattleAdmin.SetPaused(true);
        }

        private void UpdateCharacterView()
        {
            UpdateHp(_curHp, _curMaxHp);
            UpdateLevel();
            UpdateExp();
        }

        public override void OnUpdate(float elapseSeconds)
        {
            base.OnUpdate(elapseSeconds);

            var userPlayer = BattleAdmin.Player;
            if (userPlayer == null || !userPlayer.IsAlive)
            {
                return;
            }

            CheckPlayerActionChange(userPlayer.Action.GetNowActionId());

            var normalSkillCoolTimeHandler = userPlayer.Condition.GetTargetCoolTime(0);
            UpdateBattleSkill(specialAttack, normalSkillCoolTimeHandler);
            
            var specialSkillCoolTimeHandler = userPlayer.Condition.GetTargetCoolTime(1);
            UpdateBattleSkill(ultimate, specialSkillCoolTimeHandler);
            
            _elapsedTime += elapseSeconds;
            if (_elapsedTime > 1.0f)
            {
                UpdateTime((int)BattleAdmin.Time.ElapsedTimeExcludingPause);
            }

            if (UIManager.Instance.HasPopUp != BattleAdmin.Time.IsPaused)
            {
                BattleAdmin.SetPaused(UIManager.Instance.HasPopUp);
            }
        }

        private void UpdateBattleSkill(UIBattleAttackInfo attackInfo,BattleObjectCoolTimeHandler skillCoolTimeHandler)
        {
            attackInfo.UpdateView(skillCoolTimeHandler.CurrentSlotCoolTimeRate,
                skillCoolTimeHandler.CurrentSlotNum,
                skillCoolTimeHandler.SingleCoolTime - skillCoolTimeHandler.CurrentSlotCoolTime
            );
        }

        private void CheckPlayerActionChange(int nowActionId)
        {
            if (_actionId == nowActionId || (!IsSkillAction(_actionId) && !IsSkillAction(nowActionId)))
            {
                return;
            }

            SetSkillForce(_actionId, false);
            SetSkillForce(nowActionId, true);

            _actionId = nowActionId;
        }

        private void SetSkillForce(int actionId, bool forceValue)
        {
            switch (actionId)
            {
                case BattleCharacterLightAttackAction.ActionType:
                case BattleCharacterChargeAttackAction.ActionType:
                    basicAttack.SetForce(forceValue);
                    break;
                case BattleCharacterSpecialAttackAction.ActionType:
                    specialAttack.SetForce(forceValue);
                    break;
                case BattleCharacterUltimateAction.ActionType:
                    ultimate.SetForce(forceValue);
                    break;
                case BattleCharacterDashAction.ActionType:
                    dash.SetForce(forceValue);
                    break;
            }
        }

        private bool IsSkillAction(int actionId)
        {
            return actionId == BattleCharacterLightAttackAction.ActionType
                   || actionId == BattleCharacterChargeAttackAction.ActionType
                   || actionId == BattleCharacterSpecialAttackAction.ActionType
                   || actionId == BattleCharacterUltimateAction.ActionType
                   || actionId == BattleCharacterDashAction.ActionType;
        }
        
        private void UpdateHp(int curHp,int maxHp)
        {
            txtHp.text = $"{curHp}/{maxHp}";
            var ratio = curHp == maxHp ? 1 : curHp / (float)maxHp;
            imgHp.transform.localScale = new Vector3(ratio, 1, 1);
        }

        private void UpdateExp()
        {
            if (_curLevelUpExp == 0)
            {
                //满级
                txtExp.text = "Max";
                imgExp.fillAmount = 1;
                return;
            }
            
            var curLevelExp = _curExp - _curLevelTotalExp;
            txtExp.text = $"{curLevelExp}/{_curLevelUpExp}";
            var ratio = curLevelExp == _curLevelUpExp ? 1 :curLevelExp / (float)_curLevelUpExp;
            imgExp.fillAmount = ratio;
        }
        
        private void UpdateLevel()
        {
            txtLevel.text = _curLevel.ToString();
            
            //更新当前等级的 升级经验和总经验
            var characterLevelExp = LubanManager.Instance.Tables.TbCharacterLevelExp.Get(_curLevel);
            _curLevelUpExp = characterLevelExp.UpExp;
            _curLevelTotalExp = characterLevelExp.TotalExp;
        }
        
        private void UpdateCoin()
        {
            txtCoinNum.text = _curCoin.ToString();
        }
        
        private void UpdateGem()
        {
            txtGemNum.text = _curGem.ToString();
        }
        
        private void UpdateTime(int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            // 使用ToString("D2")确保分钟和秒数都以两位数格式显示
            string formattedTime = minutes.ToString("D2") + ":" + seconds.ToString("D2");
            txtTime.text = formattedTime;
        }
        
        private void OnPlayerCharacterHpChangeEvent(int curHp,int maxHp)
        {
            _curHp = curHp;
            _curMaxHp = maxHp;
            UpdateHp(_curHp, _curMaxHp);
        }

        private void OnPlayerCharacterExpChangeEvent(int characterId,int curExp)
        {            
            if (_curCharacterId == characterId)
            {
                return;
            }
            
            _curExp = curExp;
            UpdateExp();
        }

        private void OnPlayerCharacterLevelChangeEvent(int characterId,int curLevel)
        {
            if (_curCharacterId == characterId)
            {
                return;
            }
            
            _curLevel = curLevel;
            UpdateLevel();

            //升级
            //回满血，更新属性
            BattleAdmin.Player.Condition.HpProperty.SetCurValueFully();
        }

        private void OnPlayerCharacterCoinChangeEvent(int curCoin)
        {
            _curCoin = curCoin;
            UpdateCoin();
        }
        
        private void OnPlayerCharacterGemChangeEvent(int curGem)
        {
            _curGem = curGem;
            UpdateGem();
        }

        private void OnChangeCharacterEvent(int lastCharacterId, int curCharacterId)
        {
            var condition = BattleAdmin.Player.Condition;
            _curCharacterId = condition.CharacterId;
            _curHp = condition.HpProperty.CurShowValue;
            _curMaxHp = condition.HpProperty.TotalMaxShowValue;
            
            var userCharacterData = UserDataManager.Instance.Character.Get(_curCharacterId);
            _curExp = userCharacterData.Exp;
            _curLevel = userCharacterData.Level;
            
            UpdateCharacterView();
        }
        
        
        #region Input

        private void InitInputAction()
        {
            BattleUnityAdmin.PlayerInput.actions[Constant.InputDef.Menu].started += OnMenuStarted;
            BattleUnityAdmin.PlayerInput.actions[Constant.InputDef.Backpack].started += OnBackpackStarted;
        }

        private async void OnMenuStarted(InputAction.CallbackContext context)
        {
            if (UIManager.Instance.HasPopUp || !IsActive)
            {
                //有弹窗时暂时屏蔽
                return;
            }
            await UIManager.Instance.OpenUIPanel(UIType.UISideMenuPanel);
        }
        
        private async void OnBackpackStarted(InputAction.CallbackContext context)
        {
            if (UIManager.Instance.HasPopUp || !IsActive)
            {
                //有弹窗时暂时屏蔽
                return;
            }
            await UIManager.Instance.OpenUIPanel(UIType.UIBackpackPanel,new UIBackpackPanel.Params()); 
        }
        #endregion
    }
}