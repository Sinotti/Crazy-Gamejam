using Main.Gameplay.Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashBin : MonoBehaviour, IDropHandler
{
    [SerializeField] private BodyUnitsUIManager _bodyUnitsUIManager;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private BodyUnitSelectionUI _bodyUnitSelectionUI; 

    private void Start()
    {
        _bodyUnitsUIManager = FindFirstObjectByType<BodyUnitsUIManager>();
        _bodyUnitSelectionUI = FindFirstObjectByType<BodyUnitSelectionUI>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dropped = eventData.pointerDrag;

        if (dropped == null || !dropped.TryGetComponent(out DraggableUnit draggableUnit))
            return;

        _playerController.RemoveBodyUnit(draggableUnit.BodyUnitSO);

        BodyUnitSelectionUI.BodyUnitStoreItem unitItem = _bodyUnitSelectionUI.AvailableBodyParts
            .Find(item => item.bodyPart == draggableUnit.BodyUnitSO);

        if (unitItem != null)
        {
            if (_bodyUnitsUIManager.RegisteredBodyUnitsSO.Count <= 1) return;
            int salePrice = unitItem.price / 2;
            _bodyUnitSelectionUI.PlayerMoney += salePrice;
            _bodyUnitSelectionUI.UpdateMoneyUI(); 
        }

        _bodyUnitsUIManager.UpdateUI();
    }
}
