using UnityEngine;
using RPG.Core;
using RPG.Resources;

namespace RPG.Combat
{
    /// <summary>
    /// Handles projectile movement, homing behavior, and impact damage.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float arrowSpeed = 1f;
        [SerializeField] private bool isHoming = true;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit = null;
        [SerializeField] private float lifeAfterImpact = 2f;

        private Health target = null;
        private GameObject instigator = null;
        private float damage = 0;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (target == null) return;

            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * arrowSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;

            target.TakeDamage(instigator, damage);
            arrowSpeed = 0;

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

        /// <summary>
        /// Sets the target, instigator, and damage for this projectile.
        /// </summary>
        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
    }
}
