using System;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public abstract class AWarningObject : MonoBehaviour
    {
        private WarningData _warningData;
        
        protected void SetWarningData(WarningData warningData)
        {
            _warningData = warningData;
        }
        
        private void ReturnToPool()
        {
            GfPrefabPool.Return(this);
        }

        public void Update()
        {
            if (_warningData.IsCompleted)
            {
                ReturnToPool();
            }
            else
            {
                UpdateProgress(_warningData.Rate);
            }
        }
        protected abstract void UpdateProgress(float rate);
    }
}