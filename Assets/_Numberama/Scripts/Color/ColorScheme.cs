using UnityEngine;

namespace Numberama
{
    [System.Serializable]
    public class ColorScheme
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
    }
}
