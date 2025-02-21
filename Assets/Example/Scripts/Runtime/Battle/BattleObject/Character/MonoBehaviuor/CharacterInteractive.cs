using System;
using System.Collections.Generic;
using Akari.GfCore;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    public class CharacterInteractive : MonoBehaviour
    {
        private UIButtonTips _tips;
        private readonly List<AInteractiveObject> _interactiveObjects = new List<AInteractiveObject>();
        private AInteractiveObject _curInteractiveObject;

        public GfEntity Entity { get; private set; }

        private void OnDestroy()
        {
            Destroy(_tips);
        }

        public void SetEntity(GfEntity entity)
        {
            Entity = entity;
            
            BattleUnityAdmin.PlayerInput.actions[Constant.InputDef.Interaction].started += OnInteractionStarted;
        }

        public void AddInteractiveObject(AInteractiveObject interactiveObject)
        {
            if (interactiveObject != null)
            {
                _interactiveObjects.Add(interactiveObject);
                _curInteractiveObject = _interactiveObjects[0];
                UpdateTips();
            }
        }

        public void RemoveInteractiveObject(AInteractiveObject interactiveObject)
        {
            if (interactiveObject != null)
            {
                _interactiveObjects.Remove(interactiveObject);
                if (_interactiveObjects.Count > 0)
                {
                    _curInteractiveObject = _interactiveObjects[0];
                }
                else
                {
                    _curInteractiveObject = null;
                }

                UpdateTips();
            }
        }

        private async void UpdateTips()
        {
            if (_tips == null)
            {
                _tips = await UIManager.Instance.Factory.GetUITipsItem<UIButtonTips>("UIButtonTips");
                //强制刷新RectTransform，防止初次加载时RectTransform宽度计算为0
                LayoutRebuilder.ForceRebuildLayoutImmediate(_tips.GetComponent<RectTransform>());
            }
            
            if (_curInteractiveObject != null)
            {
                _tips.gameObject.SetActive(true);
                _tips.UpdateView(_curInteractiveObject.InteractionTips);
                float xOffset = _tips.GetComponent<RectTransform>().sizeDelta.x / 2 + 50f;
                _tips.transform.localPosition = UIHelper.WorldPositionToBattleUI(transform.position, new Vector2(xOffset, 0f));
            }
            else
            {
                _tips.gameObject.SetActive(false);
            }
        }
        
        private void OnInteractionStarted(InputAction.CallbackContext context)
        {
            if (_curInteractiveObject != null)
            {
                _curInteractiveObject.Check(this);
            }
        }
    }
}