using System;
using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public class TrainingDummy : MonoBehaviour
    {
        private BattleCharacterAccessorComponent _accessor;
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            GameCharacterModel gameCharacterModel = new GameCharacterModel(100, 0, 0, 10);
            
            var entity = BattleAdmin.EntityComponentSystem.Create(0, GfEntityGroupId.Character, "TrainingDummy");
            var entityTransform = transform.CreateGfUnityTransform();
            
            entity.AddComponent(new GfActorComponent(entityTransform));
            entity.AddComponent(new BattleCharacterConditionComponent(BattleCharacterType.Enemy, gameCharacterModel));
            
            var colliderComponent = entity.AddComponent(new GfColliderComponent2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>());
            
            var transformComponent = entity.AddComponent(new BattleCharacterTransformComponent());
            var viewComponent = entity.AddComponent(new BattleCharacterViewComponent());
            
            _accessor = entity.AddComponent(new BattleCharacterAccessorComponent(entity));
            
            colliderComponent.Add(BattleColliderGroupName.Defend,
                new GfColliderGroup2D<BattleColliderGroupId, BattleColliderAttackParameter, BattleColliderDefendParameter>(
                    new[] { RyColliderHelper.CreateColliderForDefenderSize() }, GfAxis.None, GfAxis.Z));
            var receiver = new BattleCharacterDamageReceiverHandler(_accessor);
            var defendColliderId = BattleAdmin.DefendColliderIdManager.Issue(entity.ThisHandle);
            var damageNotificator = new BattleCharacterDamageNotificator();
            damageNotificator.Initialize(_accessor,  new BattleCharacterDamageReporter(entity));
            var defendParameter = new BattleColliderDefendParameter(entity.ThisHandle, entity.ThisHandle, defendColliderId, receiver, damageNotificator);
            colliderComponent.SetParameter(BattleColliderGroupName.Defend, defendParameter);
            colliderComponent.SetEnable(BattleColliderGroupName.Defend, true);
            
            _accessor.SetDamageReceiver(receiver);
            _accessor.SetDamageNotificator(damageNotificator); 
            
            entity.On<CurHpMakeZeroRequest>(OnCurHpMakeZeroRequest);
        }

        private void OnCurHpMakeZeroRequest(in CurHpMakeZeroRequest request)
        {
            if (_accessor != null)
            {
                //都行 不过HandleSimpleDamage走完整加血流程 有飘字罢了
                //_accessor.Condition.HpProperty.SetCurValueFully();
                BattleAdmin.DamageHandler.HandleSimpleDamage(_accessor.Entity.ThisHandle,
                    _accessor.DamageNotificator, (int)_accessor.Condition.HpProperty.BaseMaxValue,
                    AttackCategoryType.Heal);
            }
        }
    }
}