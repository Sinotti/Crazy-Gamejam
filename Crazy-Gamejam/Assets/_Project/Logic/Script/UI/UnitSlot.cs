using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private DraggableUnit _assignedUnit;
    private BodyUnitsUIManager _manager;

    private GameObject dropped;
    private GameObject current;

    public void SetManager(BodyUnitsUIManager manager)
    {
        _manager = manager;
    }

    public void RegisterUnit(DraggableUnit draggableUnit)
    {
        _assignedUnit = draggableUnit;
    }

    public DraggableUnit GetAssignedUnit()
    {
        return _assignedUnit;
    }

    public void OnDrop(PointerEventData eventData)
    {
        dropped = eventData.pointerDrag;

        if (dropped == null || !dropped.TryGetComponent(out DraggableUnit draggableUnit))
            return;

        if (!draggableUnit.ParentAfterDrag.TryGetComponent(out UnitSlot originalSlot))
            return;

        if (transform.childCount == 0)
        {
            draggableUnit.ParentAfterDrag = transform;
            RegisterUnit(draggableUnit);
        }
        else
        {
            current = transform.GetChild(0).gameObject;

            if (current.TryGetComponent(out DraggableUnit currentDraggable))
            {
                currentDraggable.transform.SetParent(draggableUnit.ParentAfterDrag);
                originalSlot.RegisterUnit(currentDraggable);

                draggableUnit.ParentAfterDrag = transform;
                RegisterUnit(draggableUnit);
            }
        }

        _manager?.UpdateRegisteredUnitsOrder();
    }
}
