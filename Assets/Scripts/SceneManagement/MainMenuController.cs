using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using RPG.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "MysticMeadows";
    [SerializeField] private Button playButton;        // New Game / Continue
    [SerializeField] private TMP_Text playButtonText;  // text component
    [SerializeField] private Button deleteButton;      // main-menu delete

    private string SavePath =>
        Path.Combine(Application.persistentDataPath, "Saves", "MysticMeadows_Save.sav");

    private bool HasSave => File.Exists(SavePath);

    private void Start()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        bool hasSave = HasSave;

        if (playButtonText != null)
            playButtonText.text = hasSave ? "Continue" : "New Game";

        if (deleteButton != null)
            deleteButton.gameObject.SetActive(hasSave);
    }

    // Hook this to the Play button OnClick
    public void OnPlayClicked()
    {
        if (!HasSave)
        {
            // No save: just start the scene fresh
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            // Save exists: load scene, AutoLoadOnStart will call SavingWrapper.Load()
            SceneManager.LoadScene(gameSceneName);
        }
    }

    // Hook this to the main-menu Delete button OnClick
    public void OnDeleteSaveClicked()
    {
        // Try via SavingWrapper if present (e.g. Saver in a persistent bootstrap scene),
        // otherwise just delete the file directly.
        var wrapper = FindObjectOfType<SavingWrapper>();
        if (wrapper != null)
        {
            wrapper.Delete();
        }
        else if (HasSave)
        {
            File.Delete(SavePath);
        }

        RefreshUI();
    }
    public void OnQuitClicked()
    {
    #if UNITY_EDITOR
        // Stops Play mode in the editor
        EditorApplication.isPlaying = false;
    #else
        // Quits the built game
        Application.Quit();
    #endif
    }
}
