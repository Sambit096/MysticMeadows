using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.Resources
{
    /// <summary>
    /// Displays the player's current experience points.
    /// </summary>
    public class PlayerExpDisplay : MonoBehaviour
    {
        private Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text = experience.GetExperience().ToString();
        }
    }
}
