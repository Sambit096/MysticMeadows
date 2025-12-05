using UnityEngine;
using RPG.Resources;

namespace RPG.SceneManagement
{
public class PlayerDeathListener : MonoBehaviour
{
    [SerializeField] private GameObject loseScreenPrefab;
    [SerializeField] private Transform uiParent;
    private Health playerHealth;
    private bool wasDead = false;

    private void Awake()
    {
        playerHealth = GameObject.FindWithTag("Player")?.GetComponent<Health>();
    }

    private void Update()
    {
        if (playerHealth == null) return;

        bool dead = playerHealth.IsDead();
        if (!wasDead && dead)
        {
            Instantiate(loseScreenPrefab, uiParent);
        }
        wasDead = dead;
    }
}
}