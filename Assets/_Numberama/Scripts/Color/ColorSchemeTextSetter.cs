using TMPro;
using UnityEngine;

namespace Numberama
{
    public class ColorSchemeTextSetter : MonoBehaviour
    {
        public enum Mode
        {
            Primary,
            Secondary
        }

        [SerializeField]
        private Mode _mode = default;

        [SerializeField]
        private TextMeshProUGUI _text = null;

        [SerializeField]
        private bool _overrideTransparency = true;

        [SerializeField]
        private ColorSchemeManager _colorSchemeManager = null;

        private void Start()
        {
            Refresh();
            _colorSchemeManager.RegisterOnColorSchemeChanged(Refresh);
        }

        private void OnDestroy()
        {
            _colorSchemeManager.UnregisterOnColorSchemeChanged(Refresh);
        }

        private void Refresh()
        {
            float transparency = _text.color.a;
            ColorScheme scheme = _colorSchemeManager.CurrentColorScheme;
            Color color = _mode == Mode.Primary ? scheme.Primary : scheme.Secondary;
            color.a = _overrideTransparency ? color.a : transparency;
            _text.color = color;
        }
    }
}
