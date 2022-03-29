using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanctuary.Harry.Movement;
using Sanctuary.Harry.Core;
using System;
using GameDevTV.Saving;
using Sanctuary.Harry.Attributes;
using Sanctuary.Harry.Stats;
using GameDevTV.Utils;
using GameDevTV.Inventories;

namespace Sanctuary.Harry.Combat
{
    public class CombatController : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform rightHandTrans = null, leftHandTrans = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        Health tgt;
        Equipment equipment;
        float timeSinceLastAtk = Mathf.Infinity;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapons> currentWeapon;


        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapons>(SetupDefaultWeapon);

            equipment = GetComponent<Equipment>();

            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private Weapons SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
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

            if (!GetIsInRange(tgt.transform)) { GetComponent<MovementController>().MoveTo(tgt.transform.position, 1f); }
            else
            {
                GetComponent<MovementController>().Cancel();
                AtkBehaviour();
            }
        }

        private void AtkBehaviour()
        {
            transform.LookAt(tgt.transform); //rotate towards enemy after you start attacking

            if(timeSinceLastAtk > currentWeaponConfig.GetAttackSpeed())
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

            if(currentWeapon.value != null) { currentWeapon.value.OnHit(); }

            if (currentWeaponConfig.HasProjectile()) { currentWeaponConfig.LaunchProjectile(rightHandTrans, leftHandTrans, tgt, gameObject, damage); }
            else {  tgt.TakeDamage(gameObject, damage); }            
        }

        void Shoot()
        {
            Hit();
        }

        public bool CanAtk(GameObject fightTgt)
        {
            if(fightTgt == null) { return false; }
            if (!GetComponent<MovementController>().CanMoveTo(fightTgt.transform.position) && !GetIsInRange(fightTgt.transform)) { return false; }

            Health trgtToTst = fightTgt.GetComponent<Health>();
            return trgtToTst != null && !trgtToTst.IsDead();
        }

        public void Attack(GameObject fightTgt)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            tgt = fightTgt.GetComponent<Health>();
        }


        private bool GetIsInRange(Transform tgtTransform)
        {
            return Vector3.Distance(transform.position, tgt.transform.position) < currentWeaponConfig.GetWeaponRange();
        }

        public void Cancel()
        {
            StopAttack();
            tgt = null;
            GetComponent<MovementController>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAtk");
        }

        /*private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, currentWeapon.GetWeaponRange());
        }*/

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private void UpdateWeapon()
        {
            var weapon = equipment.GetItemInSlot(EquipLocation.MainHand) as WeaponConfig;
            if(weapon == null) { EquipWeapon(defaultWeapon); }
            else { EquipWeapon(weapon); }
        }

        private Weapons AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTrans, leftHandTrans, animator);
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
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
