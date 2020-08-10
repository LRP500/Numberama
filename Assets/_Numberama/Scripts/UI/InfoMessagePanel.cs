using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Numberama.InfoMessage;

namespace Numberama
{
    public class InfoMessagePanel : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup = null;

        [SerializeField]
        private ContentSizeFitter _contentSizeFitter = null;

        [SerializeField]
        private TextMeshProUGUI _messageTMP = null;

        [SerializeField]
        private InfoMessagePanelChoice _choicePrefab = null;

        [SerializeField]
        private Transform _choiceContainer = null;

        private InfoMessage _infoMessage = null;

        private List<InfoMessagePanelChoice> _choices = null;

        private bool _isOpen = false;

        private void Awake()
        {
            _choices = new List<InfoMessagePanelChoice>();

            Close();
        }

        public void SetInfoMessage(InfoMessage infoMessage)
        {
            _infoMessage = infoMessage;
        }

        public void Initialize(InfoMessage data)
        {
            _infoMessage = data;
            _messageTMP.text = _infoMessage.Message;

            foreach (InfoMessageChoice choice in _infoMessage.Choices)
            {
                InfoMessagePanelChoice instance = Instantiate(_choicePrefab, _choiceContainer);
                instance.Initialize(choice, this);
                _choices.Add(instance);
            }
        }

        private void Clear()
        {
            foreach (InfoMessagePanelChoice choice in _choices)
            {
                Destroy(choice.gameObject);
            }

            _choices.Clear();

            _messageTMP.text = string.Empty;
        }

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public void Open(InfoMessage data)
        {
            if (_isOpen)
            {
                Close();
            }

            if (data == null)
            {
                return;
            }

            Initialize(data);
            StartCoroutine(RefreshLayoutAndOpen());
        }

        private IEnumerator RefreshLayoutAndOpen()
        {
            _isOpen = true;
            _contentSizeFitter.enabled = false;

            yield return new WaitForEndOfFrame();

            _contentSizeFitter.enabled = true;

            Show();
        }

        public void Close()
        {
            Hide();
            Clear();
            _isOpen = false;
        }

        [Button]
        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        [Button]
        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
