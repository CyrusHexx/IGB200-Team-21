using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerFuseBox : MonoBehaviour
{
    public Color onColor = Color.red; 
    public Color offColor = Color.green; 
    private bool isOn;
    public bool powerRestarted = false;

    private Renderer rend; 

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>(); 
        isOn = false;

        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void powerRestart()
    {
        isOn = !isOn;
        UpdateColor(); 
    }

    private void UpdateColor()
    {
        rend.material.color = isOn ? onColor : offColor; 
    }

    public bool IsOn() => isOn;
}
