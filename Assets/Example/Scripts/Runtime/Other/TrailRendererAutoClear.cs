using System;
using UnityEngine;

namespace GameMain.Runtime
{
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailRendererAutoClear : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;
        
        private void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            _trailRenderer.Clear();
        }
    }
}