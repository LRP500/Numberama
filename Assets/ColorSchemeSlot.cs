using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numberama
{
    public class ColorSchemeSlot : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private Image _primary = null;

        [SerializeField]
        private Image _secondary = null;

        [SerializeField]
        private ColorSchemeManager _colorSchemeManager = null;

        private ColorScheme _scheme = null;

        public void SetScheme(ColorScheme scheme)
        {
            _scheme = scheme;
            _primary.color = scheme.Primary;
            _secondary.color = scheme.Secondary;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _colorSchemeManager.SetCurrentColorScheme(_scheme);
        }
    }
}
