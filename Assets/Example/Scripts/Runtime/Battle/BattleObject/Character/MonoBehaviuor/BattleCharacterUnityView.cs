using System.Collections.Generic;
using Akari.GfCore;
using Akari.GfUnity;
using GfAnimation;
using UnityEngine;

namespace GameMain.Runtime
{
    public class BattleCharacterUnityView : MonoBehaviour
    {
        [SerializeField] private Transform root;
        [SerializeField] private MultilayerAnimation multilayerAnimation = null;
        [SerializeField] private GfBoneComponentView boneComponentView = default;
        [SerializeField] private CharacterDamageBlinkingView damageBlinkingView  = default;
        [SerializeField] private AkariCharacterController characterController  = default;
        [SerializeField] private List<GfSimpleAnimationTrackView> subsidiaryAnimationTrackViews  = default;
        public Transform Root => root;
        public MultilayerAnimation Animation => multilayerAnimation;
        public GfBoneComponentView Bone => boneComponentView;
        public CharacterDamageBlinkingView DamageBlinkingView => damageBlinkingView;
        public AkariCharacterController CharacterController => characterController;
        public List<GfSimpleAnimationTrackView> SubsidiaryAnimationTrackViews => subsidiaryAnimationTrackViews;
        
        private GameObject _forward;
        private CharacterInteractive _interactive;
        private Vector2 _mouseDirection = Vector2.one;

        private void Awake()
        {

        }

        private void LateUpdate()
        {
            if (_forward != null)
            {
                _forward.transform.forward = new Vector3(_mouseDirection.x, 0, _mouseDirection.y);
            }
        }

        public void UpdateMouseDirection(Vector2 direction)
        {
            if (direction.sqrMagnitude < 0.1F)
            {
                return;
            }
            _mouseDirection = direction;
        }

        public async void Init(BattleCharacterType battleCharacterType,GfEntity entity)
        {
            if (characterController != null)
            {
                characterController.Init(entity);
            }

            SetLayer(battleCharacterType == BattleCharacterType.Player
                ? Constant.Layer.Player
                : Constant.Layer.Character);
            
            if (battleCharacterType == BattleCharacterType.Player)
            {
                AudioManager.Instance.SetListenerLocation(gameObject);
                
                if (_forward == null)
                {
                    _forward =  await AssetManager.Instance.Instantiate(AssetPathHelper.GetCharacterFittingPath("CharacterForward"), this.transform);
                }
                if (_interactive == null)
                {
                    _interactive = gameObject.GetOrAddComponent<CharacterInteractive>();
                    _interactive.SetEntity(entity);   
                }
            }
            else
            {
                if (_interactive != null)
                {
                    Destroy(_interactive);
                }
                _interactive = null;
                
                if (_forward != null)
                {
                    Destroy(_forward);
                }
                _forward = null;
            }
        }

        private void SetLayer(string layerName)
        {
            gameObject.layer = LayerMask.NameToLayer(layerName);
            CharacterController?.UpdateCollidableLayer();
        }
    }
}