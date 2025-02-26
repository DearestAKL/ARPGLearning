using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public sealed class BattleCharacterViewComponent : AGfGameComponent<BattleCharacterViewComponent>
    {
        private GfComponentCache<BattleCharacterAccessorComponent> _accessorCache;
        private GfComponentCache<GfBoneComponent>                  _boneComponentCache;

        private BattleCharacterAccessorComponent Accessor => Entity.GetComponent(ref _accessorCache);
        public GfBoneComponent BoneComponent => Entity.GetComponent(ref _boneComponentCache);

        private BattleCharacterUnityView _unityView;
        public BattleCharacterUnityView UnityView => _unityView;
        
        private UIBattleStatusPiece _statusPiece;

        private List<GfEntity> _subsidiaryList = new List<GfEntity>();
        public List<GfEntity> SubsidiaryList => _subsidiaryList;

        public BattleCharacterViewComponent(BattleCharacterUnityView unityView = null)
        {
            _unityView = unityView;
        }

        public override void OnAwake()
        {
            Entity.On<BattlePlayDamageEffectAndUIRequest>(OnBattlePlayDamageEffectAndUIRequest);
            Entity.On<GfActiveChangedRequest>(OnGfActiveChangedRequest);
            Entity.On<ChangeAnimationRequest>(OnChangeAnimationRequest);
        }

        public override async void OnStart()
        {
            _statusPiece = await UIManager.Instance.Factory.GetUIBattleItem<UIBattleStatusPiece>("UIBattleStatusPiece",true);
            bool isUserPlayer = Accessor.Entity == BattleAdmin.Player.Entity;
            _statusPiece.Init(Accessor.Condition.HpProperty.CurValueRatio, Accessor.Condition.PoiseHandler.CurrentRatio,
                Accessor.Condition.TeamId == TeamId.TeamA, isUserPlayer);

            // 将屏幕坐标转换为UI坐标
            _statusPiece.UpdatePosition(UIHelper.WorldPositionToBattleUI(Accessor.Entity.Transform.Position.ToVector3(), new Vector2(0, 150F)));
        }

        public override void OnEnd()
        {
            base.OnEnd();
            GfPrefabPool.Return(_statusPiece);
        }

        public override void OnDelete()
        {
            base.OnDelete();
            foreach (var subEntity in _subsidiaryList)
            {
                subEntity.Delete();
            }
        }

        public override void OnEndUpdate(float deltaTime)
        {
            base.OnEndUpdate(deltaTime);

            if (_unityView != null)
            {
                _unityView.UpdateForward(Accessor.Condition.MouseDirection.ToVector2());
            }

            if (_statusPiece != null && _statusPiece.IsVisibility)
            {
                // 将屏幕坐标转换为UI坐标
                _statusPiece.UpdatePosition(UIHelper.WorldPositionToBattleUI(Accessor.Entity.Transform.Position.ToVector3(), new Vector2(0, 150F)));
                _statusPiece.UpdatePoiseBar(Accessor.Condition.PoiseHandler.CurrentRatio,Accessor.Condition.PoiseHandler.IsFailure);
                
                if (Accessor.Condition.HpProperty.IsViewDirty)
                {
                    UpdateHpView();
                    Accessor.Condition.HpProperty.IsViewDirty = false;
                }
            }
        }

        public void AddSubsidiary(GfEntity entity)
        {
            _subsidiaryList.Add(entity);
        }

        private void OnBattlePlayDamageEffectAndUIRequest(in BattlePlayDamageEffectAndUIRequest request)
        {
            for (var i = 0; i < request.DamageResults.Length; i++)
            {
                //TODO:根据配置表现
                var result = request.DamageResults[i];
                PlayDamageUI(result,result.IsCritical);
                if (result.AttackCategoryType == AttackCategoryType.Damage)
                {
                    PlayVoice(result);
                    PlayDamageEffect(result);
                }
            }
            
            for (var i = 0; i < request.SimpleDamageResults.Length; i++)
            {
                var result =  request.SimpleDamageResults[i];
                if (result.DamageViewType != DamageViewType.Hide)
                {
                    PlayDamageUI(result);
                }
            }

            UpdateHpView();
        }

        private void UpdateHpView()
        {
            _statusPiece.UpdateHp(Accessor.Condition.HpProperty.CurValueRatio);
            
            if (Accessor.Condition.BattleCharacterType == BattleCharacterType.Player)
            {
                EventManager.Instance.UIEvent.OnPlayerCharacterHpChangeEvent.Invoke(
                    Accessor.Condition.HpProperty.CurShowValue,
                    Accessor.Condition.HpProperty.TotalMaxShowValue);
            }
        }

        private void PlayDamageUI(IBattleSimpleDamageResult damageResult,bool isCritical = false) 
        {
            var uiPosition =  UIHelper.WorldPositionToBattleUI(Accessor.Entity.Transform.Position.ToVector3(), new Vector2(0, 100f));
            UIHelper.ShowDamageNum(uiPosition, damageResult, isCritical);
        }

        private void PlayVoice(IBattleDamageResult damageResult) 
        {
            //AudioManager.Instance.PlaySound(Constant.Sound.HitSound,true,Accessor.Transform.CurrentPosition.ToVector3());
        }

        private void PlayDamageEffect(IBattleDamageResult damageResult) 
        {
            if (_unityView != null && _unityView.DamageBlinkingView != null)
            {
                _unityView.DamageBlinkingView.StartBlinking();
            }

            BattleAdmin.Factory.Effect.CreateEffectByEntity(Entity, CommonEffectType.Hit.GetPath(), EffectGroup.OneShot, GfFloat3.Zero, GfQuaternion.Identity, GfFloat3.One, GfVfxDeleteMode.Delete, GfVfxPriority.High);
            
            var cameraShakeParam = damageResult.CameraShakeData;
            if (cameraShakeParam.Power != BattleCameraShakeParam.ShakePower.NONE)
            {
                BattleAdmin.EntityComponentSystem.RequestToEntity(BattleUnityAdmin.BattleMainCameraView.SelfHandle,
                    new CameraShakeRequest(cameraShakeParam.Power, cameraShakeParam.Direction));
            }
        }

        private void OnGfActiveChangedRequest(in GfActiveChangedRequest request)
        {
            _statusPiece.SetVisibility(request.Enable);
        }
        private void OnChangeAnimationRequest(in ChangeAnimationRequest request)
        {
            if (_subsidiaryList == null || _subsidiaryList.Count <= 0)
            {
                return;
            }
            
            for (int i = 0; i < _subsidiaryList.Count; i++)
            {
                _subsidiaryList[i].Request(new SubsidiaryPlayAnimationRequest(request.AnimationName));
            }
        }
    }
}
