using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sanctuary.Harry.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TMP_Text dmgText = null;

        public void DestroyText()
        {
            Destroy(gameObject);
        }

        public void SetValue(float amount)
        {
            dmgText.text = String.Format("{0:0}", amount);
        }
    }
}
