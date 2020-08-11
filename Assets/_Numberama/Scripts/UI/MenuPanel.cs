using Sirenix.OdinInspector;
using UnityEngine;

namespace Numberama
{
    public class MenuPanel : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup = null;

        public bool IsOpen { get; private set; } = false;

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
