using UnityEngine;

namespace Sanctuary.Harry.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Sanctuary/Stats/Make New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass progClass in characterClasses)
            {
                if(progClass.characterClass == characterClass)
                {
                    return progClass.health[level - 1];
                }
            }

            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public float[] health;
        }
    }
}