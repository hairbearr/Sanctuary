using RPG.Saving;
using Sanctuary.Harry.Core;
using Sanctuary.Harry.Stats;
using System.Collections;
using UnityEngine;

namespace Sanctuary.Harry.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPts = 100f;

        bool isDead = false;

        private void Start()
        {
            healthPts = GetComponent<BaseStats>().GetHealth();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float dmgTaken)
        {
            healthPts = Mathf.Max(healthPts - dmgTaken, 0);
            if(healthPts == 0) { DeathBehaviour(); }
        }

        private void DeathBehaviour()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        public object CaptureState()
        {
            return healthPts;
        }
        public void RestoreState(object state)
        {
            healthPts = (float)state;
            if (healthPts == 0) { DeathBehaviour(); }
        }
    }
}