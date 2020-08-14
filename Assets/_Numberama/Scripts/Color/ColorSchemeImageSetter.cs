using UnityEngine;
using UnityEngine.UI;

namespace Numberama
{
    public class ColorSchemeImageSetter : MonoBehaviour
    {
        public enum Mode
        {
            Primary,
            Secondary
        }

        [SerializeField]
        private Mode _mode = default;

        [SerializeField]
        private Image _image = null;

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
            float transparency = _image.color.a;

            ColorScheme scheme = _colorSchemeManager.CurrentColorScheme;
            Color color = _mode == Mode.Primary ? scheme.Primary : scheme.Secondary;
            color.a = _overrideTransparency ? color.a : transparency;

            _image.color = color;
        }
    }
}