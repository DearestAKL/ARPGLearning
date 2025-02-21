using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace GameMain.Runtime
{
    public class JsonSerializer : ISerializer
    {
        /// <summary>
        /// 加载游戏配置。
        /// </summary>
        /// <returns></returns>
        public T Load<T>(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return default;
                }

                using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return Deserialize<T>(fileStream);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Load {filePath} failure with exception '{exception}'.");
                return default;
            }
        }

        /// <summary>
        /// 保存游戏配置。
        /// </summary>
        /// <returns>是否保存游戏配置成功。</returns>
        public bool Save(string filePath, object obj)
        {
            try
            {
                using FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                Serialize(fileStream, obj);
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Save {filePath} failure with exception '{exception}'.");
                return false;
            }
        }
        
        
        /// <summary>
        /// 序列化数据。
        /// </summary>
        /// <param name="stream">目标流。</param>
        public void Serialize(Stream stream,object obj)
        {
            using var writer = new StreamWriter(stream);
            string json = JsonConvert.SerializeObject(obj);
            writer.Write(json);
        }

        /// <summary>
        /// 反序列化数据。
        /// </summary>
        /// <param name="stream">指定流。</param>
        public T Deserialize<T>(Stream stream)
        {
            using var reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}