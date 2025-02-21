using System.IO;
using Akari.GfCore;
using Akari.GfGame;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public class CharacterAnimationContainerView : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private bool enablePlayMode = false;
        
        private GfEntity _entity;
        
        public void SetEntity(GfEntity entity)
        {
            _entity = entity;
        }
        
        // 处理运行时 修改AnimationEvent 实时生效
        private void Start()
        {
            GfPlayModeEvent.OnAnimationEventBundlerSerialized.GfSubscribe(OnSerializedAnimationEventBundler);
        }
        
        private void OnSerializedAnimationEventBundler(string path, MemoryStream stream)
        {
            if (!enablePlayMode)
            {
                return;
            }

            var component = _entity?.GetComponent<GfAnimationComponent>();
            var track     = component?.GetTrack<GfAnimationEventTrack>();
            if (track == null) { return; }
            
            var infos = track.GetAnimationEventInfos();
            
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].ResourcePath == path)
                {
                    var animationEvent = GfAnimationEventResourceFactory.Instance.CreateResource(stream);
                    var animationEventData = animationEvent.CreateData();
                    track?.ForceSet(animationEventData, path);
                    break;
                }
            }
        }
#endif
    }
}