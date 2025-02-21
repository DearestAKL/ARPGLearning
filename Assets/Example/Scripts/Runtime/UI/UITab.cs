// using System;
// using System.Collections.Generic;
// using Akari.GfCore;
// using TMPro;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.UI;
//
// namespace GameMain.Runtime
// {
//     public class UITabGroup : MonoBehaviour
//     {
//         [SerializeField] private List<UITab> taps = new List<UITab>();
//         
//         public UnityEvent<string> OnValueChanged = new UnityEvent<string>();
//         
//         private UITab _curOnTap;
//
//         private void Awake()
//         {
//             //全置为空
//             foreach (var tap in taps)
//             {
//                 tap.SetIsOn(false);
//             }
//         }
//
//         public void SetCurTab(string key)
//         {
//             foreach (var tap in taps)
//             {
//                 if (tap.Key == key)
//                 {
//                     _curOnTap = tap;
//                     _curOnTap.SetIsOn(true);
//                     OnValueChanged.Invoke(key);
//                     return;
//                 }
//             }
//             
//             GfLog.Error($"SetCurTab can not find {key}");
//         }
//
//         public void ChangeCurTab(UITab tap)
//         {
//             if (_curOnTap == tap)
//             {
//                 return;
//             }
//             
//             _curOnTap.SetIsOn(false);
//             _curOnTap = tap;
//
//             OnValueChanged.Invoke(_curOnTap.Key);
//         }
//     }
//
//     public class UITab : MonoBehaviour
//     {
//         private enum Type
//         {
//             None,
//             Label,
//             Color
//         }
//         
//         [SerializeField] private Button btnClick;
//         [SerializeField] private TextMeshProUGUI txtLabel;
//         
//         [SerializeField] private UITabGroup tabGroup;
//         [SerializeField] private string key;
//         
//         [SerializeField] private Type type = Type.None;
//         //可空
//         [SerializeField] private TextMeshProUGUI txtOnLabel;
//         [SerializeField] private Color colorNormal;
//         [SerializeField] private Color colorOn;
//
//         private bool _isOn;
//         public bool IsOn => _isOn;
//         public string Key => key;
//
//         private void Awake()
//         {
//             btnClick.onClick.AddListener(OnClick);
//         }
//
//         public void SetIsOn(bool isOn)
//         {
//             _isOn = isOn;
//             UpdateView();
//         }
//
//         public void OnClick()
//         {
//             if (_isOn) { return; }
//             _isOn = true;
//             UpdateView();
//             tabGroup.ChangeCurTab(this);
//         }
//
//         private void UpdateView()
//         {
//             if (type == Type.Label)
//             {
//                 if (txtOnLabel != null)
//                 {
//                     txtOnLabel.gameObject.SetActive(_isOn);
//                 }
//             }
//             else if(type == Type.Color)
//             {
//                 txtLabel.color = _isOn ? colorOn : colorNormal;
//             }
//         }
//     }
// }