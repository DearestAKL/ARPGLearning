using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 矩形警告
    /// 以边填充
    /// </summary>
    public class WarningRectangle : AWarningObject
    {        
        [SerializeField] private Transform topTransform;

        private float _length;
        private float _weight;
        //朝向
        private Vector2 rotation;
        
        public void SetData(WarningData warningData, float weight,float length)
        {
            SetWarningData(warningData);

            _weight = weight;
            _length = length;
            
            var localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x * 2 * weight, localScale.y, localScale.z * 2 * length);
            topTransform.localPosition = new Vector3(0, 0, -0.5F);
            topTransform.localScale = Vector3.zero;
        }

        protected override void UpdateProgress(float rate)
        {
            topTransform.localScale = new Vector3(1, rate, 1);
            topTransform.localPosition = new Vector3(0, 0, (rate - 1) / 2f);
        }
        
        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            transform.DrawBox(_weight, _length, Color.cyan);
#endif
        }
    }
}