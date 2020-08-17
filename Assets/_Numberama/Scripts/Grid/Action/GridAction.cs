using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numberama
{
    public class GridAction : MonoBehaviour, IPointerDownHandler
    {
        public delegate bool GridActionCallback();

        [SerializeField]
        private GridActionInfo _gridActionInfo = null;
        public GridActionInfo Info => _gridActionInfo;

        [SerializeField]
        private Image _image = null;

        [SerializeField]
        private TextMeshProUGUI _stack = null;

        [SerializeField]
        private Slider _slider = null;

        private float _reloadTime = 0f;
        private float _lastUseTime = 0f;

        private GridActionCallback OnExecute = null;

        private void Awake()
        {
            _image.sprite = _gridActionInfo.Icon;
            _reloadTime = _gridActionInfo.ReloadTime;
            _slider.minValue = 0;
            _slider.maxValue = _reloadTime;
            _lastUseTime = float.MinValue;
        }

        public void SetStackValue(int value)
        {
            _stack.text = value.ToString();
        }

        public void SetPurchased(bool purchased)
        {
            _reloadTime = purchased ? 0 : _gridActionInfo.ReloadTime;
            _slider.maxValue = _reloadTime;
        }

        private void Update()
        {
            float sliderValue = _slider.maxValue - (Time.time - _lastUseTime);
            _slider.value = Mathf.Clamp(sliderValue, 0, _slider.maxValue);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _lastUseTime = Time.time;
        }

        public void RegisterOnExecute(GridActionCallback action)
        {
            OnExecute = action;
        }

        public bool CanUse()
        {
            return Time.time - _lastUseTime > _gridActionInfo.ReloadTime;
        }
    }
}