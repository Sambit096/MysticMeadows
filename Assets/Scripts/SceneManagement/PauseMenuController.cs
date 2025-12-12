using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace RPG.SceneManagement
{
    /// <summary>
    /// Controls the pause menu UI and pause/unpause game state.
    /// </summary>
    public class PauseMenuController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;
        [Header("Saving")]
        [SerializeField] private SavingWrapper savingWrapper;
        [Tooltip("Name of the main menu scene to load when Main Menu is pressed.")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        private float previousTimeScale = 1f;
        private float previousFixedDelta = 0.02f;
        private bool previousAudioPause = false;
        private PauseManager ownerManager;

        /// <summary>
        /// Initializes the controller with a reference to the owning PauseManager.
        /// </summary>
        public void Init(PauseManager manager)
        {
            ownerManager = manager;
        }

        private void Awake()
        {
            if (resumeButton != null) resumeButton.onClick.AddListener(OnResumePressed);
            if (mainMenuButton != null) mainMenuButton.onClick.AddListener(OnMainMenuPressed);
            if (quitButton != null) quitButton.onClick.AddListener(OnQuitPressed);
        }

        private void OnEnable()
        {
            PauseGame();
            StartCoroutine(SetInitialSelected());
        }

        private IEnumerator SetInitialSelected()
        {
            yield return null;
            if (EventSystem.current != null && resumeButton != null)
            {
                EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
            }
        }

        public void OnSavePressed()   
        {
            if (savingWrapper == null)
            {
                savingWrapper = FindObjectOfType<SavingWrapper>();
            }

            if (savingWrapper != null)
            {
                savingWrapper.Save();
            }
            else
            {
                Debug.LogError("SavingWrapper not found in scene when trying to save.");
            }
        }
        private void PauseGame()
        {
            previousTimeScale = Time.timeScale;
            previousFixedDelta = Time.fixedDeltaTime;
            previousAudioPause = AudioListener.pause;

            Time.timeScale = 0f;
            Time.fixedDeltaTime = Mathf.Max(0.0001f, previousFixedDelta * Time.timeScale);
            AudioListener.pause = true;
        }

        private void UnpauseGame()
        {
            Time.timeScale = previousTimeScale;
            Time.fixedDeltaTime = previousFixedDelta;
            AudioListener.pause = previousAudioPause;
        }

        /// <summary>
        /// Called when the Resume button is pressed.
        /// </summary>
        public void OnResumePressed()
        {
            UnpauseGame();
            if (ownerManager != null) ownerManager.NotifyPauseMenuClosed();
            Destroy(gameObject);
        }

        /// <summary>
        /// Called when the Main Menu button is pressed.
        /// </summary>
        public void OnMainMenuPressed()
        {
            UnpauseGame();
            if (ownerManager != null) ownerManager.NotifyPauseMenuClosed();
            SceneManager.LoadScene(mainMenuSceneName);
        }

        /// <summary>
        /// Called when the Quit button is pressed.
        /// </summary>
        public void OnQuitPressed()
        {
            UnpauseGame();
            if (ownerManager != null) ownerManager.NotifyPauseMenuClosed();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
