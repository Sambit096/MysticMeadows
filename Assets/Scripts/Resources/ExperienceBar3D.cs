using UnityEngine;
using TMPro;
using RPG.Stats;

namespace RPG.Resources
{
    /// <summary>
    /// Displays a 3D experience bar that fills based on current XP.
    /// </summary>
    public class ExperienceBar3D : MonoBehaviour
    {
        [SerializeField] private Experience experience;
        [SerializeField] private Transform barFill;
        [SerializeField] private TMP_Text valueText;

        [Tooltip("How much XP fills this bar (e.g. XP needed for current level).")]
        [SerializeField] private float maxExperienceForThisBar = 100f;

        private Vector3 initialScale;

        private void Awake()
        {
            if (experience == null)
            {
                experience = GetComponentInParent<Experience>();
            }

            if (barFill != null)
            {
                initialScale = barFill.localScale;
            }
        }

        private void Update()
        {
            if (experience == null || barFill == null) return;

            float current = experience.GetExperience();
            float fraction = 0f;

            if (maxExperienceForThisBar > 0f)
            {
                fraction = Mathf.Clamp01(current / maxExperienceForThisBar);
            }

            barFill.localScale = new Vector3(
                initialScale.x * fraction,
                initialScale.y,
                initialScale.z
            );

            if (valueText != null)
            {
                int currentInt = Mathf.FloorToInt(current);
                int maxInt = Mathf.FloorToInt(maxExperienceForThisBar);
                valueText.text = currentInt + " / " + maxInt;
            }
        }

        /// <summary>
        /// Sets the maximum experience value for this bar.
        /// </summary>
        public void SetMaxExperience(float newMax)
        {
            maxExperienceForThisBar = Mathf.Max(1f, newMax);
        }
    }
}
