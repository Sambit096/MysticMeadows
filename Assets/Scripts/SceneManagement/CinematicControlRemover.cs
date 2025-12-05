using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.SceneManagement
{
    /// <summary>
    /// Disables player control during cinematic playback and re-enables it afterward.
    /// </summary>
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject player;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        private void DisableControl(PlayableDirector pd)
        {
            Debug.Log("Cinematic started - disabling player control");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
            Debug.Log("Cinematic ended - enabling player control");
        }
    }
}
