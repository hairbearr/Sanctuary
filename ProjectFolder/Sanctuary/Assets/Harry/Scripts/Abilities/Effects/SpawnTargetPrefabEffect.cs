using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Spawn Target Prefab Effect", menuName = "Sanctuary/Abilities/Effects/Make New Spawn Target Prefab", order = 0)]
    public class SpawnTargetPrefabEffect : EffectStrategy
    {
        [SerializeField] Transform prefabToSpawn;
        [SerializeField] float destroyDelay = -1;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            Transform instance = Instantiate(prefabToSpawn);
            instance.position = data.GetTargetedPoint();
            if(destroyDelay>0){ yield return new WaitForSeconds(destroyDelay); Destroy(instance.gameObject);}
            finished();
            
        }
    }

}
