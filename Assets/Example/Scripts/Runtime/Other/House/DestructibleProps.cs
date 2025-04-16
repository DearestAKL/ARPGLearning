// using System;
// using Akari.GfCore;
// using Akari.GfGame;
// using Akari.GfUnity;
// using UnityEngine;
//
// namespace GameMain.Runtime
// {
//     /// <summary>
//     /// 可破坏物品
//     /// </summary>
//     public class DestructibleProps : MonoBehaviour
//     {
//         [SerializeField] private Vector2 extents = new Vector2(0.5f, 0.5f);
//         [SerializeField] private int hp = 1;
//         [SerializeField] private Animation animation;
//
//         private GfEntity _entity;
//
//         private const float IntervalTime = 1f;
//         private float _elapsedTime = 0F;
//
//         private bool _isShowing = false;
//         
//         private void Awake()
//         {
//             Create();
//         }
//
//         private void OnEnable()
//         {
//             _entity?.SetActive(true);
//         }
//
//         private void Update()
//         {
//             if (_isShowing)
//             {
//                 if (!animation.isPlaying)
//                 {
//                     _isShowing = false;
//                     //todo:do something
//                 }
//             }
//
//             if (_entity == null)
//             {
//                 _elapsedTime += Time.deltaTime;
//                 if (_elapsedTime > IntervalTime)
//                 {
//                     _elapsedTime = 0;
//                     Create();
//                 }
//             }
//         }
//
//         private void OnDisable()
//         {
//             _entity?.SetActive(false);
//         }
//         
//         private void OnDestroy()
//         {
//             if (_entity!=null)
//             {
//                 _entity.Delete();
//                 _entity = null;
//             }
//         }
//
//         private void Create()
//         {
//             if (!BattleAdmin.HasInstance)
//             {
//                 return;
//             }
//             
//             //依赖ECS初始化
//             _entity = BattleAdmin.EntityComponentSystem.Create(0, GfEntityGroupId.DestructibleProps, "DestructibleProps");
//             _entity.AddComponent(new GfActorComponent(new GfUnityTransform(transform)));
//             var colliderComponent = _entity.AddComponent(new GfColliderComponent2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>());
//             colliderComponent.Add(BattleColliderGroupName.Defend,new GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(
//                 new[] { RyColliderHelper.CreateColliderForDefenderSize(GfFloat2.Zero,extents.ToGfFloat2()) }, GfAxis.None, GfAxis.Z));
//             
//             var defendColliderId = BattleAdmin.DefendColliderIdManager.Issue(_entity.ThisHandle);
//             colliderComponent.SetParameter(BattleColliderGroupName.Defend,
//                 new BattleColliderDefendParameter(_entity.ThisHandle, _entity.ThisHandle, defendColliderId,
//                     new GimmickDamageReceiverHandler(),
//                     new GimmickDamageNotificator(_entity)));
//             colliderComponent.SetEnable(BattleColliderGroupName.Defend, true);
//             
//             _entity.On<BattleReceivedDamageRequest>(OnBattleReceivedDamageRequest);
//         }
//
//         private void OnBattleReceivedDamageRequest(in BattleReceivedDamageRequest request)
//         {
//             hp--;
//             if (hp <= 0)
//             {
//                 //破坏
//                 _entity.Delete();
//
//                 if (animation != null)
//                 {
//                     //播放破坏动画
//                     animation.Stop();
//                     animation.Play();
//                     _isShowing = true;
//                 }
//                 else
//                 {
//                     //todo:do something
//                 }
//             }
//         }
//     }
// }