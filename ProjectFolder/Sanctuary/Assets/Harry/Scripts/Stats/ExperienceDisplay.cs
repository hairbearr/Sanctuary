using UnityEngine;
using TMPro;
using System;

namespace Sanctuary.Harry.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience xp;

        private void Awake()
        {
            xp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}", xp.GetXP());
        }
    }
}
