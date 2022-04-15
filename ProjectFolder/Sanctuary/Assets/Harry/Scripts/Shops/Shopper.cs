using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Shops
{
    public class Shopper : MonoBehaviour
    {
        Shop activeShop = null;

        public event Action activeShopChange;


        public void SetActiveShop(Shop shop)
        {
            activeShop = shop;
            if(activeShopChange != null)
            {
                activeShopChange();
            }
        }

        public Shop GetActiveShop()
        {
            return activeShop;
        }
    }
}
