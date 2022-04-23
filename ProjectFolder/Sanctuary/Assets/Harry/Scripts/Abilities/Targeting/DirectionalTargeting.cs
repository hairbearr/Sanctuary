using System;
using System.Collections.Generic;
using Sanctuary.Harry.Control;
using UnityEngine;

namespace Sanctuary.Harry.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "New Directional Targeting", menuName = "Sanctuary/Abilities/Targeting/Make New Directional Targeting", order = 0)]
    public class DirectionalTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float groundOffset = 1;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            RaycastHit raycastHit;
            Ray ray = PlayerController.GetMouseRay();
            if (Physics.Raycast(ray, out raycastHit, 1000, layerMask))
            {
                data.SetTargetedPoint(raycastHit.point + ray.direction * groundOffset / ray.direction.y);
            }
            finished();
        }
    }
}
