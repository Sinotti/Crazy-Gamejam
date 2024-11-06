using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUnit : MonoBehaviour, IBeginDragHandler, IDragHandler ,IEndDragHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private BodyPartSO _bodyUnitSO;

    private Transform _parentAfterDrag;

    public Transform ParentAfterDrag { get => _parentAfterDrag; set => _parentAfterDrag = value; }
    public BodyPartSO BodyUnitSO { get => _bodyUnitSO; set => _bodyUnitSO = value; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parentAfterDrag = transform.parent;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        _canvasGroup.blocksRaycasts = false;
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_parentAfterDrag);

        _canvasGroup.blocksRaycasts = true;
        _image.raycastTarget = true;
    }
}
