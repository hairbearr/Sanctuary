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


        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            //GetComponent<TMP_Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPts(), health.GetMaxHealthPts());

            foreground.fillAmount = health.GetHealthPts() / health.GetMaxHealthPts();
        }
    }
}
