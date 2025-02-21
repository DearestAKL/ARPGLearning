using Akari.GfUnity;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class UICharacterDetailView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtName = null;
        [SerializeField] private UIStarCollection ascensionStarCollection = null;
        [SerializeField] private TextMeshProUGUI txtLevel = null;
        [SerializeField] private Image imgExp = null;

        [SerializeField] private UIAttributeItem[] attributeItems = new UIAttributeItem[5];
        [SerializeField] private Button btnAttributeDetail = null;
        
        [SerializeField] private Button btnAscension = null;
        [SerializeField] private TextMeshProUGUI txtAscension = null;

        private UICharacterModel _characterModel;
        private void Awake()
        {
            btnAttributeDetail.onClick.AddListener(OnAttributeDetail);
            btnAscension.onClick.AddListener(OnAscension);
        }

        public void UpdateView(UICharacterModel characterModel)
        {
            _characterModel = characterModel;
            var characterData = characterModel.CharacterData;
            txtName.text = characterData.Config.Name;
            ascensionStarCollection.UpdateView(characterData.AscensionLevel);
            txtLevel.text = $"{UICommonLabel.Level.GetLocalization()} {characterData.Level}<color=#{ColorUtility.ToHtmlStringRGB(Constant.ColorDef.Gray)}>/{characterData.CurMaxLevel}</color>";
            imgExp.transform.localScale = new Vector3(characterData.CurExpRatio, 1, 1);
            
            //需要构建一个 角色ui的数据结构 得到所有info
            attributeItems[0].UpdateView(characterModel.Hp);
            attributeItems[1].UpdateView(characterModel.Attack);
            attributeItems[2].UpdateView(characterModel.Defense);
            
            attributeItems[3].UpdateView(characterModel.CriticalHitRate);
            attributeItems[4].UpdateView(characterModel.CriticalHitDamage);

            btnAscension.gameObject.SetActive(!characterData.IsMaxLevel);
            txtAscension.FormatLocalization(characterData.IsCurMaxLevel ? UICommonLabel.Ascension : UICommonLabel.Upgrade);
        }

        private async void OnAttributeDetail()
        {
            //Open AttributeDetail Dialog
            await UIManager.Instance.OpenUIPanel(UIType.UIAttributeDetailDialog,new UIAttributeDetailDialog.Params(_characterModel));
        }

        //升级或者突破
        private async void OnAscension()
        {
            //Open Ascension Panel
            await UIManager.Instance.OpenUIPanel(UIType.UICharacterAscensionPanel,new UICharacterAscensionPanel.Params(_characterModel.CharacterData));
        }
    }
}