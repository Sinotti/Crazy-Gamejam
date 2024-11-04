using System.Collections.Generic;
using UnityEngine;

namespace Main.Gameplay.Player
{
    public class PlayerController : MonoBehaviour, IDestroyable
    {
        [Header("Movement Parameters")]
        [Space(6)]
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _rotationSpeed;

        [Header("Body Parameters")]
        [Space(6)]
        [SerializeField] private float _UnitsDistance = 0.25f;
        [SerializeField] private float _addUnitOffset = 0.5f;
        [SerializeField] private int _beginSize;

        [Space(6)]
        [SerializeField] private List<Transform> _bodyUnits = new List<Transform>();

        // Nova lista para especificar prefabs personalizados para o início
        [Header("Initial Body Prefabs")]
        [SerializeField] private List<GameObject> initialBodyPrefabs = new List<GameObject>();

        [Header("References")]
        [Space(6)]
        [SerializeField] private GameObject _bodyUnitPrefab;

        private float _delayPerUnit;

        private Transform _currentBodyUnit;
        private Transform _previousBodyUnit;
        private Transform _lastBodyUnit;
        private Transform _newUnit;

        private Vector3 _newPosition;

        private void Start()
        {
            InitializeBodyUnits();
        }

        private void Update() // Move to a Coroutine.
        {
            HandleRotation();
            Movement();

            if (Input.GetButtonDown("Jump")) AddBodyUnit(); // Replace to New Input System.
        }

        public void Destroyed(Transform obj)
        {
            RemoveBodyUnit(obj);
        }

        private void Movement()
        {
            if (_bodyUnits.Count == 0) return;

            _bodyUnits[0].Translate(_bodyUnits[0].forward * Input.GetAxis("Vertical") * _movementSpeed * Time.smoothDeltaTime, Space.World);

            if (Input.GetAxis("Vertical") != 0)
            {
                for (int i = 1; i < _bodyUnits.Count; i++)
                {
                    _currentBodyUnit = _bodyUnits[i];
                    _previousBodyUnit = _bodyUnits[i - 1];

                    if (_currentBodyUnit == null || _previousBodyUnit == null)
                    {
                        continue;
                    }

                    float distance = Vector3.Distance(_previousBodyUnit.position, _currentBodyUnit.position);
                    Vector3 newpos = _previousBodyUnit.position;

                    newpos.y = _bodyUnits[0].position.y;

                    _delayPerUnit = Time.deltaTime * distance / _UnitsDistance * _movementSpeed;

                    if (_delayPerUnit > 0.5f) _delayPerUnit = 0.5f;

                    _currentBodyUnit.position = Vector3.Slerp(_currentBodyUnit.position, newpos, _delayPerUnit);
                    _currentBodyUnit.rotation = Quaternion.Slerp(_currentBodyUnit.rotation, _previousBodyUnit.rotation, _delayPerUnit);
                }
            }
        }

        private void HandleRotation()
        {
            if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
                _bodyUnits[0].Rotate(Vector3.up * _rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        }

        public void AddBodyUnit()
        {
            _lastBodyUnit = _bodyUnits[_bodyUnits.Count - 1];
            _newPosition = _lastBodyUnit.position - _lastBodyUnit.forward * _addUnitOffset;

            _newUnit = Instantiate(_bodyUnitPrefab, _newPosition, _lastBodyUnit.rotation).transform;
            _newUnit.SetParent(transform);
            _bodyUnits.Add(_newUnit);
        }

        public void RemoveBodyUnit(Transform unitToRemove)
        {
            if (_bodyUnits.Contains(unitToRemove))
            {
                int index = _bodyUnits.IndexOf(unitToRemove);

                _bodyUnits.RemoveAt(index);

                for (int i = index; i < _bodyUnits.Count - 1; i++)
                {
                    _bodyUnits[i].position = _bodyUnits[i + 1].position;
                    _bodyUnits[i].rotation = _bodyUnits[i + 1].rotation;
                }

                Destroy(unitToRemove.gameObject);
            }
        }

        private void InitializeBodyUnits()
        {
            foreach (var prefab in initialBodyPrefabs)
            {
                Transform newUnit = Instantiate(prefab, transform.position, transform.rotation).transform;
                newUnit.SetParent(transform);
                _bodyUnits.Add(newUnit);
            }

            int remainingUnits = _beginSize - initialBodyPrefabs.Count;
            for (int i = 0; i < remainingUnits; i++)
            {
                AddBodyUnit();
            }
        }
    }
}
