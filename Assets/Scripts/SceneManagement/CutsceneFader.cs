using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.SceneManagement
{
    public class CutsceneFader : MonoBehaviour
    {
                                                                                                                                                        
        [SerializeField] private PlayableDirector director;
        [SerializeField] private Fader fader;
        [SerializeField] private float fadeInTime = 1.5f;
        [SerializeField] private float fadeOutTime = 1.5f;
        

        private void Awake()
        {   
            // Auto-hook references if not set
            if (director == null)
            {
                director = GetComponent<PlayableDirector>();
            }
            if (fader == null)
            {
                fader = FindObjectOfType<Fader>();
            }

            if (director != null)
            {
                director.played += OnCutsceneStarted;
                director.stopped += OnCutsceneEnded;
            }
        }

        private void OnDestroy()
        {
            if (director != null)
            {
                director.played -= OnCutsceneStarted;
                director.stopped -= OnCutsceneEnded;
            }
        }

        private void OnCutsceneStarted(PlayableDirector d)
        {
            if (fader == null) return;

            // Defensive: make sure overlay is black and visible, then fade in (black -> clear).
            fader.ForceImageBlack();
            fader.FadeOutImmediate();                // alpha = 1
            // Start fade in to alpha 0. If a previous fade coroutine exists, Fader will stop it.
            StartCoroutine(fader.FadeIn(fadeInTime));
        }

        private void OnCutsceneEnded(PlayableDirector d)
        {
            if (fader == null) fader = FindObjectOfType<Fader>();
            if (fader == null) return;

            // Ensure the image is black (defensive), then fade out to black
            fader.ForceImageBlack();
            // Fade to black (alpha -> 1)
            StartCoroutine(fader.FadeOut(fadeOutTime));

            // After a small pause or immediately, fade back in to reveal gameplay
            // Option A: fade back in after fadeOut finishes (keeps black briefly)
            StartCoroutine(FadeBackInAfterDelay(fadeOutTime));
        }

        private IEnumerator FadeBackInAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);           // wait for fade out to finish
            yield return fader.FadeIn(fadeInTime);           // fade back to transparent
        }
    }
}
