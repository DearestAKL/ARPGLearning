using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    [RequireComponent(typeof(ToggleGroup))]
    public class UIToggleGroupEx : MonoBehaviour
    {
        [SerializeField] private UIToggleEx toggleExPrefab;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private List<UIToggleEx> toggleExes = new List<UIToggleEx>();

        private int _curTypeId = -1;
        public UnityEvent<int> OnToggleChanged = new UnityEvent<int>();

        public void CreateToggleEx(int index, string toggleName = null,string iconPath = null)
        {
            UIToggleEx toggleEx = null;
            //是否已经有符合条件的Toggle
            for (int i = 0; i < toggleExes.Count; i++)
            {
                if (toggleExes[i].Index == index)
                {
                    toggleEx = toggleExes[i];
                    break;
                }
            }

            //是否有待使用的Toggle
            if (toggleEx == null)
            {
                for (int i = 0; i < toggleExes.Count; i++)
                {
                    //待使用
                    if (toggleExes[i].Index < 0)
                    {
                        toggleEx = toggleExes[i];
                        toggleEx.Init(this, index, toggleName,iconPath);
                        break;
                    }
                }
            }

            //创建一个新的的Toggle
            if (toggleEx == null)
            {
                toggleEx = Instantiate(toggleExPrefab, transform);
                toggleEx.Toggle.group = toggleGroup;
                toggleEx.Init(this, index, toggleName,iconPath);
                toggleExes.Add(toggleEx);
            }
            
            toggleEx.gameObject.SetActive(true);
        }

        public bool SetToggleOn(int typeId)
        {
            foreach (var toggleEx in toggleExes)
            {
                if (toggleEx.Index == typeId)
                {
                    if (toggleEx.Toggle.isOn)
                    {
                        return false;
                    }
                    else
                    {
                        toggleEx.Toggle.isOn = true;
                        return true;
                    }
                }
            }
            
            return false;
        }

        public void ChangeCurIndex(int index)
        {
            if (_curTypeId == index)
            {
                return;
            }

            _curTypeId = index;
            OnToggleChanged.Invoke(_curTypeId);
        }

        public void SortSibling()
        {
            toggleExes.Sort((a, b) => a.Index.CompareTo(b.Index));//从小到大
            foreach (var toggleEx in toggleExes)
            {
                toggleEx.transform.SetAsLastSibling();
            }
        }


        public void Clear()
        {
            _curTypeId = -1;
            OnToggleChanged.RemoveAllListeners();
            for (int i = 0; i < toggleExes.Count; i++)
            {
                toggleExes[i].Clear();
            }
        }
    }
}