using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    /// <summary>
    /// Tracks player experience points and provides save/load functionality.
    /// </summary>
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints = 0f;

        /// <summary>
        /// Event fired when experience is gained.
        /// </summary>
        public event Action onExperienceGained;

        /// <summary>
        /// Adds experience points and fires the gain event.
        /// </summary>
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained?.Invoke();
        }

        /// <summary>
        /// Returns the current experience points.
        /// </summary>
        public float GetExperience()
        {
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
