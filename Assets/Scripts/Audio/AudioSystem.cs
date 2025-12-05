using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Audio
{
    /// <summary>
    /// Manages scene-based background music playback.
    /// </summary>
    public class AudioSystem : MonoBehaviour
    {
        private static AudioSystem _instance;
        public static AudioSystem Instance => _instance;

        [Serializable]
        public class SceneMusic
        {
            public string sceneName;
            public AudioClip[] tracks;
            public bool loop = true;
        }

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private List<SceneMusic> sceneMusicList = new List<SceneMusic>();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlaySceneMusic(scene.name);
        }

        /// <summary>
        /// Plays the music configured for the specified scene.
        /// </summary>
        public void PlaySceneMusic(string sceneName)
        {
            var data = sceneMusicList.Find(s => s.sceneName == sceneName);
            if (data == null || data.tracks == null || data.tracks.Length == 0)
                return;

            musicSource.clip = data.tracks[0];
            musicSource.loop = data.loop;
            musicSource.Play();
        }
    }
}
