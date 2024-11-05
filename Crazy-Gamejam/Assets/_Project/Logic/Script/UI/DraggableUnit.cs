using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUnit : MonoBehaviour, IBeginDragHandler, IDragHandler ,IEndDragHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _canvasGroup;

    private Transform _parentAfterDrag;

    public Transform ParentAfterDrag { get => _parentAfterDrag; set => _parentAfterDrag = value; }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");

        _parentAfterDrag = transform.parent;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        _canvasGroup.blocksRaycasts = false;
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        transform.SetParent(_parentAfterDrag);

        _canvasGroup.blocksRaycasts = true;
        _image.raycastTarget = true;
    }
}
