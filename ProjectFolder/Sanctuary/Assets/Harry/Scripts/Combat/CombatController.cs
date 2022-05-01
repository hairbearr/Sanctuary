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
    public class CombatController : MonoBehaviour, IAction
    {
        [SerializeField] Transform rightHandTrans = null, leftHandTrans = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] float maxSpeedBoostPercentage = 80f, autoAttackRange = 4f;

        Health target;
        Equipment equipment;
        float timeSinceLastAtk = Mathf.Infinity;
        [SerializeField] WeaponConfig currentWeaponConfig;
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

        public bool GetInCombat()
        {
            return target != null;
        }

        private void Update()
        {
            GetComponent<Health>().SetInCombat( target != null );

            if(GetComponent<Health>().IsDead()){ target = null; }

            timeSinceLastAtk += Time.deltaTime;

            if (target == null)  return;

            if(target.IsDead())
            {
                TargetNewTargetInRange();
                if(target == null) return;
            }

            if (!GetIsInRange(target.transform)) { GetComponent<MovementController>().MoveTo(target.transform.position, 1f); }
            else
            {
                GetComponent<MovementController>().Cancel();
                
                AttackBehaviour();
            }

            #if UNITY_EDITOR
            if(Input.GetKey(KeyCode.Pause)){ Time.timeScale = 5;}
            #endif
        }

        public void TargetNewTargetInRange()
        {
            //target = null;
            target = FindNewTargetInRange();
            if(target == null) return;
        }


        private Weapons SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInitialization();
            

        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform); //rotate towards enemy after you start attacking
            
            //

            if(timeSinceLastAtk > currentWeaponConfig.GetAttackSpeed() && target.IsDead() == false)
            {   
                // This will trigger the Hit Event
                TriggerAtk();
                timeSinceLastAtk = 0;
            }

        }

        public Health FindNewTargetInRange()
        {
            Health best = null;
            float bestDistance = Mathf.Infinity;
            foreach (var candidate in FindAllTargetsInRange())
            {
                float candidateDistance = Vector3.Distance(transform.position, candidate.transform.position);
                if(candidateDistance<bestDistance)
                {
                    best = candidate;
                    bestDistance = candidateDistance;
                }
            }
            return best;
        }

        private IEnumerable<Health> FindAllTargetsInRange()
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, autoAttackRange, Vector3.up);

            foreach (var hit in raycastHits)
            {
                Health health = hit.transform.GetComponent<Health>();
                if(health == null) continue;
                if(health.IsDead()) continue;
                if(health.gameObject == gameObject) continue;
                yield return health;
            }
        }

        public float GetSpeedPercentageModifier()
        {
            BaseStats baseStats = GetComponent<BaseStats>();
            float speedBoost = baseStats.GetStat(Stat.AttackSpeedPercentage);
            return (1 - Mathf.Min(speedBoost, maxSpeedBoostPercentage) / 100);
        }

        private void TriggerAtk()
        {
            GetComponent<Animator>().ResetTrigger("stopAtk");
            GetComponent<Animator>().SetTrigger("attack");
        }

        void Hit() //Animation Event
        {
            if (target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Attack);

            BaseStats targetBaseStats = target.GetComponent<BaseStats>();

            if(targetBaseStats != null)
            {
                float defence = targetBaseStats.GetStat(Stat.Defence);
                damage /= 1 + defence / damage;
            }

            target.SetInCombat(true);

            if(currentWeapon.value != null) { currentWeapon.value.OnHit(); }

            if (currentWeaponConfig.HasProjectile()) { currentWeaponConfig.LaunchProjectile(rightHandTrans, leftHandTrans, target, gameObject, damage); }
            else {  target.TakeDamage(gameObject, damage); }            
        }

        void Shoot()
        {
            Hit();
        }

        public bool CanAtk(GameObject combatControllerTarget)
        {
            if(combatControllerTarget == null) { return false; }
            if (!GetComponent<MovementController>().CanMoveTo(combatControllerTarget.transform.position) && !GetIsInRange(combatControllerTarget.transform)) { return false; }

            Health trgtToTst = combatControllerTarget.GetComponent<Health>();
            return trgtToTst != null && !trgtToTst.IsDead();
        }

        public void Attack(GameObject combatControllerTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatControllerTarget.GetComponent<Health>();
        }


        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.GetWeaponRange();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
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

        public Health GetTarget()
        {
            return target;
        }

        public Transform GetHandTransform(bool isRightHand)
        {
            if(isRightHand){ return rightHandTrans; }
            else { return leftHandTrans; }
        }

    }
}
