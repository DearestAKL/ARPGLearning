using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Runtime
{
    [RequireComponent(typeof(Image))]
    public class UIImageExpand  : MonoBehaviour
    {
        [SerializeField] private PlaceholderType placeholderType;
        [SerializeField] private Sprite placeholderImage;
        
        public enum PlaceholderType
        {
            Image,
            Enable
        }
        
        private Image _img;
        
        private void Awake()
        {
            _img = GetComponent<Image>();
        }

        public void OnBeforeLoadIcon()
        {
            if (placeholderType == PlaceholderType.Image) 
            {
                if (placeholderImage != null)
                {
                    _img.sprite = placeholderImage;
                }
            }
            else if(placeholderType == PlaceholderType.Enable)
            {
                _img.enabled = false;
            }
        }
        
        public void OnAfterLoadIcon()
        {
            if (placeholderType == PlaceholderType.Enable)
            {
                _img.enabled = true;
            }
        }
    }
}