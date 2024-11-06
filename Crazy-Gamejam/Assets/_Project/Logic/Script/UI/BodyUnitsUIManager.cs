using Main.Gameplay.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BodyUnitsUIManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _unitSlotPrefab;
    [SerializeField] private Transform _slotsContainer;

    [SerializeField] private List<BodyPartSO> _registeredBodyUnitsSO = new List<BodyPartSO>();
    private Transform _currentSlot;

    private void OnEnable()
    {
        UpdateUI();
    }
    private void OnDisable()
    {
        if (_playerController != null)
        {
            _playerController.UpdateBodyUnits(_registeredBodyUnitsSO);
        }
    }

    public void UpdateUI()
    {
        foreach (Transform child in _slotsContainer)
        {
            Destroy(child.gameObject);
        }

        _registeredBodyUnitsSO.Clear();

        for (int i = 0; i < _playerController.BodyUnitsSO.Count; i++)
        {
            _currentSlot = Instantiate(_unitSlotPrefab, _slotsContainer).transform;

            var unitPrefab = _playerController.BodyUnitsSO[i].UnitUIPrefab;
            var unitObject = Instantiate(unitPrefab, _currentSlot);

            if (unitObject.TryGetComponent(out DraggableUnit draggableUnit))
            {
                if (_currentSlot.TryGetComponent(out UnitSlot unitSlot))
                {
                    unitSlot.RegisterUnit(draggableUnit);
                    unitSlot.SetManager(this);
                }

                _registeredBodyUnitsSO.Add(draggableUnit.BodyUnitSO);
            }
        }
    }

    public void UpdateRegisteredUnitsOrder()
    {
        _registeredBodyUnitsSO.Clear();

        foreach (Transform slotTransform in _slotsContainer)
        {
            if (slotTransform.TryGetComponent(out UnitSlot slot) && slot.GetAssignedUnit() != null)
            {
                _registeredBodyUnitsSO.Add(slot.GetAssignedUnit().BodyUnitSO);
            }
        }
    }

    public List<BodyPartSO> RegisteredBodyUnitsSO => _registeredBodyUnitsSO;
}
