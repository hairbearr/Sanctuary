using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Shops;
using UnityEngine;

namespace Sanctuary.Harry.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        Shopper shopper = null;
        Shop currentShop = null;


        // Start is called before the first frame update
        void Start()
        {
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();

            if(shopper == null)  return;

            shopper.activeShopChange += ShopChanged;

            ShopChanged();
        }

        private void ShopChanged()
        {
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);
        }
    }
}
