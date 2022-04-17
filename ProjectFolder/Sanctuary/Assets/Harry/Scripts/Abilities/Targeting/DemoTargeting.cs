using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Demo Targeting", menuName = "Sanctuary/Abilities/Targeting/Demo", order = 0)]
    public class DemoTargeting : TargetingStrategy
    {
        public override void StartTargeting(GameObject user, Action<IEnumerable<GameObject>> finished)
        {
            Debug.Log("Demo Targeting Started");
            finished(null);
        }
    }
}