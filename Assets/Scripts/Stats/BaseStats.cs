using System;
using UnityEngine;

namespace RPG.Stats
{
    /// <summary>
    /// Handles character statistics including level calculation and stat modifiers.
    /// </summary>
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;
        [SerializeField] private GameObject levelUpParticleEffect = null;
        [SerializeField] private bool shouldUseModifiers = false;

        private int currentLevel = 0;
        private Experience experience;

        public event Action onLevelUp;

        private void Awake()
        {
            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            currentLevel = CalculateLevel();
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
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
                print("Leveled Up");
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        /// <summary>
        /// Gets the calculated value for a given stat, including all modifiers.
        /// </summary>
        public float GetStat(Stat stat)
        {
            return GetBaseStat(stat) + GetAdditiveModifiers(stat) * (1 + (GetPercentageModifier(stat) / 100));
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        /// <summary>
        /// Gets the current level of this character.
        /// </summary>
        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        /// <summary>
        /// Calculates the level based on current experience.
        /// </summary>
        public int CalculateLevel()
        {
            Experience exp = GetComponent<Experience>();
            if (exp == null)
            {
                return startingLevel;
            }

            float currentXP = exp.GetExperience();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (xpToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

        private float GetAdditiveModifiers(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvder provider in GetComponents<IModifierProvder>())
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
            foreach (IModifierProvder provider in GetComponents<IModifierProvder>())
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
