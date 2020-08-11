using System.Collections.Generic;
using UnityEngine;

namespace Numberama
{
    public class ColorSchemeSelection : MenuPanel
    {
        [SerializeField]
        private CanvasGroup _canvasGroup = null;

        [SerializeField]
        private ColorSchemeManager _manager = null;

        [SerializeField]
        private ColorSchemeSlot _slotPrefab = null;

        [SerializeField]
        private Transform _slotContainer = null;

        private List<ColorSchemeSlot> _slots = null;

        private ColorSchemeSlot _selected = null;

        private void Awake()
        {
            Close();
            Initialize();
        }

        private void OnDestroy()
        {
            foreach (ColorSchemeSlot slot in _slots)
            {
                Destroy(slot.gameObject);
            }

            _slots.Clear();
        }

        private void Initialize()
        {
            _slots = new List<ColorSchemeSlot>();

            foreach (ColorScheme scheme in _manager.ColorSchemes)
            {
                ColorSchemeSlot instance = Instantiate(_slotPrefab, _slotContainer);
                instance.SetScheme(scheme);
                instance.SetCallback(Select);

                if (scheme == _manager.CurrentColorScheme)
                {
                    instance.SetSelected(true);
                    _selected = instance;
                }
                else
                {
                    instance.SetSelected(false);
                }

                _slots.Add(instance);
            }
        }

        private void Select(ColorSchemeSlot selection)
        {
            _manager.SetCurrentColorScheme(selection.Scheme);
            _selected?.SetSelected(false);
            _selected = selection;
            _selected.SetSelected(true);
        }
    }
}
