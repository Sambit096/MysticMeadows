using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    /// <summary>
    /// Wrapper for the saving system that handles save, load, and delete operations.
    /// </summary>
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private float fadeInTime = 0.2f;

        private const string DefaultSaveFile = "save";

        private void Awake()
        {
           /* StartCoroutine(LoadLastScene());*/
        }

        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(DefaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Delete();
            }
        }

        /// <summary>
        /// Saves the current game state to the default save file.
        /// </summary>
        public void Save()
        {
            GetComponent<SavingSystem>().Save(DefaultSaveFile);
        }

        /// <summary>
        /// Loads the game state from the default save file.
        /// </summary>
        public void Load()
        {
            GetComponent<SavingSystem>().Load(DefaultSaveFile);
        }

        /// <summary>
        /// Deletes the default save file.
        /// </summary>
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(DefaultSaveFile);
        }
    }
}
