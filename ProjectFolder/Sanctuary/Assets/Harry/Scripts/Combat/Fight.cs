using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanctuary.Harry.Movement;
using Sanctuary.Harry.Core;
using System;
using RPG.Saving;
using Sanctuary.Harry.Attributes;
using Sanctuary.Harry.Stats;
using GameDevTV.Utils;

namespace Sanctuary.Harry.Combat
{
    public class Fight : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] Transform rightHandTrans = null, leftHandTrans = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        Health tgt;
        float timeSinceLastAtk = Mathf.Infinity;
        LazyValue<WeaponConfig> currentWeapon;


        private void Awake()
        {
            currentWeapon = new LazyValue<WeaponConfig>(SetupDefaultWeapon);
        }

        private WeaponConfig SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

       

        private void Update()
        {
            timeSinceLastAtk += Time.deltaTime;

            if (tgt == null)  return;

            if(tgt.IsDead()) return;

            if (!GetIsInRange()) { GetComponent<Move>().MoveTo(tgt.transform.position, 1f); }
            else
            {
                GetComponent<Move>().Cancel();
                AtkBehaviour();
            }
        }

        private void AtkBehaviour()
        {
            transform.LookAt(tgt.transform); //rotate towards enemy after you start attacking

            if(timeSinceLastAtk > currentWeapon.value.GetAttackSpeed())
            {
                // This will trigger the Hit Event
                TriggerAtk();
                timeSinceLastAtk = 0;
            }

        }

        private void TriggerAtk()
        {
            GetComponent<Animator>().ResetTrigger("stopAtk");
            GetComponent<Animator>().SetTrigger("attack");
        }

        void Hit() //Animation Event
        {
            if (tgt == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.value.HasProjectile()) { currentWeapon.value.LaunchProjectile(rightHandTrans, leftHandTrans, tgt, gameObject, damage); }
            else {  tgt.TakeDamage(gameObject, damage); }            
        }

        void Shoot()
        {
            Hit();
        }

        public bool CanAtk(GameObject fightTgt)
        {
            if(fightTgt == null) { return false; }

            Health trgtToTst = fightTgt.GetComponent<Health>();
            return trgtToTst != null && !trgtToTst.IsDead();
        }

        public void Attack(GameObject fightTgt)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            tgt = fightTgt.GetComponent<Health>();
        }


        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, tgt.transform.position) < currentWeapon.value.GetWeaponRange();
        }

        public void Cancel()
        {
            StopAttack();
            tgt = null;
            GetComponent<Move>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAtk");
        }

        public IEnumerable<float> GetAdditiveMods(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetWeaponDamage();
            }
        }
        public IEnumerable<float> GetPercentageMods(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }

        /*private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, currentWeapon.GetWeaponRange());
        }*/

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTrans, leftHandTrans, animator);
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        public Health GetTarget()
        {
            return tgt;
        }

        
    }
}
