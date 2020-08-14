using Sirenix.OdinInspector;
using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/Color Scheme")]
    public class ColorScheme : ScriptableObject
    {
        [SerializeField]
        private string _name = string.Empty;
        public string Name => _name;

        [SerializeField]
        private Color _primary = Color.black;
        public Color Primary => _primary;

        [SerializeField]
        private Color _secondary = Color.white;
        public Color Secondary => _secondary;

        [SerializeField]
        private Color _selected = new Color(1, 0.5f, 0);
        public Color Selected => _selected;

        [SerializeField]
        private Color _highlighted = new Color(1, 1, 0);
        public Color Highlighted => _highlighted;

        [Button]
        private void Randomize()
        {
            _primary = Random.ColorHSV();
            _secondary = Random.ColorHSV();
        }

        [Button]
        private void Invert()
        {
            Color temp = _primary;
            _primary = _secondary;
            _secondary = temp;
        }
    }
}
