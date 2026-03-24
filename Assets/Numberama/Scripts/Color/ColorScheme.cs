using Sirenix.OdinInspector;
using UnityEngine;

namespace Numberama.Color
{
    [CreateAssetMenu(menuName = "Numberama/Color Scheme")]
    public class ColorScheme : ScriptableObject
    {
        [SerializeField]
        private string _name = string.Empty;
        public string Name => _name;

        [SerializeField]
        private UnityEngine.Color _primary = UnityEngine.Color.black;
        public UnityEngine.Color Primary => _primary;

        [SerializeField]
        private UnityEngine.Color _secondary = UnityEngine.Color.white;
        public UnityEngine.Color Secondary => _secondary;

        [SerializeField]
        private UnityEngine.Color _selected = new UnityEngine.Color(1, 0.5f, 0);
        public UnityEngine.Color Selected => _selected;

        [SerializeField]
        private UnityEngine.Color _highlighted = new UnityEngine.Color(1, 1, 0);
        public UnityEngine.Color Highlighted => _highlighted;

        [Button]
        private void Randomize()
        {
            _primary = Random.ColorHSV();
            _secondary = Random.ColorHSV();
        }

        [Button]
        private void Invert()
        {
            UnityEngine.Color temp = _primary;
            _primary = _secondary;
            _secondary = temp;
        }
    }
}
