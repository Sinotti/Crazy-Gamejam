using Main.Gameplay.Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashBin : MonoBehaviour, IDropHandler
{
    [SerializeField] private BodyUnitsUIManager _bodyUnitsUIManager;
    [SerializeField] private PlayerController _playerController;

    private void Start()
    {
        _bodyUnitsUIManager = FindFirstObjectByType<BodyUnitsUIManager>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        var dropped = eventData.pointerDrag;

        if (dropped == null || !dropped.TryGetComponent(out DraggableUnit draggableUnit))
            return;

        _playerController.RemoveBodyUnit(draggableUnit.BodyUnitSO);
        _bodyUnitsUIManager.UpdateUI();
    }
}
