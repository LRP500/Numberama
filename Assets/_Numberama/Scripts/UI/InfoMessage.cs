using UnityEngine;
using UnityEngine.Events;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/Info Message")]
    public class InfoMessage : ScriptableObject
    {
        [System.Serializable]
        public class InfoMessageChoice
        {
            [SerializeField]
            private string _text = string.Empty;
            public string Text => _text;

            public UnityEvent callback;
        }

        [SerializeField]
        [Multiline, TextArea]
        private string _message = null;
        public string Message => _message;

        [SerializeField]
        private InfoMessageChoice[] _choices = null;
        public InfoMessageChoice[] Choices => _choices;
    }
}
