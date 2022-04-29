using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace Sanctuary.Harry.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        [SerializeField] Image foreground = null;
        [SerializeField] TMP_Text healthText = null;


        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            healthText.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
            foreground.fillAmount = health.GetHealthPoints() / health.GetMaxHealthPoints();
        }
    }
}
