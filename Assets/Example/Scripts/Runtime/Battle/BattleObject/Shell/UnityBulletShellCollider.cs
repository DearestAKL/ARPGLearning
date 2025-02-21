using System.Linq;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 投机做法，因为当前GfColliderComponent2D不支持shell碰撞墙体后销毁
    /// 利用unity的Collider 当碰撞到墙体后 给shell发送Delete请求
    /// </summary>
    public class UnityBulletShellCollider : MonoBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;
        private BattleShellBulletComponent _shellBulletComponent;
        
        private void Update()
        {
            if (_shellBulletComponent == null)
            {
                return;
            }
            
            if (_shellBulletComponent.Entity == null)
            {
                //已Delete
                _shellBulletComponent = null;
                GfPrefabPool.Return(this);
            }
            else
            {
                //同步pos
                transform.position = _shellBulletComponent.Entity.Transform.Position.ToVector3();
                transform.rotation = _shellBulletComponent.Entity.Transform.Rotation.ToQuaternion();
            }
        }

        public void SetData(BattleShellBulletComponent shellBulletComponent,AttackDefinitionCollisionData collisionData)
        {
            _shellBulletComponent = shellBulletComponent;

            boxCollider.center = new Vector3(collisionData.Offset.X, boxCollider.center.y, collisionData.Offset.Y);
            boxCollider.size = new Vector3(collisionData.Extents.X * 2, boxCollider.size.y, collisionData.Extents.Y* 2);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_shellBulletComponent == null)
            {
                return;
            }

            _shellBulletComponent.Entity.Request(new DeleteShellRequest());
            
            _shellBulletComponent = null;
            GfPrefabPool.Return(this);
        }
        
        public static async void Create(BattleShellBulletComponent shellBulletComponent)
        {
            if (shellBulletComponent == null)
            {
                return;
            }
            
            var attackDefinition = shellBulletComponent.DamageCauserHandler.AttackDefinitions.FirstOrDefault();
            if (attackDefinition == null)
            {
                return;
            }

            var collision = attackDefinition.Collisions.FirstOrDefault();
            if (collision == null)
            {
                return;
            }
            
            var unityBulletShellCollider =  await AssetManager.Instance.InstantiateFormPool<UnityBulletShellCollider>(AssetPathHelper.GetOtherPath("UnityBulletShellCollider"));
            
            unityBulletShellCollider.SetData(shellBulletComponent,collision);
        }
    }
}