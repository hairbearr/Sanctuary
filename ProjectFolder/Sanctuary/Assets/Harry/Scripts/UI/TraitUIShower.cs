using System;
using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Attributes;
using Sanctuary.Harry.Combat;
using Sanctuary.Harry.Stats;
using UnityEngine;

namespace Sanctuary.Harry.UI
{
    public class TraitUIShower : MonoBehaviour
    {
        [SerializeField] GameObject traitsWindow;

        CombatController playerCombatController = null;
        BaseStats playerBaseStats = null;
        Health playerHealth = null;
        bool hasLeveled = false;

        void Awake()
        {
            playerCombatController = GameObject.FindWithTag("Player").GetComponent<CombatController>();
            playerBaseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        // Start is called before the first frame update
        void Start()
        {
            playerBaseStats.onLevelUp += HasLeveled;
        }

        private void HasLeveled()
        {
            hasLeveled = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(hasLeveled && ! playerCombatController.GetInCombat() && !playerHealth.GetInCombat())
            {
                hasLeveled = false;
                traitsWindow.SetActive(true);
            }
        }
    }
}
