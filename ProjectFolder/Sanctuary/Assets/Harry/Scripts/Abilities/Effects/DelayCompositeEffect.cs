using System;
using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Attributes;
using UnityEngine;

namespace Sanctuary.Harry.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Delay Composite Effect", menuName = "Sanctuary/Abilities/Effects/Make New Delay Composite Effect", order = 0)]
    public class DelayCompositeEffect : EffectStrategy
    {

        [SerializeField] float delay = 0;
        [SerializeField] EffectStrategy[] delayedEffects;
        [SerializeField] bool abortIfCancelled = false;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(DelayedEffect(data, finished));
        }

        private IEnumerator DelayedEffect(AbilityData data, Action finished)
        {
            yield return new WaitForSeconds(delay);
            if(abortIfCancelled && data.IsCancelled()) yield break;

            foreach (var effect in delayedEffects)
            {
                effect.StartEffect(data, finished);
            }
        }
    }
}
