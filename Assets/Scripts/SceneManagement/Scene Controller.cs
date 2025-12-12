using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace RPG.SceneManagement
{
public class SceneController : MonoBehaviour
{
   /// <summary>
    /// Load a scene by its name.
    /// Make sure the scene is added to Build Settings.
    /// </summary>
    /// <param name="sceneName">The exact name of the target scene.</param>
    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("LoadSceneByName called with an empty scene name.");
            return;
        }

        // Optionally, you can check if the scene exists in build settings
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' cannot be loaded. " +
                           "Is it added to Build Settings?");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Quit the application.
    /// Works in build. In the editor it will just log a message.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("QuitGame called. Exiting application...");

#if UNITY_EDITOR
        // This will stop play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
}