using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.Resources
{
    /// <summary>
    /// Displays the player's current level.
    /// </summary>
    public class PlayerLevelDisplay : MonoBehaviour
    {
        private BaseStats baseStats;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", baseStats.GetLevel());
        }
    }
}
