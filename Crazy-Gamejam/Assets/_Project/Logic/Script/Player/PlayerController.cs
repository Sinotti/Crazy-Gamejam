using System.Collections.Generic;
using UnityEngine;

namespace Main.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
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
            for (int i = 0; i < _beginSize - 1; i++) AddBodyUnit();
        }

        private void Update() // Move to a Coroutine.
        {
            HandleRotation();
            Movement();

            if (Input.GetButtonDown("Jump")) AddBodyUnit(); // Replace to New Input System.
        }

        private void Movement()
        {
            // Verifica se há pelo menos uma unidade para evitar erros ao acessar _bodyUnits[0]
            if (_bodyUnits.Count == 0) return;

            // Movimento da primeira unidade
            _bodyUnits[0].Translate(_bodyUnits[0].forward * Input.GetAxis("Vertical") * _movementSpeed * Time.smoothDeltaTime, Space.World);

            if (Input.GetAxis("Vertical") != 0)
            {
                for (int i = 1; i < _bodyUnits.Count; i++)
                {
                    _currentBodyUnit = _bodyUnits[i];
                    _previousBodyUnit = _bodyUnits[i - 1];

                    // Verifica se ambas as unidades (atual e anterior) existem antes de calcular a distância
                    if (_currentBodyUnit == null || _previousBodyUnit == null)
                    {
                        continue; // Ignora esta unidade se houver uma referência nula
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

                // Remove a unidade da lista
                _bodyUnits.RemoveAt(index);

                // Se a unidade não é a última, avança as próximas para preencher a lacuna
                for (int i = index; i < _bodyUnits.Count - 1; i++)
                {
                    _bodyUnits[i].position = _bodyUnits[i + 1].position;
                    _bodyUnits[i].rotation = _bodyUnits[i + 1].rotation;
                }

                // Destrói a unidade removida
                Destroy(unitToRemove.gameObject);
            }
        }
    }
}