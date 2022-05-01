using System;
using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Attributes;
using UnityEngine;

namespace Sanctuary.Harry.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Resource Effect", menuName = "Sanctuary/Abilities/Effects/Make New Resource Effect", order = 0)]
    public class ResourceEffect : EffectStrategy
    {
        [SerializeField] float playerResourceChange;
        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                var playerResources = target.GetComponent<PlayerResources>();
                if(playerResources)
                {
                    if(playerResourceChange<0)
                    {
                        playerResources.Degenerate(playerResourceChange);
                    }
                    else
                    {
                        playerResources.Regenerate(playerResourceChange);
                    }
                }
            }
        }
    }
}