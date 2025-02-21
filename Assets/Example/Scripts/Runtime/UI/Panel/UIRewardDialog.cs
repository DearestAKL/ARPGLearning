using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class UIRewardDialog : UIRewardDialogSign
    {
        public class Params
        {
            public List<AUIItemData> ItemDataList;

            public Params(List<AUIItemData> itemDataList)
            {
                ItemDataList = itemDataList;
            }
        }

        private Params _data;

        private float defaultPaddingTop = 0; 
        
        private AkariLoopScroll<AUIItemData, UIItemCell> _akariLoopScroll;
        
        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);

            btnBg.onClick.AddListener(Close);
            
            _akariLoopScroll = new AkariLoopScroll<AUIItemData, UIItemCell>(itemScrollView, OnItemUpdateView);
        }
        
        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
            if (userData is Params data)
            {
                _data = data;
                
                //数量少时自动居中
                if (_data.ItemDataList.Count > 14)
                {
                    layoutGroup.childAlignment = TextAnchor.MiddleLeft;
                }
                else
                {
                    layoutGroup.childAlignment = TextAnchor.MiddleCenter;
                }
                
                _akariLoopScroll.SetDataRefresh(_data.ItemDataList);
            }
        }
        
        private void OnItemUpdateView(int index, AUIItemData data, UIItemCell item, bool select)
        {
            item.UpdateView(data, select);
        }
    }
}