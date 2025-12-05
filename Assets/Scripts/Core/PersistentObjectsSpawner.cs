using UnityEngine;

namespace RPG.Core
{
    /// <summary>
    /// Spawns persistent objects that persist across scene loads.
    /// </summary>
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistentObjectPrefab;

        private static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned)
            {
                return;
            }

            SpawnPersistentObjects();
            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}
