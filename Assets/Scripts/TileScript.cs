using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData) {
        if(transform.childCount == 0) {
            GameObject dropped = eventData.pointerDrag;
            WireDrag wireDrag = dropped.GetComponent<WireDrag>();
            wireDrag.parentAfterDrag = transform;
        }
    }
}
