using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton reference
    public Appliance[] appliances; // Reference to all appliances
    public PowerFuseBox[] fuseBoxes; // reference to all power fuse boxes
    public Slider loadSlider; // Reference to the UI Slider
    public Light mainLight;
    public float maxLoad = 100f; // Maximum load before game over
    public bool overload = false;
    public bool poweredDown = false;
    public float currentLoad;

    public GameObject tutorialPanel;
    public Timer timerScript;
    public AMPM ampmScript;
    public MonoBehaviour ghostScript;
    public Ghost[] ghostScripts;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ShowTutorial();

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
    }

    private void Update()
    {
        if (!tutorialPanel.activeSelf)
        {
            UpdateAppliance();
        }
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
    
   public void 
        resetLoad()
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
            mainLight.intensity = 0.1f;

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
        else
        {
            mainLight.intensity = 1f;
        }
    }

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true); // Show the tutorial panel

        // Deactivate the Timer and Ghost scripts
        timerScript.enabled = false;
        ampmScript.enabled = false;

        foreach (var ghost in ghostScripts)
        {
            ghost.enabled = false;
        }

    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false); // Hide the tutorial panel

        // Reactivate the Timer and Ghost scripts
        timerScript.enabled = true;
        ampmScript.enabled = true;

        foreach (var ghost in ghostScripts)
        {
            ghost.enabled = true;
        }

    }
}