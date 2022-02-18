using Sanctuary.Harry.Core;
using Sanctuary.Harry.Attributes;
using System;
using UnityEngine;

namespace Sanctuary.Harry.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Sanctuary/Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] float wepRange=0, atkSpd=0, dmg=0;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if(equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null) { animator.runtimeAnimatorController = overrideController.runtimeAnimatorController; }
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

        public float GetWeaponDamage()
        {
            return dmg;
        }

        public float GetAttackSpeed()
        {
            return atkSpd;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projInstance.SetTarget(target,dmg);
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform previousWeapon = rightHand.Find(weaponName);
            if(previousWeapon == null) { previousWeapon = leftHand.Find(weaponName); }
            if(previousWeapon == null) { return; }

            previousWeapon.name = "DESTROYING";
            Destroy(previousWeapon.gameObject);
        }
    }
}