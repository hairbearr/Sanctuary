using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] CombatController[] combatants;
        [SerializeField] bool activateOnStart = false;


        private void Start()
        {
            Activate(activateOnStart);
        }


        public void Activate(bool shouldActivate)
        {
            foreach (CombatController combatant in combatants)
            {
                CombatTarget target = combatant.GetComponent<CombatTarget>();
                if (target != null) { target.enabled = shouldActivate; }
                combatant.enabled = shouldActivate;
            }
        }


    }
}
