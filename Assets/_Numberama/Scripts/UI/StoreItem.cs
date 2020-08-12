using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numberama
{
    public class StoreItem : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private StoreItemInfo _storeItemInfo = null;

        [SerializeField]
        private Image _image = null;

        [SerializeField]
        private TextMeshProUGUI _name = null;

        [SerializeField]
        private TextMeshProUGUI _description = null;

        [SerializeField]
        private TextMeshProUGUI _price = null;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
    }
}
