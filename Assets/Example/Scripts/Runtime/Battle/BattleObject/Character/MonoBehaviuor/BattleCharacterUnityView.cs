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
        [SerializeField] private CharacterController characterController  = default;
        [SerializeField] private List<GfSimpleAnimationTrackView> subsidiaryAnimationTrackViews  = default;
        public Transform Root => root;
        public MultilayerAnimation Animation => multilayerAnimation;
        public GfBoneComponentView Bone => boneComponentView;
        public CharacterDamageBlinkingView DamageBlinkingView => damageBlinkingView;
        public CharacterController CharacterController => characterController;
        public List<GfSimpleAnimationTrackView> SubsidiaryAnimationTrackViews => subsidiaryAnimationTrackViews;
        
        private GameObject _forward;
        private CharacterInteractive _interactive;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        public void UpdateForward(Vector2 direction)
        {
            if (_forward == null || direction.sqrMagnitude <= 0.1f) 
            {
                return;
            }
            
            _forward.transform.forward = new Vector3(direction.x, 0, direction.y);
        }

        public async void Init(BattleCharacterType battleCharacterType,GfEntity entity)
        {
            gameObject.layer = LayerMask.NameToLayer(battleCharacterType == BattleCharacterType.Player
                ? Constant.Layer.Player
                : Constant.Layer.Enemy);
            
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
    }
}