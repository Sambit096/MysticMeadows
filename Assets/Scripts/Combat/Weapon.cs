using UnityEngine;
using RPG.Resources;

namespace RPG.Combat
{
    /// <summary>
    /// ScriptableObject defining weapon properties including damage, range, and animations.
    /// </summary>
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject equippedWeaponPrefab = null;
        [SerializeField] private float weaponDamageInflicted = 20f;
        [SerializeField] private float percentageBonus = 0f;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile = null;

        private const string WeaponName = "Weapon";

        /// <summary>
        /// Spawns the weapon prefab and applies animator override.
        /// </summary>
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (equippedWeaponPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(equippedWeaponPrefab, handTransform);
                weapon.name = WeaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                // Reset to root controller for non-overwritten weapons
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(WeaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(WeaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        /// <summary>
        /// Returns whether this weapon has a projectile attack.
        /// </summary>
        public bool HasProjectile()
        {
            return projectile != null;
        }

        /// <summary>
        /// Launches a projectile from the appropriate hand toward the target.
        /// </summary>
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        /// <summary>
        /// Returns the base damage of this weapon.
        /// </summary>
        public float GetDamage()
        {
            return weaponDamageInflicted;
        }

        /// <summary>
        /// Returns the percentage bonus of this weapon.
        /// </summary>
        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        /// <summary>
        /// Returns the attack range of this weapon.
        /// </summary>
        public float GetWeaponRange()
        {
            return weaponRange;
        }
    }
}