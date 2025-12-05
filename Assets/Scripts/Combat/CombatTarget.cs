using UnityEngine;
using RPG.Resources;

namespace RPG.Combat
{
    /// <summary>
    /// Marks an entity as a valid combat target.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
    }
}
