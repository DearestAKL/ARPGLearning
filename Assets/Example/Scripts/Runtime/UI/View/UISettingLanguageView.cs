using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UISettingLanguageView : MonoBehaviour
    {
        [SerializeField] private UISettingDropdown dropdownTextLanguage = null;
        [SerializeField] private UISettingDropdown dropdownVoiceLanguage = null;
        
        private LanguageType[] _textLanguageValues;
        private LanguageType[] _voiceLanguageValues;
        
        private void Awake()
        {
            //设置语言
            _textLanguageValues = (LanguageType[])Enum.GetValues(typeof(LanguageType));
            var curTextLanguageValue = SettingManager.Instance.GetInt(Constant.Setting.TextLanguage, (int)LanguageType.Chinese);
            var curTextLanguageIndex = 0;//当前dropdown index
            List<string> textLanguageOptions = new List<string>(_textLanguageValues.Length);
            for (int i = 0; i < _textLanguageValues.Length; i++)
            {
                var str = _textLanguageValues[i].ToString();
                textLanguageOptions.Add(str);
                if (curTextLanguageValue == (int)_textLanguageValues[i])
                {
                    curTextLanguageIndex = i;
                }
            }
            dropdownTextLanguage.AddOptions(textLanguageOptions,curTextLanguageIndex);
            dropdownTextLanguage.OnValueChange.AddListener(OnChangeTextLanguage);
            
            //设置语音
            _voiceLanguageValues = new LanguageType[] { LanguageType.Chinese, LanguageType.English };
            var curVoiceLanguageValue = SettingManager.Instance.GetInt(Constant.Setting.VoiceLanguage, (int)LanguageType.Chinese);
            var curVoiceLanguageIndex = 0;//当前dropdown index
            List<string> voiceLanguageOptions = new List<string>(_voiceLanguageValues.Length);
            for (int i = 0; i < _voiceLanguageValues.Length; i++)
            {
                var str = _voiceLanguageValues[i].ToString();
                voiceLanguageOptions.Add(str);
                if (curVoiceLanguageValue == (int)_voiceLanguageValues[i])
                {
                    curVoiceLanguageIndex = i;
                }
            }
            dropdownVoiceLanguage.AddOptions(voiceLanguageOptions,curVoiceLanguageIndex);
            dropdownVoiceLanguage.OnValueChange.AddListener(OnChangeVoiceLanguage);
        }
        
        private void OnChangeTextLanguage(int index)
        {
            SettingManager.Instance.SetInt(Constant.Setting.TextLanguage,(int)_textLanguageValues[index]);
        }

        private void OnChangeVoiceLanguage(int index)
        {
            SettingManager.Instance.SetInt(Constant.Setting.VoiceLanguage,(int)_voiceLanguageValues[index]);
        }
    }
}