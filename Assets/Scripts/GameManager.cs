using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton reference
    public Appliance[] appliances; // Reference to all appliances
    public PowerFuseBox[] fuseBoxes; // reference to all power fuse boxes
    public float switchInterval = 5f; // Time between random switches
    public Slider loadSlider; // Reference to the UI Slider
    public float maxLoad = 100f; // Maximum load before game over
    public bool overload = false;
    public bool poweredDown = false;
    public float currentLoad;

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
        /// StartCoroutine(RandomSwitch());
    }

    private void Update()
    {
        UpdateAppliance();
    }

    private void UpdateAppliance()
    {
        List<Appliance> offAppliances = new List<Appliance>();
        foreach (var appliance in appliances)
        {
            bool inGhostRange = appliance.GetComponent<Appliance>().inGhostRange;
            if (inGhostRange == true && overload == false && !appliance.IsOn())
            {
                offAppliances.Add(appliance);
            }
        }
        for(int i = 0; i < offAppliances.Count; i++)
        {
            Appliance turnedONAppliance = offAppliances[i];
            turnedONAppliance.ToggleState(false);
        }
    }
    
   public void resetLoad()
    {
        overload = false;
        currentLoad = 0;
        loadSlider.value = currentLoad;
    }
    
    public void UpdateLoadMeter(float loadChange)
    {
        currentLoad += loadChange;
        currentLoad = Mathf.Clamp(currentLoad, 0, maxLoad);

        loadSlider.value = currentLoad; // Update the slider's value

        ///Debug.Log("Current Load: " + currentLoad);


        if (currentLoad >= maxLoad)
        {
            overload = true;

            List<PowerFuseBox> onPowerBox = new List<PowerFuseBox>();
            foreach (var fuses in fuseBoxes)
            {
                if (!fuses.IsOn())
                {
                    onPowerBox.Add(fuses);
                }
            }
            for (int i = 0; i < onPowerBox.Count; i++)
            {

                PowerFuseBox turnedOFFPowerBox = onPowerBox[i];
                turnedOFFPowerBox.powerRestart();
                
            }
  
            List<Appliance> onAppliances = new List<Appliance>();
            foreach (var appliance in appliances)
            {
                if (appliance.IsOn())
                {
                    onAppliances.Add(appliance);
                }
            }
            for (int i = 0; i < onAppliances.Count; i++)
            {
                
                Appliance turnedOFFAppliance = onAppliances[i];
                turnedOFFAppliance.ToggleState(true);
            }
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
                randomAppliance.ToggleState(false);
            }
        }
    }
}