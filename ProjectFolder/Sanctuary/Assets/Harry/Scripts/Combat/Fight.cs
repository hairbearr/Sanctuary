using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanctuary.Harry.Movement;
using Sanctuary.Harry.Core;
using System;

namespace Sanctuary.Harry.Combat
{
    public class Fight : MonoBehaviour, IAction
    {
        [SerializeField] Transform rightHandTrans = null, leftHandTrans = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health tgt;
        float timeSinceLastAtk = Mathf.Infinity;
        Weapon currentWeapon = null;


        private void Start()
        {
            EquipWeapon(defaultWeapon);
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

            if(timeSinceLastAtk > currentWeapon.GetAttackSpeed())
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

            if (currentWeapon.HasProjectile()) { currentWeapon.LaunchProjectile(rightHandTrans, leftHandTrans, tgt); }
            else { tgt.TakeDamage(currentWeapon.GetWeaponDamage()); }            
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
            return Vector3.Distance(transform.position, tgt.transform.position) < currentWeapon.GetWeaponRange();
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, currentWeapon.GetWeaponRange());
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTrans, leftHandTrans, animator);
        }

    }
}
