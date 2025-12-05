using UnityEngine;

namespace RPG.Control
{
    /// <summary>
    /// Defines a patrol path using child transforms as waypoints.
    /// </summary>
    public class PatrolPath : MonoBehaviour
    {
        private const float WaypointGizmoRadius = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextWayIndex(i);
                Gizmos.DrawSphere(GetWayPoint(i), WaypointGizmoRadius);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }
        }

        /// <summary>
        /// Returns the next waypoint index, wrapping to 0 if at the end.
        /// </summary>
        public int GetNextWayIndex(int i)
        {
            if (i + 1 >= transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        /// <summary>
        /// Returns the world position of the waypoint at the given index.
        /// </summary>
        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}