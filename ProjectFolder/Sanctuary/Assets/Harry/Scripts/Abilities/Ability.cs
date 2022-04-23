using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using Sanctuary.Harry.Attributes;
using Sanctuary.Harry.Core;
using UnityEngine;

namespace Sanctuary.Harry.Abilities
{

    [CreateAssetMenu(fileName = "New Ability", menuName = "Sanctuary/Abilities/Ability/Make New Ability", order = 0)]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
        [SerializeField] float cooldownTime = 0;
        [SerializeField] float playerResourceCost = 0;
        
        private GameObject effectUser;

        public override void Use(GameObject user)
        {
            PlayerResources playerResouces = user.GetComponent<PlayerResources>();
            effectUser = user.gameObject;
            if(playerResouces.GetResources() < playerResourceCost)
            {
                return;
            }

            CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
            if(cooldownStore.GetTimeRemaining(this) > 0) return; 

            AbilityData data = new AbilityData(user);

            ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(data);

            targetingStrategy.StartTargeting(data, ()=> TargetAcquired(data));
        }

        private void TargetAcquired(AbilityData data)
        {
            if(data.IsCancelled()) { return; }
            
            PlayerResources playerResouces = data.GetUser().GetComponent<PlayerResources>();
            if(!playerResouces.UseResources(playerResourceCost)) return;

            CooldownStore cooldownStore = data.GetUser().GetComponent<CooldownStore>();
            cooldownStore.StartCooldown(this, cooldownTime);

            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
        }

        private void EffectFinished()
        {
            
        }
    }
    
}
