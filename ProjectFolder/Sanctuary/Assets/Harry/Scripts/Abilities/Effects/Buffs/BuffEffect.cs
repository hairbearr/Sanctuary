using System;
using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Attributes;
using UnityEngine;

namespace Sanctuary.Harry.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Buff Effect", menuName = "Sanctuary/Abilities/Effects/Make New Buff Effect", order = 0)]
    public class BuffEffect : EffectStrategy
    {
        [SerializeField] bool invulnerableEffect;
        [SerializeField] float buffTimeOut = -1, damageToReduce=-1;
        [SerializeField] bool damageShield;
        [SerializeField] Transform prefabToSpawn;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {

            Transform instance = Instantiate(prefabToSpawn);

            if(invulnerableEffect)
            {
                foreach(var target in data.GetTargets())
                {
                    var health = target.GetComponent<Health>();
                    var transform = target.GetComponent<Transform>();
                    instance.SetParent(transform);
                    instance.position = data.GetTargetedPoint() + Vector3.up;;
                    if(health)
                    {
                        health.SetInvulnerability(true);
                        if(buffTimeOut > 0)
                        {
                            yield return new WaitForSeconds(buffTimeOut);
                            health.SetInvulnerability(false);
                        }
                    }
                }
            }

            if(damageShield)
            {
                foreach(var target in data.GetTargets())
                {
                    var health = target.GetComponent<Health>();
                    var transform = target.GetComponent<Transform>();
                    instance.SetParent(transform);
                    instance.position = target.transform.position + Vector3.up;
                    health.SetShielded(true);
                    health.SetShieldPoints(damageToReduce);
                    if(health)
                    {
                        if(buffTimeOut > 0)
                        {
                            yield return new WaitForSeconds(buffTimeOut);
                            health.SetShielded(false);
                        }
                    }
                }
            }
            finished();
        }
    }
}
