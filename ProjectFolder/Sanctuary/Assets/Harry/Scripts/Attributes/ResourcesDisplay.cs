using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sanctuary.Harry.Attributes
{
    public class ResourcesDisplay : MonoBehaviour
    {
        PlayerResources playerResources;
        [SerializeField] Image foreground = null;
        [SerializeField] TMP_Text playerResourcesText = null;

        private void Awake()
        {
            playerResources = GameObject.FindWithTag("Player").GetComponent<PlayerResources>();
        }

        private void Update()
        {
            playerResourcesText.text = String.Format("{0:0}/{1:0}", playerResources.GetResources(), playerResources.GetMaxResources());
            foreground.fillAmount = playerResources.GetResources() / playerResources.GetMaxResources();
        }
    }
}

