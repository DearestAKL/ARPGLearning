using Akari;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class UICharacterCell : LoopScrollItem
    {
        [SerializeField] private Button btnClick = default;
        [SerializeField] private TextMeshProUGUI txtName = default;
        [SerializeField] private GameObject goContent = default;
        [SerializeField] private GameObject goEmpty = default;
        [SerializeField] private GameObject goSelect = default;
        
        private UICharacterItemData _itemData;
        
    //     public override void Initialize()
    //     {
    //         btnClick.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
    //
    //         btnClick.GetComponent<UIButtonExpand>()?.AddEnterAndExitAction(
    //             () => Context.OnCellEnterAction?.Invoke(Index),
    //             () => Context.OnCellExitAction?.Invoke(Index));
    //     }
    //     
    //     public override void UpdateContent(FancyItemData itemData)
    //     {
    //         _itemData = itemData as UICharacterItemData;
    //         if (_itemData == null) { return; }
    //
    //         bool isEmpty = _itemData.CharacterConfig == null;
    //         goEmpty.SetActive(isEmpty);
    //         goContent.SetActive(!isEmpty);
    //         
    //         if (isEmpty)
    //         {
    //             //空
    //             return;
    //         }
    //         
    //         txtName.text = _itemData.CharacterConfig.Name;
    //         
    //         var selected = Context.SelectedIndex == Index;
    //         goSelect.SetActive(selected);
    //     }
    // }
    }
}