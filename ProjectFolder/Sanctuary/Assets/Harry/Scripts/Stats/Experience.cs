using System.Collections;
using UnityEngine;
using GameDevTV.Saving;
using System;

namespace Sanctuary.Harry.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {

        [SerializeField] float xpPoints = 0;

        //Actions are functions that return void and take no Arguments
        public event Action onExperienceGained;

        public float GetXP()
        {
            return xpPoints;
        }

        public void GainXP(float xp)
        {
            // the action, onExperienceGained, is being called in Gain XP
            xpPoints += xp;
            onExperienceGained();
        }

        public object CaptureState()
        {
            return xpPoints;
        }

        public void RestoreState(object state)
        {
            xpPoints = (float) state;
        }
    }
}