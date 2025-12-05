using UnityEngine;

namespace RPG.Core
{
    /// <summary>
    /// Keeps the camera position synchronized with a target transform.
    /// </summary>
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}
