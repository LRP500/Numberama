using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/Color Scheme Manager")]
    public class ColorSchemeManager : ScriptableObject
    {
        [AssetList]
        [SerializeField]
        private List<ColorScheme> _colorSchemes = null;
        public List<ColorScheme> ColorSchemes => _colorSchemes;

        [SerializeField]
        private ColorScheme _currentColorScheme = null;
        public ColorScheme CurrentColorScheme => _currentColorScheme;

        private System.Action OnColorSchemeChanged = null;

        public void SetCurrentColorScheme(ColorScheme scheme)
        {
            _currentColorScheme = scheme;
            OnColorSchemeChanged?.Invoke();
        }

        public void SetRandom()
        {
            SetCurrentColorScheme(_colorSchemes[Random.Range(0, _colorSchemes.Count)]);
        }

        public void RegisterOnColorSchemeChanged(System.Action action)
        {
            OnColorSchemeChanged += action;
        }

        public void UnregisterOnColorSchemeChanged(System.Action action)
        {
            OnColorSchemeChanged -= action;
        }
    }
}