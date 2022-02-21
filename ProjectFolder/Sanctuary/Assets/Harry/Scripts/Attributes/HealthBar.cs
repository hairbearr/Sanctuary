using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthBar = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        private void Update()
        {
            if (Mathf.Approximately(healthBar.GetFraction(), 0) || Mathf.Approximately(healthBar.GetFraction(), 1))
            {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healthBar.GetFraction(), 1, 1);
        }
    }
}
