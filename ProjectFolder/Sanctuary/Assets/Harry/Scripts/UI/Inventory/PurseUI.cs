using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Inventories;
using TMPro;
using UnityEngine;

namespace Sanctuary.Harry.UI.Inventory
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI balanceField;

        Purse playerPurse = null;

        void Start()
        {
            playerPurse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();     
            if(playerPurse != null) {playerPurse.onChange += RefreshUI;}
            RefreshUI();
        }

        private void RefreshUI()
        {
            balanceField.text = $"{playerPurse.GetBalance():N0}";
        }
    }
}
