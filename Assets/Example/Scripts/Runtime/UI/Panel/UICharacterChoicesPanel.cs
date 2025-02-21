// using System.Collections.Generic;
// using Akari.GfCore;
// using UnityEngine;
//
// namespace GameMain.Runtime
// {
//     public class UICharacterChoicesPanel : UICharacterChoicesPanelSign
//     {
//         private CharacterItemData[] _characterItemDataArray;
//         private int _curSelectIndex;
//
//         private const int MinCount = 16;
//
//         public override void OnInit(string name, GameObject go, Transform parent, object userData)
//         {
//             base.OnInit(name, go, parent, userData);
//
//             btnStart.onClick.AddListener(ClickStart);
//             characterScrollView.OnCellClicked(ClickCharacterCell);
//
//             InitCharacterScrollView();
//         }
//         
//         private void InitCharacterScrollView()
//         {
//             var characterDataList = LubanManager.Instance.Tables.TbCharacter.DataList;
//             int characterMinCount = GfMathf.Max(MinCount, characterDataList.Count);
//             _characterItemDataArray = new CharacterItemData[characterMinCount];
//             
//             for (int i = 0; i < characterMinCount; i++)
//             {
//                 if (characterDataList.Count > i)
//                 {
//                     _characterItemDataArray[i] = new CharacterItemData(characterDataList[i]);
//                 }
//                 else
//                 {
//                     _characterItemDataArray[i] = new CharacterItemData(null);
//                 }
//             }
//             
//             characterScrollView.UpdateContents(_characterItemDataArray);
//
//             _curSelectIndex = 0;
//             characterScrollView.UpdateSelection(_curSelectIndex);
//             UpdateCharacterInfo();
//         }
//
//         private void ClickStart()
//         {
//             Close();
//             EventManager.Instance.BattleEvent.OnBattleStartEvent.Invoke();
//         }
//
//         private void ClickCharacterCell(int index)
//         {
//             if (_curSelectIndex == index)
//             {
//                 return;
//             }
//
//             _curSelectIndex = index;
//             characterScrollView.UpdateSelection(_curSelectIndex);
//             UpdateCharacterInfo();
//         }
//
//         private void UpdateCharacterInfo()
//         {
//             var characterItemData = _characterItemDataArray[_curSelectIndex];
//             txtCharacterName.text = characterItemData.CharacterConfig.Name;
//         }
//     }
// }