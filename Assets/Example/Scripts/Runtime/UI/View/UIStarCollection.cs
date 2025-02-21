using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Runtime
{
    public class UIStarCollection : MonoBehaviour
    {
        [SerializeField] private List<GameObject> activeGameObjects;
        [SerializeField] private List<GameObject> previewGameObjects;

        public void UpdateView(int curNum,bool isPreviewNext = false)
        {
            for (int i = 0; i < activeGameObjects.Count; i++)
            {
                activeGameObjects[i].SetActive(curNum > i);
            }
            
            for (int i = 0; i < previewGameObjects.Count; i++)
            {
                previewGameObjects[i].SetActive(isPreviewNext && curNum == i);
            }
        }
    }
}