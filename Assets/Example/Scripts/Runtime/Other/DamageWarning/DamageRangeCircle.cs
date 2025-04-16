using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 圆形警告
    /// 以圆心向外填充
    /// </summary>
    public class DamageRangeCircle : ADamageRangeObject
    {
        [SerializeField] private Transform topTransform;
        private float _radius = 0.5f;
        
        public void SetData(DamageRangeData damageRangeData, float radius)
        {
            SetWarningData(damageRangeData);
            
            _radius = radius;
            
            var localScale = transform.localScale;
            transform.localScale = new Vector3(2 * radius, localScale.y, 2 * radius);
            topTransform.localScale = Vector3.zero;
        }
        
        protected override void UpdateProgress(float rate)
        {
            topTransform.localScale = new Vector3(rate, rate, 1);
        }
    }
}