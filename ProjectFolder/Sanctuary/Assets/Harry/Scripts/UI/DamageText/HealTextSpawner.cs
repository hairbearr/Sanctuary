using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sanctuary.Harry.DamageText
{
    public class HealTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText dmgTextPrefab = null;

        public void Spawn(float dmgAmount)
        {
            DamageText instance = Instantiate<DamageText>(dmgTextPrefab, transform);
            instance.SetValue(dmgAmount);
        }
    }
}