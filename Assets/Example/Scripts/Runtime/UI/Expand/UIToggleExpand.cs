using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    [RequireComponent(typeof(Toggle))]
    public class UIToggleExpand : MonoBehaviour
    {
        [SerializeField] private ButtonSoundType onSoundType;
        [SerializeField] private ButtonSoundType offSoundType;

        private Toggle _toggle;
        
        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool isOn)
        {
            if (isOn)
            {
                PlayButtonSound(onSoundType);
            }
            else
            {
                PlayButtonSound(offSoundType);
            }
        }

        private void PlayButtonSound(ButtonSoundType soundType)
        {
            if (soundType == ButtonSoundType.None)
            {
                return;
            }
            
            AudioManager.Instance.PlaySound(soundType.GetSoundAssetName());
        }
    }
}