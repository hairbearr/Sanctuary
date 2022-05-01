using Sanctuary.Harry.Core;
using Sanctuary.Harry.Attributes;
using System;
using UnityEngine;
using GameDevTV.Inventories;
using Sanctuary.Harry.Stats;
using System.Collections.Generic;
using Sanctuary.Harry.Inventories;

namespace Sanctuary.Harry.Combat
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Sanctuary/Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : StatsEquipableItem
    {
        [SerializeField] float weaponRange = 0, attackSpeed = 0, damage = 0, percentageBonus = 0;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapons equippedPrefab = null;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public Weapons Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapons weapon = null;

            if(equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null) { animator.runtimeAnimatorController = overrideController.runtimeAnimatorController; }

            return weapon;
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;

            if (isRightHanded) { handTransform = rightHand; }
            else { handTransform = leftHand; }

            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        public float GetWeaponDamage()
        {
            return damage;
        }

        public float GetAttackSpeed()
        {
            return attackSpeed;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDmg)
        {
            Projectile projInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projInstance.SetTarget(target, instigator, calculatedDmg);
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform previousWeapon = rightHand.Find(weaponName);
            if(previousWeapon == null) { previousWeapon = leftHand.Find(weaponName); }
            if(previousWeapon == null) { return; }

            previousWeapon.name = "DESTROYING";
            Destroy(previousWeapon.gameObject);
        }

        public override IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (float mod in base.GetAdditiveModifiers(stat))
            {
                yield return mod;
            }
            if(stat == Stat.Attack)
            {
                yield return damage;
            }
            if(stat == Stat.AttackSpeed)
            {
                yield return attackSpeed;
            }
        }

        public override IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (float mod in base.GetPercentageModifiers(stat))
            {
                yield return mod;
            }
            if(stat == Stat.Attack)
            {
                yield return percentageBonus;
            }
        }
    }
}