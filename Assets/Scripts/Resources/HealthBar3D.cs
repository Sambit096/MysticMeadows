using UnityEngine;
using TMPro;

namespace RPG.Resources
{
    /// <summary>
    /// Displays a 3D health bar that scales based on current health percentage.
    /// </summary>
    public class HealthBar3D : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Transform barFill;
        [SerializeField] private TMP_Text percentText;

        private Vector3 initialScale;

        private void Awake()
        {
            if (health == null)
            {
                health = GetComponentInParent<Health>();
            }

            if (barFill != null)
            {
                initialScale = barFill.localScale;
            }
        }

        private void Update()
        {
            if (health == null || barFill == null) return;

            float current = health.GetHealthPoints();
            float max = health.GetMaxHealthPoints();
            float fraction = Mathf.Clamp01(current / max);

            barFill.localScale = new Vector3(
                initialScale.x * fraction,
                initialScale.y,
                initialScale.z
            );

            if (percentText != null)
            {
                int percent = Mathf.RoundToInt(fraction * 100f);
                percentText.text = percent + "%";
            }
        }
    }
}