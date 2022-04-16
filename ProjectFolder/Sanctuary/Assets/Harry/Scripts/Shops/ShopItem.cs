using System;
using GameDevTV.Inventories;
using UnityEngine;

namespace   Sanctuary.Harry.Shops
{
    public class ShopItem
    {
        InventoryItem item;
        int availability, quantityInTransaction;
        float price;



        public ShopItem(InventoryItem item, int availability, float price, int quantityInTransaction)
        {
            this.item = item;
            this.availability = availability;
            this.price = price;
            this.quantityInTransaction = quantityInTransaction;
        }

        public Sprite GetIcon()
        {
            return item.GetIcon();
        }

        internal int GetAvailability()
        {
            return availability;
        }

        public string GetName()
        {
            return item.GetDisplayName();
        }

        public float GetPrice()
        {
            return price;

        }

        public InventoryItem GetInventoryItem()
        {
            return item;
        }
    }
}