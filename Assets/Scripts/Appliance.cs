using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appliance : MonoBehaviour
{
    public float loadContribution = 5.0f; // Load added to the meter when this appliance is on
    public Color onColor = Color.red; // Color when the appliance is on
    public Color offColor = Color.green; // Color when the appliance is off
    private bool isOn;

    private Renderer rend; // Reference to the Renderer component

    private void Start()
    {
        rend = GetComponent<Renderer>(); // Get the Renderer component

        // Randomly set initial state
        isOn = Random.Range(0, 2) == 0;

        // Update the load meter based on the initial state
        if (isOn)
        {
            GameManager.instance.UpdateLoadMeter(loadContribution);
        }

        UpdateColor(); // Set the initial color
    }


    public void ToggleState()
    {
        isOn = !isOn;
        UpdateColor(); // Update the color based on the new state
        GameManager.instance.UpdateLoadMeter(isOn ? loadContribution : -loadContribution);

        Debug.Log(gameObject.name + " toggled " + (isOn ? "on" : "off") + ", load contribution: " + (isOn ? loadContribution : -loadContribution));
    }

    private void UpdateColor()
    {
        rend.material.color = isOn ? onColor : offColor; // Set the color based on the state
    }

    public bool IsOn() => isOn;
}