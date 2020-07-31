using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static Numberama.InfoMessage;

namespace Numberama
{
    public class InfoMessagePanelChoice : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private TextMeshProUGUI _choiceTMP = null;

        private InfoMessagePanel _infoMessagePanel = null;

        private InfoMessageChoice _data = null;

        public void Initialize(InfoMessageChoice data, InfoMessagePanel panel)
        {
            _data = data;
            _infoMessagePanel = panel;
            _choiceTMP.text = data.Text;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _data.callback?.Invoke();
            _infoMessagePanel.Close();
        }
    }
}
