 using Akari.GfCore;
 using Cysharp.Threading.Tasks;
 using DG.Tweening;
 using UnityEngine;
 using UnityEngine.Rendering;

 namespace GameMain.Runtime
{
    public class RenderManager : GfSingleton<RenderManager>
    {
        private Transform _root;
        
        private Volume _normalVolume;
        private Volume _witchTimeVolume;
        
        private Sequence _witchTimeSequence;  // 动画序列
        
        protected override void OnCreated()
        {
            _root = new GameObject("RenderRoot").transform;
            //Object.DontDestroyOnLoad(_root);
        }
        
        public async UniTask Init()
        {
            var render = await AssetManager.Instance.Instantiate(AssetPathHelper.GetOtherPath("Render"), _root);
            _normalVolume = render.transform.Find("Normal Volume").GetComponent<Volume>();
            _witchTimeVolume = render.transform.Find("WitchTime Volume").GetComponent<Volume>();
        }

        public void StartWitchTime(float time = 2f)
        {
            if (_witchTimeSequence != null && _witchTimeSequence.IsActive()) 
            {
                _witchTimeSequence.Kill();  // 终止当前动画
            }
            else
            {
                _witchTimeVolume.weight = 0f;
            }

            // 创建一个顺序动画
            _witchTimeSequence = DOTween.Sequence()
                .Append(DOTween.To(() => _witchTimeVolume.weight, x => _witchTimeVolume.weight = x, 1f, 0.5f))
                .AppendInterval(time - 1f)
                .Append(DOTween.To(() => _witchTimeVolume.weight, x => _witchTimeVolume.weight = x, 0f, 0.5f));
        }
    }
}