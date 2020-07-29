using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numberama
{
    public class SettingToggle : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private Sprite _toggleOnSprite = null;

        [SerializeField]
        private Sprite _toggleOffSprite = null;

        [SerializeField]
        private Image _graphicTarget = null;

        public bool IsOn { get; private set; } = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            IsOn = !IsOn;
            _graphicTarget.sprite = IsOn ? _toggleOnSprite : _toggleOffSprite;
        }
    }
}
