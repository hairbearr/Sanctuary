using System;
using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Attributes;
using UnityEngine;

namespace Sanctuary.Harry.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Debuff Effect", menuName = "Sanctuary/Abilities/Effects/Make New Debuff Effect", order = 0)]
    public class DebuffEffect : EffectStrategy
    {
        [Range(1, 10)]
        [SerializeField] float damageTakenModifier;
        [SerializeField] float debuffDuration = -1;
        [SerializeField] Transform prefabToSpawn;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
            finished();
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            


            foreach (var target in data.GetTargets())
            {
                Transform instance = null;
                var health = target.GetComponent<Health>();
                health.SetDamageTakenModifier(damageTakenModifier);
                var transform = target.GetComponent<Transform>();
                
                if(target.CompareTag("Enemy"))
                {
                    instance = Instantiate(prefabToSpawn);
                    instance.SetParent(transform);
                    instance.position = transform.position;

                    if(health.IsDead() == true){ Destroy(instance.gameObject);}
                }
                
                if(health)
                {
                    if(debuffDuration > 0)
                    {
                        yield return new WaitForSeconds(debuffDuration);
                        health.SetDamageTakenModifier(1);
                    }

                    
                }
            }
        }
    }
}
