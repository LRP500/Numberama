using UnityEngine;
using UnityEngine.Purchasing;

namespace Tools.Monetization
{
    /// <summary>
    /// Manages store purchases and restoration on all platforms.
    /// </summary>
    /// <remarks>
    /// https://docs.unity3d.com/Manual/UnityIAPInitialization.html
    /// https://forum.unity.com/threads/solved-in-app-purchase-restoring-saving-understanding.459467/
    /// </remarks>
    public abstract class StorePurchaser : MonoBehaviour, IStoreListener
    {
        private static IStoreController _storeController = null;
        private static IExtensionProvider _storeExtensionProvider = null;

        private void Start()
        {
            if (!IsInitialized())
            {
                InitializePurchasing();
            }
        }

        /// <summary>
        /// Initializes store purchasing.
        /// </summary>
        private void InitializePurchasing()
        {
            if (IsInitialized())
            {
                return;
            }

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            AddProducts(builder);

            UnityPurchasing.Initialize(this, builder);
        }

        /// <summary>
        /// Adds all products to store on initialization.
        /// </summary>
        /// <param name="builder"></param>
        public virtual void AddProducts(ConfigurationBuilder builder) { }

        /// <summary>
        /// Returns true if store purchasing is initialized, else returns false.
        /// </summary>
        /// <returns></returns>
        private bool IsInitialized()
        {
            return _storeController != null && _storeExtensionProvider != null;
        }

        /// <summary>
        /// Buy a product from the store with his id.
        /// </summary>
        /// <param name="productID"></param>
        public void BuyProduct(string productID)
        {
            if (IsInitialized())
            {
                Product product = _storeController.products.WithID(productID);

                if (product != null && product.availableToPurchase)
                {
                    Log(string.Format("Purchasing product {0}...", product.definition.id));
                    _storeController.InitiatePurchase(product);
                }
            }
            else
            {
                Log("Purchase failed. Product not found or not available for purchase.", LogType.Error);
            }
        }

        /// <summary>
        /// Restore products previously purchased from store. Specific to Apple platforms.
        /// </summary>
        public void RestorePurchases()
        {
            if (!IsInitialized())
            {
                Log("Restore purchases failed. Store purchasing not initialized.");
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                Log("Restoring purchases...");

                IAppleExtensions apple = _storeExtensionProvider.GetExtension<IAppleExtensions>();

                apple.RestoreTransactions((result) =>
                {
                    Log(string.Format("Purchase restored : {0}.", result));
                });
            }
            else
            {
                Log(string.Format("Purchases restoration not supported on this platform ({0}).", Application.platform));
            }
        }

        private void Log(string text, LogType priority = LogType.Log)
        {
            string message = string.Format("[{0}] {1}", GetType().Name, text);

            switch (priority)
            {
                case LogType.Log: Debug.Log(message); break;
                case LogType.Warning: Debug.LogWarning(message); break;
                case LogType.Error: Debug.LogError(message); break;
                default: break;
            }
        }

        #region IStoreListener Callbacks

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _storeExtensionProvider = extensions;

            Log("Store purchasing initialized successfully.");
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Log("Store purchasing failed to initialize.", LogType.Error);
        }

        public virtual PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            Log(string.Format("Store purchase successful ! (Product : {0})", e.purchasedProduct.definition.id));
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Log(string.Format("Store purchase failed ! (Product : {0}, FailureReason : {1})", product.definition.id, failureReason), LogType.Error);
        }

        #endregion IStoreListener Callbacks
    }
}
