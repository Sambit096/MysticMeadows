using UnityEngine;
using RPG.Audio;   

namespace RPG.SceneManagement
{
    public class EnemyRemainingUI : MonoBehaviour
    {
        [SerializeField] private FadingText fadingTextPrefab;   
        [SerializeField] private Transform uiParent;           

        // Tracks whether we've already shown the startup ("N enemies remaining") message
        private bool introShown = false;

        private void Awake()
        {
            if (uiParent == null) uiParent = transform;
        }

        private void OnEnable()
        {
            EnemyDeathAudio.OnEnemyDied += HandleEnemyDied;
        }

        private void OnDisable()
        {
            EnemyDeathAudio.OnEnemyDied -= HandleEnemyDied;
        }

        private void HandleEnemyDied(int remaining)
        {
            if (fadingTextPrefab == null || uiParent == null) return;

            string message;

            if (!introShown)
            {
                // First announcement (level start) — only show count, not "Enemy down."
                introShown = true;
                message = remaining > 0
                    ? $"{remaining} {(remaining == 1 ? "enemy" : "enemies")} remaining"
                    : "Victory!";
            }
            else
            {
                // Subsequent announcements — show the "enemy down" phrasing
                message = remaining > 0
                    ? $"Enemy down. {remaining} {(remaining == 1 ? "enemy" : "enemies")} remaining"
                    : "Victory!";
            }

            FadingText instance = Instantiate(fadingTextPrefab, uiParent);
            instance.SetText(message);
        }
    }
}
