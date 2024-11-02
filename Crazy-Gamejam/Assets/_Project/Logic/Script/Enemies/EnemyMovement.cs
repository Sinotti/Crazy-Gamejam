using UnityEngine;
using UnityEngine.AI;

namespace Main.Gameplay.Enemies
{
    public class EnemyMovement : MonoBehaviour, IDestroyable
    {
        [Header("Movement Parameters")]
        [Space(6)]
        [SerializeField] private float _stopDistance = 2f;
        [SerializeField] private float _moveSpeed = 3f;

        [Header("References")]
        [Space(6)]
        [SerializeField] private EnemyCombat _enemyCombat; 
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private float _distanceToTarget;

        private void Start()
        {
            _navMeshAgent.speed = _moveSpeed;
        }

        private void Update() // Move to a Coroutine
        {
            MoveTowardsCloserUnit();
        }

        public void Destroyed(Transform obs)
        {
            Destroy(gameObject);
        }

        private void MoveTowardsCloserUnit()
        {
            if (_enemyCombat != null && _enemyCombat.CloserUnit != null)
            {
                _distanceToTarget = Vector3.Distance(transform.position, _enemyCombat.CloserUnit.position);

                if (_distanceToTarget > _stopDistance)
                {
                    _navMeshAgent.SetDestination(_enemyCombat.CloserUnit.position);
                }
                else
                {
                    _navMeshAgent.ResetPath();
                }
            }
            else
            {
                _navMeshAgent.ResetPath();
            }
        }
    }
}
