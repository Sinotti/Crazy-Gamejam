using UnityEngine;

namespace Main.Gameplay.Enemies
{
    public class EnemyCombat : MonoBehaviour
    {
        [Header("Detection Parameters")]
        [Space(6)]
        [SerializeField] private float _radius;
        [SerializeField] private float _height;
        [SerializeField] private LayerMask _detectionLayer;

        [Header("Shoot Parameters")]
        [Space(6)]
        [SerializeField] private float _aimSpeed = 7;

        [Header("References")]
        [Space(6)]
        [SerializeField] private Transform _aimPoint;

        [SerializeField] private Transform _closerUnit;
        [SerializeField] private Collider[] _unitsAround;

        private float _sqrTarget;
        private float _currentCloserSqr;

        private Quaternion _lookRotation; 
        private Vector3 _target;
        private Vector3 direction;

        private void Start()
        {

        }

        private void Update()
        {
            AroundDetection();
            if(_closerUnit != null) Debug.DrawLine(_aimPoint.position, _closerUnit.transform.position, Color.yellow);
        }

        public void AroundDetection()
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

            RotateToCloser();
        }

        public void RotateToCloser()
        {
            if (_closerUnit == null) return;

            direction = _closerUnit.position - transform.position;

            _lookRotation = Quaternion.LookRotation(direction);
            _aimPoint.rotation = Quaternion.Slerp(_aimPoint.rotation, _lookRotation, Time.deltaTime * _aimSpeed);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.up * _height, _radius);
        }
    }
}
