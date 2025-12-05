using UnityEngine;

namespace RPG.Resources
{
    /// <summary>
    /// Makes a UI element face the camera (billboard effect).
    /// </summary>
    public class BillBoard3D : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (Camera.main == null) return;
            transform.LookAt(Camera.main.transform);
        }
    }
}
