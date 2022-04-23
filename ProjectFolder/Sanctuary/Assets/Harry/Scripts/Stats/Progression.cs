using UnityEngine;
using System.Collections.Generic;
using System;

namespace Sanctuary.Harry.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Sanctuary/Stats/Make New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            if(!lookupTable[characterClass].ContainsKey(stat)) { return 0;}

            float[] levels = lookupTable[characterClass][stat];

            if(levels.Length == 0){ return 0; }

            if(levels.Length < level)
            {
                return levels[levels.Length -1];
            }

            return levels[level - 1];
        }
        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progStat in progClass.stats)
                {
                    statLookupTable[progStat.stat] = progStat.levels;
                }

                lookupTable[progClass.characterClass] = statLookupTable;
            }
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}