using GameDevTV.Utils;
using RPG.Saving;
using Sanctuary.Harry.Core;
using Sanctuary.Harry.Stats;
using System;
using System.Collections;
using UnityEngine;

namespace Sanctuary.Harry.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        LazyValue<float> healthPts;

        bool isDead = false;

        private void Awake()
        {
            healthPts = new LazyValue<float>(GetInitialHealth);
        }

        private void Start()
        {
            healthPts.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenHealth;
        }

        private void DeathBehaviour()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardXP(GameObject instigator)
        {
            Experience xp = instigator.GetComponent<Experience>();
            if (xp == null) return;

            xp.GainXP(GetComponent<BaseStats>().GetStat(Stat.XPReward));
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float dmgTaken)
        {
            print($"{gameObject.name} took {dmgTaken} damage.");
            healthPts.value = Mathf.Max(healthPts.value - dmgTaken, 0);
            if(healthPts.value == 0) { DeathBehaviour(); AwardXP(instigator); }
        }

        public float GetHealthPts()
        {
            return healthPts.value;
        }

        public float GetMaxHealthPts()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (healthPts.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        public object CaptureState()
        {
            return healthPts;
        }
        public void RestoreState(object state)
        {
            healthPts.value = (float)state;
            if (healthPts.value <= 0) { DeathBehaviour(); }
        }

        private void RegenHealth()
        {
            healthPts.value = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
    }
}