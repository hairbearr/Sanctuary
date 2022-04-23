using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using GameDevTV.Utils;
using Sanctuary.Harry.Stats;
using UnityEngine;

namespace Sanctuary.Harry.Attributes
{
    public class PlayerResources : MonoBehaviour, ISaveable
    {
        LazyValue<float> playerResources;

        private void Awake()
        {
            playerResources = new LazyValue<float>(GetMaxResources);
        }

        private void Update()
        {
            if(playerResources.value < GetMaxResources())
            {
                playerResources.value += GetRegenRate() * Time.deltaTime;
                if(playerResources.value > GetMaxResources()) { playerResources.value = GetMaxResources(); }
            }
        }

        public float GetResources()
        {
            return playerResources.value;
        }

        public float GetMaxResources()
        {
            return GetComponent<BaseStats>().GetStat(Stat.PlayerResources);
        }

        public float GetRegenRate()
        {
            return GetComponent<BaseStats>().GetStat(Stat.PlayerResourcesRegenRate);
        }

        public bool UseResources(float playerResourcesToUse)
        {
            if(playerResourcesToUse > playerResources.value)
            {
                return false;
            }
            playerResources.value -= playerResourcesToUse;
            return true;
        }

        public object CaptureState()
        {
            return playerResources.value;
        }

        public void RestoreState(object state)
        {
            playerResources.value = (float) state;
        }
    }
}
