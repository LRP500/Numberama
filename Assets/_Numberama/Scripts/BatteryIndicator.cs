using UnityEngine;

namespace Numberama
{
    public class BatteryIndicator : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup = null;

        [SerializeField]
        private float _criticalTreshold = 0.25f;

        private void Awake()
        {
#if UNITY_EDITOR
            _canvasGroup.alpha = 0;
            enabled = false;
#endif
        }

        private void Update()
        {
            _canvasGroup.alpha = SystemInfo.batteryLevel < _criticalTreshold ? 1 : 0;
        }
    }
}
