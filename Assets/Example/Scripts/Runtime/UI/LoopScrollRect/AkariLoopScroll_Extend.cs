using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 快捷方法/属性
    /// </summary>
    public partial class AkariLoopScroll<TData, TItemRenderer>
    {
        public int           TotalCount             => _owner.totalCount; //总数
        public RectTransform Content                => _owner.content;
        public RectTransform CacheRect              => _owner.u_CacheRect;
        public int           StartLine              => _owner.u_StartLine; //可见的第一行
        public int           CurrentLines           => _owner.u_CurrentLines; //滚动中的当前行数
        public int           TotalLines             => _owner.u_TotalLines; //总数
        public int           EndLine                => Mathf.Min(StartLine + CurrentLines, TotalLines); //可见的最后一行
        public int           ContentConstraintCount => _owner.u_ContentConstraintCount; //限制 行/列 数
        public float         ContentSpacing         => _owner.u_ContentSpacing; //间隔
        public int           ItemStart              => _owner.u_ItemStart; //当前显示的第一个的Index                
        public int           ItemEnd                => _owner.u_ItemEnd; //当前显示的最后一个index 被+1了注意                      

        //在开始时用startItem填充单元格，同时清除现有的单元格
        public void RefillCells(int startItem = 0, bool fillViewRect = false, float contentOffset = 0)
        {
            _owner.RefillCells(startItem, fillViewRect, contentOffset);
        }

        //在结束时重新填充endItem中的单元格，同时清除现有的单元格
        public void RefillCellsFromEnd(int endItem = 0, bool alignStart = false)
        {
            _owner.RefillCellsFromEnd(endItem, alignStart);
        }

        public void RefreshCells()
        {
            _owner.RefreshCells();
        }

        public void ClearCells()
        {
            _owner.ClearCells();
        }

        public int GetFirstItem(out float offset)
        {
            return _owner.GetFirstItem(out offset);
        }

        public int GetLastItem(out float offset)
        {
            return _owner.GetLastItem(out offset);
        }

        private int GetValidIndex(int index)
        {
            return Mathf.Clamp(index, 0, TotalCount - 1);
        }

        public void ScrollToCell(int index, float speed)
        {
            if (TotalCount <= 0) return;
            _owner.ScrollToCell(GetValidIndex(index), speed);
        }

        public void ScrollToCellWithinTime(int index, float time)
        {
            if (TotalCount <= 0) return;
            _owner.ScrollToCellWithinTime(GetValidIndex(index), time);
        }

        public void StopMovement()
        {
            _owner.StopMovement();
        }
    }
}