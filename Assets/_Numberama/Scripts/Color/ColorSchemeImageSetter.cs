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
        private ColorSchemeManager _colorSchemeManager = null;

        private void Awake()
        {
            Refresh();
        }

        private void Start()
        {
            _colorSchemeManager.RegisterOnColorSchemeChanged(Refresh);
        }

        private void OnDestroy()
        {
            _colorSchemeManager.UnregisterOnColorSchemeChanged(Refresh);
        }

        private void Refresh()
        {
            ColorScheme scheme = _colorSchemeManager.CurrentColorScheme;
            _image.color = _mode == Mode.Primary ? scheme.Primary : scheme.Secondary;
        }
    }
}