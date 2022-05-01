using GameDevTV.UI;
using GameDevTV.Utils;
using Sanctuary.Harry.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleFX = null;
        [SerializeField] bool shouldUseModifiers = false;
        [SerializeField] ShowHideUI traitWindow = null;

        public event Action onLevelUp;

        LazyValue<int> currentLevel;

        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInitialization();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveMod(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        public int GetLevel()
        {
            if(currentLevel.value < 1) { currentLevel.value = CalculateLevel(); }
            return currentLevel.value;
        }

        public float GetXPToLevelUp()
        {
            float XPToLevelUp = progression.GetStat(Stat.XPToLevelUp, characterClass, GetLevel());
            return XPToLevelUp;
        }

        public float GetPreviousLevelXPToLevelUp()
        {
            float XPToLevelUp;
            if(GetLevel() <= 1){ XPToLevelUp = 0;}
            else { XPToLevelUp = progression.GetStat(Stat.XPToLevelUp, characterClass, GetLevel()-1);}
            return XPToLevelUp;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) { return startingLevel; }

            float currentXP = experience.GetXP();
            int penultimateLevel = progression.GetLevels(Stat.XPToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.XPToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleFX, transform);
            

            ShowTraitWindow();
        }

        private IEnumerator ShowTraitWindow()
        {
            while(GetComponent<Health>().GetInCombat()) yield return null;
            traitWindow.Open();
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveMod(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }
    }
}
