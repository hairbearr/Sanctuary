using UnityEngine;
using TMPro;
using System;

namespace Sanctuary.Harry.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}", baseStats.GetLevel());
        }
    }
}
