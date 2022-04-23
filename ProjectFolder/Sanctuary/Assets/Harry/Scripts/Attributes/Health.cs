using GameDevTV.Utils;
using GameDevTV.Saving;
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
        LazyValue<float> shieldPts;

        [SerializeField] UnityEvent<float> takeDamage, takeHeal;
        [SerializeField] UnityEvent onDie;

        internal object GetShieldPoints()
        {
            return shieldPts.value;
        }

        bool isDead = false;
        bool inCombat = false;
        bool invulnerable = false;
        bool shielded = false;
        float increasedDamageModifier = 1f;

        private void Awake()
        {
            healthPts = new LazyValue<float>(GetInitialHealth);
            shieldPts = new LazyValue<float>(GetInitialShieldPoints);
        }

        private void Update()
        {

            if(inCombat == false && isDead == false)
            {
                if(healthPts.value < GetMaxHealthPts())
                {
                    healthPts.value += GetRegenRate() * Time.deltaTime;
                }

                if(healthPts.value > GetMaxHealthPts())
                {
                    healthPts.value = GetMaxHealthPts(); 
                }
            }

            if(shieldPts.value > 0) {shielded = true;}
            if(shieldPts.value <=0 )
            { 
                shielded = false;
            }
        }

        internal void Heal(object healthChange)
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            healthPts.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RestoreFullHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RestoreFullHealth;
        }

        private void DeathBehaviour()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void RestoreFullHealth()
        {
            healthPts.value = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public bool GetShielded()
        {
            return shielded;
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private float GetInitialShieldPoints()
        {
            return 0f;
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
            dmgTaken = dmgTaken * increasedDamageModifier;

            if(invulnerable == true) { return; }

            if(healthPts.value <= 0) { return; }

            if(shielded == true ) { shieldPts.value -= dmgTaken; return; }
            
            healthPts.value = Mathf.Max(healthPts.value - dmgTaken, 0);

            takeDamage.Invoke(dmgTaken);

            if (healthPts.value == 0)
            {
                onDie.Invoke();
                DeathBehaviour();
                AwardXP(instigator);
            }
        }

        public void Heal(float healthToRestore)
        {
            healthPts.value = Mathf.Min(healthPts.value + healthToRestore, GetMaxHealthPts());
            takeHeal.Invoke(healthToRestore);

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

        public void SetInCombat(bool state)
        {
            inCombat = state;
        }

        public float GetRegenRate()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HealthRegenRate);
        }

        public void SetInvulnerability(bool state)
        {
            invulnerable = state;
        }

        public void SetShielded(bool state)
        {
            shielded = state;
        }
        
        public void SetShieldPoints(float pointsToShield)
        {
            shieldPts.value = pointsToShield;
        }

        public void SetDamageTakenModifier(float damageMod)
        {
            increasedDamageModifier = damageMod;
        }
    }
}