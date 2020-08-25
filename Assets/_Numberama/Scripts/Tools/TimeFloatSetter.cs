using System.Collections.Generic;
using TMPro;
using Tools.Variables;
using UnityEngine;

namespace Tools
{
    public class TimeFloatSetter : MonoBehaviour
    {
        private enum RefreshMode
        {
            OnUpdate,
            OnValueChanged
        }

        private enum FormatType
        {
            FF,
            SS,
            MM,
            HH,
            SSFF,
            MMSS,
            MMSSFF,
            HHMM,
            HHMMSS,
            HHMMSSFF,
        }

        private readonly Dictionary<FormatType, string> _formats = new Dictionary<FormatType, string>
        {
            { FormatType.FF, "ff" },
            { FormatType.SS, "ss" },
            { FormatType.MM, "mm" },
            { FormatType.HH, "hh" },
            { FormatType.SSFF, "ss\\:ff" },
            { FormatType.MMSS, "mm\\:ss" },
            { FormatType.MMSSFF, "mm\\:ss\\:ff" },
            { FormatType.HHMM, "hh\\:mm" },
            { FormatType.HHMMSS, "hh\\:mm\\:ss" },
            { FormatType.HHMMSSFF, "hh\\:mm\\:ss\\:ff" },
        };

        [SerializeField]
        private FormatType _format = FormatType.HHMMSS;

        [SerializeField]
        private FloatVariable _time = null;

        [SerializeField]
        private TextMeshProUGUI _text = null;

        [SerializeField]
        private RefreshMode _refreshMode = default;

        private void Awake()
        {
            if (_refreshMode == RefreshMode.OnValueChanged)
            {
                _time.Subscribe(Refresh);
            }
        }

        private void OnDestroy()
        {
            _time.Unsubscribe(Refresh);
        }

        private void Update()
        {
            if (_refreshMode == RefreshMode.OnUpdate)
            {
                Refresh();
            }
        }

        public void Refresh()
        {
            _text.text = System.TimeSpan.FromSeconds(_time.Value).ToString(_formats[_format]);
        }
    }
}
