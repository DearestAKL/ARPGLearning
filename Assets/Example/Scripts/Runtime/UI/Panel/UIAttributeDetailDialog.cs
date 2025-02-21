namespace GameMain.Runtime
{
    public class UIAttributeDetailDialog : UIAttributeDetailDialogSign
    {
        public class Params
        {
            public UICharacterModel CharacterModel;
            public Params(UICharacterModel characterModel)
            {
                CharacterModel = characterModel;
            }
        }
        
        private Params _data;
        
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent,
            object userData)
        {
            base.OnInit(name, go, parent, userData);
            
            btnClose.onClick.AddListener(Close);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            if (userData is Params data)
            {
                _data = data;

                UpdateView(_data.CharacterModel);
            }
        }

        private void UpdateView(UICharacterModel characterModel)
        {
            attributeMaxHp.UpdateView(characterModel.Hp);
            attributeAttack.UpdateView(characterModel.Attack);
            attributeDefense.UpdateView(characterModel.Defense);
            
            attributeCriticalHitRate.UpdateView(characterModel.CriticalHitRate);
            attributeCriticalHitDamage.UpdateView(characterModel.CriticalHitDamage);
            attributeDamageBonus.UpdateView(characterModel.DamageBonus);
            attributeDamageReduction.UpdateView(characterModel.DamageReduction);
        }
    }
}