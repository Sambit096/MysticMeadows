using UnityEngine;
using RPG.Resources;
using RPG.Stats;

namespace RPG.Audio
{
    /// <summary>
    /// Controls audio playback for the player character including locomotion, attack, and death sounds.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Health))]
    public class PlayerAudioController : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField] private AudioSource loopSource;
        [SerializeField] private AudioSource oneShotSource;

        [Header("Clips")]
        [SerializeField] private AudioClip runLoop;
        [SerializeField] private AudioClip attackClip;
        [SerializeField] private AudioClip deathClip;

        [Header("Experience")]
        [SerializeField] private AudioClip gainExperienceClip;

        [Header("Locomotion")]
        [Tooltip("Animator float parameter that drives the locomotion blend tree.")]
        [SerializeField] private string speedParam = "ForwardSpeed";

        [Tooltip("Speed above this is considered 'running'.")]
        [SerializeField] private float runThreshold = 0.1f;

        private Experience experience;
        private Animator animator;
        private Health health;

        private string currentLoopState = "";
        private bool deathPlayed = false;
        private bool wasAttackingLastFrame = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            experience = GetComponent<Experience>();

            if (loopSource == null)
            {
                loopSource = gameObject.AddComponent<AudioSource>();
                loopSource.loop = true;
                loopSource.playOnAwake = false;
                loopSource.spatialBlend = 1f;
            }

            if (oneShotSource == null)
            {
                oneShotSource = gameObject.AddComponent<AudioSource>();
                oneShotSource.loop = false;
                oneShotSource.playOnAwake = false;
                oneShotSource.spatialBlend = 1f;
            }
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += OnExperienceGained;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= OnExperienceGained;
            }
        }

        private void Update()
        {
            if (health != null && health.IsDead())
            {
                HandleDeathAudio();
                return;
            }

            HandleAttackAudio();
            HandleLocomotionAudio();
        }

        private void HandleLocomotionAudio()
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsTag("Attack"))
            {
                SetLoopState("None", null);
                return;
            }

            float speed = animator.GetFloat(speedParam);

            if (Mathf.Abs(speed) > runThreshold)
            {
                SetLoopState("Run", runLoop);
            }
            else
            {
                SetLoopState("None", null);
            }
        }

        private void SetLoopState(string stateName, AudioClip clip)
        {
            if (currentLoopState == stateName) return;

            currentLoopState = stateName;

            if (loopSource == null) return;

            if (clip == null)
            {
                loopSource.Stop();
                loopSource.clip = null;
                return;
            }

            loopSource.clip = clip;
            if (!loopSource.isPlaying)
                loopSource.Play();
        }

        private void HandleAttackAudio()
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            bool isAttacking = state.IsTag("Attack");

            if (isAttacking && !wasAttackingLastFrame)
            {
                if (attackClip != null && oneShotSource != null)
                {
                    oneShotSource.PlayOneShot(attackClip);
                }
            }

            wasAttackingLastFrame = isAttacking;
        }

        private void HandleDeathAudio()
        {
            if (deathPlayed) return;

            if (loopSource != null && loopSource.isPlaying)
                loopSource.Stop();

            if (deathClip != null && oneShotSource != null)
                oneShotSource.PlayOneShot(deathClip);

            deathPlayed = true;
        }

        private void OnExperienceGained()
        {
            if (gainExperienceClip == null || oneShotSource == null) return;

            oneShotSource.pitch = Random.Range(0.98f, 1.02f);
            oneShotSource.PlayOneShot(gainExperienceClip);
        }
    }
}
