using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanctuary.Harry.Stats;

namespace Sanctuary.Harry.Inventories
{
    public class StatsEquipment : Equipment, IModifierProvider
    {
        public IEnumerable<float> GetAdditiveMods(Stat stat)
        {
            foreach(var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if (item == null) continue;

                foreach(float modifier in item.GetAdditiveMods(stat))
                {
                    yield return modifier;
                }
            }
                
        }

        public IEnumerable<float> GetPercentageMods(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if (item == null) continue;

                foreach (float modifier in item.GetPercentageMods(stat))
                {
                    yield return modifier;
                }
            }
        }
    }
}
