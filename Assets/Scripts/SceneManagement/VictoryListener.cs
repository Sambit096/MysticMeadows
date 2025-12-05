using UnityEngine;
using RPG.Audio;

namespace RPG.SceneManagement
{
public class VictoryListener : MonoBehaviour
{
    [SerializeField] private GameObject winScreenPrefab; // assign prefab
    [SerializeField] private Transform uiParent;         // assign Canvas transform

    private void OnEnable()
    {
        EnemyDeathAudio.OnEnemyDied += HandleEnemyDied;
    }

    private void OnDisable()
    {
        EnemyDeathAudio.OnEnemyDied -= HandleEnemyDied;
    }

    private void HandleEnemyDied(int remaining)
    {
        if (remaining <= 0)
        {
            Instantiate(winScreenPrefab, uiParent);
        }
    }
}
}