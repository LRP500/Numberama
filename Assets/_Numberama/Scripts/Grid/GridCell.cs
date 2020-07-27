using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numberama
{
    public sealed class GridCell : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private TextMeshProUGUI _number = null;

        [SerializeField]
        private Image _background = null;

        [SerializeField]
        private CanvasGroup _canvasGroup = null;

        public Vector2Int Coordinates { get; private set; } = default;
        public int Number { get; private set; } = 0;
        public bool Checked { get; private set; } = false;
        public bool Selected { get; private set; } = false;

        private System.Action<GridCell> _callback = null;

        [DisplayAsString]
        [LabelText("Coordinates")]
        public string _coordinatesDisplay = string.Empty;

        public void Initialize(GridCell other)
        {
            SetNumber(other.Number);
            SetChecked(other.Checked);
        }

        public void SetCoordinates(Vector2Int coordinates)
        {
            Coordinates = coordinates;
            _coordinatesDisplay = $"{coordinates.x} : {coordinates.y}";
        }

        public void SetNumber(int number)
        {
            Number = number;
            Refresh();
        }

        public void SetCallback(System.Action<GridCell> callback)
        {
            _callback = callback;
        }

        private void Refresh()
        {
            _number.text = Number.ToString();
        }

        public void SetChecked(bool value)
        {
            SetSelected(false);
            Checked = value;
            _canvasGroup.alpha = Checked ? 0.2f : 1;
            _canvasGroup.interactable = !Checked;
            _canvasGroup.blocksRaycasts = !Checked;
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