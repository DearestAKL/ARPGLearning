using System;
using DG.Tweening;
using UnityEngine;

namespace GameMain.Runtime
{
    public class CharacterDamageBlinkingView : MonoBehaviour
    {
        [SerializeField] private new SkinnedMeshRenderer renderer = default;
        [SerializeField] private float blinkingTime = 0.2f;
        
        private float _elapsedTime;

        private const string MaterialName = "DamageFresnel";
        
        private readonly int _fresnel = Shader.PropertyToID("_Fresnel");
        private readonly Vector4 _blinkingParam = new Vector4(0.3f, 1, 0, 0);

        private bool _isBlinking = false;

        private void Update()
        {
            if (_isBlinking)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime > blinkingTime)
                {
                    StopBlinking();
                }
            }
        }
        
        public void StartBlinking()
        {
            _isBlinking = true;
            _elapsedTime = 0f;

            ChangeMaterial(_blinkingParam);
        }

        private void StopBlinking()
        {
            _isBlinking = false;

            ChangeMaterial(Vector4.zero);
        }

        private void ChangeMaterial(Vector4 fresnelParam)
        {
            if (renderer == null)
            {
                return;
            }
            
            // 动态修改参数
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                var material = renderer.materials[i];
                if (material.name.StartsWith(MaterialName))
                {
                    material.SetVector(_fresnel, fresnelParam);
                    break;
                }
            }
        }
    }
}