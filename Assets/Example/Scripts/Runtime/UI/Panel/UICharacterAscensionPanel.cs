using Akari.GfCore;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UICharacterAscensionPanel : UICharacterAscensionPanelSign
    {
        public class Params
        {
            public CharacterData CharacterData;
            public Params(CharacterData characterData)
            {
                CharacterData = characterData;
            }
        }
        
        private Params _data;

        private int _curItemExpTotal = 0;
        private int _nextLevel = 0;
        
        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);  
            
            btnClose.onClick.AddListener(Close);
            btnAscension.onClick.AddListener(OnAscension);
            
            btnTestAddExpItem.onClick.AddListener(OnTestAddExpItem);
            btnTestRemoveExpItem.onClick.AddListener(OnTestRemoveExpItem);
        }
        
        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
            if (userData is Params data)
            {
                _data = data;
                UpdateView();
            }
        }
        
        private void UpdateView()
        {
            _curItemExpTotal = 0;
            var characterData = _data.CharacterData;
            float nextHp = 0F, nextAttack = 0F, nextDefense = 0F;
            if (characterData.IsCurMaxLevel && !characterData.IsMaxLevel)
            {
                //突破
                goAscension.SetActive(true);
                goLevelUp.SetActive(false);

                txtTitle.text = $"{characterData.Config.Name}/突破";
                txtAscension.text = "突破";

                txtLevelLimit.text = $"等级 {characterData.Level}<color=#{ColorUtility.ToHtmlStringRGB(Constant.ColorDef.Gray)}>/{characterData.CurMaxLevel}</color>";

                ascensionStarCollection.UpdateView(characterData.AscensionLevel,true);
                
                btnTestAddExpItem.gameObject.SetActive(false);
                btnTestRemoveExpItem.gameObject.SetActive(false);
                
                UpdateAttributeView(characterData.Level, characterData.AscensionLevel + 1);
            }
            else
            {
                //升级
                goAscension.SetActive(false);
                goLevelUp.SetActive(true);
                
                txtTitle.text = $"{characterData.Config.Name}/升级";
                txtAscension.text = "升级";

                txtLevel.text = $"Lv.{characterData.Level}";
                
                txtExp.text = $"{characterData.CurLevelExp}/{characterData.CurLevelUpExp}";
                imgExp.transform.localScale = new Vector3(GfMathf.Min(1f, characterData.CurExpRatio), 1, 1);
                
                txtLevelAdd.text = string.Empty;
                txtExpAdd.text = string.Empty;
                goMax.SetActive(characterData.IsMaxLevel);
                
                ascensionItemBonus.gameObject.SetActive(false);
                
                btnTestAddExpItem.gameObject.SetActive(true);
                btnTestRemoveExpItem.gameObject.SetActive(true);
                
                UpdateAttributeView(characterData.Level, characterData.AscensionLevel);
            }
            
            
            btnAscension.gameObject.SetActive(!characterData.IsMaxLevel);
        }

        private void OnAscension()
        {
            bool hasChange = false;
            if (_data.CharacterData.IsCurMaxLevel)
            {
                //TODO:检测材料是否足够
               //突破 
               UserDataManager.Instance.Character.Ascension(_data.CharacterData.Id);

               hasChange = true;
            }
            else
            {
                hasChange = _nextLevel > _data.CharacterData.Level;
                //升级
                UserDataManager.Instance.Character.AddExp(_data.CharacterData.Id, _curItemExpTotal);
                _curItemExpTotal = 0;
                _nextLevel = 0;
            }

            if (hasChange)
            {
                _data.CharacterData.UpdateData();
                //TODO:战斗角色属性更新时机
                BattleAdmin.Player.Entity.Request(new UpdatePropertyRequest());
            }
            UpdateView();
        }


        private async void OnTestAddExpItem()
        {
            int limitExp = _data.CharacterData.CurAscensionMaxExp - _data.CharacterData.Exp;

            if (_curItemExpTotal == limitExp)
            {
                await UIHelper.OpenTips("当前阶段经验已满");
                return;
            }
            _curItemExpTotal += 10000;
            _curItemExpTotal = GfMathf.Min(limitExp, _curItemExpTotal);
            
            UpdateCurItemExp();
        }
        
        private async void OnTestRemoveExpItem()
        {
            if (_curItemExpTotal == 0)
            {
                await UIHelper.OpenTips("没有经验道具");
                return;
            }
            
            _curItemExpTotal -= 10000;

            _curItemExpTotal = GfMathf.Max(0, _curItemExpTotal);

            UpdateCurItemExp();
        }

        private void UpdateCurItemExp()
        {
            var characterData = _data.CharacterData;
            var newLevel = CharacterDataHelper.GetLevelUp(characterData.Level, characterData.Exp + _curItemExpTotal);
            int upLevel = newLevel - characterData.Level;
            txtLevelAdd.text = upLevel > 0 ? $"+{upLevel}" : string.Empty;

            txtExpAdd.text = _curItemExpTotal > 0 ? $"+{_curItemExpTotal}" : string.Empty;
            
            goMax.SetActive(_curItemExpTotal == characterData.CurAscensionMaxExp - characterData.Exp);

            var curExpRatio = (_data.CharacterData.CurLevelExp  + _curItemExpTotal) / (float)characterData.CurLevelUpExp;
            imgExp.transform.localScale = new Vector3(GfMathf.Min(1f, curExpRatio), 1, 1);

            if (_nextLevel != newLevel)
            {
                UpdateAttributeView(newLevel, characterData.AscensionLevel);
            }

            _nextLevel = newLevel;
        }

        private void UpdateAttributeView(int nextLevel, int nextAscensionLevel)
        {
            var characterData = _data.CharacterData;
            float nextHp = 0F, nextAttack = 0F, nextDefense = 0F;
            if (characterData.Level < nextLevel || characterData.AscensionLevel < nextAscensionLevel)
            {
                nextHp = characterData.GetAfterAscensionAttribute(AttributeType.Hp, nextLevel, nextAscensionLevel);
                nextAttack = characterData.GetAfterAscensionAttribute(AttributeType.Attack, nextLevel, nextAscensionLevel);
                nextDefense = characterData.GetAfterAscensionAttribute(AttributeType.Defense, nextLevel, nextAscensionLevel);
            }
            
            ascensionItemHp.UpdateView(Mathf.CeilToInt(characterData.Hp),Mathf.CeilToInt(nextHp));
            ascensionItemAttack.UpdateView(Mathf.CeilToInt(characterData.Attack),Mathf.CeilToInt(nextAttack));
            ascensionItemDefense.UpdateView(Mathf.CeilToInt(characterData.Defense),Mathf.CeilToInt(nextDefense));
        }
    }
}