using UnityEngine;

namespace Numberama
{
    public class StorePanel : MenuPanel
    {
        [SerializeField]
        private NumberamaStore _store = null;

        [SerializeField]
        private StoreItemSlot _hintBooster = null;

        [SerializeField]
        private StoreItemSlot _undoBooster = null;

        [SerializeField]
        private StoreItemSlot _boosterBundle = null;

        private void Awake()
        {
            Close();
        }

        private void Start()
        {
            _hintBooster.SetPurchased(_store.IsHintBoosterPurchased());
            _undoBooster.SetPurchased(_store.IsUndoBoosterPurchased());
            _boosterBundle.SetPurchased(_store.IsBoosterBundlePurchased());
        }
    }
}
