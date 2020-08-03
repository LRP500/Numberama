using UnityEngine;
using UnityEngine.UI;

namespace Numberama
{
    public class ColorSchemeImageSetter : MonoBehaviour
    {
        public enum Mode
        {
            Primary,
            Seconady
        }

        [SerializeField]
        private Mode _mode = default;

        [SerializeField]
        private Image _image = null;

        [SerializeField]
        private ColorSchemeManager _colorSchemeManager = null;

        private void Awake()
        {
            ColorScheme scheme = _colorSchemeManager.CurrentColorScheme;
            _image.color = _mode == Mode.Primary ? scheme.Primary : scheme.Secondary;
        }
    }
}
