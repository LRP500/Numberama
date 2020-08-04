﻿using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Numberama
{
    public class ColorSchemeSelection : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup = null;

        [SerializeField]
        private ColorSchemeManager _manager = null;

        [SerializeField]
        private ColorSchemeSlot _slotPrefab = null;

        [SerializeField]
        private Transform _slotContainer = null;

        public bool IsOpen { get; private set; } = false;

        private List<ColorSchemeSlot> _slots = null;

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
                _slots.Add(instance);
            }
        }

        public void Open()
        {
            Show();
            IsOpen = true;
        }

        public void Close()
        {
            Hide();
            IsOpen = false;
        }

        [Button]
        private void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        [Button]
        private void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
