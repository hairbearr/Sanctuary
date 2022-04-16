using System;
using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sanctuary.Harry.UI.Shops
{
    public class RowUI : MonoBehaviour
    {
        [SerializeField] Image iconField;
        [SerializeField] TextMeshProUGUI nameField, availabilityField, priceField, quantityField;

        Shop currentShop = null;
        ShopItem item = null;

        public void Setup(Shop currentShop, ShopItem item)
        {
            this.currentShop = currentShop;
            this.item = item;

            iconField.sprite = item.GetIcon();
            nameField.text = item.GetName();
            availabilityField.text = $"{item.GetAvailability()}";
            priceField.text = $"{item.GetPrice():N0}";
            quantityField.text = $"{item.GetQuantityInTransaction()}";
        }

        public void Add()
        {
            currentShop.AddToTransaction(item.GetInventoryItem(), 1);
        }

        public void Remove()
        {
            currentShop.AddToTransaction(item.GetInventoryItem(), -1);
        }
    }
}
