using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Abilities
{
    public abstract class TargetingStrategy: ScriptableObject
    {
        public abstract void StartTargeting(AbilityData data, Action finished);
    }
}