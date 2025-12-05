using UnityEngine;

namespace RPG.Core
{
    /// <summary>
    /// Destroys the game object after its particle effect finishes playing.
    /// </summary>
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject targetToDestroy = null;

        private void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (targetToDestroy != null)
                {
                    Destroy(targetToDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
