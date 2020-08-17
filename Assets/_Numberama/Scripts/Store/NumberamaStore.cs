using Tools.Monetization;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Numberama
{
    public class NumberamaStore : StorePurchaser
    {
        public static readonly string HintBoosterID = "numberama_hint_booster";
        public static readonly string UndoBoosterID = "numberama_undo_booster";
        public static readonly string BoosterBundleID = "numberama_booster_bundle";

        public override void AddProducts(ConfigurationBuilder builder)
        {
            builder.AddProduct(HintBoosterID, ProductType.NonConsumable);
            builder.AddProduct(UndoBoosterID, ProductType.NonConsumable);
            builder.AddProduct(BoosterBundleID, ProductType.NonConsumable);
        }

        public override PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs transaction)
        {
            PlayerPrefs.SetInt(transaction.purchasedProduct.definition.id, 1);
            PlayerPrefs.Save();

            return base.ProcessPurchase(transaction);
        }

        public bool IsHintBoosterPurchased()
        {
            if (IsBoosterBundlePurchased())
            {
                return true;
            }

            return PlayerPrefs.GetInt(HintBoosterID) == 1 ? true : false;
        }

        public bool IsUndoBoosterPurchased()
        {
            if (IsBoosterBundlePurchased())
            {
                return true;
            }

            return PlayerPrefs.GetInt(UndoBoosterID) == 1 ? true : IsBoosterBundlePurchased();
        }

        public bool IsBoosterBundlePurchased()
        {
            return PlayerPrefs.GetInt(BoosterBundleID) == 1 ? true : false;
        }
    }
}
