using System;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Resources
{
    /// <summary>
    /// Manages health points, damage, death, and health regeneration on level up.
    /// </summary>
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regenerationPercentage = 70f;

        private float healthPoints = -1f;
        private bool isDead = false;

        private void Start()
        {
            if (healthPoints <= 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        /// <summary>
        /// Returns whether this entity is dead.
        /// </summary>
        public bool IsDead()
        {
            return isDead;
        }

        /// <summary>
        /// Applies damage from the specified instigator.
        /// </summary>
        public void TakeDamage(GameObject instigator, float damage)
        {
            Debug.Log(gameObject.name + " took damage: " + damage);
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            Debug.Log(healthPoints + " " + gameObject.name);

            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        /// <summary>
        /// Returns the current health points.
        /// </summary>
        public float GetHealthPoints()
        {
            return healthPoints;
        }

        /// <summary>
        /// Returns the maximum health points.
        /// </summary>
        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        /// <summary>
        /// Returns the current health as a percentage.
        /// </summary>
        public float GetPercentage()
        {
            return (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}
