using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSpawner : MonoBehaviour
{
    public GameObject wire;
    public Transform parentObject;
    // Update is called once per frame
    void Update()
    {
        if(transform.childCount == 0) {
            //Instantiate(wire, this.transform, worldPositionStays:false);
            //wire.transform.parent = this.gameObject.transform;
            GameObject newWire = Instantiate(wire, parentObject);
            newWire.transform.localScale = new Vector3(1, 3.5f, 1);
            wire = newWire;
        }
    }
}
