using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuseBoxGameState : MonoBehaviour
{
    private Image image;
    public int collCount = 0;
    public bool isOn = false;

    void Start() {
        image = GetComponent<Image>();
        image.color = Color.white;
    }

    void Update() {
        if(collCount == 0) {
            isOn = false;
        }
        if(isOn == true) {
            image.color = Color.green;
        } else {
            image.color = Color.white;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        collCount = collCount + 1;
        if (isOn == false){
            isOn = other.gameObject.GetComponent<FuseBoxGameState>().isOn;
        }  
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(collCount >= 1) {
            collCount = collCount - 1;
        }
    }
}
