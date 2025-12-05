using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace RPG.SceneManagement
{
public class LoseScreen : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI titleText;

    private void Awake()
    {
        if (titleText != null)
            titleText.text = "You Died";

        if (replayButton != null)
            replayButton.onClick.AddListener(OnReplay);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenu);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuit);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    private void OnReplay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnQuit()
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