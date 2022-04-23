using System;
using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Attributes;
using Sanctuary.Harry.Combat;
using UnityEngine;

namespace Sanctuary.Harry.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Projectile Effect", menuName = "Sanctuary/Abilities/Effects/Make New Projectile Effect", order = 0)]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] Projectile projectileToSpawn;
        [SerializeField] float damage;
        [SerializeField] bool isRightHand;
        [SerializeField] bool useTargetPoint = true;

        public override void StartEffect(AbilityData data, Action finished)
        {
            CombatController combatController = data.GetUser().GetComponent<CombatController>();
            Vector3 spawnPosition = combatController.GetHandTransform(isRightHand).position;
            if(useTargetPoint){ SpawnProjectileForTargetPoint(data, spawnPosition); }
            else { SpawnProjectilesForTargets(data, spawnPosition); }
            finished();
        }

        private void SpawnProjectileForTargetPoint(AbilityData data, Vector3 spawnPosition)
        {
            Projectile projectile = Instantiate(projectileToSpawn);
            projectile.transform.position = spawnPosition;
            projectile.SetTarget(data.GetTargetedPoint(), data.GetUser(), damage);
        }

        private void SpawnProjectilesForTargets(AbilityData data, Vector3 spawnPosition)
        {
            foreach (var target in data.GetTargets())
            {
                Health health = target.GetComponent<Health>();
                if (health)
                {
                    Projectile projectile = Instantiate(projectileToSpawn);
                    projectile.transform.position = spawnPosition;
                    projectile.SetTarget(health, data.GetUser(), damage);
                }
            }
        }
    }

}
