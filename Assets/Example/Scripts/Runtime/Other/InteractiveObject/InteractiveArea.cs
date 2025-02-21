using Akari.GfCore;
using UnityEngine;

namespace GameMain.Runtime
{
    /// <summary>
    /// 功能交互区域
    /// 交互后重置状态数据
    /// 商店,铁匠铺等
    /// </summary>
    public class InteractiveArea : AClickInteractiveObject
    {
        [SerializeField] private InteractiveAreaType type;
        public override string InteractionTips => _name;

        private string _name;

        public void Awake()
        {
            _name = type.ToString();
        }

        protected override void OnInteracting(CharacterInteractive characterInteractive)
        {
            //open panel
            if (type == InteractiveAreaType.Center)
            {
                GfLog.Debug("打开Center");
            }
            else if(type == InteractiveAreaType.Shop)
            {
                GfLog.Debug("打开Shop");
            }
            else if(type == InteractiveAreaType.Blacksmith)
            {
                GfLog.Debug("打开Blacksmith");
            }

            ResetStateData();
        }
    }
    
    public enum InteractiveAreaType
    {
        Center,
        Shop,//商店
        Blacksmith,//铁匠铺
    }
}