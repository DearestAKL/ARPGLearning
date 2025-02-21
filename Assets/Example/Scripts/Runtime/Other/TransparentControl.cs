using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Runtime
{
    public class TransparentControl : MonoBehaviour
    {
        public class TransparentParam
        {
            public Material[] materials;
            public Material[] sharedMats;
            public float currentFadeTime = 0;
            public bool isTransparent = true;
        }

        public Transform targetObject;   //目标对象
        public float height = 3.0f;             //目标对象Y方向偏移
        public float destTransparent = 0.2f;    //遮挡半透的最终半透强度，
        public float fadeInTime = 1.0f;         //开始遮挡半透时渐变时间
        public LayerMask occlusionLayers;       // 定义哪些层会被视为遮挡层
        private Dictionary<Renderer,TransparentParam> transparentDic = new Dictionary<Renderer, TransparentParam>();
        private readonly List<Renderer> clearList = new();

        private Vector3 lastTargetPos;

        void Update()
        {
            if (targetObject == null)
                return;

            // 仅当目标位置改变或超过更新间隔时更新射线投射
            if (targetObject.position != lastTargetPos)
            {
                lastTargetPos = targetObject.position;
                UpdateRayCastHit();
            }
            UpdateTransparentObject();
        }

        void UpdateTransparentObject()
        {
            foreach (var kvp in transparentDic)
            {
                TransparentParam param = kvp.Value;
                param.isTransparent =false;

                // 实现渐入半透明效果，避免太过突兀
                foreach (var mat in param.materials)
                {
                    float destAlphaScale = destTransparent;
                    float currentAlphaScale = mat.GetFloat("_AlphaScale");
                    float newAlphaScale = Mathf.MoveTowards(currentAlphaScale, destAlphaScale, Time.deltaTime / fadeInTime);
                    mat.SetFloat("_AlphaScale", newAlphaScale);
                }
            }
        }

        void RemoveUnuseTransparent()
        {
            clearList.Clear();
            foreach (var kvp in transparentDic)
            {
                if (!kvp.Value.isTransparent)
                {
                    kvp.Key.materials = kvp.Value.sharedMats;
                    clearList.Add(kvp.Key);
                }
            }
            foreach (var renderer in clearList)
                transparentDic.Remove(renderer);
        }

        void UpdateRayCastHit()
        {
            RaycastHit[] rayHits;
            //视线方向为从自身（相机）指向目标位置
            Vector3 targetPos = targetObject.position + new Vector3(0, height, 0);
            Vector3 viewDir = (targetPos - transform.position).normalized;

            RaycastHit HitInfo;
            Physics.Raycast(transform.position, viewDir, out HitInfo);

            // 假如角色没有被遮挡，则删除半透明效果
            if (HitInfo.transform != null && HitInfo.transform.gameObject.layer == LayerMask.NameToLayer(Constant.Layer.Player))
            {
                RemoveUnuseTransparent();
            }
            // 角色被遮挡时，获取所有遮挡物，并设置为透明
            else
            {
                Vector3 oriPos = transform.position;
                float distance = Vector3.Distance(oriPos, targetPos);
                Ray ray = new Ray(oriPos, viewDir);
                rayHits = Physics.RaycastAll(ray, distance, occlusionLayers);
                // 直接在Scene画一条线，方便观察射线
                Debug.DrawLine(oriPos, targetPos, Color.red);

                foreach (var hit in rayHits)
                {
                    Renderer[] renderers = hit.collider.GetComponentsInChildren<Renderer>();
                    foreach (Renderer r in renderers)
                    {
                        AddTransparent(r);
                    }
                }
            }
        }

        void AddTransparent(Renderer renderer)
        {
            TransparentParam param;
            if (!transparentDic.TryGetValue(renderer, out param))
            {
                param = new TransparentParam();
                transparentDic.Add(renderer, param);
                // 此处顺序不能反，调用material会产生材质实例。
                param.sharedMats = renderer.sharedMaterials;
                param.materials = renderer.materials;
                foreach (var v in param.materials)
                {
                    v.shader = Shader.Find("Custom/TransparentColorURP");
                }
            }
            param.isTransparent = true;
        }
    }
}