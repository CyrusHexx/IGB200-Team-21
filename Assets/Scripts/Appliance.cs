using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appliance : MonoBehaviour
{
    public float loadContribution = 5.0f; // Load added to the meter when this appliance is on
    public Color onColor = Color.red; // Color when the appliance is on
    public Color offColor = Color.green; // Color when the appliance is off
    private bool isOn;
    public bool inGhostRange;

    private Renderer rend; // Reference to the Renderer component

    private void Start()
    {
        rend = GetComponent<Renderer>(); // Get the Renderer component

        // Randomly set initial state
        isOn = false;

        // Update the load meter based on the initial state
        if (isOn)
        {
            GameManager.instance.UpdateLoadMeter(loadContribution);
        }

        UpdateColor(); // Set the initial color
    }


    public void ToggleState(bool overloaded)
    {
        if (overloaded == false)
        {
            isOn = !isOn;
            UpdateColor(); // Update the color based on the new state
            GameManager.instance.UpdateLoadMeter(isOn ? loadContribution : -loadContribution);
        }
        else if (overloaded == true)
        {
            isOn = !isOn;
            UpdateColor(); // Update the color based on the new state
        } 
    }

    private void UpdateColor()
    {
        rend.material.color = isOn ? onColor : offColor; // Set the color based on the state
    }

    public bool IsOn() => isOn;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ghost")
        {
            inGhostRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Ghost")
        {
            inGhostRange = false;
        }
    }
}