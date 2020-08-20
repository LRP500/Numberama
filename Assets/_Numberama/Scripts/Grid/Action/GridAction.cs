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

        private float _cooldownDuration = 0f;
        private float _cooldownTimer = 0f;

        private GridActionCallback OnExecuteSuccess = null;
        private System.Action OnExecuteFail = null;

        private void Awake()
        {
            _image.sprite = _gridActionInfo.Icon;
            _cooldownDuration = _gridActionInfo.ReloadTime;
            _slider.minValue = 0;
            _slider.maxValue = _cooldownDuration;
            _cooldownTimer = _cooldownDuration;
        }

        public void SetStackValue(int value)
        {
            _stack.text = value.ToString();
        }

        public void SetPurchased(bool purchased)
        {
            _cooldownDuration = purchased ? 0 : _gridActionInfo.ReloadTime;
            _slider.maxValue = _cooldownDuration;
        }

        public float GetCooldown()
        {
            return _cooldownTimer;
        }

        public void SetCooldown(float value)
        {
            _cooldownTimer = value;
        }

        private void Update()
        {
            _cooldownTimer -= Time.deltaTime;
            _cooldownTimer = Mathf.Clamp(_cooldownTimer, 0, _cooldownDuration);
            _slider.value = _cooldownTimer;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (CanUse())
            {
                Execute();
            }
            else
            {
                OnExecuteFail?.Invoke();
            }
        }

        private void Execute()
        {
            if (OnExecuteSuccess?.Invoke() == true)
            {
                _cooldownTimer = _cooldownDuration;
            }
        }

        public void RegisterOnExecute(GridActionCallback success, System.Action fail)
        {
            OnExecuteSuccess = success;
            OnExecuteFail = fail;
        }

        public bool CanUse()
        {
            return _cooldownTimer <= 0;
        }
    }
}