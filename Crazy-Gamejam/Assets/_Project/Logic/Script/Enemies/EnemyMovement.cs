using UnityEngine;
using UnityEngine.AI;

namespace Main.Gameplay.Enemies
{
    public class EnemyMovement : MonoBehaviour, IDestroyable
    {
        [Header("Movement Parameters")]
        [Space(6)]
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _stopDistance = 2f;

        [Header("Detection Parameters")]
        [Space(6)]
        [SerializeField] private float _radius;
        [SerializeField] private float _height;
        [SerializeField] private LayerMask _detectionLayer;

        [Header("Unit Parameters")]
        [SerializeField] private int _unitMoneyValue = 10;

        [Header("References")]
        [Space(6)]
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform _closerUnit;
        [SerializeField] private Collider[] _unitsAround;

        private float _distanceToTarget;
        private float _sqrTarget;
        private float _currentCloserSqr;

        private Vector3 _target;

        private void Start()
        {
            _navMeshAgent.speed = _moveSpeed;
            _navMeshAgent.acceleration = _moveSpeed;
        }

        private void Update() 
        {
            AroundDetection();
            MoveTowardsCloserUnit();
        }

        public void Destroyed(Transform obs)
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if(BodyUnitSelectionUI.Instance != null) BodyUnitSelectionUI.Instance.AddMoney(_unitMoneyValue);
        }

        private void MoveTowardsCloserUnit()
        {
            if (_closerUnit != null)
            {
                _distanceToTarget = Vector3.Distance(transform.position, _closerUnit.position);

                if (_distanceToTarget > _stopDistance)
                {
                    _navMeshAgent?.SetDestination(_closerUnit.position);
                }
                else
                {
                    _navMeshAgent?.ResetPath();
                }
            }
            else
            {
                _navMeshAgent?.ResetPath();
            }
        }

        private void AroundDetection()
        {
            _unitsAround = Physics.OverlapSphere(transform.position + transform.up * _height, _radius, _detectionLayer);
            _currentCloserSqr = Mathf.Infinity;

            foreach (var currentUnit in _unitsAround)
            {
                _target = currentUnit.transform.position - transform.position;
                _sqrTarget = _target.sqrMagnitude;

                if (_sqrTarget < _currentCloserSqr)
                {
                    _currentCloserSqr = _sqrTarget;
                    _closerUnit = currentUnit.transform;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.up * _height, _radius);
        }

        public void Stop()
        {
            //_navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
        }
    }
}
