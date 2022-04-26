using UnityEngine;
using TMPro;
using System;
using Sanctuary.Harry.Attributes;
using UnityEngine.UI;

namespace Sanctuary.Harry.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        CombatController fight;
        [SerializeField] GameObject targetRoot = null;
        [SerializeField] Image foreground = null;
        [SerializeField] TMP_Text enemyName, healthText;

        private void Awake()
        {
            fight = GameObject.FindWithTag("Player").GetComponent<CombatController>();
        }

        private void Update()
        {
            if(fight.GetTarget() == null)
            {
                targetRoot.SetActive(false);
                return;
            }
            else{targetRoot.SetActive(true);}

            Health health = fight.GetTarget();
            
            enemyName.text = fight.GetTarget().name;
            healthText.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
            foreground.fillAmount = health.GetHealthPoints() / health.GetMaxHealthPoints();
        }
    }
}
