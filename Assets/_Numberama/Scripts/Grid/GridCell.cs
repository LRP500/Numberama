using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numberama
{
    public sealed class GridCell : MonoBehaviour, IPointerDownHandler
    {
        #region State

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

        public bool Selected { get; private set; } = false;

        private System.Action<GridCell> _callback = null;

        [DisplayAsString]
        [LabelText("Coordinates")]
        public string _coordinatesDisplay = string.Empty;

        private void Awake()
        {
            State = new CellState();
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
        }

        public void SetChecked(bool value)
        {
            SetSelected(false);
            State.isChecked = value;
            _canvasGroup.alpha = State.isChecked ? 0.2f : 1;
            _canvasGroup.interactable = !State.isChecked;
            _canvasGroup.blocksRaycasts = !State.isChecked;
        }

        public void SetSelected(bool value)
        {
            Selected = value;
            _number.color = value ? new Color(1, 0.5f, 0) : Color.white;
            _background.color = value ? new Color(1, 0.5f, 0) : Color.white;
        }

        public void SetHighlighted(bool value)
        {
            Selected = value;
            _number.color = value ? Color.yellow : Color.white;
            _background.color = value ? Color.yellow : Color.white;
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
    }
}