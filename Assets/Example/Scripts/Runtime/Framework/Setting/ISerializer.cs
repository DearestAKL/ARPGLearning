using System.IO;

namespace GameMain.Runtime
{
    public interface ISerializer
    {
        /// <summary>
        /// 加载游戏配置。
        /// </summary>
        /// <returns>是否读取配置成功</returns>
        T Load<T>(string filePath);

        /// <summary>
        /// 保存游戏配置。
        /// </summary>
        /// <returns>是否保存配置成功。</returns>
        bool Save(string filePath, object obj);
        
        /// <summary>
        /// 序列化数据。
        /// </summary>
        /// <param name="stream">目标流。</param>
        void Serialize(Stream stream, object obj);

        /// <summary>
        /// 反序列化数据。
        /// </summary>
        /// <param name="stream">指定流。</param>
        T Deserialize<T>(Stream stream);
    }
}