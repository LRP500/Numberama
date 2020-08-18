using UnityEngine;

namespace Numberama
{
    public class StorePanel : MenuPanel
    {
        [SerializeField]
        private NumberamaStoreVariable _store = null;

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
            _hintBooster.SetPurchased(_store.Value.IsHintBoosterPurchased());
            _undoBooster.SetPurchased(_store.Value.IsUndoBoosterPurchased());
            _boosterBundle.SetPurchased(_store.Value.IsBoosterBundlePurchased());
        }
    }
}
