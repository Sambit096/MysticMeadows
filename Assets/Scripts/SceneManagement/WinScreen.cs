using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace RPG.SceneManagement
{
public class WinScreen : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI titleText;

    private void Awake()
    {
        if (titleText != null)
            titleText.text = "Victory!";

        if (replayButton != null)
            replayButton.onClick.AddListener(OnReplay);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenu);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuit);

        // Unlock cursor for menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause gameplay
        Time.timeScale = 0f;
    }

    public void OnReplay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void OnQuit()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
}