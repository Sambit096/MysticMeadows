using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    /// <summary>
    /// ScriptableObject that defines stat progression curves for different character classes.
    /// </summary>
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookUpTable = null;

        /// <summary>
        /// Gets the stat value for a given character class at a specific level.
        /// </summary>
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookUp();
            float[] levels = lookUpTable[characterClass][stat];
            if (levels.Length < level)
            {
                return 0;
            }
            return levels[level - 1];
        }

        /// <summary>
        /// Gets the number of levels defined for a stat and character class.
        /// </summary>
        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookUp();
            float[] levels = lookUpTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookUp()
        {
            if (lookUpTable != null) return;

            lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookUpTable[progressionStat.stat] = progressionStat.levels;
                }
                lookUpTable[progressionClass.characterClass] = statLookUpTable;
            }
        }

        [System.Serializable]
        private class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        private class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}