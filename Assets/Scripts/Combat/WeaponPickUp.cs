using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    /// <summary>
    /// Handles weapon pickup and respawn behavior.
    /// </summary>
    public class WeaponPickUp : MonoBehaviour
    {
        [SerializeField] private Weapon weapon = null;
        [SerializeField] private float respawnTime = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickUp(false);
            yield return new WaitForSeconds(seconds);
            ShowPickUp(true);
        }

        private void ShowPickUp(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}
