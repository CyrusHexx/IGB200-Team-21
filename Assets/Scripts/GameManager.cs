using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton reference
    public Appliance[] appliances; // Reference to all appliances
    public float switchInterval = 5f; // Time between random switches
    public Slider loadSlider; // Reference to the UI Slider
    public float maxLoad = 100f; // Maximum load before game over
    private float currentLoad;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Initialize the load slider's max value
        loadSlider.maxValue = maxLoad;

        // Calculate initial load based on the state of the appliances
        foreach (Appliance appliance in appliances)
        {
            if (appliance.IsOn())
            {
                currentLoad += appliance.loadContribution;
            }
        }

        loadSlider.value = currentLoad; // Set the initial value of the load slider

        // Start the random switching coroutine
        StartCoroutine(RandomSwitch());
    }


    public void UpdateLoadMeter(float loadChange)
    {
        currentLoad += loadChange;
        currentLoad = Mathf.Clamp(currentLoad, 0, maxLoad);

        loadSlider.value = currentLoad; // Update the slider's value

        Debug.Log("Current Load: " + currentLoad);


        if (currentLoad >= maxLoad)
        {
            // Game over logic
        }
    }

    private IEnumerator RandomSwitch()
    {
        while (true) // Infinite loop to keep switching appliances
        {
            yield return new WaitForSeconds(switchInterval); // Wait for the specified interval

            // Get all appliances that are currently off
            List<Appliance> offAppliances = new List<Appliance>();
            foreach (var appliance in appliances)
            {
                if (!appliance.IsOn())
                {
                    offAppliances.Add(appliance);
                }
            }

            // If there are any off appliances, select a random one to turn on
            if (offAppliances.Count > 0)
            {
                int randomIndex = Random.Range(0, offAppliances.Count);
                Appliance randomAppliance = offAppliances[randomIndex];

                // Turn on the selected appliance
                randomAppliance.ToggleState();
            }
        }
    }
}