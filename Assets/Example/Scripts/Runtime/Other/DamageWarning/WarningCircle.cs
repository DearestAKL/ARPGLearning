using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 圆形警告
    /// 以圆心向外填充
    /// </summary>
    public class WarningCircle : AWarningObject
    {
        [SerializeField] private Transform topTransform;
        private float _radius = 0.5f;
        
        public void SetData(WarningData warningData, float radius)
        {
            SetWarningData(warningData);
            
            _radius = radius;
            
            var localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x * 2 * radius, localScale.y, localScale.z * 2 * radius);
            topTransform.localScale = Vector3.zero;
        }
        
        protected override void UpdateProgress(float rate)
        {
            topTransform.localScale = new Vector3(rate, rate, 1);
        }
        
        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            transform.DrawCircle(_radius, Color.cyan);
#endif
        }
    }
}