using Akari.GfCore;
using Akari.GfGame;

namespace GameMain.Runtime
{
    /// <summary>
    /// 配件组件 控制角色的配件，例如弓箭的动画
    /// </summary>
    public class SubsidiaryComponent : AGfGameComponent<SubsidiaryComponent>
    {
        private GfComponentCache<GfAnimationComponent> _animationCache;
        private GfAnimationComponent Animation => Entity.GetComponent(ref _animationCache);
        private int _animationClipIndex = 0;
        
        public override void OnAwake()
        {
            base.OnAwake();
            Entity.On<SubsidiaryPlayAnimationRequest>(OnCharacterSubsidiaryPlayAnimationRequest);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (!Animation.IsThatPlaying(_animationClipIndex))
            {
                _animationClipIndex = 0;
                Animation.Play(_animationClipIndex);
            }
        }

        private void OnCharacterSubsidiaryPlayAnimationRequest(in SubsidiaryPlayAnimationRequest request)
        {
            _animationClipIndex = Animation.GetClipIndex(request.AnimationName);
            Animation.Play(_animationClipIndex);
        }
    }
}