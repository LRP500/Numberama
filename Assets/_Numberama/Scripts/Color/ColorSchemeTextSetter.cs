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
            _text.color = _mode == Mode.Primary ? scheme.Primary : scheme.Secondary;
        }
    }
}
