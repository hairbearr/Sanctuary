using GameDevTV.Utils;
using RPG.Saving;
using Sanctuary.Harry.Core;
using Sanctuary.Harry.Stats;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Sanctuary.Harry.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        LazyValue<float> healthPts;

        [SerializeField] UnityEvent<float> takeDamage;

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
            
            healthPts.value = Mathf.Max(healthPts.value - dmgTaken, 0);

            takeDamage.Invoke(dmgTaken);

            if (healthPts.value == 0)
            {
                DeathBehaviour();
                AwardXP(instigator);
            }
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
            return 100 * (GetFraction());
        }

        public float GetFraction()
        {
            return healthPts.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return healthPts.value;
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