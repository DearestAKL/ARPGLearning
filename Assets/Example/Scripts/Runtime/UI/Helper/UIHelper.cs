using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameMain.Runtime
{
    public static class UIHelper
    {
        public static async UniTask OpenCommonMessageDialog(string content, Action confirmAction = null, Action cancelAction = null,string confirmContent = "", string cancelContent = "")
        {
            var data = new UICommonMessageDialog.Params(content, confirmAction, cancelAction, confirmContent,
                cancelContent);
            await UIManager.Instance.OpenUIPanel(UIType.UICommonMessageDialog, data);
        }
        
        public static async UniTask OpenTips(string content)
        {
            var data = new UITipsDialog.Params(content);
            await UIManager.Instance.OpenUIPanel(UIType.UITipsDialog, data);
        }
        
        public static async UniTask StartLoading()
        {
            await UIManager.Instance.OpenUIPanel(UIType.UILoading);
        }
        
        public static void EndLoading()
        {
            var uiPanel = UIManager.Instance.GetUIPanel(UIType.UILoading);
            if (uiPanel is UILoading uiLoading)
            {
                uiLoading.SetFinish();
            }
        }
        
        // 将3D坐标转换为战斗UI坐标的方法
        public static Vector2 WorldPositionToBattleUI(Vector3 worldPosition,Vector2 offset)
        {

            // 将3D坐标转换为屏幕坐标
            Vector3 screenPosition = UIManager.Instance.MainCamera.WorldToScreenPoint(worldPosition);

            // 将屏幕坐标转换为UI坐标
            RectTransform canvasRect =  UIManager.Instance.GetUIGroup(UIGroupType.Battle).RootGo.GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition,
                UIManager.Instance.UICamera, out var uiPosition);
            
            return uiPosition + offset;
        }

        private static readonly Dictionary<AttackCategoryType,string> UIDamageNumPrefabIds = new Dictionary<AttackCategoryType, string>()
        {
            {AttackCategoryType.Damage,"UIBattleDamageNumPiece"},
            {AttackCategoryType.Heal,"UIBattleHealNumPiece"},
        };
        
        public static async void ShowDamageNum(Vector2 position, IBattleSimpleDamageResult simpleDamageResult, bool isCritical)
        {
            var numPiece = await UIManager.Instance.Factory.GetUIBattleItem<UIBattleDamageNumPiece>(UIDamageNumPrefabIds[simpleDamageResult.AttackCategoryType], true);
            numPiece.Show(position, simpleDamageResult.OriginalVariation, isCritical,simpleDamageResult.AttackCategoryType == AttackCategoryType.Heal);
        }
    }
}