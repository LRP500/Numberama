using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Numberama
{
    public class GridAction : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private TextMeshProUGUI _stack = null;

        private System.Action OnExecute = null;

        public void SetStackValue(int value)
        {
            _stack.text = value.ToString();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            OnExecute?.Invoke();
        }

        public void RegisterOnExecute(System.Action action)
        {
            OnExecute += action;
        }
    }
}