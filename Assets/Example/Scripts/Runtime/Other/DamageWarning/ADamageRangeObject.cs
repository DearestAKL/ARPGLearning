using System;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public abstract class ADamageRangeObject : MonoBehaviour
    {
        private DamageRangeData damageRangeData;
        
        protected void SetWarningData(DamageRangeData damageRangeData)
        {
            this.damageRangeData = damageRangeData;
        }
        
        private void ReturnToPool()
        {
            GfPrefabPool.Return(this);
        }

        public void Update()
        {
            if (damageRangeData == null)
            {
                return;
            }
            
            if (damageRangeData.IsCompleted)
            {
                ReturnToPool();
            }
            else
            {
                UpdateProgress(damageRangeData.Rate);
            }
        }
        protected abstract void UpdateProgress(float rate);
    }
}