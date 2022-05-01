using Sanctuary.Harry.Core;
using Sanctuary.Harry.Attributes;
using System;
using UnityEngine;
using GameDevTV.Inventories;
using Sanctuary.Harry.Stats;
using System.Collections.Generic;

namespace Sanctuary.Harry.Combat
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Sanctuary/Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        [SerializeField] float wepRange = 0, atkSpd = 0, dmg = 0, percentageBonus = 0;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapons equippedPrefab = null;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        [SerializeField] Modifier[] additiveModifiers, percentageModifiers;

        [System.Serializable] struct Modifier
        {
            public Stat stat;
            public float value;
        }

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
            return wepRange;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        public float GetWeaponDamage()
        {
            return dmg;
        }

        public float GetAttackSpeed()
        {
            return ((float)Stat.AttackSpeed);
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

        public IEnumerable<float> GetAdditiveMods(Stat stat)
        {
            foreach(var modifier in additiveModifiers)
            {
                if(modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<float> GetPercentageMods(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }
    }
}