using System.Collections.Generic;
using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/Color Scheme Manager")]
    public class ColorSchemeManager : ScriptableObject
    {
        [SerializeField]
        private List<ColorScheme> _colorSchemes = null;
        public List<ColorScheme> ColorSchemes => _colorSchemes;

        public ColorScheme CurrentColorScheme { get; private set; } = null;

        public void SetCurrentColorScheme(ColorScheme scheme)
        {
            CurrentColorScheme = scheme;
        }
    }
}