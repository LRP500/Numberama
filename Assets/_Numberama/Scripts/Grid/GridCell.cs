﻿using Sirenix.OdinInspector;
using TMPro;
using Tools.Persistence;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numberama
{
    public sealed class GridCell : MonoBehaviour, IPointerDownHandler, IPersistable
    {
        #region State

        [System.Serializable]
        public class CellState
        {
            public Vector2Int coordinates = default;
            public bool isChecked = false;
            public int number = 0;
        }

        public CellState State { get; private set; } = default;

        public Vector2Int Coordinates => State.coordinates;
        public bool IsChecked => State.isChecked;
        public int Number => State.number;

        #endregion State

        [SerializeField]
        private TextMeshProUGUI _number = null;

        [SerializeField]
        private Image _background = null;

        [SerializeField]
        private CanvasGroup _canvasGroup = null;

        [SerializeField]
        private ColorSchemeManager _colorSchemeManager = null;

        private System.Action<GridCell> _callback = null;

        private ColorScheme _colorScheme = null;

        [DisplayAsString]
        [LabelText("Coordinates")]
        public string _coordinatesDisplay = string.Empty;

        public bool Selected { get; private set; } = false;

        private void Awake()
        {
            State = new CellState();
            _colorScheme = _colorSchemeManager.CurrentColorScheme;
        }

        public void SetState(CellState state)
        {
            State = state;
            Refresh();
        }

        public void SetCoordinates(Vector2Int coordinates)
        {
            State.coordinates = coordinates;
        }

        public void SetCallback(System.Action<GridCell> callback)
        {
            _callback = callback;
        }

        private void Refresh()
        {
#if UNITY_EDITOR
            _coordinatesDisplay = $"{State.coordinates.x} : {State.coordinates.y}";
#endif
            _number.text = State.number.ToString();
            SetChecked(State.isChecked);
        }

        public void SetChecked(bool value)
        {
            SetSelected(false);
            State.isChecked = value;
            _canvasGroup.alpha = State.isChecked ? 0.1f : 1;
            _canvasGroup.interactable = !State.isChecked;
            _canvasGroup.blocksRaycasts = !State.isChecked;
        }

        public void SetSelected(bool value)
        {
            Selected = value;
            _number.color = value ? _colorScheme.Selected : _colorScheme.Secondary;
            _background.color = value ? _colorScheme.Selected : _colorScheme.Secondary;
        }

        public void SetHighlighted(bool value)
        {
            Selected = value;
            _number.color = value ? _colorScheme.Highlighted : _colorScheme.Secondary;
            _background.color = value ? _colorScheme.Highlighted : _colorScheme.Secondary;
        }

        public void Clear()
        {
            SetChecked(false);
            SetSelected(false);
        }

        #region Event Handlers

        public void OnPointerDown(PointerEventData eventData)
        {
            _callback.Invoke(this);
        }

        #endregion Event Handlers

        #region Persistence

        public void Save(GameDataWriter writer)
        {
            writer.Write(State.number);
            writer.Write(State.isChecked ? 1 : 0);
            writer.Write(State.coordinates.x);
            writer.Write(State.coordinates.y);
        }

        public void Load(GameDataReader reader)
        {
            SetState(new CellState
            {
                number = reader.ReadInt(),
                isChecked = reader.ReadInt() == 1 ? true : false,
                coordinates = new Vector2Int(reader.ReadInt(), reader.ReadInt())
            });
        }

        #endregion Persistence
    }
}