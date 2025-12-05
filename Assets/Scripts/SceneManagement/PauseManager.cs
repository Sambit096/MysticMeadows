using UnityEngine;

namespace RPG.SceneManagement
{
    /// <summary>
    /// Manages pause menu instantiation and state.
    /// </summary>
    public class PauseManager : MonoBehaviour
    {
        [Tooltip("Assign the PauseMenu prefab (the prefab should have a PauseMenuController attached).")]
        [SerializeField] private GameObject pauseMenuPrefab;

        private GameObject currentPauseMenu;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentPauseMenu == null)
                {
                    OpenPauseMenu();
                }
                else
                {
                    var controller = currentPauseMenu.GetComponent<PauseMenuController>();
                    if (controller != null)
                    {
                        controller.OnResumePressed();
                    }
                    else
                    {
                        ClosePauseMenuImmediate();
                    }
                }
            }
        }

        private void OpenPauseMenu()
        {
            if (pauseMenuPrefab == null)
            {
                Debug.LogError("PauseManager: pauseMenuPrefab not assigned.");
                return;
            }

            currentPauseMenu = Instantiate(pauseMenuPrefab, Vector3.zero, Quaternion.identity);

            var controller = currentPauseMenu.GetComponent<PauseMenuController>();
            if (controller != null)
            {
                controller.Init(this);
            }
        }

        /// <summary>
        /// Called by PauseMenuController when it is destroyed or resumed.
        /// </summary>
        public void NotifyPauseMenuClosed()
        {
            currentPauseMenu = null;
        }

        /// <summary>
        /// Force-closes the pause menu immediately.
        /// </summary>
        public void ClosePauseMenuImmediate()
        {
            if (currentPauseMenu != null)
            {
                Destroy(currentPauseMenu);
                currentPauseMenu = null;
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                AudioListener.pause = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
