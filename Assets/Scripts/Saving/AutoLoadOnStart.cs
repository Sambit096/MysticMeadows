using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;
namespace RPG.Saving
{
public class AutoLoadOnStart : MonoBehaviour
{
    private void Start()
    {
        var wrapper = FindObjectOfType<SavingWrapper>();
        if (wrapper != null)
        {
            wrapper.Load();   // restores player, enemies, health, XP, positions
        }
        else
        {
            Debug.LogError("SavingWrapper not found for auto-load.");
        }
    }
}
}