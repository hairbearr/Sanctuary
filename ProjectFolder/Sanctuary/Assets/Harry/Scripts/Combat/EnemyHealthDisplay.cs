using UnityEngine;
using TMPro;
using System;
using Sanctuary.Harry.Attributes;

namespace Sanctuary.Harry.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        CombatController fight;

        private void Awake()
        {
            fight = GameObject.FindWithTag("Player").GetComponent<CombatController>();
        }

        private void Update()
        {
            if(fight.GetTarget() == null)
            {
                GetComponent <TMP_Text>().text = "N/A";
                return;
            }

            Health health = fight.GetTarget();
            GetComponent<TMP_Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPts(), health.GetMaxHealthPts());
        }
    }
}
