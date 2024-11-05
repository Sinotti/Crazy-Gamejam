using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSlot : MonoBehaviour, IDropHandler
{
    private GameObject _dropped;

    public void OnDrop(PointerEventData eventData)
    {
        _dropped = eventData.pointerDrag;
        
        if (_dropped.TryGetComponent(out DraggableUnit draggableUnit))
        {
            Debug.Log("Dropped");
            draggableUnit.ParentAfterDrag = transform;
        }
    }
}
