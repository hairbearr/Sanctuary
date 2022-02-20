using UnityEngine;
using TMPro;
using System;

namespace Sanctuary.Harry.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPts(), health.GetMaxHealthPts());
        }
    }
}
