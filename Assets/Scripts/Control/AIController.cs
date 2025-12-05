using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;

namespace RPG.Control
{
    /// <summary>
    /// Controls AI enemy behavior including patrol, chase, and attack states.
    /// </summary>
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 4f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float wayPointTolerance = 1f;
        [SerializeField] private float wayPointDwellTime = 3f;
        [Range(0, 1)]
        [SerializeField] private float patrolSpeedFraction = 0.2f;

        private Fighter fighter;
        private GameObject player;
        private Health health;
        private Mover mover;
        private Vector3 guardLocation;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWayPoint = Mathf.Infinity;
        private int currentWayPointIndex = 0;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            guardLocation = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWayPoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardLocation;

            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedAtWayPoint = 0f;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }

            if (timeSinceArrivedAtWayPoint > wayPointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWayPoint(currentWayPointIndex);
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextWayIndex(currentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWayPoint < wayPointTolerance;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            return distance < chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
