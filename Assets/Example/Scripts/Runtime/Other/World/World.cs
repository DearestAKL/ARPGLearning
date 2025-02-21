using UnityEngine;

namespace GameMain.Runtime
{
    public class World : MonoBehaviour
    {
        private float _elapsedTime;
        public bool CheckTime(float deltaTime)
        {
            _elapsedTime += deltaTime;
            if (_elapsedTime > 100)//1000s
            {
                Destroy(this);
                return false;
            }
            return true;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            _elapsedTime = 0f;

            if (active)
            {
                //激活
                //定位角色位置
            }
        }
    }
}