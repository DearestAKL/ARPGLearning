using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UISettingGraphicsView : MonoBehaviour
    {
        [SerializeField] private UISettingDropdown dropdownScreenMode = null;
        [SerializeField] private UISettingDropdown dropdownResolution = null;

        private ScreenModeType[] _screenModeValues;
        private ResolutionType[] _resolutionValues;
        
        private void Awake()
        {
            //设置屏幕
            //画面
            _screenModeValues = (ScreenModeType[])Enum.GetValues(typeof(ScreenModeType));
            var curScreenModeValue = SettingManager.Instance.GetInt(Constant.Setting.ScreenMode, (int)ScreenModeType.FullScreen);
            var curScreenModeIndex = 0;//当前dropdown index
            List<string> screenModeOptions = new List<string>(_screenModeValues.Length);
            for (int i = 0; i < _screenModeValues.Length; i++)
            {
                var str = _screenModeValues[i].ToString();//TODO:本地化
                screenModeOptions.Add(str);
                if (curScreenModeValue == (int)_screenModeValues[i])
                {
                    curScreenModeIndex = i;
                }
            }
            dropdownScreenMode.AddOptions(screenModeOptions,curScreenModeIndex);
            dropdownScreenMode.OnValueChange.AddListener(OnChangeScreenMode);
            
            //分辨率
            _resolutionValues = (ResolutionType[])Enum.GetValues(typeof(ResolutionType));
            var curResolutionValue = SettingManager.Instance.GetInt(Constant.Setting.Resolution, (int)ResolutionType.R1920x1080);
            var curResolutionIndex = 0;//当前dropdown index
            List<string> resolutionOptions = new List<string>(_resolutionValues.Length);
            for (int i = 0; i < _resolutionValues.Length; i++)
            {
                var str = _resolutionValues[i].ToString().Remove(0, 1);
                resolutionOptions.Add(str);
                if (curResolutionValue == (int)_resolutionValues[i])
                {
                    curResolutionIndex = i;
                }
            }
            dropdownResolution.AddOptions(resolutionOptions, curResolutionIndex);
            dropdownResolution.OnValueChange.AddListener(OnChangeResolution);
        }

        private void OnChangeScreenMode(int index)
        {
            SettingManager.Instance.SetInt(Constant.Setting.ScreenMode,(int)_screenModeValues[index]);
            SettingHelper.SetScreenMode(_screenModeValues[index]);
        }
        
        private void OnChangeResolution(int index)
        {
            SettingManager.Instance.SetInt(Constant.Setting.Resolution,(int)_resolutionValues[index]);
            SettingHelper.SetResolution(_resolutionValues[index]);
        }
    }
}