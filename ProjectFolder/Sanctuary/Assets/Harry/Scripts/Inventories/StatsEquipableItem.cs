using GameDevTV.Inventories;
using Sanctuary.Harry.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Inventories
{
    [CreateAssetMenu(fileName = "New Armor", menuName = ("Sanctuary/Armor/Make New Armor"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [SerializeField] Modifier[] additiveModifiers;
        [SerializeField] Modifier[] percentageModifiers;

        [System.Serializable] public struct Modifier
        {
            public Stat stat;
            public float value;
        }

        public virtual IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach(var modifier in additiveModifiers)
            {
                if(modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public virtual IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }
    }
}
