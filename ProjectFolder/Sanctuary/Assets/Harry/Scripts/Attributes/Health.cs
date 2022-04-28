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

        LazyValue<float> healthPoints;
        LazyValue<float> shieldPoints;

        [SerializeField] UnityEvent<float> takeDamage, takeHeal;
        public UnityEvent onDie;
        public event Action<GameObject> OnHitTaken;

        internal object GetShieldPoints()
        {
            return shieldPoints.value;
        }

        bool wasDeadLastFrame = false;
        bool inCombat = false;
        bool invulnerable = false;
        bool shielded = false;
        float increasedDamageModifier = 1f;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
            shieldPoints = new LazyValue<float>(GetInitialShieldPoints);
        }

        private void Update()
        {

            if(inCombat == false && wasDeadLastFrame == false)
            {
                if(healthPoints.value < GetMaxHealthPoints())
                {
                    healthPoints.value += GetRegenRate() * Time.deltaTime;
                }

                if(healthPoints.value > GetMaxHealthPoints())
                {
                    healthPoints.value = GetMaxHealthPoints(); 
                }
            }

            if(shieldPoints.value > 0) {shielded = true;}
            if(shieldPoints.value <=0 )
            { 
                shielded = false;
            }
        }

        private void Start()
        {
            healthPoints.ForceInitialization();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RestoreFullHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RestoreFullHealth;
        }

        private void UpdateState()
        {
            Animator animator = GetComponent<Animator>();
            if(!wasDeadLastFrame && IsDead())
            {
                animator.SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if(wasDeadLastFrame && !IsDead())
            {
                animator.Rebind();
            }
            wasDeadLastFrame = IsDead();
        }

        private void RestoreFullHealth()
        {
            healthPoints.value = GetComponent<BaseStats>().GetStat(Stat.Health);
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
            return healthPoints.value <= 0;
        }

        public void TakeDamage(GameObject instigator, float damageTaken)
        {
            damageTaken = damageTaken * increasedDamageModifier;

            if(invulnerable == true) { return; }

            if(healthPoints.value <= 0) { return; }

            if(shielded == true ) { shieldPoints.value -= damageTaken; return; }
            
            healthPoints.value = Mathf.Max(healthPoints.value - damageTaken, 0);

            if (IsDead())
            {
                onDie.Invoke();
                AwardXP(instigator);
            }
            else
            {
                takeDamage.Invoke(damageTaken);
            }

            UpdateState();
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
            takeHeal.Invoke(healthToRestore);

            UpdateState();

        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (GetFraction());
        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }
        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            UpdateState();
        }

        public void SetInCombat(bool state)
        {
            inCombat = state;
        }

        public bool GetInCombat()
        {
            return inCombat;
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
            shieldPoints.value = pointsToShield;
        }

        public void SetDamageTakenModifier(float damageMod)
        {
            increasedDamageModifier = damageMod;
        }
    }
}