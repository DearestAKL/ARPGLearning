using GameMain.Runtime;

namespace UnityEngine.UI
{
    public abstract partial class LoopScrollRect
    {
        [SerializeField]
        internal LoopScrollItem u_ItemPrefab;
        
        [SerializeField]
        //缓存父级对象
        internal RectTransform u_CacheRect;

        //最大可点击数
        [SerializeField]
        internal int u_MaxClickCount = 1;

        //自动取消上一个选择
        [SerializeField]
        internal bool u_AutoCancelLast = true;
        
        //重复点击则取消
        [SerializeField]
        internal bool u_RepetitionCancel;

        internal LoopScrollItem   ItemPrefab         => u_ItemPrefab;                                          
        internal int   u_StartLine              => StartLine;                                             //可见的第一行
        internal int   u_CurrentLines           => CurrentLines;                                          //滚动中的当前行数
        internal int   u_TotalLines             => TotalLines;                                            //总数
        internal int   u_EndLine                => Mathf.Min(u_StartLine + u_CurrentLines, u_TotalLines); //可见的最后一行
        internal int   u_ContentConstraintCount => contentConstraintCount;                                //限制 行/列 数
        internal float u_ContentSpacing         => contentSpacing;                                        //间隔
        internal int   u_ItemStart              => itemTypeStart;                                         //当前显示的第一个的Index 
        internal int   u_ItemEnd                => itemTypeEnd;                                           //当前显示的最后一个index 被+1了注意
    }
}