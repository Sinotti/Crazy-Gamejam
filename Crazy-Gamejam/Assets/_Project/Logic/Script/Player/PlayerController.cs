using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private List<BodyPartSO> _bodyUnitsSO = new List<BodyPartSO>();

        [Header("Initial Body Prefabs")]
        [SerializeField] private List<BodyPartSO> initialBodyPrefabs = new List<BodyPartSO>();


        [Header("References")]
        [Space(6)]

        [SerializeField] private List<BodyPartSO> _bodyPartPrefabs = new List<BodyPartSO>();


        private float _delayPerUnit;
        private float _horizontalInput;
        private bool _jumpInput;

        private Transform _currentBodyUnit;
        private Transform _previousBodyUnit;
        private Transform _lastBodyUnit;
        private Transform _newUnit;

        private Vector3 _newPosition;

        public List<BodyPartSO> BodyUnitsSO => _bodyUnitsSO;

        private void Start()
        {
            InitializeBodyUnits();
        }

        private void Update()
        {
            if (_bodyUnits.Count <= 0) SceneController.Instance.RestartGame();

            ReadInputs();
            HandleRotation();
            Movement();

            if (_jumpInput) AddBodyUnit(); // Replace to New Input System.
        }

        private void ReadInputs()
        {
            if (!UIManager.Instance.IsPaused)
            {
                _horizontalInput = Input.GetAxis("Horizontal");
                //_jumpInput = Input.GetButtonDown("Jump");
            }
        }
        public void Destroyed(Transform obj)
        {
            RemoveBodyUnit(obj);
        }

        private void Movement()
        {
            if (_bodyUnits.Count == 0) return;

            _bodyUnits[0].Translate(_bodyUnits[0].forward * _movementSpeed * Time.smoothDeltaTime, Space.World);

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

        private void HandleRotation()
        {
            if (_horizontalInput != 0 && _bodyUnits[0] != null)
                _bodyUnits[0].Rotate(Vector3.up * _rotationSpeed * Time.deltaTime * _horizontalInput);
        }

        public void AddBodyUnit()
        {
            _lastBodyUnit = _bodyUnits[_bodyUnits.Count - 1];
            _newPosition = _lastBodyUnit.position - _lastBodyUnit.forward * _addUnitOffset;

            BodyPartSO randomBodyPartPrefab = _bodyPartPrefabs[UnityEngine.Random.Range(0, _bodyPartPrefabs.Count)];
            _bodyUnitsSO.Add(randomBodyPartPrefab);

            _newUnit = Instantiate(randomBodyPartPrefab.UnitInGamePrefab, _newPosition, _lastBodyUnit.rotation).transform;
            _newUnit.SetParent(transform);
            _bodyUnits.Add(_newUnit);
            
        }

        public void RemoveBodyUnit(Transform unitToRemove)
        {
            if (_bodyUnits.Contains(unitToRemove))
            {
                int index = _bodyUnits.IndexOf(unitToRemove);
                
                _bodyUnits.RemoveAt(index);
                _bodyUnitsSO.RemoveAt(index);

                for (int i = index; i < _bodyUnits.Count - 1; i++)
                {
                    _bodyUnits[i].position = _bodyUnits[i + 1].position;
                    _bodyUnits[i].rotation = _bodyUnits[i + 1].rotation;
                }

                Destroy(unitToRemove.gameObject);
            }
        }
        public void UpdateBodyUnits(List<BodyPartSO> updatedBodyUnitsSO)
        {
            foreach (Transform unit in _bodyUnits)
            {
                if (unit != null)
                {
                    Destroy(unit.gameObject);
                }
            }
            _bodyUnits.Clear();
            _bodyUnitsSO.Clear();

            foreach (BodyPartSO bodyPartSO in updatedBodyUnitsSO)
            {
                _bodyUnitsSO.Add(bodyPartSO);

                Transform newUnit = Instantiate(bodyPartSO.UnitInGamePrefab, transform.position, transform.rotation).transform;
                newUnit.SetParent(transform);
                _bodyUnits.Add(newUnit);
            }
        }

        private void InitializeBodyUnits()
        {
            foreach (BodyPartSO prefab in initialBodyPrefabs)
            {
                Transform newUnit = Instantiate(prefab.UnitInGamePrefab, transform.position, transform.rotation).transform;
                _bodyUnitsSO.Add(prefab);
                newUnit.SetParent(transform);
                _bodyUnits.Add(newUnit);
            }

            int remainingUnits = _beginSize - initialBodyPrefabs.Count;
            for (int i = 0; i < remainingUnits; i++)
            {
                AddBodyUnit();
            }
        }

        public void AddBodyUnit(BodyPartSO bodyPartSO)
        {
            _lastBodyUnit = _bodyUnits[_bodyUnits.Count - 1];
            _newPosition = _lastBodyUnit.position - _lastBodyUnit.forward * _addUnitOffset;

            _bodyUnitsSO.Add(bodyPartSO);

            _newUnit = Instantiate(bodyPartSO.UnitInGamePrefab, _newPosition, _lastBodyUnit.rotation).transform;
            _newUnit.SetParent(transform);
            _bodyUnits.Add(_newUnit);
        }

        public void RemoveBodyUnit(BodyPartSO bodyPartSO)
        {
            // Verifica se o BodyPartSO está na lista e encontra o índice correspondente
            int index = _bodyUnitsSO.IndexOf(bodyPartSO);
            if (index != -1)
            {
                // Remove o BodyPartSO da lista
                _bodyUnitsSO.RemoveAt(index);

                // Remove o Transform correspondente do corpo
                Transform unitToRemove = _bodyUnits[index];
                _bodyUnits.RemoveAt(index);

                // Destrói o objeto do corpo removido
                Destroy(unitToRemove.gameObject);

                // Reposiciona as unidades restantes
                for (int i = index; i < _bodyUnits.Count - 1; i++)
                {
                    _bodyUnits[i].position = _bodyUnits[i + 1].position;
                    _bodyUnits[i].rotation = _bodyUnits[i + 1].rotation;
                }
            }
        }

    }
}
