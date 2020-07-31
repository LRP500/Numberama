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
        private TextMeshProUGUI _messageTMP = null;

        [SerializeField]
        private InfoMessagePanelChoice _choicePrefab = null;

        [SerializeField]
        private Transform _choiceContainer = null;

        [SerializeField]
        private InfoMessage _infoMessage = null;

        [SerializeField]
        private ContentSizeFitter _contentSizeFitter = null;

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

        public void Initialize()
        {
            // Panel already in use
            if (_isOpen)
            {
                return;
            }

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
        public void Open()
        {
            Initialize();
            StartCoroutine(RefreshLayoutAndOpen());
        }

        private IEnumerator RefreshLayoutAndOpen()
        {
            _isOpen = true;
            _contentSizeFitter.enabled = false;

            yield return new WaitForEndOfFrame();

            _contentSizeFitter.enabled = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        [Button]
        public void Close()
        {
            Clear();
            _isOpen = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
