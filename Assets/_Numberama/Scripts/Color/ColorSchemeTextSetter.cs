using TMPro;
using UnityEngine;

namespace Numberama
{
    public class ColorSchemeTextSetter : MonoBehaviour
    {
        public enum Mode
        {
            Primary,
            Seconady
        }

        [SerializeField]
        private Mode _mode = default;

        [SerializeField]
        private TextMeshProUGUI _text = null;

        [SerializeField]
        private ColorSchemeManager _colorSchemeManager = null;

        private void Awake()
        {
            ColorScheme scheme = _colorSchemeManager.CurrentColorScheme;
            _text.color = _mode == Mode.Primary ? scheme.Primary : scheme.Secondary;
        }
    }
}
