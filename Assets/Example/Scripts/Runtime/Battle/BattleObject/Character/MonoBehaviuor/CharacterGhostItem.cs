using System;
using Akari.GfUnity;
using UnityEngine;

namespace GameMain.Runtime
{
    public class CharacterGhostItem : MonoBehaviour
    {
        public float durationTime = 10f;
        
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;

        private float elapsedTime = 0f;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > durationTime)
            {
                elapsedTime = 0;
                GfPrefabPool.Return(this);
            }
        }

        public void SetMesh(Mesh mesh)
        {
            meshFilter.mesh = mesh;
            elapsedTime = 0;
        }
    }
}