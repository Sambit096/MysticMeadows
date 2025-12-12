using System;
using UnityEngine;
using RPG.Resources;

namespace RPG.Audio
{
    /// <summary>
    /// Handles enemy death announcements and tracks remaining enemies.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class EnemyDeathAudio : MonoBehaviour
    {
        /// <summary>
        /// Fired when an enemy dies. Parameter is the number of enemies remaining.
        /// </summary>
        public static Action<int> OnEnemyDied;

        [Header("Audio Source")]
        [SerializeField] private AudioSource audioSource;

        [Header("Voice Lines")]
        [Tooltip("Optional death announcement clip.")]
        [SerializeField] private AudioClip deathClip;
        [SerializeField] private AudioClip enemiesRemaining5;
        [SerializeField] private AudioClip enemiesRemaining4;
        [SerializeField] private AudioClip enemiesRemaining3;
        [SerializeField] private AudioClip enemiesRemaining2;
        [SerializeField] private AudioClip enemiesRemaining1;
        [SerializeField] private AudioClip victoryClip;

        [Header("Pitch/Volume")]
        [SerializeField] private float minPitch = 0.95f;
        [SerializeField] private float maxPitch = 1.05f;
        [SerializeField] private float volume = 1f;

        private Health health;
        private bool hasPlayed = false;

        private static bool initialized = false;
        private static bool introPlayed = false;
        private static int enemiesRemaining = 0;
        private bool wasDeadOnLoad = false;
        private void Awake()
        {
            health = GetComponent<Health>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.loop = false;
                audioSource.spatialBlend = 1f;
            }
        }

        private void Start()
            {
                if (!initialized)
                {
                    EnemyDeathAudio[] all = FindObjectsOfType<EnemyDeathAudio>();
                    enemiesRemaining = 0;

                    foreach (var e in all)
                    {
                        if (e != null && e.health != null && !e.health.IsDead())
                        {
                            enemiesRemaining++;
                        }
                    }

                    initialized = true;

                    if (!introPlayed)
                    {
                        introPlayed = true;
                        PlayIntroRemainingLine();
                        OnEnemyDied?.Invoke(enemiesRemaining);
                    }
                }

                //remember if this enemy is already dead after load
                if (health != null && health.IsDead())
                {
                    wasDeadOnLoad = true;
                    hasPlayed = true; // so Update won't announce this death
                }
            }


        private void Update()
        {
            if (hasPlayed) return;
            if (health == null) return;

            if (health.IsDead())
            {
                // Enemy died during this play session, not before load
                PlayDeathAnnouncement();
            }
        }

        private void PlayIntroRemainingLine()
        {
            AudioClip clip = GetRemainingClip(enemiesRemaining);
            if (clip == null || audioSource == null) return;

            audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(clip, volume);
        }

        private void PlayDeathAnnouncement()
        {
            if (audioSource == null) return;

            hasPlayed = true;
            enemiesRemaining = Mathf.Max(0, enemiesRemaining - 1);

            audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);

            if (deathClip != null)
            {
                audioSource.PlayOneShot(deathClip, volume);
            }

            AudioClip remainingClip = GetRemainingClip(enemiesRemaining);
            if (remainingClip != null)
            {
                audioSource.PlayOneShot(remainingClip, volume);
            }

            OnEnemyDied?.Invoke(enemiesRemaining);
        }

        private AudioClip GetRemainingClip(int remaining)
        {
            switch (remaining)
            {
                case 5: return enemiesRemaining5;
                case 4: return enemiesRemaining4;
                case 3: return enemiesRemaining3;
                case 2: return enemiesRemaining2;
                case 1: return enemiesRemaining1;
                case 0: return victoryClip;
            }
            return null;
        }
    }
}
