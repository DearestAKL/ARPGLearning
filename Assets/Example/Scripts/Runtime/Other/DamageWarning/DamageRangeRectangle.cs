using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 矩形警告
    /// 以边填充
    /// </summary>
    public class DamageRangeRectangle : ADamageRangeObject
    {        
        [SerializeField] private Transform topTransform;

        private float _length;
        private float _width;
        //朝向
        private Vector2 rotation;
        
        public void SetData(DamageRangeData damageRangeData, float width,float length)
        {
            SetWarningData(damageRangeData);

            _width = width;
            _length = length;
            
            var localScale = transform.localScale;
            transform.localScale = new Vector3(2 * width, localScale.y, 2 * length);
            topTransform.localPosition = new Vector3(0, 0, -0.5F);
            topTransform.localScale = Vector3.zero;
        }

        protected override void UpdateProgress(float rate)
        {
            topTransform.localScale = new Vector3(1, rate, 1);
            topTransform.localPosition = new Vector3(0, 0, (rate - 1) / 2f);
        }
    }
}