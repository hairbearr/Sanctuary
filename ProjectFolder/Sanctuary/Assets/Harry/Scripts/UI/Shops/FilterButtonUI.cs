using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using Sanctuary.Harry.Shops;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sanctuary.Harry.UI.Shops
{
    public class FilterButtonUI : MonoBehaviour
    {
        [SerializeField] ItemCategory category = ItemCategory.None;

        Button button;
        Shop currentShop;
        
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(SelectFilter);
        }

        public void SetShop(Shop currentShop)
        {
            this.currentShop = currentShop;
        }

        public void RefreshUI()
        {
            button.interactable = currentShop.GetFilter() != category;
        }

        private void SelectFilter()
        {
            currentShop.SelectFilter(category);
        }
    }
}
