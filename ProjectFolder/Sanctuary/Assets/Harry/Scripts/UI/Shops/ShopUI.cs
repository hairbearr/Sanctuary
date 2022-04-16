using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Shops;
using UnityEngine;
using TMPro;
using System;

namespace Sanctuary.Harry.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        Shopper shopper = null;
        Shop currentShop = null;
        [SerializeField] TextMeshProUGUI shopName;
        [SerializeField] Transform listRoot;
        [SerializeField] RowUI rowPrefab;


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
            if(currentShop != null) 
            {
                currentShop.onChange -= RefreshUI;
            }
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);

            if(currentShop == null) return;

            shopName.text = currentShop.GetShopName();
            
            currentShop.onChange += RefreshUI;

            RefreshUI();
        }

        private void RefreshUI()
        {
            foreach (Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (ShopItem item in currentShop.GetFilteredItems())
            {
                RowUI row = Instantiate<RowUI>(rowPrefab, listRoot);
                row.Setup(currentShop, item);
            }
        }

        public void Close()
        {
            shopper.SetActiveShop(null);
        }
    }
}
