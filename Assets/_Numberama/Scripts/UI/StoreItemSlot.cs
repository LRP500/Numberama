using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numberama
{
    public class StoreItemSlot : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private StoreItem _item = null;

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
            if (_item == null)
            {
                return;
            }

            _name.text = _item.Name;
            _image.sprite = _item.Icon;
            _price.text = $"{_item.Price.ToString("0.00")}€";
            _description.text = _item.Description;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        private void OnValidate()
        {
            Initialize();
        }
    }
}
