using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Abilities.Filters
{
    [CreateAssetMenu(fileName = "New Tag Filter", menuName = "Sanctuary/Abilities/Filters/Make New Tag", order = 0)]
    public class TagFilter : FilterStrategy
    {
        [SerializeField] string tagToFilter = "";

        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
        {
            foreach (var gameObject in objectsToFilter)
            {
                if(gameObject.CompareTag(tagToFilter)) { yield return gameObject; }
            }
        }
    }
}
