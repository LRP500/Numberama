using Sirenix.OdinInspector;
using UnityEngine;

namespace Numberama
{
    public class MenuPanel : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup = null;

        [SerializeField]
        private MenuPanelVariable _activePanel = null;

        public bool IsOpen { get; private set; } = false;

        private void OnDestroy()
        {
            if (_activePanel.Value == this)
            {
                _activePanel.Clear();
            }
        }

        public void Open()
        {
            Show();
            IsOpen = true;
            _activePanel.Value?.Close();
            _activePanel.SetValue(this);
        }

        public void Close()
        {
            Hide();
            IsOpen = false;
            _activePanel.Clear();
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
