using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public static class UnityExtension
    {
        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <typeparam name="T">要获取或增加的组件。</typeparam>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>获取或增加的组件。</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        public static string AddPersistentDataPath(this string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName).Replace('\\', '/');
        }
        
        /// <summary>
        /// 查找指定深度内的组件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="depth">深度 默认为1</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetComponentsInChildrenWithDepth<T>(this Transform parent, int depth = 1) where T : Component
        {
            // 如果深度小于等于0，返回空数组
            if (depth <= 0)
            {
                return new T[0];
            }

            // 用于存储找到的组件的列表
            var foundComponents = new System.Collections.Generic.List<T>();

            // 递归查找组件
            SearchInChildrenWithDepth(parent, depth, 1, foundComponents);

            // 将列表转换为数组并返回
            return foundComponents.ToArray();
        }

        // 递归查找组件的辅助函数
        private static void SearchInChildrenWithDepth<T>(Transform parent, int targetDepth, int currentDepth, System.Collections.Generic.List<T> foundComponents) where T : Component
        {
            foreach (Transform child in parent)
            {
                // 获取子对象上的组件
                T component = child.GetComponent<T>();
            
                if (component != null)
                {
                    // 如果找到组件，将其添加到列表中
                    foundComponents.Add(component);
                }

                // 递归查找下一层子对象
                if (currentDepth < targetDepth)
                {
                    SearchInChildrenWithDepth(child, targetDepth, currentDepth + 1, foundComponents);
                }
            }
        }

        #region Transform
        public static void SetLocalPosX(this Transform transform, float value)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(value, localPosition.y, localPosition.z);
            transform.localPosition = localPosition;
        }
        
        public static void SetLocalPosY(this Transform transform, float value)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x, value, localPosition.z);
            transform.localPosition = localPosition;
        }
        
        public static void SetLocalPosZ(this Transform transform, float value)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y, value);
            transform.localPosition = localPosition;
        }

        #endregion
        
        #region TextMeshProUGUI

        public static void FormatLocalization(this TextMeshProUGUI textMeshProUGUI,string format)
        {
            format = format.GetLocalization();
            
            textMeshProUGUI.text = format;
        }
        
        public static void FormatLocalization(this TextMeshProUGUI textMeshProUGUI,string format,object arg0)
        {
            format = format.GetLocalization();
            
            textMeshProUGUI.text = string.Format(format, arg0);
        }
        
        public static void FormatLocalization(this TextMeshProUGUI textMeshProUGUI,string format,object arg0, object arg1)
        {
            format = format.GetLocalization();
            
            textMeshProUGUI.text = string.Format(format, arg0, arg1);
        }
        
        public static void FormatLocalization(this TextMeshProUGUI textMeshProUGUI,string format,object arg0, object arg1, object arg2)
        {
            format = format.GetLocalization();
            
            textMeshProUGUI.text = string.Format(format, arg0, arg1, arg2);
        }
        
        public static void FormatLocalization(this TextMeshProUGUI textMeshProUGUI,string format, params object[] args)
        {
            format = format.GetLocalization();
            
            textMeshProUGUI.text = string.Format(format, args);
        }

        public static string GetLocalization(this string key)
        {
            var localization = LubanManager.Instance.Tables.TbLocalization.GetOrDefault(key);

            return localization == null ? "未本地化" : localization.Cn;
        }
        #endregion
        
        #region Image

        public static async void SetIcon(this Image image,string path)
        {
            if (image.TryGetComponent<UIImageExpand>(out var imageExpand))
            {
                imageExpand.OnBeforeLoadIcon();
            }
            
            //异步加载过程中图片如何表现
            image.sprite = await AssetManager.Instance.LoadAsset<Sprite>(path);
            
            if (imageExpand != null)
            {
                imageExpand.OnAfterLoadIcon();
            }
        }
        #endregion
    }
}