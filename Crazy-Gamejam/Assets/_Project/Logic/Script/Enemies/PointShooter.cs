using UnityEngine;
using Main.Utilities;

namespace Main.Gameplay.Enemies
{
    public class PointShooter : MonoBehaviour
    {
        [Header("Detection Parameters")]
        [Space(6)]
        [SerializeField] private float _radius;
        [SerializeField] private float _height;
        [SerializeField] private LayerMask _detectionLayer;

        [Header("Shoot Parameters")]
        [Space(6)]
        [SerializeField] private float _aimSpeed = 7;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private int _damagePerTick = 2;
        [SerializeField] private Cooldown _damageTickCooldown;

        [Header("References")]
        [Space(6)]
        [SerializeField] private Transform _aimPointVisual;
        [SerializeField] private GameObject _bulletPrefab;

        [SerializeField] private Transform _closerUnit;
        [SerializeField] private Collider[] _unitsAround;

        private float _sqrTarget;
        private float _currentCloserSqr;

        private Quaternion _lookRotation;
        private Vector3 _target;
        private Vector3 _direction;

        public Transform CloserUnit { get => _closerUnit; set => _closerUnit = value; }

        private void Update()
        {
            AroundDetection();

            if (CloserUnit != null)
            {
                Debug.DrawLine(_aimPointVisual.position, CloserUnit.position, Color.yellow);
                ShootInEnemy();
            }
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
                    CloserUnit = currentUnit.transform;
                }
            }

            RotateToCloser();
        }

        public void RotateToCloser()
        {
            if (CloserUnit == null) return;

            _direction = CloserUnit.position - transform.position;

            _lookRotation = Quaternion.LookRotation(_direction);
            _aimPointVisual.rotation = Quaternion.Slerp(_aimPointVisual.rotation, _lookRotation, Time.deltaTime * _aimSpeed);
        }

        private void ShootInEnemy()
        {
            if (CloserUnit != null && !_damageTickCooldown.IsCoolingDown)
            {
                if (CloserUnit.TryGetComponent(out Health health))
                {
                    Bullet();
                    //health.TakeDamage(_damagePerTick); Moved to Projectile
                    _damageTickCooldown.StartCoolDown();
                }
            }
        }

        private void Bullet()
        {
            GameObject projectileInstance = Instantiate(_bulletPrefab, _aimPointVisual.transform.position, Quaternion.identity);
            projectileInstance.TryGetComponent(out Projectile projectile);
            projectile.Initialize(_closerUnit.transform, _damagePerTick, _bulletSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.up * _height, _radius);
        }
    }
}
