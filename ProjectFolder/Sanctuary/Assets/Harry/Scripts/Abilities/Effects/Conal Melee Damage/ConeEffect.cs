using System;
using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Combat;
using UnityEngine;

namespace Sanctuary.Harry.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Cone Effect", menuName = "Sanctuary/Abilities/Effects/Make New Cone Effect", order = 0)]
    public class ConeEffect : EffectStrategy
    {
        [SerializeField] GameObject coneObjectToSpawn;


        public override void StartEffect(AbilityData data, Action finished)
        {
            CombatController combatController = data.GetUser().GetComponent<CombatController>();
            Vector3 spawnPosition = combatController.transform.position;
            SpawnCone(data, spawnPosition);
            finished();
        }

        private void SpawnCone(AbilityData data, Vector3 spawnPosition)
        {
            GameObject coneObject = Instantiate(coneObjectToSpawn);
            coneObject.GetComponent<ConeEffectPrefabScript>().SetInstigator(data.GetUser());
            coneObject.transform.position = spawnPosition;
            coneObject.transform.rotation = data.GetUser().transform.rotation;
        }
    }
}
