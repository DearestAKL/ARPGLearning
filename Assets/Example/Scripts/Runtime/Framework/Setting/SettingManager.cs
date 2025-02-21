using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Akari.GfCore;

namespace GameMain.Runtime
{
    public class SettingManager : GfSingleton<SettingManager>
    {
        private const string SettingFileName = "AkariSetting.json";
        private Dictionary<string, string> _settings = new Dictionary<string, string>();
        private string SettingFilePath => SettingFileName.AddPersistentDataPath();
        
        private ISerializer _serializer;
        
        protected override void OnCreated()
        {
            _serializer = new JsonSerializer();
            LoadSetting();
        }
        
        protected override void OnDisposed()
        {
            base.OnDisposed();
            SaveSetting();
        }

        /// <summary>
        /// 加载游戏配置。
        /// </summary>
        /// <returns>是否加载游戏配置成功。</returns>
        private bool LoadSetting()
        {
            _settings.Clear();
            var loaded = _serializer.Load<Dictionary<string, string>>(SettingFilePath);
            _settings = loaded ?? _settings;

            return false;
        }
        
        /// <summary>
        /// 保存游戏配置。
        /// </summary>
        /// <returns>是否保存游戏配置成功。</returns>
        private bool SaveSetting()
        {
            return _serializer.Save(SettingFilePath, _settings);
        }

        /// <summary>
        /// 从指定游戏配置项中读取布尔值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <param name="defaultValue"></param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string settingName, bool defaultValue = false)
        {
            if (!_settings.TryGetValue(settingName, out var value))
            {
                return defaultValue;
            }

            return int.Parse(value) != 0;
        }
        
        /// <summary>
        /// 向指定游戏配置项写入布尔值。
        /// </summary>
        /// <param name="settingName">要写入游戏配置项的名称。</param>
        /// <param name="value">要写入的布尔值。</param>
        public void SetBool(string settingName, bool value)
        {
            _settings[settingName] = value ? "1" : "0";
        }

        /// <summary>
        /// 从指定游戏配置项中读取整数值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <param name="defaultValue">当指定的游戏配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string settingName, int defaultValue = 0)
        {
            if (!_settings.TryGetValue(settingName, out var value))
            {
                Debug.LogWarning($"Setting '{settingName}' is not exist.");
                return defaultValue;
            }

            return int.Parse(value);
        }
        
        /// <summary>
        /// 向指定游戏配置项写入整数值。
        /// </summary>
        /// <param name="settingName">要写入游戏配置项的名称。</param>
        /// <param name="value">要写入的整数值。</param>
        public void SetInt(string settingName, int value)
        {
            _settings[settingName] = value.ToString();
        }
        
        /// <summary>
        /// 从指定游戏配置项中读取浮点数值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <param name="defaultValue">当指定的游戏配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string settingName, float defaultValue = 0f)
        {
            if (!_settings.TryGetValue(settingName, out var value))
            {
                return defaultValue;
            }

            return float.Parse(value);
        }
        
        /// <summary>
        /// 向指定游戏配置项写入浮点数值。
        /// </summary>
        /// <param name="settingName">要写入游戏配置项的名称。</param>
        /// <param name="value">要写入的浮点数值。</param>
        public void SetFloat(string settingName, float value)
        {
            _settings[settingName] = value.ToString();
        }
        
        /// <summary>
        /// 从指定游戏配置项中读取字符串值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <param name="defaultValue">当指定的游戏配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string settingName, string defaultValue ="")
        {
            if (!_settings.TryGetValue(settingName, out var value))
            {
                return defaultValue;
            }

            return value;
        }
        
        /// <summary>
        /// 向指定游戏配置项写入字符串值。
        /// </summary>
        /// <param name="settingName">要写入游戏配置项的名称。</param>
        /// <param name="value">要写入的字符串值。</param>
        public void SetString(string settingName, string value)
        {
            _settings[settingName] = value;
        }
    }
}