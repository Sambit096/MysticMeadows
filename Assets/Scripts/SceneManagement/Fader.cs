using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        private Coroutine fadeCoroutine;
        [SerializeField] private Image image; // optional assign in inspector

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (image == null) image = GetComponentInChildren<Image>();

            // Defensive defaults: start transparent, image black
            if (image != null) image.color = Color.black;
            if (canvasGroup != null) canvasGroup.alpha = 0f;
        }

        /// <summary>
        /// Immediately set alpha to opaque (1).
        /// </summary>
        public void FadeOutImmediate()
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            if (canvasGroup != null) canvasGroup.alpha = 1f;
        }

        /// <summary>
        /// Fade canvasGroup alpha from current value to 1 over time seconds.
        /// </summary>
        public IEnumerator FadeOut(float time)
        {
            if (canvasGroup == null)
                yield break;

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(Fade(canvasGroup.alpha, 1f, time));
            yield return fadeCoroutine;
        }

        /// <summary>
        /// Fade canvasGroup alpha from current value to 0 over time seconds.
        /// </summary>
        public IEnumerator FadeIn(float time)
        {
            if (canvasGroup == null)
                yield break;

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(Fade(canvasGroup.alpha, 0f, time));
            yield return fadeCoroutine;
        }

        private IEnumerator Fade(float from, float to, float time)
        {
            if (canvasGroup == null)
                yield break;

            // if time is 0 or negative, snap instantly
            if (time <= 0f)
            {
                canvasGroup.alpha = to;
                fadeCoroutine = null;
                yield break;
            }

            float elapsed = 0f;
            // ensure starting value is exact 'from' to avoid small leftover differences
            canvasGroup.alpha = from;

            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / time);
                canvasGroup.alpha = Mathf.Lerp(from, to, t);
                yield return null;
            }

            // ensure we end exactly on 'to'
            canvasGroup.alpha = to;
            fadeCoroutine = null;
        }

        /// <summary>
        /// Force the image (if any) to black. Useful if Timeline or Inspector accidentally set it white.
        /// </summary>
        public void ForceImageBlack()
        {
            if (image != null) image.color = Color.black;
        }

        /// <summary>
        /// Set the canvas alpha immediately (utility).
        /// </summary>
        public void SetAlphaImmediate(float alpha)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            if (canvasGroup != null) canvasGroup.alpha = Mathf.Clamp01(alpha);
        }
    }
}
