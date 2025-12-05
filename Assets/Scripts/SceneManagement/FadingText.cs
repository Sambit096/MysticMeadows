using UnityEngine;
using TMPro;

namespace RPG.SceneManagement
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadingText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float duration = 1.2f;      // total lifetime
        [SerializeField] private float startScale = 1.6f;    // big at spawn
        [SerializeField] private float endScale = 0.9f;      // small at end
        [SerializeField] private Vector3 localOffset = default; // optional local positional offset
        private CanvasGroup canvasGroup;
        private float timer;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (text == null) text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            timer = 0f;
            transform.localScale = Vector3.one * startScale;
            if (canvasGroup != null) canvasGroup.alpha = 1f;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            // scale big -> small
            float scale = Mathf.Lerp(startScale, endScale, t);
            transform.localScale = Vector3.one * scale;

            // fade out alpha 1 -> 0
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            }

            if (t >= 1f)
            {
                Destroy(gameObject);
            }
        }

        public void SetText(string message)
        {
            if (text != null) text.text = message;
        }
    }
}
