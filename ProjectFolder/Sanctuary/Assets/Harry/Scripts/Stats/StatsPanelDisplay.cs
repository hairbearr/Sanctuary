using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Attributes;
using TMPro;
using UnityEngine;

namespace Sanctuary.Harry.Stats
{
    public class StatsPanelDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text strengthText, intellectText, staminaText, armorText, damageText, healthText, healthRegenText, manaText, manaRegenText;

        BaseStats baseStats;


        // Start is called before the first frame update
        void Start()
        {
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }

        // Update is called once per frame
        void Update()
        {
            strengthText.text = baseStats.GetStat(Stat.Strength).ToString();
            intellectText.text = baseStats.GetStat(Stat.Intellect).ToString();
            staminaText.text = baseStats.GetStat(Stat.Stamina).ToString();
            armorText.text = baseStats.GetStat(Stat.Armor).ToString();
            damageText.text = baseStats.GetStat(Stat.Attack).ToString();
            healthRegenText.text = baseStats.GetStat(Stat.HealthRegenRate).ToString() + " Per Second";
            manaRegenText.text = baseStats.GetStat(Stat.PlayerResourcesRegenRate).ToString() + " Per Second";
            healthText.text = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().GetMaxHealthPoints().ToString();
            manaText.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>().GetMaxResources().ToString();
        }
    }
}
