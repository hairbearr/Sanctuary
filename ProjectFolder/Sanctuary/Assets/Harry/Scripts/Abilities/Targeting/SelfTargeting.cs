using UnityEngine;
using Sanctuary.Harry.Control;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Sanctuary.Harry.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "New Self Targeting", menuName = "Sanctuary/Abilities/Targeting/Create New Self Targeting", order = 0)]
    public class SelfTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            data.SetTargets(new GameObject[]{data.GetUser()});
            data.SetTargetedPoint(data.GetUser().transform.position);
            finished();
        }
    }
}
