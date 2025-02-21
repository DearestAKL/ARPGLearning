//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System.Collections.Generic;
using Akari.GfUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class LoopScrollItem : MonoBehaviour
    {
        [SerializeField] protected Button btnClick;

        public void AddEventClick(UnityAction action)
        {
            if (btnClick == null)
            {
                return;
            }
            btnClick.onClick.RemoveAllListeners();
            btnClick.onClick.AddListener(action);
        }
    }

    public partial class AkariLoopScroll<TData, TItemRenderer> : LoopScrollPrefabSource, LoopScrollDataSource
        where TItemRenderer : LoopScrollItem
    {
        /// <summary>
        /// 列表项渲染器
        /// </summary>
        /// <param name="index">数据的索引</param>
        /// <param name="data">数据项</param>
        /// <param name="item">显示对象</param>
        /// <param name="select">是否被选中</param>
        public delegate void OnItemUpdateView(int index, TData data, TItemRenderer item, bool select);
        private OnItemUpdateView                       _onItemUpdateView;
        private IList<TData>                         _data;
        private LoopScrollRect                       _owner;
        private Dictionary<Transform, TItemRenderer> _itemTransformDic = new Dictionary<Transform, TItemRenderer>();
        private Dictionary<Transform, int>           m_ItemTransformIndexDic = new Dictionary<Transform, int>();

        public AkariLoopScroll(
            LoopScrollRect   owner,
            OnItemUpdateView onItemUpdateView)
        {
            _itemTransformDic.Clear();
            m_ItemTransformIndexDic.Clear();
            _onItemUpdateView       = onItemUpdateView;
            _owner              = owner;
            _owner.prefabSource = this;
            _owner.dataSource   = this;
            InitCacheParent();
            InitClearContent();
        }

        #region Private

        private void InitCacheParent()
        {
            if (_owner.u_CacheRect != null)
            {
                _owner.u_CacheRect.gameObject.SetActive(false);
            }
            else
            {
                var cacheObj  = new GameObject("Cache");
                var cacheRect = cacheObj.GetOrAddComponent<RectTransform>();
                _owner.u_CacheRect = cacheRect;
                cacheRect.SetParent(_owner.transform, false);
                cacheObj.SetActive(false);
            }
        }

        private void InitClearContent()
        {
            //不应该初始化时有内容 所有不管是什么全部摧毁
            for (var i = 0; i < Content.childCount; i++)
            {
                var child = Content.GetChild(i);
                Object.Destroy(child.gameObject);
            }
        }

        private TItemRenderer GetItemRendererByDic(Transform tsf)
        {
            if (_itemTransformDic.TryGetValue(tsf, out var value))
            {
                return value;
            }

            Debug.LogError($"{tsf.name} 没找到这个关联对象 请检查错误");
            return null;
        }

        private void AddItemRendererByDic(Transform tsf, TItemRenderer item)
        {
            if (!_itemTransformDic.ContainsKey(tsf))
            {
                _itemTransformDic.Add(tsf, item);
            }
        }

        private int GetItemIndex(Transform tsf)
        {
            if (m_ItemTransformIndexDic.TryGetValue(tsf, out var value))
            {
                return value;
            }

            return -1;
        }

        private void ResetItemIndex(Transform tsf, int index)
        {
            if (!m_ItemTransformIndexDic.ContainsKey(tsf))
            {
                m_ItemTransformIndexDic.Add(tsf, index);
            }
            else
            {
                m_ItemTransformIndexDic[tsf] = index;
            }
        }

        #endregion

        #region LoopScrollRect Interface
        public GameObject GetObject(int index)
        {
            var uiBase = GfPrefabPool.IssueOrCreatPool(_owner.ItemPrefab);
            
            TItemRenderer itemRenderer = uiBase.GetComponent<TItemRenderer>();
            AddItemRendererByDic(uiBase.transform, itemRenderer);
            AddOnClickEvent(itemRenderer);
            
            return uiBase.gameObject;
        }

        public void ReturnObject(Transform transform)
        {
            var uiBase = GetItemRendererByDic(transform);
            if (uiBase == null) return;
            GfPrefabPool.Return(transform);
            ResetItemIndex(transform, -1);
            transform.SetParent(_owner.u_CacheRect, false);
        }

        public void ProvideData(Transform transform, int index)
        {
            var uiBase = GetItemRendererByDic(transform);
            if (uiBase == null) return;
            ResetItemIndex(transform, index);
            var select = _onClickItemHashSet.Contains(index);
            if (_data == null)
            {
                Debug.LogError($"当前没有设定数据 m_Data == null");
                return;
            }

            _onItemUpdateView?.Invoke(index, _data[index], uiBase, select);
        }

        #endregion
    }
}