using UnityEngine;
using UnityEngine.EventSystems;

namespace Numberama
{
    public class GridAction : MonoBehaviour, IPointerDownHandler
    {
        private System.Action OnExecute = null;

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