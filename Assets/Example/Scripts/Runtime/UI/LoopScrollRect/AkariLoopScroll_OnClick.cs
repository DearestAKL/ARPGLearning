using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 额外点击相关
    /// </summary>
    public partial class AkariLoopScroll<TData, TItemRenderer>
    {
        /// <summary>
        /// 列表元素被点击的事件
        /// </summary>
        public delegate void OnClickItemEvent(int index, TData data, TItemRenderer item, bool select);

        private bool             _onClickInit;                             //是否已初始化
        private OnClickItemEvent _onClickItemEvent;                        //点击回调
        private Queue<int>       _onClickItemQueue   = new Queue<int>();   //当前所有已选择 遵循先进先出 有序
        private HashSet<int>     _onClickItemHashSet = new HashSet<int>(); //当前所有已选择 无序 为了更快查找
        private int              _maxClickCount      = 1;                  //可选最大数量 >=2 就是复选 最小1
        private bool             _repetitionCancel = true;                 //重复选择 则取消选择
        private bool             _autoCancelLast = true;                   //当选择操作最大数量过后 自动取消第一个选择的 否则选择无效

        public AkariLoopScroll<TData, TItemRenderer> SetOnClickInfo(OnClickItemEvent onClickItemEvent)
        {
            if (_onClickInit)
            {
                Debug.LogError($"OnClick 相关只能初始化一次 且不能修改");
                return this;
            }

            if (onClickItemEvent == null)
            {
                Debug.LogError($"必须有点击事件");
                return this;
            }

            _maxClickCount      = Mathf.Max(1, _owner.u_MaxClickCount);
            _onClickItemEvent   = onClickItemEvent;
            _repetitionCancel   = _owner.u_RepetitionCancel;
            _onClickInit        = true;
            _autoCancelLast     = _owner.u_AutoCancelLast;
            _onClickItemQueue.Clear();
            _onClickItemHashSet.Clear();
            return this;
        }

        //reset=吧之前选择的都取消掉 讲道理应该都是true
        //false出问题自己查
        public void ClearSelect(bool reset = true)
        {
            if (reset)
            {
                var selectCount = _onClickItemHashSet.Count;
                for (var i = 0; i < selectCount; i++)
                {
                    OnClickItemQueuePeek();
                }
            }

            _onClickItemQueue.Clear();
            _onClickItemHashSet.Clear();
        }

        //动态改变 自动取消上一个选择的
        public void ChangeAutoCancelLast(bool autoCancelLast)
        {
            _autoCancelLast = autoCancelLast;
        }

        //动态改变 重复选择 则取消选择
        public void ChangeRepetitionCancel(bool repetitionCancel)
        {
            _repetitionCancel = repetitionCancel;
        }

        //动态改变 最大可选数量
        public void ChangeMaxClickCount(int count, bool reset = true)
        {
            ClearSelect(reset);
            _maxClickCount = Mathf.Max(1, count);
        }

        //传入对象 选中目标
        public void OnClickItem(TItemRenderer item)
        {
            var index  = GetItemIndex(item);
            if (index < 0)
            {
                Debug.LogError($"无法选中一个不在显示中的对象");
                return;
            }
            var select = OnClickItemQueueEnqueue(index);
            OnClickItem(index, item, select);
        }
        
        //传入索引 选中目标
        public void OnClickItem(int index)
        {
            if (index < 0 || index >= _data.Count)
            {
                Debug.LogError($"索引越界{index}  0 - {_data.Count}");
                return;
            }
            var item  = GetItemByIndex(index,false);
            var select = OnClickItemQueueEnqueue(index);
            if (item != null)
            {
                OnClickItem(index, item, select);
            }
        }
        
        private bool OnClickItemQueueEnqueue(int index)
        {
            if (_onClickItemHashSet.Contains(index))
            {
                if (_repetitionCancel)
                {
                    RemoveSelectIndex(index);
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (_onClickItemQueue.Count >= _maxClickCount)
            {
                if (_autoCancelLast)
                {
                    OnClickItemQueuePeek();
                }
                else
                {
                    return false;
                }
            }

            OnClickItemHashSetAdd(index);
            _onClickItemQueue.Enqueue(index);
            return true;
        }

        private void SetDefaultSelect(int index)
        {
            OnClickItemQueueEnqueue(index);
        }

        private void SetDefaultSelect(List<int> indexs)
        {
            foreach (var index in indexs)
            {
                SetDefaultSelect(index);
            }
        }

        private void OnClickItem(int index, TItemRenderer item, bool select)
        {
            _onClickItemEvent?.Invoke(index, _data[index], item, select);
        }

        private void AddOnClickEvent(TItemRenderer uiBase)
        {
            if (!_onClickInit) return;
            uiBase.AddEventClick(() => { OnClickItem(uiBase); });
        }

        private void OnClickItemQueuePeek()
        {
            var index = _onClickItemQueue.Dequeue();
            OnClickItemHashSetRemove(index);
            if (index < ItemStart || index >= ItemEnd) return;
            var item = GetItemByIndex(index);
            if (item != null)
                OnClickItem(index, item, false);
        }

        private void OnClickItemHashSetAdd(int index)
        {
            _onClickItemHashSet.Add(index);
        }

        private void OnClickItemHashSetRemove(int index)
        {
            _onClickItemHashSet.Remove(index);
        }

        private void RemoveSelectIndex(int index)
        {
            var list = _onClickItemQueue.ToList();
            list.Remove(index);
            _onClickItemQueue = new Queue<int>(list);
            OnClickItemHashSetRemove(index);
        }
    }
}