using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerFuseBox : MonoBehaviour
{
    public Color onColor = Color.red; // Color when the appliance is on
    public Color offColor = Color.green; // Color when the appliance is off
    private bool isOn;
    public bool powerRestarted = false;

    private Renderer rend; // Reference to the Renderer component

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>(); // Get the Renderer component
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
        UpdateColor(); // Update the color based on the new state      
    }

    private void UpdateColor()
    {
        rend.material.color = isOn ? onColor : offColor; // Set the color based on the state
    }

    public bool IsOn() => isOn;
}
