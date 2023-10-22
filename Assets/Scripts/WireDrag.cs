using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WireDrag : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler, IPointerClickHandler
{
    void Start() {
        image.raycastTarget = true;
    }

    public Image image;
    public Transform parentAfterDrag;
    public void OnBeginDrag(PointerEventData eventData) {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        transform.Rotate(new Vector3(0,0,90));
    }
}
