using UnityEngine;
using UnityEngine.Playables;

namespace RPG.SceneManagement
{
    /// <summary>
    /// Triggers a cinematic sequence when the player enters the trigger zone.
    /// </summary>
    public class CinematicTrigger : MonoBehaviour
    {
        private bool isTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!isTriggered && other.gameObject.CompareTag("Player"))
            {
                GetComponent<PlayableDirector>().Play();
                isTriggered = true;
            }
        }
    }
}
