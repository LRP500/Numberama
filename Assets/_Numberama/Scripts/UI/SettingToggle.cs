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

        public bool IsOn { get; private set; } = true;

        private System.Action _onClick = null;

        private void Awake()
        {
            SetOn(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetOn(!IsOn);
            _onClick?.Invoke();
        }

        private void SetOn(bool value)
        {
            IsOn = value;
            _graphicTarget.sprite = IsOn ? _toggleOnSprite : _toggleOffSprite;
        }

        public void RegisterOnClick(System.Action action)
        {
            _onClick += action;
        }

        public void UnregisterOnClick(System.Action action)
        {
            _onClick -= action;
        }
    }
}
