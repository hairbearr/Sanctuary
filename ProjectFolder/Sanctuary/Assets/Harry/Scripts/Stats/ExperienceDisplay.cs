using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace Sanctuary.Harry.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience xp;
        BaseStats baseStats;

        public float previousXPToLevelUp = 0;

        [SerializeField] Image foreground = null;


        private void Awake()
        {
            xp = GameObject.FindWithTag("Player").GetComponent<Experience>();
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        void Update()
        {
            //GetComponent<TMP_Text>().text = String.Format("{0:0}", xp.GetXP());

            foreground.fillAmount = (xp.GetXP() - baseStats.GetPreviousLevelXPToLevelUp() )/ (baseStats.GetXPToLevelUp() - baseStats.GetPreviousLevelXPToLevelUp());
        }
    }
}
