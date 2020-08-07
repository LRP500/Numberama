using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numberama
{
    public class ColorSchemeSlot : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private Image _image = null;

        [SerializeField]
        private Image _primary = null;

        [SerializeField]
        private Image _secondary = null;

        [SerializeField]
        private Image _border = null;

        public ColorScheme Scheme { get; private set; } = null;

        private System.Action<ColorSchemeSlot> OnSelected = null;

        public void SetScheme(ColorScheme scheme)
        {
            Scheme = scheme;

            _image.material = Instantiate(_image.material);
            _image.material.SetColor("_PrimaryColor", Scheme.Primary);
            _image.material.SetColor("_SecondaryColor", Scheme.Secondary);

            _primary.color = Scheme.Primary;
            _secondary.color = Scheme.Secondary;
        }

        public void SetCallback(System.Action<ColorSchemeSlot> callback)
        {
            OnSelected += callback;
        }

        public void SetSelected(bool selected)
        {
            _border.enabled = selected;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSelected?.Invoke(this);
        }
    }
}
